using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Wfm.App.ConfigManager;

namespace Wfm.App.Scheduler
{
    public class SchedulerManager
    {

        public void StartAllSchedulers()
        {
            //Configurations.LoadBackEndJobSettings();

            if (Configurations.IsBioMetricImportEnabled)
            {
                BioMetricData objBio = new BioMetricData();

                //objBio.Initiate();

                startImportBioMatricDatascheduler("ImportBioMetricData","BioMetricImport","TriggerBioMetricImport", "TriggerBioMetricImportGroup");
            }

            if (Configurations.IsSalaryGenerationEnabled)
            {
                startGenerateSalarycheduler("GenerateSalaryData", "GenerateSalary", "TriggerGenerateSalaryImport", "TriggerGenerateSalaryGroup");
            }
        }

        private void startImportBioMatricDatascheduler(string JobName, string ScheduledJobGroup, string TriggerName, string TriggerGroup)
        {
            JobDetail importBioMatricData = null;
            importBioMatricData = new JobDetail(JobName, ScheduledJobGroup, typeof(JobScheduleManagerImportBioMetric));

            if (importBioMatricData != null)
            {
                string counter = Configurations.ScheduleJobKey_BioMetricImport;
                var jobTrigger = new CronTrigger(TriggerName,TriggerGroup,counter);
                var schedulerfactory = new StdSchedulerFactory();
                var scheduler = schedulerfactory.GetScheduler();
                scheduler.ScheduleJob(importBioMatricData, jobTrigger);
                scheduler.Start();
                var executingJobs = scheduler.IsStarted;
            }
        }

        private void startGenerateSalarycheduler(string JobName, string ScheduledJobGroup, string TriggerName, string TriggerGroup)
        {
            JobDetail generateSalary = null;
            generateSalary = new JobDetail(JobName, ScheduledJobGroup, typeof(JobScheduleManagerGenerateSalary));

            if (generateSalary != null)
            {
                string counter = Configurations.ScheduleJobKey_SalaryGeneration;
                var jobTrigger = new CronTrigger(TriggerName, TriggerGroup, counter);
                var schedulerfactory = new StdSchedulerFactory();
                var scheduler = schedulerfactory.GetScheduler();
                scheduler.ScheduleJob(generateSalary, jobTrigger);
                scheduler.Start();
            }
        }
    }
}
