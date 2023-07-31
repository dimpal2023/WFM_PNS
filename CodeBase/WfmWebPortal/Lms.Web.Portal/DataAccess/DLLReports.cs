using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Lms.Web.Portal.Models;
using System.Web.Services;
using Newtonsoft.Json;
using Wfm.App.Common;

namespace Lms.Web.Portal.DataAccess
{
    public class DLLReports
    {
        public DataSet ExecuteProcedureReturnString1(string procName, string BUILDING_ID, string DEPT_ID, string SUBDEPT_ID, int WF_EMP_TYPE, string EMP_NAME, int MONTH_ID, int YEAR_ID, params SqlParameter[] paramters)
        {
            Guid UserID=  SessionHelper.Get<Guid>("UserId");
            string constr = ConfigurationManager.ConnectionStrings["db_con"].ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(procName))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BUILDING_ID", BUILDING_ID);
                    cmd.Parameters.AddWithValue("@DEPT", DEPT_ID);
                    cmd.Parameters.AddWithValue("@SUB_DEPT", SUBDEPT_ID);
                    cmd.Parameters.AddWithValue("@WF_EMP_TYPE", WF_EMP_TYPE);
                    cmd.Parameters.AddWithValue("@EMP_NAME", EMP_NAME);
                    cmd.Parameters.AddWithValue("@MONTH_ID", MONTH_ID);
                    cmd.Parameters.AddWithValue("@YEAR_ID", YEAR_ID);
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@OpCode", "41");
                    
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(ds);
                    }

