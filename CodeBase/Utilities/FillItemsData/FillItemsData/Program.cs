using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Configuration;
using System.Data.SqlClient;

namespace FillItemsData
{
    class Program
    {
        static void Main(string[] args)
        {
            string emplayeeMatserExcelPath = @"C:\Users\hp\Downloads\Turning Shop.xlsx";
            string connectionString = ConfigurationManager.ConnectionStrings["ConStr"].ToString();

            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            Console.WriteLine("Loading excel sheet....");
            Application xlApp = new Application();
            Workbook xlWorkBook = xlApp.Workbooks.Open(emplayeeMatserExcelPath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            Worksheet xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1); ;
            Range range = xlWorkSheet.UsedRange;

            string str = string.Empty;
            int rCnt;
            int cCnt;
            int rw = 0;
            int cl = 0;

            int serialNo = 0;
            string itemCode = string.Empty;
            string itemName = string.Empty;
            string operation = string.Empty;            
            string department = string.Empty;
            string subdept = string.Empty;
            float rate = 0.0f;
            DateTime wef = DateTime.Now;

            rw = range.Rows.Count;
            cl = range.Columns.Count;

            for (rCnt = 2; rCnt <= rw; rCnt++)
            {
                str = string.Empty;
                for (cCnt = 1; cCnt <= cl; cCnt++)
                {
                    dynamic val1 = (range.Cells[rCnt, cCnt] as Range).Value2;
                    if (val1 != null)
                    {
                        switch (cCnt)
                        {
                            case 1: //Sr. No
                                serialNo = int.Parse(GetValue(val1));
                                break;
                            case 2: //Item Code
                                itemCode = GetValue(val1);
                                break;
                            case 3: //item Name
                                itemName = GetValue(val1);
                                break;
                            case 4: //Operation
                                operation = GetValue(val1);
                                break;
                            case 5: //WEF
                                wef = DateTime.Parse(GetValue(val1));
                                break;
                            case 6: //department
                                department = GetValue(val1);
                                break;
                            case 7: //sub department
                                subdept = GetValue(val1);
                                break;
                            case 8: //Rate
                                rate = float.Parse(GetValue(val1));
                                break;
                        }
                    }
                }


                Guid dept_id;
                Guid subDept_id;
                Guid company_id;

                string departmentSql = "select DEPT_ID from [dbo].[TAB_DEPARTMENT_MASTER] where DEPT_NAME = '" + department + "'";
                string subDepartmentSql = "select SUBDEPT_ID from [dbo].[TAB_SUBDEPARTMENT_MASTER] where SUBDEPT_NAME = '" + subdept + "'";
                string companySql = "select COMPANY_ID from [dbo].[TAB_COMPANY_MASTER] where COMPANY_NAME = 'PNI'";

                dept_id = GetGuidValue(con, departmentSql);
                subDept_id = GetGuidValue(con, subDepartmentSql);
                subDept_id = subDept_id == Guid.Empty ? Guid.Parse("8B860BE1-DC00-49AA-B4DC-760E7233BEBA") : subDept_id;
                company_id = GetGuidValue(con, companySql);

                Guid item_code_id = Guid.NewGuid();

                string item_code_sql = "INSERT INTO [dbo].[TAB_ITEM_CODE] " +
                    "(ITEM_CODE_ID, ITEM_CODE_NAME, COMPANY_ID, DEPT_ID, SUBDEPT_ID, CREATED_DATE, CREATED_BY, UPDATED_DATE, UPDATED_BY) " +
                    "VALUES(@ITEM_CODE_ID, @ITEM_CODE_NAME, @COMPANY_ID, @DEPT_ID, @SUBDEPT_ID, @CREATED_DATE, @CREATED_BY, @UPDATED_DATE, @UPDATED_BY)";

                try
                {
                    SqlCommand cmd = new SqlCommand(item_code_sql);
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@ITEM_CODE_ID", item_code_id);
                    cmd.Parameters["@ITEM_CODE_ID"].SqlDbType = System.Data.SqlDbType.UniqueIdentifier;

                    cmd.Parameters.AddWithValue("@ITEM_CODE_NAME", itemCode);
                    cmd.Parameters["@ITEM_CODE_NAME"].SqlDbType = System.Data.SqlDbType.NVarChar;

                    cmd.Parameters.AddWithValue("@COMPANY_ID", company_id);
                    cmd.Parameters["@COMPANY_ID"].SqlDbType = System.Data.SqlDbType.UniqueIdentifier;

                    cmd.Parameters.AddWithValue("@DEPT_ID", dept_id);
                    cmd.Parameters["@DEPT_ID"].SqlDbType = System.Data.SqlDbType.UniqueIdentifier;

                    cmd.Parameters.AddWithValue("@SUBDEPT_ID", subDept_id);
                    cmd.Parameters["@SUBDEPT_ID"].SqlDbType = System.Data.SqlDbType.UniqueIdentifier;

                    cmd.Parameters.AddWithValue("@CREATED_DATE", DateTime.Now);
                    cmd.Parameters["@CREATED_DATE"].SqlDbType = System.Data.SqlDbType.DateTime;

                    cmd.Parameters.AddWithValue("@CREATED_BY", "System");
                    cmd.Parameters["@CREATED_BY"].SqlDbType = System.Data.SqlDbType.NVarChar;

                    cmd.Parameters.AddWithValue("@UPDATED_DATE", DBNull.Value);
                    cmd.Parameters["@UPDATED_DATE"].SqlDbType = System.Data.SqlDbType.DateTime;

                    cmd.Parameters.AddWithValue("@UPDATED_BY", DBNull.Value);
                    cmd.Parameters["@UPDATED_BY"].SqlDbType = System.Data.SqlDbType.NVarChar;

                    Console.WriteLine("Inserting record for TAB_ITEM_CODE");
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Insertion done for TAB_ITEM_CODE");

                    Guid item_id = Guid.NewGuid();

                    string item_master_sql = "INSERT INTO [dbo].[TAB_ITEM_MASTER] " +
                    "(ITEM_ID, DEPT_ID, SUBDEPT_ID, ITEM_CODE_ID, COMPANY_ID, ITEM_NAME, CREATED_DATE, CREATED_BY, UPDATED_DATE, UPDATED_BY, STATUS) " +
                    "VALUES(@ITEM_ID, @DEPT_ID, @SUBDEPT_ID, @ITEM_CODE_ID, @COMPANY_ID, @ITEM_NAME, @CREATED_DATE, @CREATED_BY, @UPDATED_DATE, @UPDATED_BY, @STATUS)";

                    cmd = new SqlCommand(item_master_sql);
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@ITEM_ID", item_id);
                    cmd.Parameters["@ITEM_ID"].SqlDbType = System.Data.SqlDbType.UniqueIdentifier;

                    cmd.Parameters.AddWithValue("@DEPT_ID", dept_id);
                    cmd.Parameters["@DEPT_ID"].SqlDbType = System.Data.SqlDbType.UniqueIdentifier;

                    cmd.Parameters.AddWithValue("@SUBDEPT_ID", subDept_id);
                    cmd.Parameters["@SUBDEPT_ID"].SqlDbType = System.Data.SqlDbType.UniqueIdentifier;

                    cmd.Parameters.AddWithValue("@ITEM_CODE_ID", item_code_id);
                    cmd.Parameters["@ITEM_CODE_ID"].SqlDbType = System.Data.SqlDbType.UniqueIdentifier;

                    cmd.Parameters.AddWithValue("@COMPANY_ID", company_id);
                    cmd.Parameters["@COMPANY_ID"].SqlDbType = System.Data.SqlDbType.UniqueIdentifier;

                    cmd.Parameters.AddWithValue("@ITEM_NAME", itemName);
                    cmd.Parameters["@ITEM_NAME"].SqlDbType = System.Data.SqlDbType.NVarChar;

                    cmd.Parameters.AddWithValue("@CREATED_DATE", DateTime.Now);
                    cmd.Parameters["@CREATED_DATE"].SqlDbType = System.Data.SqlDbType.DateTime;

                    cmd.Parameters.AddWithValue("@CREATED_BY", "System");
                    cmd.Parameters["@CREATED_BY"].SqlDbType = System.Data.SqlDbType.NVarChar;

                    cmd.Parameters.AddWithValue("@UPDATED_DATE", DBNull.Value);
                    cmd.Parameters["@UPDATED_DATE"].SqlDbType = System.Data.SqlDbType.DateTime;

                    cmd.Parameters.AddWithValue("@UPDATED_BY", DBNull.Value);
                    cmd.Parameters["@UPDATED_BY"].SqlDbType = System.Data.SqlDbType.NVarChar;

                    cmd.Parameters.AddWithValue("@STATUS", 'Y');
                    cmd.Parameters["@STATUS"].SqlDbType = System.Data.SqlDbType.Char;

                    Console.WriteLine("Inserting record for TAB_ITEM_MASTER");
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Insertion done for TAB_ITEM_MASTER");

                    Guid unique_operation_id = Guid.NewGuid();

                    string item_operation_master_sql = "INSERT INTO [dbo].[TAB_ITEM_OPERATION_MASTER] " +
                    "(UNIQUE_OPERATION_ID, COMPANY_ID, ITEM_ID, RATE, OPERATION, PERCENTAGE, RATE_APPLIED_DATE, CREATED_DATE, CREATED_BY, UPDATED_DATE, UPDATED_BY, STATUS) " +
                    "VALUES(@UNIQUE_OPERATION_ID, @COMPANY_ID, @ITEM_ID, @RATE, @OPERATION, @PERCENTAGE, @RATE_APPLIED_DATE, @CREATED_DATE, @CREATED_BY, @UPDATED_DATE, @UPDATED_BY, @STATUS)";

                    cmd = new SqlCommand(item_operation_master_sql);
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@UNIQUE_OPERATION_ID", unique_operation_id);
                    cmd.Parameters["@UNIQUE_OPERATION_ID"].SqlDbType = System.Data.SqlDbType.UniqueIdentifier;

                    cmd.Parameters.AddWithValue("@COMPANY_ID", company_id);
                    cmd.Parameters["@COMPANY_ID"].SqlDbType = System.Data.SqlDbType.UniqueIdentifier;

                    cmd.Parameters.AddWithValue("@ITEM_ID", item_id);
                    cmd.Parameters["@ITEM_ID"].SqlDbType = System.Data.SqlDbType.UniqueIdentifier;

                    cmd.Parameters.AddWithValue("@RATE", rate);
                    cmd.Parameters["@RATE"].SqlDbType = System.Data.SqlDbType.Float;

                    cmd.Parameters.AddWithValue("@OPERATION", operation);
                    cmd.Parameters["@OPERATION"].SqlDbType = System.Data.SqlDbType.NVarChar;

                    cmd.Parameters.AddWithValue("@PERCENTAGE", DBNull.Value);
                    cmd.Parameters["@PERCENTAGE"].SqlDbType = System.Data.SqlDbType.Float;

                    cmd.Parameters.AddWithValue("@RATE_APPLIED_DATE", DBNull.Value);
                    cmd.Parameters["@RATE_APPLIED_DATE"].SqlDbType = System.Data.SqlDbType.Date;

                    cmd.Parameters.AddWithValue("@CREATED_DATE", DateTime.Now);
                    cmd.Parameters["@CREATED_DATE"].SqlDbType = System.Data.SqlDbType.DateTime;

                    cmd.Parameters.AddWithValue("@CREATED_BY", "System");
                    cmd.Parameters["@CREATED_BY"].SqlDbType = System.Data.SqlDbType.NVarChar;

                    cmd.Parameters.AddWithValue("@UPDATED_DATE", DBNull.Value);
                    cmd.Parameters["@UPDATED_DATE"].SqlDbType = System.Data.SqlDbType.DateTime;

                    cmd.Parameters.AddWithValue("@UPDATED_BY", DBNull.Value);
                    cmd.Parameters["@UPDATED_BY"].SqlDbType = System.Data.SqlDbType.NVarChar;

                    cmd.Parameters.AddWithValue("@STATUS", 'Y');
                    cmd.Parameters["@STATUS"].SqlDbType = System.Data.SqlDbType.Char;

                    Console.WriteLine("Inserting record for TAB_ITEM_OPERATION_MASTER");
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Insertion done for TAB_ITEM_OPERATION_MASTER");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static Guid GetGuidValue(SqlConnection con, string sql)
        {
            Guid value = Guid.Empty;
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Connection = con;
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                value = reader.GetGuid(0);
            }
            reader.Close();

            return value;
        }

        private static string GetValue(dynamic value)
        {
            if (value.GetType().ToString() == "System.Double")
                return ((double)value).ToString();
            else
                return (string)value;
        }
    }
}
