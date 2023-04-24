using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Net.Mail;
using System.Data.OleDb;

namespace Biometric
{
    class Program
    {
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger
        (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
   
        static void Main(string[] args)
        {
            string compid = args[0].ToString();
          
            log.Info("==============================Biometric Process Started==============================");

            Biometric_Upload(compid);

            log.Info("==============================Biometric Process Ended==============================");

            //GridView1.DataSource = ds;
            //GridView1.DataBind();
        }

        static void Biometric_Upload(string companyid)
        {
            
           
            //bool flag = false;
            string strMdbFileName = "";
            string strfilelocation = "";
            string fullpath = "";
            DateTime strStart_Date = new DateTime();
            DateTime strEnd_date =  new DateTime();
            string connectionString_Access = "";
            string mdbid;
            
            try
            {
                
                DataTable dt = GetData(companyid);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        try
                        {
                            
                            log.Info("Data Fetching start For " + dr["MDBFILENAME"].ToString() + " &  " + dr["COMPANY_ID"].ToString() + " from  " + dr["file_location"].ToString() + " Started");

                            strMdbFileName = dr["MDBFILENAME"].ToString() + ".mdb";
                            strfilelocation = dr["file_location"].ToString();
                            strStart_Date = Convert.ToDateTime(Convert.ToDateTime(dr["start_date"]).ToShortDateString());
                            //DateTime final_start_date = strStart_Date.ToShortDateString();
                            strEnd_date = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                            mdbid= dr["id"].ToString(); ;
                            fullpath = strfilelocation + strMdbFileName;

                            Console.WriteLine(System.DateTime.Now + " Biometric Process started for " + strMdbFileName);
                            int i = DeleteData(mdbid);

                            if (i > 0)
                            {
                                connectionString_Access = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + fullpath + ";";

                                using (OleDbConnection sourceConnection = new OleDbConnection(connectionString_Access))
                                {
                                    sourceConnection.Open();
                                    OleDbCommand commandRowCount = new OleDbCommand("SELECT COUNT(*) FROM AttendanceLogs where AttendanceDate>= #" + strStart_Date + "#;", sourceConnection);
                                    long countStart = System.Convert.ToInt32(commandRowCount.ExecuteScalar());
                                    log.Info("Data Records to be fetched " + countStart + "  in DB");
                                    Console.WriteLine(System.DateTime.Now + " Starting row count = {0}", countStart);

                                    OleDbCommand commandSourceData = new OleDbCommand("SELECT AL.AttendanceDate, AL.EmployeeId,(Select NumericCode from Employees where EmployeeId = AL.EmployeeId) as NumericCode, AL.InTime,AL.OutTime,AL.PunchRecords, AL.InDeviceId, AL.StatusCode, SH.ShiftId,SH.BeginTime,SH.EndTime,SH.Break1BeginTime,SH.Break1EndTime,AttendanceLogId," + mdbid + ",Date () FROM Shifts SH, AttendanceLogs AL WHERE SH.ShiftId = AL.ShiftId AND(((AL.AttendanceDate) >=#" + strStart_Date + "#)) AND (((AL.AttendanceDate)<#" + strEnd_date + "#))  AND AL.EmployeeId not in (Select EmployeeId from Employees where EmployeeName like 'del_*') order by AL.EmployeeId,AL.AttendanceDate;", sourceConnection);

                                    OleDbDataReader reader = commandSourceData.ExecuteReader();
                                    DataTable dt1 = reader.GetSchemaTable();
                                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["ConStr"].ToString()))
                                    {
                                        bulkCopy.BulkCopyTimeout = 10000;
                                        bulkCopy.DestinationTableName = "dbo.TAB_TMP_BIOMETRIC_DATA";

                                        try
                                        {
                                            bulkCopy.WriteToServer(reader);
                                            log.Info("Data inserted from Access DB to Sql Server temp table for " + strMdbFileName);
                                            Console.WriteLine(System.DateTime.Now + " Data inserted from Access DB to Sql Server temp table for " + strMdbFileName);
                                            reader.Close();

                                            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr"].ToString());
                                            try
                                            {
                                                //SqlCommand cmd = new SqlCommand("DELETE FROM TAB_TMP_BIOMETRIC_DATA where dbid=" + Dbid + ";", con);
                                                SqlCommand cmd_exec = new SqlCommand("dbo.USP_InsertBioMetricData", con);

                                                //cmd_exec.Parameters.Add("@MBID", SqlDbType.VarChar).Value = mdbid;
                                                //cmd_exec = new SqlCommand("dbo.USP_InsertBioMetricData", con);
                                                cmd_exec.CommandType = CommandType.StoredProcedure;
                                                SqlParameter Param = new SqlParameter("@MBID", mdbid);
                                                Param.Direction = ParameterDirection.Input;
                                                Param.DbType = DbType.String;
                                                cmd_exec.Parameters.Add(Param);
                                                log.Info("Procedure execution started.moving data to main table for " + strMdbFileName);
                                                Console.WriteLine(System.DateTime.Now + " Procedure execution started.moving data to main table for " + strMdbFileName);
                                                con.Open();
                                                cmd_exec.CommandTimeout = 0;
                                                cmd_exec.ExecuteNonQuery();
                                                con.Close();
                                                log.Info("Procedure execution Completed.Data moved to main table for " + strMdbFileName);
                                                Console.WriteLine(System.DateTime.Now + " Procedure execution Completed.Data moved to main table for " + strMdbFileName);
                                                Console.WriteLine(System.DateTime.Now + " Biometric Process Completed for " + strMdbFileName);

                                            }
                                            catch (Exception ex)
                                            {
                                                log.Error("Error while executing [USP_InsertBioMetricData]  " + ex.Message);

                                                Console.WriteLine(System.DateTime.Now + " -- "+ex.Message);
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            log.Error("Error  " + ex.Message);
                                            Console.WriteLine(ex.Message);
                                        }
                                        finally
                                        {
                                            reader.Close();
                                        }
                                    }

                                }
                            }
                            else
                            {
                                Console.WriteLine(System.DateTime.Now + " Process failed as application not able to delete temp data");
                                log.Info("Error -- Process failed as application not able to delete temp data");
                            }
                        }
                        catch (Exception ex)
                        {
                            // Exception logs here
                            log.Error("Error  " + ex.Message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                // exception logging
                log.Error("Error Fetching Data From DB : Exception - " + ex.Message);
                Console.WriteLine(System.DateTime.Now + " Error Fetching Data From DB", strMdbFileName);
            }
            //return flag;
        }

         static DataTable GetData(string companyid)
        {
            DataTable dt;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr"].ToString());
            SqlDataAdapter da;
            //DataSet ds;

            string qry = "select * from tab_biometric_database tbd,tab_bioletric_start_end_date tbsed where tbd.id=tbsed.mdbid and STATUS='Y' and company_id='"+ companyid + "';";
            da = new SqlDataAdapter(qry, con);
            dt = new DataTable();
            da.Fill(dt);
            log.Info("Fetched Biometric details From DB Sucessfully");
            return dt;
        }

    
        static int DeleteData(string Dbid)
        {
            int Updated = 0;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr"].ToString());
            try
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM TAB_TMP_BIOMETRIC_DATA where dbid=" + Dbid + ";", con);

                con.Open();

                cmd.ExecuteNonQuery();

                Updated = 1;
                log.Info("Deleted data from temp table for DBID " + Dbid);
                Console.WriteLine(System.DateTime.Now+" Deleted data from temp table for DBID "+ Dbid);
            }
            catch (Exception ex)
            {
                log.Error("Error while Deleting Data For " + Dbid + " : Exception " + ex.Message);
                Console.WriteLine(System.DateTime.Now + " Error while Deleting Data from temp table for DBID "+ Dbid);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            return Updated;
        }
    }
}
