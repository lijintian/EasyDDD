using log4net;
using System;
using System.Collections.Generic;
using System.Threading;
using MyILog = EasyDDD.Infrastructure.Crosscutting.Logging.ILog;
using Log4netIlog = log4net.ILog;
namespace EasyDDD.Infrastructure.Crosscutting.Logging.MyLog
{
    public sealed class Log4NetLog : MyILog
    {
        private ThreadLocal<Dictionary<Type, Log4netIlog>> _threadLocalLogger;


        public Log4NetLog()
        {
            log4net.Config.XmlConfigurator.Configure();
            _threadLocalLogger = new ThreadLocal<Dictionary<Type, Log4netIlog>>
            {
                Value = new Dictionary<Type, Log4netIlog>()
            };

        }

        private ThreadLocal<Dictionary<Type, Log4netIlog>> ThreadLocalLogger
        {
            get
            {
                if (_threadLocalLogger == null)
                {
                    _threadLocalLogger = new ThreadLocal<Dictionary<Type, Log4netIlog>>
                                            {
                                                Value = new Dictionary<Type, Log4netIlog>()
                                            };
                }

                if (_threadLocalLogger.Value == null) _threadLocalLogger.Value = new Dictionary<Type, Log4netIlog>();

                return _threadLocalLogger;
            }
        }

        private Log4netIlog GetLogger(Type type)
        {
            if (!ThreadLocalLogger.Value.ContainsKey(type))
            {
                ThreadLocalLogger.Value.Add(type, LogManager.GetLogger(type));
            }
            return ThreadLocalLogger.Value[type];
        }


        public void Debug(Type type, string message,
            string memberName = "",
            string sourceFilePath = "",
            int sourceLineNumber = 0)
        {
            if (!String.IsNullOrWhiteSpace(message))
            {
                GetLogger(type).Debug(FormatMessage(message, memberName, sourceFilePath, sourceLineNumber));
            }
        }



        public void Debug(Type type, string message, Exception exception,
            string memberName = "",
            string sourceFilePath = "",
            int sourceLineNumber = 0)
        {
            if (!String.IsNullOrWhiteSpace(message))
            {

                GetLogger(type).Debug(FormatMessage(message, memberName, sourceFilePath, sourceLineNumber), exception);
            }
        }

        public void Debug(Type type, object item,
            string memberName = "",
            string sourceFilePath = "",
            int sourceLineNumber = 0)
        {
            if (item != null)
            {
                GetLogger(type).Debug(FormatMessage(item.ToString(), memberName, sourceFilePath, sourceLineNumber));
            }
        }

        public void LogInfo(Type type, string message,
            string memberName = "",
            string sourceFilePath = "",
            int sourceLineNumber = 0)
        {
            if (!String.IsNullOrWhiteSpace(message))
            {
                GetLogger(type).Info(FormatMessage(message, memberName, sourceFilePath, sourceLineNumber));
            }
        }

        public void LogWarning(Type type, string message,
            string memberName = "",
            string sourceFilePath = "",
            int sourceLineNumber = 0)
        {
            if (!String.IsNullOrWhiteSpace(message))
            {
                GetLogger(type).Warn(FormatMessage(message, memberName, sourceFilePath, sourceLineNumber));
            }
        }

        public void LogError(Type type, string message,
            string memberName = "",
            string sourceFilePath = "",
            int sourceLineNumber = 0)
        {
            if (!String.IsNullOrWhiteSpace(message))
            {
                GetLogger(type).Error(FormatMessage(message, memberName, sourceFilePath, sourceLineNumber));
            }
        }

        public void LogError(Type type, string message, Exception exception,
            string memberName = "",
            string sourceFilePath = "",
            int sourceLineNumber = 0)
        {
            if (!String.IsNullOrWhiteSpace(message)
                &&
                exception != null)
            {
                GetLogger(type).Error(FormatMessage(message, memberName, sourceFilePath, sourceLineNumber), exception);
            }
        }



        public void LogError(Type type, Exception exception,
            string memberName = "",
            string sourceFilePath = "",
            int sourceLineNumber = 0)
        {
            if (exception != null)
            {
                GetLogger(type).Error(FormatMessage("", memberName, sourceFilePath, sourceLineNumber), exception);
            }
        }

        public void Fatal(Type type, string message,
            string memberName = "",
            string sourceFilePath = "",
            int sourceLineNumber = 0)
        {
            if (!String.IsNullOrWhiteSpace(message))
            {

                GetLogger(type).Fatal(FormatMessage(message, memberName, sourceFilePath, sourceLineNumber));
            }
        }

        public void Fatal(Type type, string message, Exception exception,
            string memberName = "",
            string sourceFilePath = "",
            int sourceLineNumber = 0)
        {
            if (!String.IsNullOrWhiteSpace(message)
                &&
                exception != null)
            {

                GetLogger(type).Fatal(FormatMessage(message, memberName, sourceFilePath, sourceLineNumber), exception);
            }
        }

        private string FormatMessage(string message, string memberName, string sourceFilePath, int sourceLineNumber)
        {
            return string.Format(
                "Method: {0}  File: {1} line:{2} Msg: {3}",
                memberName, sourceFilePath, sourceLineNumber, message);
        }


    }
}
