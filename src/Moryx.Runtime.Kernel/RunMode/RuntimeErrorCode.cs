// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

namespace Moryx.Runtime.Kernel
{
    /// <summary>
    /// Error Codes of the runtime
    /// </summary>
    public enum RuntimeErrorCode
    {
        /// <summary>
        /// Everything was fine. No error was occured
        /// </summary>
        NoError = 0,

        /// <summary>
        /// There was a warning while running the kernel
        /// </summary>
        Warning = 1,

        /// <summary>
        /// There was an error while running the kernel
        /// </summary>
        Error = 2
    }
}
