using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wfm.App.ConfigManager;
using Wfm.App.Scheduler;

namespace Wfm.App.BackEndJobsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Configurations.LoadBackEndJobSettings();

            try
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.INFO, "WFMBackEndJobConsole", "Wfm.App.BackEndJobsConsole", "Program", "Main", "", "Import Started.");
                BioMetricData objBio = new BioMetricData();
                objBio.Initiate();
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.INFO, "WFMBackEndJobConsole", "Wfm.App.BackEndJobsConsole", "Program", "Main", "", "Import Complete.");
            }

            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMBackEndJobConsole", "Wfm.App.BackEndJobsConsole", "Program", "Main", "", "Error in import :" + ex.ToString());
            }
        }
    }
}
