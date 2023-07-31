using Microsoft.Office.Interop.Excel;
using System;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace FillWorkForceSalaryMasterData
{
    class Program
    {
        static void Main(string[] args)
        {
            string emplayeeMatserExcelPath = @"C:\Users\hp\Downloads\Contract Worker Database.xlsx";
            string connectionString = ConfigurationManager.ConnectionStrings["ConStr"].ToString();

            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            Console.WriteLine("Loading excel sheet....");
            Application xlApp = new Application();
            Workbook xlWorkBook = xlApp.Workbooks.Open(emplayeeMatserExcelPath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            Worksheet xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1); ;
            Range range = xlWorkSheet.UsedRange;

            string str;
            int rCnt;
            int cCnt;
            int rw = 0;
            int cl = 0;

            rw = range.Rows.Count;
            cl = range.Columns.Count;
            string employeeCode = string.Empty;
            string bioMetricCode = string.Empty;
            string employeeName = string.Empty;
            string fatherName = string.Empty;
            string department = string.Empty;
            string subdepartment = string.Empty;
            DateTime dateOfBirth = DateTime.Now;
            string nationality = string.Empty;
            string education = string.Empty;
            DateTime dateOfJoinAsPerHrForm = DateTime.Now;
            DateTime dateOfJoinAsPerEpf = DateTime.Now;
            string designation = string.Empty;
            string category = string.Empty;
            string employementType = string.Empty;
            string mobileNo = string.Empty;
            string alternateNo = string.Empty;
            string uan = string.Empty;
            string epfAccountNo = string.Empty;
            string panNo = string.Empty;
            string esicipNo = string.Empty;
            string adhaarNo = string.Empty;
            string accountNo = string.Empty;
            string bankName = string.Empty;
            string ifscCode = string.Empty;
            string presentAddress = string.Empty;
            string state = string.Empty;
            string permanentAddress = string.Empty;
            string basic = string.Empty;
            int DA = 0;
            int hra = 0;
            int specialAllowance = 0;
            int gross = 0;
            DateTime dateOfExit;
            string reasonOfExit;
            string remarks;
            DateTime dateOfRetirement = DateTime.Now;
            string agencyName = string.Empty;

            //table field variable
            Guid WF_ID = Guid.Empty;
            string EMP_ID = string.Empty;
            short WF_EMP_TYPE = 0;
            Guid COMPANY_ID = Guid.Empty;
            Guid AGENCY_ID = Guid.Empty;
            string BIOMETRIC_CODE = string.Empty;
            string EMP_NAME = string.Empty;
            string FATHER_NAME = string.Empty;
            char GENDER = 'M';
            Guid DEPT_ID = Guid.Empty;
            DateTime DATE_OF_BIRTH = DateTime.Now;
            string NATIONALITY = string.Empty;
            Int16 WF_EDUCATION_ID = 0;
            byte MARITAL_ID = 0;
            DateTime DOJ = DateTime.Now;
            DateTime DOJ_AS_PER_EPF = DateTime.Now;
            Int16 WF_DESIGNATION_ID = 0;
            Guid SKILL_ID = Guid.Empty;
            Int16 EMP_TYPE_ID = 0;
            string MOBILE_NO = string.Empty;
            string ALTERNATE_NO = string.Empty;
            string UAN = string.Empty;
            string PAN_CARD = string.Empty;
            string EPF_NO = string.Empty;
            string ESIC_NO = string.Empty;
            Guid BANK_ID = Guid.Empty;
            string BANK_IFSC = string.Empty;
            string BANK_BRANCH = string.Empty;
            string BANK_ACCOUNT_NO = string.Empty;
            int BASIC_DA = 0;
            int SPECIAL_ALLOWANCES = 0;
            int HRA = 0;
            int GROSS = 0;
            string EMAIL_ID = string.Empty;
            string PRESENT_ADDRESS = string.Empty;
            Guid PRESENT_ADDRESS_CITY = Guid.Empty;
            Guid PRESENT_ADDRESS_STATE = Guid.Empty;
            int PRESENT_ADDRESS_PIN = 0;
            string PERMANENT_ADDRESS = string.Empty;
            Guid PERMANENT_ADDRESS_CITY = Guid.Empty;
            Guid PARMANENT_ADDRESS_STATE = Guid.Empty;
            int PARMANENT_ADDRESS_PIN = 0;
            long AADHAR_NO = 0;            
            byte EMP_STATUS_ID = 0;
            string PHOTO = string.Empty;
            string IDENTIFICATION_MARK = string.Empty;
            string EMP_SIGNATURE = string.Empty;
            DateTime EXIT_DATE = DateTime.Now;
            string EXIT_REASON = string.Empty;
            DateTime RETIREMMENT_DATE = DateTime.Now;
            DateTime CREATED_DATE = DateTime.Now;
            string CREATED_BY = "ADMIN";
            DateTime UPDATED_DATE = DateTime.Now;
            char STATUS = 'Y';
            int totalRecords = rw - 9;
            int count = 1;


            for (rCnt = 3; rCnt <= rw; rCnt++)
            {
                try
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
                                    break;
                                case 2: //Employee code (PNI)
                                    employeeCode = GetValue(val1);
                                    break;
                                case 3: //Biometric Code
                                    bioMetricCode = GetValue(val1);
                                    break;
                                case 4: //Employee Name
                                    employeeName = GetValue(val1);
                                    break;
                                case 5: // Gender
                                    GENDER = Convert.ToChar(GetValue(val1));
                                    break;
                                case 6: //Father's / Spouse name
                                    fatherName = GetValue(val1);
                                    break;
                                case 7: //department
                                    department = GetValue(val1);
                                    break;
                                case 8: //subdepartment
                                    subdepartment = GetValue(val1);
                                    break;
                                case 9: //Date of Birth (DD/MM/YY)
                                    double date = double.Parse(GetValue(val1));
                                    dateOfBirth = DateTime.FromOADate(date);
                                    break;
                                case 10: //Nationality
                                    nationality = GetValue(val1);
                                    break;
                                case 11: //Education Level (G/M )
                                    education = GetValue(val1);
                                    break;
                                case 12: //Date of joining (DD/MM/YY) - As per HR Form
                                    double dateAsPerHrForm = double.Parse(GetValue(val1));
                                    dateOfJoinAsPerHrForm = DateTime.FromOADate(dateAsPerHrForm);
                                    break;
                                //case 13: //Date of joining (DD/MM/YY) - As per EPF
                                //double dateAsPerHrEpf = double.Parse(GetValue(val1));
                                //dateOfJoinAsPerEpf = DateTime.FromOADate(dateAsPerHrEpf);
                                // break;
                                case 13: //Designation
                                    designation = GetValue(val1);
                                    break;
                                case 14: //Category (HS/S/SS/US)
                                    category = GetValue(val1);
                                    break;
                                case 15: //Type of Employment
                                    employementType = GetValue(val1);
                                    break;
                                case 16: //Mobile No.
                                    mobileNo = GetValue(val1);
                                    break;
                                case 17: //Alternate No.
                                    alternateNo = GetValue(val1);
                                    break;
                                case 18: //UAN 
                                    uan = GetValue(val1);
                                    break;
                                //case 19: //EPF Account no.
                                //epfAccountNo = GetValue(val1);
                                // break;
                                case 19: //PAN
                                    panNo = GetValue(val1);
                                    break;
                                case 20: //ESIC IP No.
                                    esicipNo = GetValue(val1);
                                    break;
                                case 21: //AADHAAR
                                    adhaarNo = GetValue(val1);
                                    break;
                                case 22: //     
                                    accountNo = GetValue(val1);
                                    break;
                                case 23: //
                                    bankName = GetValue(val1);
                                    break;
                                case 24://
                                    ifscCode = GetValue(val1); 
                                    break;
                                case 25: //Present address.
                                    presentAddress = GetValue(val1);
                                    break;
                                case 26: //Present address.
                                    state = GetValue(val1);
                                    break;
                                case 27: //Permanent address
                                    permanentAddress = GetValue(val1);
                                    break;
                                //case 28: //Service Book No.                                 
                                //    break;
                                case 28: //basic+da
                                    basic = GetValue(val1);
                                    break;
                                case 29: //HRA
                                    hra = int.Parse(GetValue(val1));
                                    break;
                                case 30: //Spl. Allowances
                                    specialAllowance = int.Parse(GetValue(val1));
                                    break;
                                case 31: //Gross
                                    gross = int.Parse(GetValue(val1));
                                    break;
                                case 32: //Date of exit                                
                                         //dateOfExit = GetValue(val1);
                                    agencyName = GetValue(val1);
                                    break;
                                case 36: //Reason of exit
                                    reasonOfExit = GetValue(val1);
                                    break;
                                case 37: //Remarks
                                    remarks = string.Concat((string)val1, ", ");
                                    break;
                                case 40: //Date of retirement
                                         //double dateOfRe = double.Parse(GetValue(val1));
                                         //dateOfRetirement = DateTime.FromOADate(dateOfRe);
                                    break;
                            }
                        }
                    }                                                        

                    string workforceSql = "select WF_ID from [dbo].[TAB_WORKFORCE_MASTER] where EMP_ID = '" + employeeCode + "'";
                    string companySql = string.Empty;
                    if (string.IsNullOrEmpty(agencyName))
                    {
                        companySql = "select AGENCY_ID from [dbo].[TAB_AGENCY_MASTER] where AGENCY_NAME = '" + agencyName + "'";
                    }
                    else
                    {
                        companySql = "select COMPANY_ID from [dbo].[TAB_COMPANY_MASTER] where COMPANY_NAME = 'PNI'";
                    }
                    string bankSql = "select BANK_ID from [dbo].[TAB_BANK_MASTER] where BANK_NAME = '" + bankName + "'";                                        

                    WF_ID = GetGuidValue(con, workforceSql);                     
                    COMPANY_ID = GetGuidValue(con, companySql);
                    UAN = uan;
                    PAN_CARD = panNo;
                    EPF_NO = epfAccountNo;
                    ESIC_NO = esicipNo;
                    BANK_ID = GetGuidValue(con, bankSql);
                    string[] branchIfscCode = ifscCode.Split('(');
                    BANK_IFSC = ifscCode != "NA" ? branchIfscCode[1].ToString().Replace(")", string.Empty).Trim() : ifscCode;
                    BANK_BRANCH = ifscCode != "NA" ? branchIfscCode[0].ToString().Trim() : bankName;
                    BANK_ACCOUNT_NO = accountNo;
                    BASIC_DA = int.Parse(basic);
                    HRA = hra;
                    SPECIAL_ALLOWANCES = specialAllowance;
                    GROSS = gross;
                    CREATED_DATE = DateTime.Now;
                    CREATED_BY = "System";
                    STATUS = 'Y';
                    
                    AADHAR_NO = long.Parse(adhaarNo);
                    

                    string sql = "INSERT INTO [dbo].[TAB_WORKFORCE_SALARY_MASTER] " +
              "([WF_ID], [COMPANY_ID], [UAN_NO], [PAN_CARD], [EPF_NO], [ESIC_NO], [BANK_ID], [BANK_IFSC], [BANK_BRANCH], [BANK_ACCOUNT_NO], [BASIC_DA], [HRA], " +
              "[SPECIAL_ALLOWANCES], [GROSS], [CREATED_DATE], [CREATED_BY], [UPDATED_DATE], [UPDATED_BY], [STATUS])" +
              "VALUES(@WF_ID, @COMPANY_ID, @UAN_NO, @PAN_CARD, @EPF_NO, @ESIC_NO, @BANK_ID, @BANK_IFSC, @BANK_BRANCH, @BANK_ACCOUNT_NO, @BASIC_DA, @HRA, @SPECIAL_ALLOWANCES, @GROSS, " + 
              "@CREATED_DATE, @CREATED_BY, @UPDATED_DATE, @UPDATED_BY, @STATUS)";

                    SqlCommand cmd = new SqlCommand(sql);
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@WF_ID", WF_ID);
                    cmd.Parameters["@WF_ID"].SqlDbType = System.Data.SqlDbType.UniqueIdentifier;

                    cmd.Parameters.AddWithValue("@COMPANY_ID", COMPANY_ID);
                    cmd.Parameters["@COMPANY_ID"].SqlDbType = System.Data.SqlDbType.UniqueIdentifier;                   

                    cmd.Parameters.AddWithValue("@UAN_NO", UAN);
                    cmd.Parameters["@UAN_NO"].SqlDbType = System.Data.SqlDbType.VarChar;

                    cmd.Parameters.AddWithValue("@PAN_CARD", PAN_CARD);
                    cmd.Parameters["@PAN_CARD"].SqlDbType = System.Data.SqlDbType.VarChar;                   

                    cmd.Parameters.AddWithValue("@EPF_NO", EPF_NO);
                    cmd.Parameters["@EPF_NO"].SqlDbType = System.Data.SqlDbType.VarChar;

                    cmd.Parameters.AddWithValue("@ESIC_NO", ESIC_NO);
                    cmd.Parameters["@ESIC_NO"].SqlDbType = System.Data.SqlDbType.VarChar;

                    if (BANK_ID != Guid.Empty)
                    {
                        cmd.Parameters.AddWithValue("@BANK_ID", BANK_ID);
                        cmd.Parameters["@BANK_ID"].SqlDbType = System.Data.SqlDbType.UniqueIdentifier;
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@BANK_ID", DBNull.Value);
                        cmd.Parameters["@BANK_ID"].SqlDbType = System.Data.SqlDbType.UniqueIdentifier;
                    }

                    cmd.Parameters.AddWithValue("@BANK_IFSC", BANK_IFSC);
                    cmd.Parameters["@BANK_IFSC"].SqlDbType = System.Data.SqlDbType.VarChar;                   

                    cmd.Parameters.AddWithValue("@BANK_BRANCH", BANK_BRANCH);
                    cmd.Parameters["@BANK_BRANCH"].SqlDbType = System.Data.SqlDbType.VarChar;

                    cmd.Parameters.AddWithValue("@BANK_ACCOUNT_NO", BANK_ACCOUNT_NO);
                    cmd.Parameters["@BANK_ACCOUNT_NO"].SqlDbType = System.Data.SqlDbType.VarChar;

                    cmd.Parameters.AddWithValue("@BASIC_DA", BASIC_DA);
                    cmd.Parameters["@BASIC_DA"].SqlDbType = System.Data.SqlDbType.Int;

                    cmd.Parameters.AddWithValue("@HRA", HRA);
                    cmd.Parameters["@HRA"].SqlDbType = System.Data.SqlDbType.Int;

                    cmd.Parameters.AddWithValue("@SPECIAL_ALLOWANCES", SPECIAL_ALLOWANCES);
                    cmd.Parameters["@SPECIAL_ALLOWANCES"].SqlDbType = System.Data.SqlDbType.Int;

                    cmd.Parameters.AddWithValue("@GROSS", GROSS);
                    cmd.Parameters["@GROSS"].SqlDbType = System.Data.SqlDbType.Int;                    

                    cmd.Parameters.AddWithValue("@CREATED_DATE", CREATED_DATE);
                    cmd.Parameters["@CREATED_DATE"].SqlDbType = System.Data.SqlDbType.DateTime;

                    cmd.Parameters.AddWithValue("@CREATED_BY", CREATED_BY);
                    cmd.Parameters["@CREATED_BY"].SqlDbType = System.Data.SqlDbType.VarChar;

                    cmd.Parameters.AddWithValue("@UPDATED_DATE", DBNull.Value);
                    cmd.Parameters["@UPDATED_DATE"].SqlDbType = System.Data.SqlDbType.DateTime;

                    cmd.Parameters.AddWithValue("@UPDATED_BY", DBNull.Value);
                    cmd.Parameters["@UPDATED_BY"].SqlDbType = System.Data.SqlDbType.VarChar;

                    cmd.Parameters.AddWithValue("@STATUS", STATUS);
                    cmd.Parameters["@STATUS"].SqlDbType = System.Data.SqlDbType.Char;


                    Console.Write("Inserting record for workforce salary: " + employeeCode);
                    cmd.ExecuteNonQuery();
                    count++;
                    Console.Write(" (Inserted)");
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }


            xlWorkBook.Close(true, null, null);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);

            con.Close();
            Console.WriteLine("Total {0} records inserted out of {1}", count, totalRecords);
            Console.ReadLine();
        }

        private static string GetValue(dynamic value)
        {
            if (value.GetType().ToString() == "System.Double")
                return ((double)value).ToString();
            else
                return (string)value;
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

        private static byte GetByteValue(SqlConnection con, string sql)
        {
            byte value = 0;
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Connection = con;
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                value = reader.GetByte(0);
            }
            reader.Close();

            return value;
        }

        private static Int32 GetInt32Value(SqlConnection con, string sql)
        {
            Int32 value = 0;
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Connection = con;
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                value = reader.GetInt32(0);
            }
            reader.Close();

            return value;
        }

        private static Int16 GetInt16Value(SqlConnection con, string sql)
        {
            Int16 value = 0;
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Connection = con;
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                value = reader.GetInt16(0);
            }
            reader.Close();

            return value;
        }        
    }
}
