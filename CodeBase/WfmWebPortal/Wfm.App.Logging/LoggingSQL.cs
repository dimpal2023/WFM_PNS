using System;
using System.Text;
using NLog;
using static Wfm.App.Core.Enums;

namespace Wfm.App.Logging
{
    internal class LoggingSQL : ILogging
    {
        private static Logger _LoggerService = LogManager.GetLogger("NLogLoggerDb");

        private StringBuilder Messages { get; set; }
        private static bool _initialized = false;

        private static bool isTraceEnabled = true;
        private static bool isDebugEnabled = true;
        private static bool isInfoEnabled = true;
        private static bool isErrorEnabled = true;
        private static bool isWarnEnabled = true;
        private static bool isFatalEnabled = true;

        static LoggingSQL()
        {
            _initialized = Initialized();
        }

        internal LoggingSQL()
        {
            Messages = new StringBuilder();
        }

        private static bool Initialized()
        {
            if (!_initialized)
            {
                if (!String.IsNullOrEmpty(LoggingHelper.SQLLoggingLevel))
                {

                    LogLevels logLevel;
                    if (Enum.TryParse(LoggingHelper.SQLLoggingLevel.ToUpper(), true, out logLevel))
                    {
                        switch (logLevel)
                        {
                            case LogLevels.TRACE:
                                isTraceEnabled = true;
                                isDebugEnabled = true;
                                isErrorEnabled = true;
                                isInfoEnabled = true;
                                isFatalEnabled = true;
                                isWarnEnabled = true;
                                break;
                            case LogLevels.DEBUG:
                                isTraceEnabled = false;
                                isDebugEnabled = true;
                                isErrorEnabled = false;
                                isInfoEnabled = false;
                                isFatalEnabled = false;
                                isWarnEnabled = false;
                                break;
                            case LogLevels.INFO:
                                isTraceEnabled = false;
                                isDebugEnabled = false;
                                isErrorEnabled = false;
                                isInfoEnabled = true;
                                isFatalEnabled = false;
                                isWarnEnabled = false;
                                break;
                            case LogLevels.ERROR:
                                isTraceEnabled = false;
                                isDebugEnabled = false;
                                isErrorEnabled = true;
                                isInfoEnabled = false;
                                isFatalEnabled = false;
                                isWarnEnabled = false;
                                break;
                            case LogLevels.WARN:
                                isTraceEnabled = false;
                                isDebugEnabled = false;
                                isErrorEnabled = false;
                                isInfoEnabled = false;
                                isFatalEnabled = false;
                                isWarnEnabled = true;
                                break;
                            case LogLevels.FATAL:
                                isTraceEnabled = false;
                                isDebugEnabled = false;
                                isErrorEnabled = false;
                                isInfoEnabled = false;
                                isFatalEnabled = true;
                                isWarnEnabled = false;
                                break;
                        }
                    }
                }
                _initialized = true;
            }

            return _initialized;
        }

        public void LogDebugMessage(string key, string ApplicationID, string Namespace, string Classname, string Functionname, string Area, string Message)
        {
            SetLogObect(ApplicationID, Namespace, Classname, Functionname, Area, Message, LogLevel.Debug);
        }

        public void LogInfoMessage(string key, string ApplicationID, string Namespace, string Classname, string Functionname, string Area, string Message)
        {
            SetLogObect(ApplicationID, Namespace, Classname, Functionname, Area, Message, LogLevel.Info);
        }
        public void LogErrorMessage(string key, string ApplicationID, string Namespace, string Classname, string Functionname, string Area, string Message)
        {
            SetLogObect(ApplicationID, Namespace, Classname, Functionname, Area, Message, LogLevel.Error);
        }
        public void LogWarnMessage(string key, string ApplicationID, string Namespace, string Classname, string Functionname, string Area, string Message)
        {
            SetLogObect(ApplicationID, Namespace, Classname, Functionname, Area, Message, LogLevel.Warn);
        }
        public void LogFatalMessage(string key, string ApplicationID, string Namespace, string Classname, string Functionname, string Area, string Message)
        {
            SetLogObect(ApplicationID,Namespace,Classname,Functionname,Area,Message, LogLevel.Fatal);
        }
        public void LogTraceMessage(string key, string ApplicationID, string Namespace, string Classname, string Functionname, string Area, string Message)
        {
            SetLogObect(ApplicationID, Namespace, Classname, Functionname, Area, Message, LogLevel.Fatal);
        }

        private static void SetLogObect(string ApplicationID, string Namespace, string Classname, string Functionname, string Area, string Message, LogLevel nloglevel)
        {
            LogEventInfo theEvent = new LogEventInfo(nloglevel,"","");
            theEvent.Properties["LoggedDate"] = DateTime.Now;
            theEvent.Properties["ApplicationID"] = ApplicationID;
            theEvent.Properties["Namespace"] = Namespace;
            theEvent.Properties["Classname"] = Classname;
            theEvent.Properties["Functionname"] = Functionname;
            theEvent.Properties["Area"] = Area;
            theEvent.Properties["Message"] = Message;
            _LoggerService.Log(theEvent);
        }

    }
}
