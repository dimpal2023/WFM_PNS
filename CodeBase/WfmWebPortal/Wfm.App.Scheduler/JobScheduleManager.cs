using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace Wfm.App.Scheduler
{
    public class JobScheduleManagerImportBioMetric : IJob
    {

        readonly Notification objNotification = new Notification();

        public void Execute(JobExecutionContext context)
        {
            try
            {
                BioMetricData objBio = new BioMetricData();
                objBio.Initiate();
            }
            catch (JobExecutionException jex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMBackEndJob", "WFM.App.BackEndJob", "JobScheduler", "Excute - ImportBioMetric", "", "Error in executing Job :" + jex.InnerException.ToString());
            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMBackEndJob", "WFM.App.BackEndJob", "JobScheduler", "Excute - ImportBioMetric", "", "Error in Job :" + ex.ToString());
            }
        }
        class Notification
        {
        // Send Email Notification once job is finished.
        }
    }

    public class JobScheduleManagerGenerateSalary : IJob
    {

        readonly Notification objNotification = new Notification();

        public void Execute(JobExecutionContext context)
        {
            try
            {
                SalaryGeneration objSal = new SalaryGeneration();
                objSal.Initiate();
            }
            catch (JobExecutionException jex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMBackEndJob", "WFM.App.BackEndJob", "JobScheduler", "Excute - Salary", "", "Error in executing Job :" + jex.InnerException.ToString());
            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMBackEndJob", "WFM.App.BackEndJob", "JobScheduler", "Excute - Salary", "", "Error in Job :" + ex.InnerException.ToString());
            }
        }
        class Notification
        {
            // Send Email Notification once job is finished.
        }
    }
}
