using System;
using System.Text;
using NLog;
using Wfm.App.ConfigManager;
using Wfm.App.Core;

namespace Wfm.App.Logging
{
    internal static class LoggingHelper
    {
        internal readonly static string SQLLoggingLevel = Configurations.SQLLogLevel;
        internal readonly static string FileLoggingLevel = Configurations.FileLogLevel;
        internal readonly static bool isFileLogEnabled = Convert.ToBoolean(Configurations.isFileLogEnabled);
        internal readonly static bool isSQLLogEnabled = Convert.ToBoolean(Configurations.isSQLLogEnabled);

        internal static string FormatMessage(string classname, string methodname, LogLevel level, string key, string ApplicationId, string Namespace, string Area, string Message)
        {
            try
            {
                return String.Format(@"LogTime: {0}, ApplicationID : {1} | Namespace {2} | Classname {3} | Methodname {4} | Area {5} | Loglevel {6} | Key {7} | Message {8}",
                    DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss:fff"), ApplicationId, Namespace, classname, methodname, Area, level.ToString(), key, Message);
            }
            catch
            {
                return String.Empty;
            }
            
        }

    }
}
