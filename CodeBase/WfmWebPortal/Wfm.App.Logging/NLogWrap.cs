using System;
using static Wfm.App.Core.Enums;

namespace Wfm.App.Logging
{
    public static class NLogWrap
    {
        public static void LogMessage(string key, LogLevels loglevel, string ApplicationID, string Namespace, string Classname, string Functionname, string Area, string Message)
        {
            try {

                ILogging logging = null;

                if (LoggingHelper.isFileLogEnabled)
                {
                    logging = new Logging();
                    Log(key,loglevel,ApplicationID,Namespace,Classname,Functionname,Area,Message,logging);
                }

                if (LoggingHelper.isSQLLogEnabled)
                {
                    logging = new LoggingSQL();
                    Log(key, loglevel, ApplicationID, Namespace, Classname, Functionname, Area, Message,logging);
                }

            }
            catch(Exception ex)
            {
                LogMessage("GeneralMessage",LogLevels.ERROR,"","Logging","NLogWrap"," LogMessage","",ex.InnerException.ToString());
            }

        }

        private static void Log (string key, LogLevels loglevel, string ApplicationID, string Namespace, string Classname, string Functionname, string Area, string Message, ILogging logmodel)
        {
            switch (loglevel)
            {
                case LogLevels.INFO:
                    logmodel.LogInfoMessage(key,ApplicationID,Namespace,Classname,Functionname,Area,Message);
                    break;
                case LogLevels.DEBUG:
                    logmodel.LogDebugMessage(key, ApplicationID, Namespace, Classname, Functionname, Area, Message);
                    break;
                case LogLevels.ERROR:
                    logmodel.LogErrorMessage(key, ApplicationID, Namespace, Classname, Functionname, Area, Message);
                    break;
                case LogLevels.WARN:
                    logmodel.LogWarnMessage(key, ApplicationID, Namespace, Classname, Functionname, Area, Message);
                    break;
                case LogLevels.FATAL:
                    logmodel.LogFatalMessage(key, ApplicationID, Namespace, Classname, Functionname, Area, Message);
                    break;
                case LogLevels.TRACE:
                    logmodel.LogTraceMessage(key, ApplicationID, Namespace, Classname, Functionname, Area, Message);
                    break;
            }
        }
    }
}
