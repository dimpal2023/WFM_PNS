using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Configuration;
using System.Configuration.Assemblies;
using System.Collections.Specialized;


namespace Wfm.App.ConfigManager
{
    public static class Configurations
    {

        public static string SQLLogLevel { get; set; }
        public static string FileLogLevel { get; set; }
        public static string isFileLogEnabled { get; set; }
        public static string isSQLLogEnabled { get; set; }

        public static string BioMetricDevice_PNI { get; set; }
        public static string BioMetricDevice_KI { get; set; }
        public static bool IsBioMetricImportEnabled_PNI { get; set; }
        public static string ScheduleJobKey_BioMetricImport { get; set; }
        public static bool IsBioMetricImportEnabled_KI { get; set; }
        public static string ScheduleJobKey_SalaryGeneration { get; set; }

        public static string BioMetricDBLocation_PNI { get; set; }
        public static string BioMetricDBLocation_KI { get; set; }

        public static Guid CompanyId_PNI { get; set; }
        public static Guid CompanyId_KI { get; set; }

        public static bool IsBioMetricImportEnabled { get; set; }
        public static bool IsSalaryGenerationEnabled { get; set; }

        public static string SqlConnectionString { get; set; }

        private static Dictionary<string, string> appSettingDictionary;

        public static void LoadBackEndJobSettings()
        {
                var reader = new AppSettingsReader();
                appSettingDictionary = new Dictionary<string, string>();
                NameValueCollection appSettings = System.Configuration.ConfigurationManager.AppSettings;

                for (int i = 0; i < appSettings.Count; i++)
                {
                    string key = appSettings.GetKey(i);
                    string value = (string)reader.GetValue(key, typeof(string));
                    appSettingDictionary.Add(key, value);
                }

                BioMetricDevice_PNI = ReadAppSettings("BioMetricDevice_PNI");
                BioMetricDevice_KI = ReadAppSettings("BioMetricDevice_KI");
                IsBioMetricImportEnabled_PNI = Convert.ToBoolean(ReadAppSettings("IsBioMetricImportEnabled_PNI"));
                ScheduleJobKey_BioMetricImport = ReadAppSettings("ScheduleJobKey_BioMetricImport");
                IsBioMetricImportEnabled_KI = Convert.ToBoolean(ReadAppSettings("IsBioMetricImportEnabled_KI"));
                ScheduleJobKey_SalaryGeneration = ReadAppSettings("ScheduleJobKey_SalaryGeneration");
                BioMetricDBLocation_PNI = ReadAppSettings("BioMetricDBLocation_PNI");
                BioMetricDBLocation_KI = ReadAppSettings("BioMetricDBLocation_KI");
                CompanyId_PNI = Guid.Parse(ReadAppSettings("CompanyId_PNI"));
                CompanyId_KI = Guid.Parse(ReadAppSettings("CompanyId_KI"));
                IsBioMetricImportEnabled = Convert.ToBoolean(ReadAppSettings("IsBioMetricImportEnabled"));
                IsSalaryGenerationEnabled = Convert.ToBoolean(ReadAppSettings("IsSalaryGenerationEnabled"));
                SqlConnectionString = ReadAppSettings("SqlConnectionString");
                SQLLogLevel = ReadAppSettings("SQLLogLevel");
                FileLogLevel = ReadAppSettings("FileLogLevel");
                isSQLLogEnabled = ReadAppSettings("isSQLLogLevelEnabled");
                isFileLogEnabled = ReadAppSettings("isFileLogLevelEnabled");

        }

        public static void LoadAppConfg()
        {
         
            XElement appSettingSections = GetCongfigValue("App.config");

            var appsettings = from el in appSettingSections.Element("appSettings").DescendantsAndSelf("add")
                              select el;

            appSettingDictionary = new Dictionary<string, string>();
            foreach (XElement itemElement in appsettings)
            {
                appSettingDictionary.Add(itemElement.Attribute("key").Value, itemElement.Attribute("value").Value);
            }
        }

        public static void LoadSettings()
        {
            LoadWebConfg();

            SQLLogLevel = ReadAppSettings("SQLLogLevel");
            FileLogLevel = ReadAppSettings("FileLogLevel");
            isSQLLogEnabled = ReadAppSettings("isSQLLogLevelEnabled");
            isFileLogEnabled = ReadAppSettings("isFileLogLevelEnabled");
            SqlConnectionString  = ReadAppSettings("SqlConnectionString");
        }


        public static void LoadWebConfg()
        {
            XElement appSettingSections = GetCongfigValue("Web.config");
            
            var appsettings = from el in appSettingSections.Element("appSettings").DescendantsAndSelf("add")
                      select el;

            appSettingDictionary = new Dictionary<string, string>();
            foreach (XElement itemElement in appsettings)
            {
                appSettingDictionary.Add(itemElement.Attribute("key").Value, itemElement.Attribute("value").Value);
            }
        }

        private static XElement GetCongfigValue(string config)
        {
            FileStream configValues = null;
            string configPath = String.Empty;

            configPath = Path.Combine(System.Threading.Thread.GetDomain().SetupInformation.ApplicationBase, config);

            if (File.Exists(configPath))
            {
                configValues = File.Open(configPath, FileMode.Open, FileAccess.Read);
            }

            XDocument document = XDocument.Load(configValues);

            return document.Root;
        }
        private static string ReadAppSettings(string key)
        {
            try
            {
                return appSettingDictionary[key];
            }
            catch
            {
                return String.Empty;
            }
        }
    }
}
