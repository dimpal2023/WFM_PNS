using System;
using System.Text;
using NLog;
using static Wfm.App.Core.Enums;

namespace Wfm.App.Logging
{
    internal class Logging : ILogging
    {
        private static Logger _LoggerFileService = LogManager.GetLogger("NLogLoggerFile");

        private StringBuilder Messages { get; set; }
        private static bool _initialized = false;
        private static bool isTraceEnabled = true;
        private static bool isDebugEnabled = true;
        private static bool isInfoEnabled = true;
        private static bool isErrorEnabled = true;
        private static bool isWarnEnabled = true;
        private static bool isFatalEnabled = true;

        static Logging()
        {
            _initialized = Initialized();
        }

        internal Logging()
        {
            Messages = new StringBuilder();
        }

        private static bool Initialized()
        {
            if (!_initialized)
            {
                if (!String.IsNullOrEmpty(LoggingHelper.FileLoggingLevel))
                {
                    LogLevels logLevel;
                    if (Enum.TryParse(LoggingHelper.FileLoggingLevel.ToUpper(), true, out logLevel))
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
            if (isDebugEnabled)
            {
                if (Messages == null)
                    Messages = new StringBuilder();

                Messages.AppendLine(LoggingHelper.FormatMessage(Classname, Functionname, LogLevel.Debug, key, ApplicationID, Namespace, Area, Message));
            }
            FlushMessagestoNLog(Messages,LogLevel.Debug.ToString());
        }

        public void LogInfoMessage(string key, string ApplicationID, string Namespace, string Classname, string Functionname, string Area, string Message)
        {
            if (isInfoEnabled)
            {
                if (Messages == null)
                    Messages = new StringBuilder();

                Messages.AppendLine(LoggingHelper.FormatMessage(Classname, Functionname, LogLevel.Info, key, ApplicationID, Namespace, Area, Message));
            }
            FlushMessagestoNLog(Messages, LogLevel.Info.ToString());
        }
        public void LogErrorMessage(string key, string ApplicationID, string Namespace, string Classname, string Functionname, string Area, string Message)
        {
            if (isErrorEnabled)
            {
                if (Messages == null)
                    Messages = new StringBuilder();

                Messages.AppendLine(LoggingHelper.FormatMessage(Classname, Functionname, LogLevel.Error, key, ApplicationID, Namespace, Area, Message));
            }
            FlushMessagestoNLog(Messages, LogLevel.Error.ToString());
        }
        public void LogWarnMessage(string key, string ApplicationID, string Namespace, string Classname, string Functionname, string Area, string Message)
        {
            if (isWarnEnabled)
            {
                if (Messages == null)
                    Messages = new StringBuilder();

                Messages.AppendLine(LoggingHelper.FormatMessage(Classname, Functionname, LogLevel.Warn, key, ApplicationID, Namespace, Area, Message));
            }
            FlushMessagestoNLog(Messages, LogLevel.Warn.ToString());
        }
        public void LogFatalMessage(string key, string ApplicationID, string Namespace, string Classname, string Functionname, string Area, string Message)
        {
            if (isFatalEnabled)
            {
                if (Messages == null)
                    Messages = new StringBuilder();

                Messages.AppendLine(LoggingHelper.FormatMessage(Classname, Functionname, LogLevel.Fatal, key, ApplicationID, Namespace, Area, Message));
            }
            FlushMessagestoNLog(Messages, LogLevel.Fatal.ToString());
        }

        public void LogTraceMessage(string key, string ApplicationID, string Namespace, string Classname, string Functionname, string Area, string Message)
        {
            if (isTraceEnabled)
            {
                if (Messages == null)
                    Messages = new StringBuilder();

                Messages.AppendLine(LoggingHelper.FormatMessage(Classname, Functionname, LogLevel.Trace, key, ApplicationID, Namespace, Area, Message));
            }
            FlushMessagestoNLog(Messages, LogLevel.Trace.ToString());
        }

        private static void FlushMessagestoNLog(StringBuilder message, string loglevel)
        {
            LogLevel nloglevel = null;
            try
            {
                // Flush to File
                nloglevel = LogLevel.FromString(loglevel);
            }
            catch( Exception ex)
            {
                _LoggerFileService.Log(LogLevel.Warn, ex);
            }


            try {
                if (nloglevel != null && nloglevel != LogLevel.Off && _LoggerFileService != null)
                {
                    _LoggerFileService.Log(nloglevel, message);
                }
            }
            catch (Exception ex)
            {

            }

        }

    }
}
