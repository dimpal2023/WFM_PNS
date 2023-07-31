using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wfm.App.Logging
{
    internal interface ILogging
    {

        void LogDebugMessage(string key, string ApplicationID, string Namespace, string Classname, string Functionname, string Area, string Message);
        void LogInfoMessage(string key, string ApplicationID, string Namespace, string Classname, string Functionname, string Area, string Message);
        void LogErrorMessage(string key, string ApplicationID, string Namespace, string Classname, string Functionname, string Area, string Message);
        void LogWarnMessage(string key, string ApplicationID, string Namespace, string Classname, string Functionname, string Area, string Message);
        void LogFatalMessage(string key, string ApplicationID, string Namespace, string Classname, string Functionname, string Area, string Message);
        void LogTraceMessage(string key, string ApplicationID, string Namespace, string Classname, string Functionname, string Area, string Message);

    }
}
