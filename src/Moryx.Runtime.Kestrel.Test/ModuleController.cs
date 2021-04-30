// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Moryx.Logging;
using Moryx.Modules;
using Moryx.Runtime.Container;
using Moryx.Runtime.Modules;

namespace Moryx.Runtime.Kestrel.Test
{
    /// <summary>
    /// Maintenance module that hosts the plugins.
    /// </summary>
    [ServerModule(ModuleName)]
    [Description("Core module to maintain the application. It provides config, database and logging support by default. " +
                 "Additional plugins can be included as well as other extensions implementing IMaintenanceModule")]
    public class ModuleController : ServerModuleBase<ModuleConfig>
    {
        internal const string ModuleName = "KestrelTest";

        /// <summary>
        /// Name of this module
        /// </summary>
        public override string Name => ModuleName;

        public IHttpFactory HttpFactory { get; set; }

        public IEnumerable<IInitializable> Initializables { get; set; }

        public IModuleManager ModuleManager { get; set; }

        /// <summary>
        /// Called when [initialize].
        /// </summary>
        protected override void OnInitialize()
        {
            var test3 = new HttpFactory();
            var test2 = Initializables.ElementAt(1);

            if (test3.GetType() == test2.GetType())
            {
                int i = 3;
            }

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            
            IHttpFactory test = Initializables.ElementAt(1) as HttpFactory;
        }

        /// <summary>
        /// Called when [start].
        /// </summary>
        /// <exception cref="System.Exception">Failed to start module  + moduleConfig.PluginName</exception>
        protected override void OnStart()
        {
        }

        /// <summary>
        /// Called when [stop].
        /// </summary>
        protected override void OnStop()
        {

        }
    }
}
