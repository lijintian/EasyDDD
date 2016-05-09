using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using EasyDDD.Infrastructure.Crosscutting.InversionOfControl;

namespace EasyDDD.Infrastructure.Crosscutting.Logging
{
    /// <summary>
    /// Log
    /// </summary>
    public static class Log<T> where T : class
    {
        private static ILog InternalLog
        {
            [DebuggerStepThrough]
            get { return IoC.Resolve<ILog>(); }
        }

        /// <summary>
        /// Log debug message
        /// </summary>
        /// <param name="message">The debug message</param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        public static void Debug(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            InternalLog.Debug(typeof(T), message, memberName, sourceFilePath, sourceLineNumber);
        }

        /// <summary>
        /// Log debug message
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="exception">Exception to write in debug message</param>
        public static void Debug(string message, Exception exception)
        {
            InternalLog.Debug(typeof(T), message, exception);
        }

        /// <summary>
        /// Log debug message 
        /// </summary>
        /// <param name="item">The item with information to write in debug</param>
        public static void Debug(object item)
        {
            InternalLog.Debug(typeof(T), item);
        }

        /// <summary>
        /// Log FATAL error
        /// </summary>
        /// <param name="message">The message of fatal error</param>
        public static void Fatal(string message)
        {
            InternalLog.Fatal(typeof(T), message);
        }

        /// <summary>
        /// log FATAL error
        /// </summary>
        /// <param name="message">The message of fatal error</param>
        /// <param name="exception">The exception to write in this fatal message</param>
        public static void Fatal(string message, Exception exception)
        {
            InternalLog.Fatal(typeof(T), message, exception);
        }

        /// <summary>
        /// Log message information 
        /// </summary>
        /// <param name="message">The information message to write</param>
        public static void LogInfo(string message)
        {
            InternalLog.LogInfo(typeof(T), message);
        }

        /// <summary>
        /// Log warning message
        /// </summary>
        /// <param name="message">The warning message to write</param>
        public static void LogWarning(string message)
        {
            InternalLog.LogWarning(typeof(T), message);
        }

        /// <summary>
        /// Log warning message
        /// </summary>
        /// <param name="message">The warning message to write</param>
        public static void LogWarning(string message, Exception ex)
        {
            InternalLog.LogWarning(typeof(T), message);
        }

        /// <summary>
        /// Log error message
        /// </summary>
        /// <param name="message">The error message to write</param>
        public static void LogError(string message)
        {
            InternalLog.LogError(typeof(T), message);
        }


        /// <summary>
        /// Log error message
        /// </summary>
        /// <param name="message">The error message to write</param>
        /// <param name="exception">The exception associated with this error</param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        public static void LogError(string message, Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            InternalLog.LogError(typeof(T), message, exception);
        }
    }

}
