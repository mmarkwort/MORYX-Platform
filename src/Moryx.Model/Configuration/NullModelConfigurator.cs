// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Collections.Generic;
using System.Data.Entity;
using Moryx.Configuration;
using Moryx.Logging;

namespace Moryx.Model.Configuration
{
    /// <summary>
    /// Null implementation of the <see cref="ModelConfiguratorBase{TConfig}"/>
    /// </summary>
    public sealed class NullModelConfigurator : IModelConfigurator
    {
        /// <inheritdoc />
        public IDatabaseConfig Config => null;

        /// <inheritdoc />
        public Type ContextType { get; private set; }

        /// <inheritdoc />
        public string TargetModel => string.Empty;

        /// <inheritdoc />
        public void Initialize(Type contextType, IConfigManager configManager, IModuleLogger logger)
        {
            ContextType = contextType;
        }

        /// <inheritdoc />
        public DbContext CreateContext(ContextMode mode)
        {
            throw new NotSupportedException("Not supported by " + nameof(NullModelConfigurator));
        }

        /// <inheritdoc />
        public DbContext CreateContext(IDatabaseConfig config, ContextMode mode)
        {
            throw new NotSupportedException("Not supported by " + nameof(NullModelConfigurator));
        }


        /// <inheritdoc />
        public string BuildConnectionString(IDatabaseConfig config)
        {
            throw new NotSupportedException("Not supported by " + nameof(NullModelConfigurator));
        }

        /// <inheritdoc />
        public string BuildConnectionString(IDatabaseConfig config, bool includeModel)
        {
            throw new NotSupportedException("Not supported by " + nameof(NullModelConfigurator));
        }

        /// <inheritdoc />
        public void UpdateConfig()
        {
            throw new NotSupportedException("Not supported by " + nameof(NullModelConfigurator));
        }

        /// <inheritdoc />
        public TestConnectionResult TestConnection(IDatabaseConfig config)
        {
            throw new NotSupportedException("Not supported by " + nameof(NullModelConfigurator));
        }

        /// <inheritdoc />
        public bool CreateDatabase(IDatabaseConfig config)
        {
            throw new NotSupportedException("Not supported by " + nameof(NullModelConfigurator));
        }

        /// <inheritdoc />
        public DatabaseUpdateSummary MigrateDatabase(IDatabaseConfig config)
        {
            throw new NotSupportedException("Not supported by " + nameof(NullModelConfigurator));
        }

        /// <inheritdoc />
        public DatabaseUpdateSummary MigrateDatabase(IDatabaseConfig config, string migrationId)
        {
            throw new NotSupportedException("Not supported by " + nameof(NullModelConfigurator));
        }

        /// <inheritdoc />
        public bool RollbackDatabase(IDatabaseConfig config)
        {
            throw new NotSupportedException("Not supported by " + nameof(NullModelConfigurator));
        }

        /// <inheritdoc />
        public IEnumerable<DatabaseUpdateInformation> AvailableMigrations(IDatabaseConfig config)
        {
            throw new NotSupportedException("Not supported by " + nameof(NullModelConfigurator));
        }

        /// <inheritdoc />
        public IEnumerable<DatabaseUpdateInformation> AppliedMigrations(IDatabaseConfig config)
        {
            throw new NotSupportedException("Not supported by " + nameof(NullModelConfigurator));
        }

        /// <inheritdoc />
        public void DeleteDatabase(IDatabaseConfig config)
        {
            throw new NotSupportedException("Not supported by " + nameof(NullModelConfigurator));
        }

        /// <inheritdoc />
        public void DumpDatabase(IDatabaseConfig config, string targetPath)
        {
            throw new NotSupportedException("Not supported by " + nameof(NullModelConfigurator));
        }

        /// <inheritdoc />
        public void RestoreDatabase(IDatabaseConfig config, string filePath)
        {
            throw new NotSupportedException("Not supported by " + nameof(NullModelConfigurator));
        }

        /// <inheritdoc />
        public IEnumerable<IModelSetup> GetAllSetups()
        {
            throw new NotSupportedException("Not supported by " + nameof(NullModelConfigurator));
        }

        /// <inheritdoc />
        public void Execute(IDatabaseConfig config, IModelSetup setup, string setupData)
        {
            throw new NotSupportedException("Not supported by " + nameof(NullModelConfigurator));
        }
    }
}