// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.Linq;
using Moryx.Configuration;
using Moryx.Logging;

namespace Moryx.Model.Configuration
{
    /// <summary>
    /// Base class for model configurators
    /// </summary>
    public abstract class ModelConfiguratorBase<TConfig> : IModelConfigurator
        where TConfig : class, IDatabaseConfig, new()
    {
        private IConfigManager _configManager;
        private string _configName;
        private DbMigrationsConfiguration _migrationsConfiguration;
        private string[] _migrations;
        private Type _contextType;

        /// <summary>
        /// Logger for this model configurator
        /// </summary>
        protected IModuleLogger Logger { get; private set; }

        /// <summary>
        /// The invariant name of the database provider
        /// </summary>
        protected abstract string ProviderInvariantName { get; }

        /// <inheritdoc />
        public IDatabaseConfig Config { get; private set; }

        /// <inheritdoc />
        public void Initialize(Type contextType, IConfigManager configManager, IModuleLogger logger)
        {
            _contextType = contextType;
            _configManager = configManager;

            // Add logger
            Logger = logger;

            // Load Config
            _configName = contextType.FullName + ".DbConfig";
            Config = _configManager.GetConfiguration<TConfig>(_configName);

            // If database is empty, fill with TargetModel name
            if (string.IsNullOrWhiteSpace(Config.Database))
                Config.Database = contextType.Name;

            // Create migrations configuration
            _migrationsConfiguration = CreateDbMigrationsConfiguration();

            // Load local migrations
            _migrations = GetAvailableMigrations();
        }

        /// <inheritdoc />
        public DbContext CreateContext(ContextMode mode)
        {
            return CreateContext(Config, mode);
        }

        /// <inheritdoc />
        public DbContext CreateContext(IDatabaseConfig config, ContextMode mode)
        {
            var context = (DbContext)Activator.CreateInstance(_contextType, BuildConnectionString(config));
            context.SetContextMode(mode);
            return context;
        }

        /// <inheritdoc />
        public void UpdateConfig()
        {
            _configManager.SaveConfiguration(Config, _configName);
        }

        /// <inheritdoc />
        public virtual TestConnectionResult TestConnection(IDatabaseConfig config)
        {
            if (string.IsNullOrWhiteSpace(config.Database))
                return TestConnectionResult.ConnectionError;

            if (!TestDatabaseConnection(config))
                return TestConnectionResult.ConnectionError;

            var context = CreateContext(config, ContextMode.AllOff);
            try
            {
                return context.Database.Exists()
                    ? TestConnectionResult.Success
                    : TestConnectionResult.ConnectionOkDbDoesNotExist;
            }
            catch
            {
                return TestConnectionResult.ConnectionOkDbDoesNotExist;
            }
            finally
            {
                context.Dispose();
            }
        }

        /// <inheritdoc />
        public virtual bool CreateDatabase(IDatabaseConfig config)
        {
            // Check is database is configured
            if (!CheckDatabaseConfig(config))
            {
                return false;
            }

            using (var context = CreateContext(config, ContextMode.AllOff))
            {
                // Check if this database is present on the server
                var dbExists = context.Database.Exists();
                if (dbExists)
                {
                    return false;
                }

                context.Database.Create();

                // Create connection to our new database
                var connection = CreateConnection(config);
                connection.Open();

                // Creation done -> close connection
                connection.Close();

                return true;
            }
        }

        /// <summary>
        /// Creates a <see cref="DbConnection"/>
        /// </summary>
        protected abstract DbConnection CreateConnection(IDatabaseConfig config);

        /// <summary>
        /// Creates a <see cref="DbConnection"/>
        /// </summary>
        protected abstract DbConnection CreateConnection(IDatabaseConfig config, bool includeModel);

        /// <summary>
        /// Creates a <see cref="DbCommand"/>
        /// </summary>
        protected abstract DbCommand CreateCommand(string cmdText, DbConnection connection);

        /// <inheritdoc />
        public string BuildConnectionString(IDatabaseConfig config)
        {
            return BuildConnectionString(config, true);
        }

        /// <inheritdoc />
        public abstract string BuildConnectionString(IDatabaseConfig config, bool includeModel);

        /// <inheritdoc />
        public DatabaseUpdateSummary MigrateDatabase(IDatabaseConfig config)
        {
            return MigrateDatabase(config, string.Empty);
        }

        /// <inheritdoc />
        public virtual DatabaseUpdateSummary MigrateDatabase(IDatabaseConfig config, string migrationId)
        {
            var result = new DatabaseUpdateSummary();

            if (string.IsNullOrEmpty(migrationId))
                migrationId = _migrations.LastOrDefault();

            var isAvailable = _migrations.Any(available => available == migrationId);
            if (isAvailable)
            {
                CreateDbMigrator(config).Update(migrationId);
                result.ExecutedUpdates = GetInstalledMigrations(config).Select(migration => new DatabaseUpdate
                {
                    Description = migration
                }).ToArray();
                result.WasUpdated = true;
            }

            return result;
        }

        /// <inheritdoc />
        public bool RollbackDatabase(IDatabaseConfig config)
        {
            var dbMigrator = CreateDbMigrator(config);

            if (dbMigrator != null)
            {
                dbMigrator.Update("0");
                return true;
            }
            return false;
        }

        /// <inheritdoc />
        public IEnumerable<DatabaseUpdateInformation> AvailableMigrations(IDatabaseConfig config)
        {
            return _migrations.Select(migration => new DatabaseUpdateInformation
            {
                Name = migration
            });
        }

        /// <inheritdoc />
        public IEnumerable<DatabaseUpdateInformation> AppliedMigrations(IDatabaseConfig config)
        {
            return GetInstalledMigrations(config).Select(migration => new DatabaseUpdateInformation
            {
                Name = migration
            });
        }

        /// <inheritdoc />
        public abstract void DeleteDatabase(IDatabaseConfig config);

        /// <inheritdoc />
        public abstract void DumpDatabase(IDatabaseConfig config, string targetPath);

        /// <inheritdoc />
        public abstract void RestoreDatabase(IDatabaseConfig config, string filePath);

        private DbMigrator CreateDbMigrator(IDatabaseConfig config)
        {
            if (_migrationsConfiguration == null)
            {
                return null;
            }

            var configuration = _migrationsConfiguration;
            configuration.TargetDatabase = new DbConnectionInfo(BuildConnectionString(config), ProviderInvariantName);

            return new DbMigrator(configuration);
        }

        /// <summary>
        /// Generally tests the connection to the database
        /// </summary>
        private bool TestDatabaseConnection(IDatabaseConfig config)
        {
            if (!CheckDatabaseConfig(config))
                return false;

            using (var conn = CreateConnection(config, false))
            {
                try
                {
                    conn.Open();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Validates the config gor Host, Database, Username and Port
        /// </summary>
        protected static bool CheckDatabaseConfig(IDatabaseConfig config)
        {
            return !(string.IsNullOrWhiteSpace(config.Host) ||
                     string.IsNullOrWhiteSpace(config.Database) ||
                     string.IsNullOrWhiteSpace(config.Username) ||
                     config.Port <= 0);
        }

        /// <summary>
        /// Returns or creates and returns the DbMigrationsConfiguration of the Model
        /// </summary>
        private DbMigrationsConfiguration CreateDbMigrationsConfiguration()
        {
            var configuration = _contextType.Assembly.DefinedTypes
                .FirstOrDefault(t => typeof(DbMigrationsConfiguration).IsAssignableFrom(t));

            if (configuration == null)
                return null;

            return (DbMigrationsConfiguration)Activator.CreateInstance(configuration);
        }

        /// <summary>
        /// Loads all available migrations for the model
        /// </summary>
        private string[] GetAvailableMigrations()
        {
            // Get configuration, if not available, no migrations are defined
            if (_migrationsConfiguration == null)
                return new string[0];

            // There is no suitable method to get model migrations without connection string - lets load them manually
            // https://stackoverflow.com/questions/23996785/get-local-migrations-from-assembly-using-ef-code-first-without-a-connection-stri
            var migrations = (from type in _migrationsConfiguration.MigrationsAssembly.GetTypes()
                              where typeof(DbMigration).IsAssignableFrom(type)
                              select (IMigrationMetadata)Activator.CreateInstance(type)).Select(m => m.Id).ToArray();

            return migrations;
        }

        /// <summary>
        /// Opens a database connection and checks the currently installed migrations
        /// </summary>
        private IEnumerable<string> GetInstalledMigrations(IDatabaseConfig config)
        {
            DbMigrator dbMigrator = null;

            if (TestDatabaseConnection(config))
            {
                dbMigrator = CreateDbMigrator(config);
            }

            return dbMigrator != null
                ? dbMigrator.GetDatabaseMigrations()
                : Enumerable.Empty<string>();
        }
    }
}