                    return ds;
                }
            }
        }


        public string ExecuteProcedureReturnString(string procName, string BUILDING_ID, string DEPT_ID, string SUBDEPT_ID, int WF_EMP_TYPE, string EMP_NAME, int MONTH_ID, int YEAR_ID, int EMPLOYMENT_TYPE, params SqlParameter[] paramters)
        {
            Guid UserID = SessionHelper.Get<Guid>("UserId");
            string constr = ConfigurationManager.ConnectionStrings["db_con"].ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(procName))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BUILDING_ID", BUILDING_ID);
                    cmd.Parameters.AddWithValue("@DEPT", DEPT_ID);
                    cmd.Parameters.AddWithValue("@SUB_DEPT", SUBDEPT_ID);
                    cmd.Parameters.AddWithValue("@WF_EMP_TYPE", WF_EMP_TYPE);
                    cmd.Parameters.AddWithValue("@EMP_NAME", EMP_NAME);
                    cmd.Parameters.AddWithValue("@MONTH_ID", MONTH_ID);
                    cmd.Parameters.AddWithValue("@YEAR_ID", YEAR_ID);
                    cmd.Parameters.AddWithValue("@EMPLOYMENT_TYPE", EMPLOYMENT_TYPE);
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@OpCode", "42");


                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandTimeout = 1000;
                        sda.Fill(ds);
                    }
                    string json = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        json += dr[0];
                    }

                    return json;
                    //return JsonConvert.SerializeObject(ds);
                }
            }
        }

        public string Get_MasterSalary(string procName, string BUILDING_ID, string DEPT_ID, string SUBDEPT_ID, int WF_EMP_TYPE, int MONTH_ID, int YEAR_ID, int EMPLOYMENT_TYPE, params SqlParameter[] paramters)
        {
            Guid UserID = SessionHelper.Get<Guid>("UserId");
            string constr = ConfigurationManager.ConnectionStrings["db_con"].ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(procName))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BUILDING_ID", BUILDING_ID);
                    cmd.Parameters.AddWithValue("@DEPT", DEPT_ID);
                    cmd.Parameters.AddWithValue("@SUB_DEPT", SUBDEPT_ID);
                    cmd.Parameters.AddWithValue("@WF_EMP_TYPE", WF_EMP_TYPE);
                    cmd.Parameters.AddWithValue("@MONTH_ID", MONTH_ID);
                    cmd.Parameters.AddWithValue("@YEAR_ID", YEAR_ID);
                    cmd.Parameters.AddWithValue("@EMPLOYMENT_TYPE", EMPLOYMENT_TYPE);
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@OpCode", "41");
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandTimeout = 1000;
                        sda.Fill(ds);
                    }
                    string json = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        json += dr[0];
                    }
                    return json;
                }
            }
        }


        public string BindEmployeeList(string procName, string deptId, string sub_dept_id, string BUILDING_ID, int SHIFT_ID)
        {
            Guid UserID = SessionHelper.Get<Guid>("UserId");
            string constr = ConfigurationManager.ConnectionStrings["db_con"].ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand(procName, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DEPT", deptId);
                cmd.Parameters.AddWithValue("@SUB_DEPT", sub_dept_id);
                cmd.Parameters.AddWithValue("@BUILDING_ID", BUILDING_ID);
                cmd.Parameters.AddWithValue("@SHIFT_ID", SHIFT_ID);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
            }
            return JsonConvert.SerializeObject(ds);
        }



        public DataSet GetBasicSalary(string procName, string WF_EMP_TYPE, string SKILL_ID, string BASIC_SALARY, string PERCENTAGE, string WEF, string OPcode)
        {
            Guid UserID = SessionHelper.Get<Guid>("UserId");
            string constr = ConfigurationManager.ConnectionStrings["db_con"].ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand(procName, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@WF_EMP_TYPE", WF_EMP_TYPE);
                cmd.Parameters.AddWithValue("@SKILL_ID", SKILL_ID);
                cmd.Parameters.AddWithValue("@BASIC_SALARY", BASIC_SALARY);
                cmd.Parameters.AddWithValue("@PERCENTAGE", PERCENTAGE);
                cmd.Parameters.AddWithValue("@WEF", WEF);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@Opcode", OPcode);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
            }
            return ds;
        }

        public string GetDailyAttendanceReport(string procName, string BUILDING_ID, string DEPT_ID, string SUBDEPT_ID, string WF_EMP_TYPE, string EMP_NAME, string SHIFT_ID, DateTime Date, string EMPLOYMENT_TYPE, params SqlParameter[] paramters)
        {
            Guid UserID = SessionHelper.Get<Guid>("UserId");
            string constr = ConfigurationManager.ConnectionStrings["db_con"].ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(procName))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BUILDING_ID", BUILDING_ID);
                    cmd.Parameters.AddWithValue("@DEPT", DEPT_ID);
                    cmd.Parameters.AddWithValue("@SUB_DEPT", SUBDEPT_ID);
                    cmd.Parameters.AddWithValue("@WF_EMP_TYPE", WF_EMP_TYPE);
                    cmd.Parameters.AddWithValue("@EMP_NAME", EMP_NAME);
                    cmd.Parameters.AddWithValue("@SHIFT_ID", SHIFT_ID);
                    cmd.Parameters.AddWithValue("@Date", Date);
                    cmd.Parameters.AddWithValue("@EMPLOYMENT_TYPE", EMPLOYMENT_TYPE);
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@OpCode", "41");


                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandTimeout = 1000;
                        sda.Fill(ds);
                    }
                    string json = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        json += dr[0];
                    }

                    return json;
                    //return JsonConvert.SerializeObject(ds);
                }
            }
        }


        public string GetYearAttendanceReport(string procName, string BUILDING_ID, string DEPT_ID, string SUBDEPT_ID, string WF_EMP_TYPE, string EMP_NAME, int YEAR_ID, string EMPLOYMENT_TYPE, params SqlParameter[] paramters)
        {
            Guid UserID = SessionHelper.Get<Guid>("UserId");
            string constr = ConfigurationManager.ConnectionStrings["db_con"].ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(procName))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BUILDING_ID", BUILDING_ID);
                    cmd.Parameters.AddWithValue("@DEPT", DEPT_ID);
                    cmd.Parameters.AddWithValue("@SUB_DEPT", SUBDEPT_ID);
                    cmd.Parameters.AddWithValue("@WF_EMP_TYPE", WF_EMP_TYPE);
                    cmd.Parameters.AddWithValue("@EMP_NAME", EMP_NAME);
                    cmd.Parameters.AddWithValue("@YEAR_ID", YEAR_ID);
                    cmd.Parameters.AddWithValue("@EMPLOYMENT_TYPE", EMPLOYMENT_TYPE);
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@OpCode", "41");


                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandTimeout = 1000;
                        sda.Fill(ds);
                    }
                    string json = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        json += dr[0];
                    }

                    return json;
                    //return JsonConvert.SerializeObject(ds);
                }
            }
        }

        public string GetReport(string procName, string BUILDING_ID, string DEPT_ID, string SUBDEPT_ID, int WF_EMP_TYPE, string EMP_NAME, DateTime FROM_DATE, DateTime TO_DATE, int EMPLOYMENT_TYPE, params SqlParameter[] paramters)
        {
            Guid UserID = SessionHelper.Get<Guid>("UserId");
            string constr = ConfigurationManager.ConnectionStrings["db_con"].ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(procName))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BUILDING_ID", BUILDING_ID);
                    cmd.Parameters.AddWithValue("@DEPT", DEPT_ID);
                    cmd.Parameters.AddWithValue("@SUB_DEPT", SUBDEPT_ID);
                    cmd.Parameters.AddWithValue("@WF_EMP_TYPE", WF_EMP_TYPE);
                    cmd.Parameters.AddWithValue("@EMP_NAME", EMP_NAME);
                    cmd.Parameters.AddWithValue("@FROM_DATE", FROM_DATE);
                    cmd.Parameters.AddWithValue("@TO_DATE", TO_DATE);
                    cmd.Parameters.AddWithValue("@EMPLOYMENT_TYPE", EMPLOYMENT_TYPE);
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@OpCode", "41");


                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandTimeout = 1000;
                        sda.Fill(ds);
                    }
                    string json = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        json += dr[0];
                    }
                    return json;
                }
            }
        }

        public string Retirement_Data(string procName, string BUILDING_ID, string DEPT_ID, string SUBDEPT_ID)
        {
            Guid UserID = SessionHelper.Get<Guid>("UserId");
            string constr = ConfigurationManager.ConnectionStrings["db_con"].ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(procName))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BUILDING_ID", BUILDING_ID);
                    cmd.Parameters.AddWithValue("@DEPT", DEPT_ID);
                    cmd.Parameters.AddWithValue("@SUB_DEPT", SUBDEPT_ID);
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@OpCode", "42");
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandTimeout = 1000;
                        sda.Fill(ds);
                    }
                    string json = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        json += dr[0];
                    }
                    return json;
                }
            }
        }

    }
}