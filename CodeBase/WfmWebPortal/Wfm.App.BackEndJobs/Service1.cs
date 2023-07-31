using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Wfm.App.ConfigManager;
using Wfm.App.Scheduler;

namespace Wfm.App.BackEndJobs
{
    public partial class BackEndJobs : ServiceBase
    {
        public BackEndJobs()
        {

            try {
                //Configurations.BackEndJobLoadSettings();

                //Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.INFO, "WFMBackEndJob", "WFM.App.BackEndJob", "ServiceInitating", "Constructor", "", "Settings loaded.");
            }
            catch (Exception ex)
            {

                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMBackEndJob", "WFM.App.BackEndJob", "ServiceInitating", "Constructor", "", "Error on load settings :" + ex.InnerException.ToString());
            }


        }

        public void OnStart()
        {
            try {
                 //Task t1 = Task.Factory.StartNew(()=>
                 Task.Factory.StartNew(() =>
                 {
                    SchedulerManager objScheduler = new SchedulerManager();
                    objScheduler.StartAllSchedulers();
                
                }
               );
                //t1.Wait();
            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMBackEndJob", "WFM.App.BackEndJob", "ServiceInitating", "Constructor", "", "Error on Start : " + ex.InnerException.ToString());
            }
        }

        protected override void OnStop()
        {
        }


    }
}
