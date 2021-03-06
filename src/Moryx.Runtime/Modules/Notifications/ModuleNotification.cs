// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using Moryx.Modules;
using Moryx.Notifications;

namespace Moryx.Runtime.Modules
{
    internal class ModuleNotification : IModuleNotification
    {
        private readonly Action<ModuleNotification> _confirmationDelegate;

        private static void EmptyDelegate(ModuleNotification notification)
        {
        }

        /// <summary>
        /// Type of this notification
        /// </summary>
        public Severity Severity { get; }

        /// <summary>
        /// Notification message
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Time stamp of occurence
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        /// Optional exception as cause of this message
        /// </summary>
        public Exception Exception { get; }

        public ModuleNotification(Severity severity, string message)
            : this(severity, message, null, EmptyDelegate)
        {
        }

        public ModuleNotification(Severity severity, string message, Action<ModuleNotification> confirmationDelegate)
            : this(severity, message, null, confirmationDelegate)
        {
        }

        public ModuleNotification(Severity severity, string message, Exception exception)
            : this(severity, message, exception, EmptyDelegate)
        {
        }

        public ModuleNotification(Severity severity, string message, Exception exception, Action<ModuleNotification> confirmationDelegate)
        {
            Severity = severity;
            Message = message;
            Timestamp = DateTime.Now;
            Exception = exception;

            _confirmationDelegate = confirmationDelegate;
        }

        public bool Confirm()
        {
            _confirmationDelegate?.Invoke(this);
            return true;
        }
    }
}
