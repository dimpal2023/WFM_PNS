using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using Wfm.App.ConfigManager;
using System.Data;
using Wfm.App.Core.Model;
using System.Data.SqlClient;

namespace Wfm.App.Scheduler
{
    public class SalaryGeneration
    {
        public void Initiate()
        {
            if (Configurations.IsBioMetricImportEnabled_PNI)
            {
                GenerateSalary(Configurations.CompanyId_PNI);
            }

            if (Configurations.IsBioMetricImportEnabled_KI)
            {
                GenerateSalary(Configurations.CompanyId_KI);
            }
        }

        private void GenerateSalary(Guid companyid)
        {
            try
            {
                var deviceidArr = String.Empty;

                // Enrolled Daily Wages Generation Logic

                // Step 1: Pull Active Salaried Emplyees
                // Loop
                // Step 2: Pull Attendance of an Employee of the Last Month
                // Step 3: Pull No. of Absent Days
                // Step 4: 
                // Step 5: 

            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMBackEndJob", "Wfm.App.Scheduler", "GenerateSalary", "Excute", "", "Error in generating Salary :" + ex.InnerException.ToString());
            }
        }

        private List<WorkforceMasterMetaData> GetSalariedEmpList(Guid companyid)
        {
            List<WorkforceMasterMetaData> lstEmp = new List<WorkforceMasterMetaData>();
            //try
            //{
            //    DataSet ds = new DataSet("MRFList");
            //    using (SqlConnection conn = new SqlConnection(Configurations.SqlConnectionString))
            //    {
            //        SqlCommand sqlComm = new SqlCommand("GetSalariedEmpList", conn);
            //        sqlComm.Parameters.AddWithValue("@COMPANYID", companyid);
            //        sqlComm.CommandType = CommandType.StoredProcedure;

            //        SqlDataAdapter da = new SqlDataAdapter();
            //        da.SelectCommand = sqlComm;
            //        da.Fill(ds);

            //        if (ds != null)
            //        {
            //            foreach (DataRow dr in ds.Tables[0].Rows)
            //            {
            //                ManPowerRequestFormMetaDataList objMRF = new ManPowerRequestFormMetaDataList();
            //                objMRF.MRF_ID = Convert.ToString(dr["MRF_ID"]);
            //                objMRF.MRP_INETRNAL_ID = Guid.Parse(Convert.ToString(dr["MRP_INETRNAL_ID"]));
            //                objMRF.COMPANY_ID = Guid.Parse(Convert.ToString(dr["COMPANY_ID"]));
            //                objMRF.BUILDING_ID = Guid.Parse(Convert.ToString(dr["BUILDING_ID"]));
            //                objMRF.FLOOR_ID = Guid.Parse(Convert.ToString(dr["FLOOR_ID"]));
            //                objMRF.SKILL_ID = Guid.Parse(Convert.ToString(dr["SKILL_ID"]));
            //                objMRF.WF_DESIGNATION_ID = Convert.ToInt16(dr["WF_DESIGNATION_ID"]);
            //                objMRF.MRF_STATUS = Convert.ToString(dr["MRF_STATUS"]);
            //                objMRF.QUANTITY = Convert.ToInt16(dr["WF_QTY"]);
            //                objMRF.WF_EMP_TYPE = Convert.ToInt16(dr["WF_EMP_TYPE"]);
            //                lstEmp.Add(objMRF);
            //            }
            //        }
            //    }
            //}
            //catch (SqlException ex)
            //{
            //    Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "WorkforceReporsitory", "GetMRFList", "", "SQL Error : " + ex.InnerException.ToString());
            //}
            //catch (Exception e)
            //{
            //    Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "WorkforceReporsitory", "GetMRFList", "", e.InnerException.ToString());
            //}


            return lstEmp;
        }
    }
}
