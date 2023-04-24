using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Collections;

namespace FillMasterData
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
                                        break;
                                    case 23: //
                                        break;
                                    case 24://
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
                                    case 33: //Date of exit                                
                                             //dateOfExit = GetValue(val1);
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

                    WF_ID = Guid.NewGuid();
                    
                    if (category.Contains(" "))
                    {
                        string[] cat = Regex.Split(category, " ");
                        category = (cat[0] == "SEMI" ? string.Concat(cat[0], "-", cat[1]) : string.Concat(cat[0], cat[1]));
                    }                    

                    string trimmedEducation = education.Trim();

                    if (trimmedEducation == "High School") trimmedEducation = trimmedEducation.ToUpper();
                    else if (trimmedEducation == "12TH/ITI") trimmedEducation = "12TH PASS";

                    string departmentSql = "select DEPT_ID from [dbo].[TAB_DEPARTMENT_MASTER] where DEPT_NAME = '" + department + "'";
                    string educationSql = "select WF_EDUCATION_ID from [dbo].[TAB_WORKFORCE_EDU_MASTER] where WF_COURSE_NAME like '%" + trimmedEducation + "%'";
                    string designationSql = "select WF_DESIGNATION_ID from [dbo].[TAB_WF_DESIGNATION_MASTER] where WF_DESIGNATION_NAME = '" + designation + "'";
                    string skillSql = "select SKILL_ID from [dbo].[TAB_SKILL_MASTER] where SKILL_NAME = '" + category + "'";
                    string empTypeSql = "select EMP_TYPE_ID from [dbo].[TAB_EMPL_TYPE_MASTER] where EMP_TYPE = '" + employementType + "'";
                    string companySql = "select COMPANY_ID from [dbo].[TAB_COMPANY_MASTER] where COMPANY_NAME = 'PNI'";
                    
                    //persent address
                    PRESENT_ADDRESS = presentAddress;
                    ArrayList persentAddressList = GetStatePin(PRESENT_ADDRESS);
                    string state1 = persentAddressList.Count == 2 ? persentAddressList[0].ToString() : "Empty";
                    string pin = persentAddressList.Count == 2 ? persentAddressList[1].ToString() : "Empty";
                    int pinresult = 0;
                    PRESENT_ADDRESS_PIN = 0;

                    string persentStateSql = string.Empty;
                    string persentPinSql = string.Empty;
                    if(state == "UTTAR PRADESH")
                        persentStateSql = "select STATE_ID from [dbo].[TAB_STATE_MASTER] where STATE_NAME = '" + state + "'";
                    else
                        persentStateSql = "select STATE_ID from [dbo].[TAB_STATE_MASTER] where STATE_NAME = '" + state1.Trim() + "'";

                    if (int.TryParse(pin, out pinresult)) PRESENT_ADDRESS_PIN = pinresult;                                           

                    PRESENT_ADDRESS_STATE = GetGuidValue(con, persentStateSql);
                    PRESENT_ADDRESS_PIN = PRESENT_ADDRESS_PIN > 0 ? PRESENT_ADDRESS_PIN : 0;
                    string presentCitySql = string.Empty;                    
                    if (state == "UTTAR PRADESH")
                        presentCitySql = "select CITY_ID from [dbo].[TAB_STATE_CITY_MASTER] where CITY_NAME = 'LUCKNOW'";
                    else
                    {
                        string cityName = GetCityName(PRESENT_ADDRESS);                        
                        presentCitySql = "select CITY_ID from [dbo].[TAB_STATE_CITY_MASTER] where CITY_NAME = '" + cityName + "'";
                    }

                    PRESENT_ADDRESS_CITY = GetGuidValue(con, presentCitySql);

                    //permanent address
                    PERMANENT_ADDRESS = permanentAddress;
                    PARMANENT_ADDRESS_PIN = 0;

                    ArrayList permanentAddressList = GetStatePin(PERMANENT_ADDRESS);
                    string permanentstate = persentAddressList.Count == 2 ? persentAddressList[0].ToString() : "EMPTY";
                    string permanentpin = persentAddressList.Count == 2 ? persentAddressList[1].ToString() : "EMPTY";
                    string parmentStateSql = string.Empty;                    

                    if (state == "UTTAR PRADESH")
                        parmentStateSql = "select STATE_ID from [dbo].[TAB_STATE_MASTER] where STATE_NAME = '" + state + "'";
                    else
                        parmentStateSql = "select STATE_ID from [dbo].[TAB_STATE_MASTER] where STATE_NAME = '" + permanentstate.Trim() + "'";

                    if (int.TryParse(permanentpin, out pinresult)) PARMANENT_ADDRESS_PIN = pinresult;

                    PARMANENT_ADDRESS_STATE = GetGuidValue(con, parmentStateSql);
                    PARMANENT_ADDRESS_PIN = PARMANENT_ADDRESS_PIN > 0 ? PARMANENT_ADDRESS_PIN : 0;

                    string permanentCitySql = string.Empty;
                    if (state == "UTTAR PRADESH")
                        permanentCitySql = "select CITY_ID from [dbo].[TAB_STATE_CITY_MASTER] where CITY_NAME = 'LUCKNOW'";
                    else
                    {
                        string cityName = GetCityName(PERMANENT_ADDRESS);
                        permanentCitySql = "select CITY_ID from [dbo].[TAB_STATE_CITY_MASTER] where CITY_NAME = '" + cityName + "'";
                    }

                    PERMANENT_ADDRESS_CITY = GetGuidValue(con, presentCitySql);

                    EMP_ID = employeeCode;
                    WF_EMP_TYPE = GetInt16Value(con, empTypeSql);
                    COMPANY_ID = GetGuidValue(con, companySql);
                    BIOMETRIC_CODE = bioMetricCode;
                    EMP_NAME = employeeName;
                    FATHER_NAME = fatherName;
                    GENDER = 'M';
                    DEPT_ID = GetGuidValue(con, departmentSql);
                    DATE_OF_BIRTH = dateOfBirth;
                    NATIONALITY = nationality;
                    WF_EDUCATION_ID = GetInt16Value(con, educationSql);
                    DOJ = dateOfJoinAsPerHrForm;
                    DOJ_AS_PER_EPF = dateOfJoinAsPerEpf;
                    WF_DESIGNATION_ID = GetInt16Value(con, designationSql);
                    SKILL_ID = GetGuidValue(con, skillSql);
                    EMP_TYPE_ID = GetInt16Value(con, empTypeSql);
                    MOBILE_NO = mobileNo;
                    ALTERNATE_NO = alternateNo;
                    AADHAR_NO = long.Parse(adhaarNo);
                    EMP_STATUS_ID = 1;
                    PHOTO = null;
                    EMP_SIGNATURE = null;
                    EXIT_DATE = DateTime.Now;
                    EXIT_REASON = string.Empty;
                    RETIREMMENT_DATE = dateOfRetirement;

                    string sql = "INSERT INTO [dbo].[TAB_WORKFORCE_MASTER] " +
              "([WF_ID], [EMP_ID], [WF_EMP_TYPE], [COMPANY_ID], [AGENCY_ID], [BIOMETRIC_CODE], [EMP_NAME], [FATHER_NAME], [GENDER], [DEPT_ID], [DATE_OF_BIRTH], [NATIONALITY]," +
              "[WF_EDUCATION_ID], [MARITAL_ID], [DOJ], [DOJ_AS_PER_EPF], [WF_DESIGNATION_ID], [SKILL_ID], [EMP_TYPE_ID], [MOBILE_NO], [ALTERNATE_NO], [EMAIL_ID], [PRESENT_ADDRESS]," +
              "[PRESENT_ADDRESS_CITY], [PRESENT_ADDRESS_STATE], [PRESENT_ADDRESS_PIN], [PERMANENT_ADDRESS], [PERMANENT_ADDRESS_CITY], [PERMANENT_ADDRESS_STATE], [PERMANENT_ADDRESS_PIN]," +
              "[AADHAR_NO], [EMP_STATUS_ID], [IDENTIFICATION_MARK], [EXIT_DATE], [EXIT_REASON], [RETIREMMENT_DATE], [CREATED_DATE], [CREATED_BY]," +
              "[UPDATED_DATE], [UPDATED_BY], [STATUS])" +
              "VALUES(@WF_ID, @EMP_ID, @WF_EMP_TYPE, @COMPANY_ID, @AGENCY_ID, @BIOMETRIC_CODE, @EMP_NAME, @FATHER_NAME, @GENDER, @DEPT_ID, @DATE_OF_BIRTH, @NATIONALITY, @WF_EDUCATION_ID, @MARITAL_ID, @DOJ, @DOJ_AS_PER_EPF," +
                     "@WF_DESIGNATION_ID, @SKILL_ID, @EMP_TYPE_ID, @MOBILE_NO, @ALTERNATE_NO, @EMAIL_ID, @PRESENT_ADDRESS, @PRESENT_ADDRESS_CITY, @PRESENT_ADDRESS_STATE, @PRESENT_ADDRESS_PIN, @PERMANENT_ADDRESS, @PERMANENT_ADDRESS_CITY, @PARMANENT_ADDRESS_STATE, @PARMANENT_ADDRESS_PIN," +
                     "@AADHAR_NO, @EMP_STATUS_ID, @IDENTIFICATION_MARK, @EXIT_DATE, @EXIT_REASON, @RETIREMMENT_DATE, @CREATED_DATE, @CREATED_BY, @UPDATED_DATE, @UPDATED_BY, @STATUS)";

                    SqlCommand cmd = new SqlCommand(sql);
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@WF_ID", WF_ID);
                    cmd.Parameters["@WF_ID"].SqlDbType = System.Data.SqlDbType.UniqueIdentifier;

                    cmd.Parameters.AddWithValue("@EMP_ID", EMP_ID);
                    cmd.Parameters["@EMP_ID"].SqlDbType = System.Data.SqlDbType.NVarChar;

                    cmd.Parameters.AddWithValue("@WF_EMP_TYPE", WF_EMP_TYPE);
                    cmd.Parameters["@WF_EMP_TYPE"].SqlDbType = System.Data.SqlDbType.SmallInt;

                    cmd.Parameters.AddWithValue("@COMPANY_ID", COMPANY_ID);
                    cmd.Parameters["@COMPANY_ID"].SqlDbType = System.Data.SqlDbType.UniqueIdentifier;

                    cmd.Parameters.AddWithValue("@AGENCY_ID", DBNull.Value);
                    cmd.Parameters["@AGENCY_ID"].SqlDbType = System.Data.SqlDbType.UniqueIdentifier;

                    if (BIOMETRIC_CODE != "NA")
                    {
                        cmd.Parameters.AddWithValue("@BIOMETRIC_CODE", BIOMETRIC_CODE);
                        cmd.Parameters["@BIOMETRIC_CODE"].SqlDbType = System.Data.SqlDbType.NVarChar;                        
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@BIOMETRIC_CODE", DBNull.Value);
                        cmd.Parameters["@BIOMETRIC_CODE"].SqlDbType = System.Data.SqlDbType.NVarChar;
                    }

                    cmd.Parameters.AddWithValue("@EMP_NAME", EMP_NAME);
                    cmd.Parameters["@EMP_NAME"].SqlDbType = System.Data.SqlDbType.NVarChar;

                    cmd.Parameters.AddWithValue("@FATHER_NAME", FATHER_NAME);
                    cmd.Parameters["@FATHER_NAME"].SqlDbType = System.Data.SqlDbType.NVarChar;

                    cmd.Parameters.AddWithValue("@GENDER", GENDER);
                    cmd.Parameters["@GENDER"].SqlDbType = System.Data.SqlDbType.NVarChar;

                    if (DEPT_ID == Guid.Empty)
                    {
                        cmd.Parameters.AddWithValue("@DEPT_ID", DBNull.Value);
                        cmd.Parameters["@DEPT_ID"].SqlDbType = System.Data.SqlDbType.UniqueIdentifier;
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@DEPT_ID", DEPT_ID);
                        cmd.Parameters["@DEPT_ID"].SqlDbType = System.Data.SqlDbType.UniqueIdentifier;
                    }

                    cmd.Parameters.AddWithValue("@DATE_OF_BIRTH", DATE_OF_BIRTH);
                    cmd.Parameters["@DATE_OF_BIRTH"].SqlDbType = System.Data.SqlDbType.Date;

                    cmd.Parameters.AddWithValue("@NATIONALITY", NATIONALITY);
                    cmd.Parameters["@NATIONALITY"].SqlDbType = System.Data.SqlDbType.NVarChar;

                    cmd.Parameters.AddWithValue("@WF_EDUCATION_ID", WF_EDUCATION_ID);
                    cmd.Parameters["@WF_EDUCATION_ID"].SqlDbType = System.Data.SqlDbType.SmallInt;

                    cmd.Parameters.AddWithValue("@MARITAL_ID", DBNull.Value);
                    cmd.Parameters["@MARITAL_ID"].SqlDbType = System.Data.SqlDbType.SmallInt;

                    cmd.Parameters.AddWithValue("@DOJ", DOJ);
                    cmd.Parameters["@DOJ"].SqlDbType = System.Data.SqlDbType.Date;

                    cmd.Parameters.AddWithValue("@DOJ_AS_PER_EPF", DBNull.Value);
                    cmd.Parameters["@DOJ_AS_PER_EPF"].SqlDbType = System.Data.SqlDbType.Date;

                    cmd.Parameters.AddWithValue("@WF_DESIGNATION_ID", WF_DESIGNATION_ID);
                    cmd.Parameters["@WF_DESIGNATION_ID"].SqlDbType = System.Data.SqlDbType.SmallInt;

                    cmd.Parameters.AddWithValue("@SKILL_ID", SKILL_ID);
                    cmd.Parameters["@SKILL_ID"].SqlDbType = System.Data.SqlDbType.UniqueIdentifier;

                    cmd.Parameters.AddWithValue("@EMP_TYPE_ID", EMP_TYPE_ID);
                    cmd.Parameters["@EMP_TYPE_ID"].SqlDbType = System.Data.SqlDbType.SmallInt;

                    cmd.Parameters.AddWithValue("@MOBILE_NO", MOBILE_NO);
                    cmd.Parameters["@MOBILE_NO"].SqlDbType = System.Data.SqlDbType.NVarChar;

                    cmd.Parameters.AddWithValue("@ALTERNATE_NO", ALTERNATE_NO);
                    cmd.Parameters["@ALTERNATE_NO"].SqlDbType = System.Data.SqlDbType.NVarChar;

                    cmd.Parameters.AddWithValue("@EMAIL_ID", DBNull.Value);
                    cmd.Parameters["@EMAIL_ID"].SqlDbType = System.Data.SqlDbType.NVarChar;

                    cmd.Parameters.AddWithValue("@PRESENT_ADDRESS", PRESENT_ADDRESS);
                    cmd.Parameters["@PRESENT_ADDRESS"].SqlDbType = System.Data.SqlDbType.NVarChar;

                    if (PRESENT_ADDRESS_CITY != Guid.Empty)
                    {
                        cmd.Parameters.AddWithValue("@PRESENT_ADDRESS_CITY", PRESENT_ADDRESS_CITY);
                        cmd.Parameters["@PRESENT_ADDRESS_CITY"].SqlDbType = System.Data.SqlDbType.UniqueIdentifier;
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@PRESENT_ADDRESS_CITY", DBNull.Value);
                        cmd.Parameters["@PRESENT_ADDRESS_CITY"].SqlDbType = System.Data.SqlDbType.UniqueIdentifier;
                    }

                    if (PRESENT_ADDRESS_STATE != Guid.Empty)
                    {
                        cmd.Parameters.AddWithValue("@PRESENT_ADDRESS_STATE", DBNull.Value);
                        cmd.Parameters["@PRESENT_ADDRESS_STATE"].SqlDbType = System.Data.SqlDbType.UniqueIdentifier;
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@PRESENT_ADDRESS_STATE", PRESENT_ADDRESS_STATE);
                        cmd.Parameters["@PRESENT_ADDRESS_STATE"].SqlDbType = System.Data.SqlDbType.UniqueIdentifier;
                    }

                    cmd.Parameters.AddWithValue("@PRESENT_ADDRESS_PIN", PRESENT_ADDRESS_PIN);
                    cmd.Parameters["@PRESENT_ADDRESS_PIN"].SqlDbType = System.Data.SqlDbType.Int;

                    cmd.Parameters.AddWithValue("@PERMANENT_ADDRESS", PERMANENT_ADDRESS);
                    cmd.Parameters["@PERMANENT_ADDRESS"].SqlDbType = System.Data.SqlDbType.NVarChar;

                    if (PERMANENT_ADDRESS_CITY != Guid.Empty)
                    {
                        cmd.Parameters.AddWithValue("@PERMANENT_ADDRESS_CITY", PERMANENT_ADDRESS_CITY);
                        cmd.Parameters["@PERMANENT_ADDRESS_CITY"].SqlDbType = System.Data.SqlDbType.UniqueIdentifier;
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@PERMANENT_ADDRESS_CITY", DBNull.Value);
                        cmd.Parameters["@PERMANENT_ADDRESS_CITY"].SqlDbType = System.Data.SqlDbType.UniqueIdentifier;
                    }

                    if (PARMANENT_ADDRESS_STATE != Guid.Empty)
                    {
                        cmd.Parameters.AddWithValue("@PARMANENT_ADDRESS_STATE", PARMANENT_ADDRESS_STATE);
                        cmd.Parameters["@PARMANENT_ADDRESS_STATE"].SqlDbType = System.Data.SqlDbType.UniqueIdentifier;
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@PARMANENT_ADDRESS_STATE", DBNull.Value);
                        cmd.Parameters["@PARMANENT_ADDRESS_STATE"].SqlDbType = System.Data.SqlDbType.UniqueIdentifier;
                    }

                    cmd.Parameters.AddWithValue("@PARMANENT_ADDRESS_PIN", PARMANENT_ADDRESS_PIN);
                    cmd.Parameters["@PARMANENT_ADDRESS_PIN"].SqlDbType = System.Data.SqlDbType.Int;

                    cmd.Parameters.AddWithValue("@AADHAR_NO", AADHAR_NO);
                    cmd.Parameters["@AADHAR_NO"].SqlDbType = System.Data.SqlDbType.BigInt;

                    cmd.Parameters.AddWithValue("@EMP_STATUS_ID", EMP_STATUS_ID);
                    cmd.Parameters["@EMP_STATUS_ID"].SqlDbType = System.Data.SqlDbType.SmallInt;

                    cmd.Parameters.AddWithValue("@IDENTIFICATION_MARK", DBNull.Value);
                    cmd.Parameters["@IDENTIFICATION_MARK"].SqlDbType = System.Data.SqlDbType.NVarChar;

                    cmd.Parameters.AddWithValue("@EXIT_DATE", DBNull.Value);
                    cmd.Parameters["@EXIT_DATE"].SqlDbType = System.Data.SqlDbType.Date;

                    cmd.Parameters.AddWithValue("@EXIT_REASON", DBNull.Value);
                    cmd.Parameters["@EXIT_REASON"].SqlDbType = System.Data.SqlDbType.NVarChar;

                    cmd.Parameters.AddWithValue("@RETIREMMENT_DATE", RETIREMMENT_DATE);
                    cmd.Parameters["@RETIREMMENT_DATE"].SqlDbType = System.Data.SqlDbType.Date;

                    cmd.Parameters.AddWithValue("@CREATED_DATE", CREATED_DATE);
                    cmd.Parameters["@CREATED_DATE"].SqlDbType = System.Data.SqlDbType.DateTime;

                    cmd.Parameters.AddWithValue("@CREATED_BY", CREATED_BY);
                    cmd.Parameters["@CREATED_BY"].SqlDbType = System.Data.SqlDbType.NVarChar;

                    cmd.Parameters.AddWithValue("@UPDATED_DATE", DBNull.Value);
                    cmd.Parameters["@UPDATED_DATE"].SqlDbType = System.Data.SqlDbType.DateTime;

                    cmd.Parameters.AddWithValue("@UPDATED_BY", DBNull.Value);
                    cmd.Parameters["@UPDATED_BY"].SqlDbType = System.Data.SqlDbType.NVarChar;

                    cmd.Parameters.AddWithValue("@STATUS", STATUS);
                    cmd.Parameters["@STATUS"].SqlDbType = System.Data.SqlDbType.Char;

                    
                        Console.WriteLine("Inserting record for employee code: " + EMP_ID);
                        cmd.ExecuteNonQuery();
                        count++;
                    }
                    catch (Exception ex)
                    {
                        Console.Write(", Error : " + ex.Message);
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

        private static string GetCityName(string address)
        {
            string[] states = { "DELHI", "HARYANA", "JHARKHAND", "BIHAR", "MAHARASHTRA", "MADHYA PRADESH" };
            string cityName = string.Empty;
            string stateName = string.Empty;
            address = address.Trim();
            string[] splitedAddress = Regex.Split(address, ",");

            if (splitedAddress != null && splitedAddress.Length > 0)
            {
                //cityName = cityName = splitedAddress[splitedAddress.Length - 3]; on roll
                //contractor
                stateName = splitedAddress[splitedAddress.Length - 2].Trim();
                if (states.Contains(stateName.ToUpper()))
                {
                    cityName = splitedAddress[splitedAddress.Length - 3];
                    cityName = cityName.Replace("Dist:", string.Empty).ToUpper().Trim();
                }
            }

            return cityName;
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

        private static ArrayList GetStatePin(string address)
        {
            ArrayList statePinList = new ArrayList();
            address = address.Trim();
            string[] splitedAddress = Regex.Split(address, ",");
            int result;

            if (splitedAddress != null && splitedAddress.Length > 0)
            {
                string stateAndPin = splitedAddress[splitedAddress.Length - 1];
                if(stateAndPin.Length > 0 && stateAndPin.Contains("-"))
                {
                    string[] splittedStateAndPin = Regex.Split(stateAndPin, "-");
                    if(splittedStateAndPin.Length == 2)
                    {
                        statePinList.Add(splittedStateAndPin[0]);
                        statePinList.Add(splittedStateAndPin[1]);
                    }
                }                
                else if (int.TryParse(stateAndPin, out result))
                {
                    statePinList.Add(splitedAddress[splitedAddress.Length - 2]);
                    statePinList.Add(result);                    
                }
                else
                {
                    statePinList.Add(stateAndPin);
                }
            }

            return statePinList;
        }
    }
}
