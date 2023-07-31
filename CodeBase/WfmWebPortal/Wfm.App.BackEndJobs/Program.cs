using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Wfm.App.ConfigManager;
using System.IO;

namespace Wfm.App.BackEndJobs
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
                File.WriteAllText(@"C:\Temp\csc.txt", "Program.Main.");
            try
            {
                Configurations.LoadBackEndJobSettings();
                
                File.WriteAllText(@"C:\Temp\csc.txt", "Configration loaded.");
            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.BackEndJob", "Program", "Main", "", "Error in loading Config." + ex.ToString());
            }

//#if (!DEBUG)
//            ServiceBase[] ServicesToRun;
//            ServicesToRun = new ServiceBase[]
//            {
//                new BackEndJobs()
//            };

//            someText = "Service Run about hit.";
//            File.WriteAllText(@"C:\Temp\csc.txt", someText);

//            ServiceBase.Run(ServicesToRun);

//            someText = "Service Run initiated.";
//            File.WriteAllText(@"C:\Temp\csc.txt", someText);
//#else
            Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.INFO, "WFMUI", "WFM.BackEndJob", "Program", "Main", "", "Service starting");
            
            BackEndJobs objBackEndob = new BackEndJobs();
            objBackEndob.OnStart();
            Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.INFO, "WFMUI", "WFM.BackEndJob", "Program", "Main", "", "Service started.");
//#endif
        }
    }
}
