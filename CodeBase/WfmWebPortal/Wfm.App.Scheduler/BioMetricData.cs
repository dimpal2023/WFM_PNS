using System;
using System.Data.OleDb;
using Wfm.App.ConfigManager;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Wfm.App.Scheduler
{
    public class BioMetricData
    {
        public void Initiate()
        {
            if (Configurations.IsBioMetricImportEnabled_PNI)
            {
                ImportData(Configurations.BioMetricDevice_PNI, Configurations.BioMetricDBLocation_PNI, Configurations.CompanyId_PNI);
            }

            if (Configurations.IsBioMetricImportEnabled_KI)
            {
                ImportData(Configurations.BioMetricDevice_KI, Configurations.BioMetricDBLocation_KI, Configurations.CompanyId_KI);
            }
        }

        private void ImportData(string deviceids, string dblocation, Guid companyid)
        {
            string fullpath = string.Empty;
            try
            {
                //DirectoryInfo d = new DirectoryInfo(@"D:\OneDrive\Projects\WFM\MockUp\");//Assuming Test is your Folder
                //DirectoryInfo d = new DirectoryInfo(ConfigManager.Configurations.BioMetricDBLocation_PNI);

                //DateTime startdate;
                //DateTime enddate;
                DateTime nextstartdate = new DateTime();
                DateTime nextenddate = new DateTime();


                List<RunDetail> lastrun = new List<RunDetail>();
                lastrun = GetLastRunDetail();

                if (lastrun == null)
                {
                    Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMBackEndJob", "Wfm.App.Scheduler", "BioMetricData", "ImportData", "", "Last Run detail not found.");
                    return;
                }
                else
                {
                    RunDetail lastrd = new RunDetail();
                    lastrd = lastrun.FirstOrDefault();
                    nextstartdate = lastrd.TO_DATE.AddDays(1);
                    nextenddate = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                    DirectoryInfo d = new DirectoryInfo(dblocation);
                    foreach (RunDetail rd in lastrun)
                    {
                        FileInfo[] Files = d.GetFiles(rd.MDBFILENAME + ".mdb"); //Getting Text files

                        if (Files.Length > 0)
                        {
                            foreach (FileInfo file in Files)
                            {
                                fullpath = file.FullName;
                                bool isimportsuccess = true;
                                string accessconnectionstring = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + fullpath + ";";
                                try
                                {
                                    OleDbConnection con = new OleDbConnection(accessconnectionstring);
                                    OleDbCommand cmd = con.CreateCommand();
                                    con.Open();

                                    //cmd.CommandText = "SELECT AL.AttendanceDate, AL.EmployeeId,EM.NumericCode, AL.InTime,AL.OutTime,AL.PunchRecords, AL.InDeviceId, AL.StatusCode, SH.ShiftId,SH.BeginTime,SH.EndTime,SH.Break1BeginTime,SH.Break1EndTime,AttendanceLogId,"+ rd.MDBFILEID +" FROM Shifts SH, EmployeeShift ES, Employees EM, AttendanceLogs AL WHERE AL.EmployeeId = EM.EmployeeId AND EM.EmployeeId = ES.EmployeeId AND SH.ShiftId = AL.ShiftId AND (((AL.AttendanceDate) >=#" + nextstartdate + "#)) AND (((AL.AttendanceDate)<#" + nextenddate + "#))" +
                                    //    "AND ES.FromDate = (Select max(FromDate) from EmployeeShift where EmployeeId = ES.EmployeeId) order by AL.EmployeeId,AL.AttendanceDate;";
                                    cmd.CommandText = "SELECT AL.AttendanceDate, AL.EmployeeId,(Select NumericCode from Employees where EmployeeId=AL.EmployeeId) as NumericCode, AL.InTime,AL.OutTime,AL.PunchRecords, AL.InDeviceId, AL.StatusCode, SH.ShiftId,SH.BeginTime,SH.EndTime,SH.Break1BeginTime,SH.Break1EndTime,AttendanceLogId," + rd.MDBFILEID + " FROM Shifts SH, AttendanceLogs AL WHERE SH.ShiftId = AL.ShiftId AND (((AL.AttendanceDate) >=#" + nextstartdate + "#)) AND (((AL.AttendanceDate)<#" + nextenddate + "#))  AND AL.EmployeeId not in (Select EmployeeId from Employees where EmployeeName like 'del_*') order by AL.EmployeeId,AL.AttendanceDate;";

                                    cmd.Connection = con;
                                    cmd.CommandTimeout = 10000;
                                    System.Data.OleDb.OleDbDataReader dr = cmd.ExecuteReader();
                                    DataTable dt = new DataTable();
                                    dt.Load(dr);
                                    dr.Close();
                                    con.Close();

                                    if (dt.Rows.Count > 0)
                                    {
                                        try
                                        {

                                            //List<int> EmpIDList = new List<int>();
                                            //dt.AsEnumerable().Select(s => s.Field<int>("EmployeeId")).ToList().ForEach(c => EmpIDList.Add(c));

                                            //foreach (int emp in EmpIDList.Distinct())
                                            //{
                                            //    try
                                            //    {
                                            //        var dtEmp = from myRow in dt.AsEnumerable()
                                            //                    where myRow.Field<int>("EmployeeId") == emp
                                            //                    select myRow;

                                            //        DataTable dtChunk = dt.Clone();
                                            //        foreach (DataRow dtRow in dtEmp)
                                            //        {
                                            //            dtChunk.Rows.Add(dtRow.ItemArray);
                                            //        }
                                            //        SqlConnection objSqlConnection = new SqlConnection(Configurations.SqlConnectionString);
                                            //        int result = SqlHelper.ExecuteNonQuery(objSqlConnection, CommandType.StoredProcedure, "InsertBioMetricTable", new SqlParameter("@biometrictable", dtChunk));
                                            //        objSqlConnection.Close();
                                            //    }
                                            //    catch (Exception ex)
                                            //    {
                                            //        Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMBackEndJob", "Wfm.App.Scheduler", "BioMetricData", "Excute", "", "Error in Batch Attendance import of Database :" + rd.MDBFILENAME + " --- Employee Id ---- " + emp.ToString() + " ----- Exception : " + ex.ToString());
                                            //    }
                                            //}





                                            int counter = 0;
                                            while (counter < dt.Rows.Count - 1)
                                            {
                                                try
                                                {
                                                    int batchsize = 10;
                                                    if ((dt.Rows.Count - counter) < 10)
                                                    {
                                                        batchsize = dt.Rows.Count - counter;
                                                    }

                                                    int subcounter = 0;
                                                    DataTable dtChunk = dt.Clone();
                                                    while (subcounter < batchsize)
                                                    {
                                                        DataRow drow = dt.Rows[counter];

                                                        dtChunk.Rows.Add(drow.ItemArray);
                                                        counter = counter + 1;
                                                        subcounter = subcounter + 1;
                                                    }

                                                    SqlConnection objSqlConnection = new SqlConnection(Configurations.SqlConnectionString);
                                                    int result = SqlHelper.ExecuteNonQuery(objSqlConnection, CommandType.StoredProcedure, "InsertBioMetricTable", new SqlParameter("@biometrictable", dtChunk));
                                                    objSqlConnection.Close();
                                                }
                                                catch (Exception ex)
                                                {
                                                    Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMBackEndJob", "Wfm.App.Scheduler", "BioMetricData", "Excute", "", "Error in Batch Attendance import of Database :" + rd.MDBFILENAME + " --- Counter ---- " + counter + " ----- Exception : " + ex.ToString());
                                                }

                                                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.INFO, "WFMBackEndJob", "Wfm.App.Scheduler", "BioMetricData", "Excute", "", "Batch imported Successfully for " + rd.MDBFILENAME + " - Counter : " + counter);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            isimportsuccess = false;
                                            Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMBackEndJob", "Wfm.App.Scheduler", "BioMetricData", "Excute", "", "Error in Batch Attendance import from :" + fullpath + " --- Exception " + ex.ToString());
                                        }
                                    }
                                    else
                                    {
                                        Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.INFO, "WFMBackEndJob", "Wfm.App.Scheduler", "BioMetricData", "Excute", "", "No Record found for " + rd.MDBFILENAME);
                                    }

                                    if (isimportsuccess)
                                    {
                                        Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.INFO, "WFMBackEndJob", "Wfm.App.Scheduler", "BioMetricData", "Excute", "", "Data Imported successfully from DB : " + rd.MDBFILENAME);
                                        //Int64 maxattlogid = 0;

                                        //DataRow datarow = dt.Rows[dt.Rows.Count - 1];
                                        //maxattlogid = Convert.ToInt64(datarow["AttendanceLogId"]);


                                        //bool lastruninsert = InsertLastRunDetail(rd.MDBFILEID, maxattlogid);
                                        //if (!lastruninsert)
                                        //{
                                        //    Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMBackEndJob", "Wfm.App.Scheduler", "BioMetricData", "Excute", "", "Error in inserting Last Run Dates");
                                        //}
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMBackEndJob", "Wfm.App.Scheduler", "BioMetricData", "Excute", "", "Error in importing data File Name:" + fullpath + " : Exception : " + ex.ToString());
                                }
                            }
                        }
                        else
                        {
                            Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.INFO, "WFMBackEndJob", "Wfm.App.Scheduler", "BioMetricData", "Excute", "", "File not found. - " + rd.MDBFILENAME);
                        }
                    }
                }

                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.INFO, "WFMBackEndJob", "Wfm.App.Scheduler", "BioMetricData", "Excute", "", "Import Completed.");
            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMBackEndJob", "Wfm.App.Scheduler", "BioMetricData", "Excute", "", "Error in importing data from " + fullpath + "######" + ex.ToString());
            }
        }

        public List<RunDetail> GetLastRunDetail()
        {
            List<RunDetail> lstRunDetails = new List<RunDetail>();
            try
            {
                DataSet ds = new DataSet("lastrun");
                using (SqlConnection conn = new SqlConnection(Configurations.SqlConnectionString))
                {
                    SqlCommand sqlComm = new SqlCommand("Select RunDate,MDBFILEID,MDBFILENAME,FROM_DATE,TO_DATE from TAB_BIOMETRIC_IMPORTHISTORY bi JOIN TAB_BIOMETRIC_DATABASE bd on bi.MDBFILEID=bd.ID where RunDate = (Select max(RunDate) from TAB_BIOMETRIC_IMPORTHISTORY where MDBFILEID=bi.MDBFILEID)", conn);
                    //SqlCommand sqlComm = new SqlCommand("Select RunDate, MDBFILEID, MDBFILENAME, Max_AttendanceLogId from TAB_BIOMETRIC_IMPORTHISTORY im join TAB_BIOMETRIC_DATABASE db on im.MDBFILEID = db.ID where RunDate = (Select max(RunDate) from TAB_BIOMETRIC_IMPORTHISTORY where MDBFILEID = db.ID)", conn);
                    sqlComm.CommandType = CommandType.Text;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;
                    da.Fill(ds);

                    if (ds != null)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            RunDetail lastrun = new RunDetail();
                            lastrun.RunDate = Convert.ToDateTime(dr["RunDate"]);
                            lastrun.MDBFILEID = Convert.ToInt16(dr["MDBFILEID"]);
                            lastrun.MDBFILENAME = Convert.ToString(dr["MDBFILENAME"]);
                            lastrun.FROM_DATE = Convert.ToDateTime(dr["FROM_DATE"]);
                            lastrun.TO_DATE = Convert.ToDateTime(dr["TO_DATE"]);

                            lstRunDetails.Add(lastrun);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMBackEndJob", "Wfm.App.Scheduler", "BioMetricData", "GetLastRunDetail", "", "SQL Error : " + ex.InnerException.ToString());
            }
            catch (Exception e)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMBackEndJob", "Wfm.App.Scheduler", "BioMetricData", "GetLastRunDetail", "", e.InnerException.ToString());
            }
            return lstRunDetails;
        }

        public bool InsertLastRunDetail(int mdbfileid, Int64 maxattlogid)
        {
            bool flag = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(Configurations.SqlConnectionString))
                {
                    conn.Open();
                    SqlCommand sqlComm = new SqlCommand("Insert into TAB_BIOMETRIC_IMPORTHISTORY (RunDate,MDBFILEID,Max_AttendanceLogId) values ('" + DateTime.Now + "'," + mdbfileid + "," + maxattlogid + ") ", conn);
                    sqlComm.CommandType = CommandType.Text;
                    sqlComm.Connection = conn;
                    sqlComm.ExecuteNonQuery();
                    flag = true;
                }
            }
            catch (SqlException ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMBackEndJob", "Wfm.App.Scheduler", "BioMetricData", "GetLastRunDetail", "", "SQL Error : " + ex.InnerException.ToString());
            }
            catch (Exception e)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMBackEndJob", "Wfm.App.Scheduler", "BioMetricData", "GetLastRunDetail", "", e.InnerException.ToString());
            }
            return flag;
        }
    }

    public class RunDetail
    {
        public DateTime RunDate { get; set; }
        public int MDBFILEID { get; set; }
        public String MDBFILENAME { get; set; }
        public DateTime FROM_DATE { get; set; }
        public DateTime TO_DATE { get; set; }
    }
}