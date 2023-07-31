using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Wfm.App.ConfigManager;
using Wfm.App.Core;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Interfaces;
using System.Data;
using Wfm.App.Common;
using System.Data.Entity;
using System.Globalization;
using System.Text;
using System.Data.Entity.Core.Objects;

namespace Wfm.App.Infrastructure.Repositories
{
    public class WorkforceRepository : IWorkforceRepository
    {
        private ApplicationEntities _appEntity;

        public WorkforceRepository()
        {
            _appEntity = new ApplicationEntities();
        }

        public List<WorkforceMasterMetaData1> GetWorkforceByDepartmentId(Guid departid)
        {
            List<WorkforceMasterMetaData1> employeeMasterMetaDatas = _appEntity.TAB_WORKFORCE_MASTER.Where(x => x.STATUS == "Y" && x.DEPT_ID == departid)
                 .Select(x => new WorkforceMasterMetaData1 { DEPT_ID = x.DEPT_ID.Value, USER_NAME = x.EMP_NAME + " - " + x.EMP_ID, USER_ID = x.WF_ID }).ToList();
            return employeeMasterMetaDatas;
        }

        public List<WorkforceMasterMetaData> GetEmployeeListByDepartmentId(Guid departId)
        {
            List<WorkforceMasterMetaData> employeeMasterMetaData = _appEntity.TAB_WORKFORCE_MASTER.Where(x => x.STATUS == "Y" && x.DEPT_ID == departId)
                .OrderBy(x => x.EMP_NAME)
                .Select(x => new WorkforceMasterMetaData { EMP_NAME = x.EMP_NAME + " - " + x.EMP_ID, WF_ID = x.WF_ID }).ToList();
            return employeeMasterMetaData;
        }

        public WorkforceSalaryMetaData FindByWFId(Guid wf_id)
        {
            WorkforceSalaryMetaData empsal = null;
            try
            {

                empsal = (from emp in _appEntity.TAB_WORKFORCE_MASTER
                          where emp.WF_ID == wf_id
                          select new WorkforceSalaryMetaData
                          {
                              EMP_NAME = emp.EMP_NAME,
                              WF_ID = emp.WF_ID,
                              EMP_ID = emp.EMP_ID

                          }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "WorkforceRepository", "Find", "", ex.ToString());
            }

            return empsal;
        }

        public List<ManPowerRequestFormMetaDataList> GetMRFList()
        {
            List<ManPowerRequestFormMetaDataList> lstMEF = new List<ManPowerRequestFormMetaDataList>();
            try
            {
                Guid companyid = SessionHelper.Get<Guid>("CompanyId");
                DataSet ds = new DataSet("MRFList");
                using (SqlConnection conn = new SqlConnection(Configurations.SqlConnectionString))
                {
                    SqlCommand sqlComm = new SqlCommand("SelectActiveMRF", conn);
                    sqlComm.Parameters.AddWithValue("@COMPANYID", companyid);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;
                    da.Fill(ds);

                    if (ds != null)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ManPowerRequestFormMetaDataList objMRF = new ManPowerRequestFormMetaDataList();
                            objMRF.MRF_ID = Convert.ToString(dr["MRF_ID"]);
                            objMRF.MRP_INETRNAL_ID = Guid.Parse(Convert.ToString(dr["MRP_INETRNAL_ID"]));
                            objMRF.COMPANY_ID = Guid.Parse(Convert.ToString(dr["COMPANY_ID"]));
                            objMRF.BUILDING_ID = Guid.Parse(Convert.ToString(dr["BUILDING_ID"]));
                            //objMRF.DEPT_ID = Guid.Parse(Convert.ToString(dr["DEPT_ID"]));
                            objMRF.SKILL_ID = Guid.Parse(Convert.ToString(dr["SKILL_ID"]));
                            objMRF.WF_DESIGNATION_ID = Convert.ToInt16(dr["WF_DESIGNATION_ID"]);
                            objMRF.MRF_STATUS = Convert.ToString(dr["MRF_STATUS"]);
                            objMRF.QUANTITY = Convert.ToInt16(dr["WF_QTY"]);
                            objMRF.WF_EMP_TYPE = Convert.ToInt16(dr["WF_EMP_TYPE"]);
                            lstMEF.Add(objMRF);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "WorkforceReporsitory", "GetMRFList", "", "SQL Error : " + ex.InnerException.ToString());
            }
            catch (Exception e)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "WorkforceReporsitory", "GetMRFList", "", e.InnerException.ToString());
            }
            return lstMEF;
        }

        public List<WorkforceSalaryMetaData> GetworkforceSalary(Guid? deptId, int? emptype_id)
        {
            Guid companyid = SessionHelper.Get<Guid>("CompanyId");
            var result = (from wfsal in _appEntity.TAB_WORKFORCE_SALARY_MASTER
                          join wf in _appEntity.TAB_WORKFORCE_MASTER on wfsal.WF_ID equals wf.WF_ID
                          where wfsal.COMPANY_ID == companyid && wf.DEPT_ID == deptId && wf.WF_EMP_TYPE == (emptype_id != null ? emptype_id : wf.WF_EMP_TYPE)
                          select new WorkforceSalaryMetaData
                          {
                              GROSS = wfsal.GROSS,
                              HRA = wfsal.HRA,
                              BASIC_DA = wfsal.BASIC_DA,
                              EMP_NAME = wf.EMP_NAME,
                              EMP_ID = wf.EMP_ID,
                              WF_ID = wfsal.WF_ID,
                              SPECIAL_ALLOWANCES = wfsal.SPECIAL_ALLOWANCES
                          }
                        ).ToList();
            return result;
        }

        public List<WorkforceSalaryMetaData> GetworkforceSalary(Guid? deptId, Guid? sub_dept_id, int? emptype_id, Guid? wf_id)
        {
            Guid companyid = SessionHelper.Get<Guid>("CompanyId");
            var result = (from wfsal in _appEntity.TAB_WORKFORCE_SALARY_MASTER
                          join wf in _appEntity.TAB_WORKFORCE_MASTER on wfsal.WF_ID equals wf.WF_ID
                          where wfsal.COMPANY_ID == companyid
                          && wf.DEPT_ID == deptId && wf.WF_EMP_TYPE == (emptype_id != null ? emptype_id : wf.WF_EMP_TYPE)
                          && wf.SUBDEPT_ID == (sub_dept_id != null ? sub_dept_id : wf.SUBDEPT_ID)
                          && wf.WF_ID == (wf_id != null ? wf_id : wf.WF_ID)
                          select new WorkforceSalaryMetaData
                          {
                              GROSS = wfsal.GROSS,
                              HRA = wfsal.HRA,
                              BASIC_DA = wfsal.BASIC_DA,
                              EMP_NAME = wf.EMP_NAME,
                              EMP_ID = wf.EMP_ID,
                              WF_ID = wfsal.WF_ID,
                              SPECIAL_ALLOWANCES = wfsal.SPECIAL_ALLOWANCES
                          }
                        ).ToList();
            return result;
        }

        public List<ExportSalaryMetaData> GetWorkForceSalarySlip(DownloadSalarySlip download)
        {
            string[] mm_yyyy = download.MM_YYYY.Split('-');
            int mm = Convert.ToInt32(mm_yyyy[0]);
            int yyyy = Convert.ToInt32(mm_yyyy[1]);
            var startOfMonth = new DateTime(yyyy, mm, 1);
            var DaysInMonth = DateTime.DaysInMonth(yyyy, mm);
            var lastDay = new DateTime(yyyy, mm, DaysInMonth);
            DateTime dt = DateTime.Now;
            var result = (from wf in _appEntity.TAB_WORKFORCE_MASTER
                          join wf_sm in _appEntity.TAB_WORKFORCE_SALARY_MASTER on wf.WF_ID equals wf_sm.WF_ID
                          join company in _appEntity.TAB_COMPANY_MASTER on wf.COMPANY_ID equals company.COMPANY_ID
                          join agency in _appEntity.TAB_AGENCY_MASTER on wf.AGENCY_ID equals agency.AGENCY_ID into jwf_agency
                          join dpt in _appEntity.TAB_DEPARTMENT_MASTER on wf.DEPT_ID equals dpt.DEPT_ID
                          join wfs in _appEntity.TAB_WORKFORCE_SALARY on wf.WF_ID equals wfs.WF_ID
                          from agency in jwf_agency.DefaultIfEmpty()
                              //from wf_sp in LOJ.DefaultIfEmpty()
                          where wf.DEPT_ID == download.DEPARTMENT_ID && wfs.STARTDATE == startOfMonth && wfs.ENDDATE == lastDay
                          && download.wfIds.Contains(wf.WF_ID)
                          select new WorkforceSalarySlip
                          {
                              EMP_NAME = wf.EMP_NAME,
                              EMP_CODE = wf.EMP_ID,
                              DEPARTMENT = dpt.DEPT_NAME,
                              UAN = wf_sm.UAN_NO,
                              ESIC = wf_sm.ESIC_NO,
                              W_BASIC_DA = wf_sm.BASIC_DA,
                              W_HR_ALL = wf_sm.HRA,
                              W_SPE_ALL = wf_sm.SPECIAL_ALLOWANCES,
                              W_ACTUAL_GROSS = wf_sm.GROSS,
                              ACTUAL_PAID_DAY = wfs.PAID_DAYS,
                              E_BASIC_DA = wfs.ACTUAL_BASIC_DA,
                              E_HR_ALL = wfs.ACTUAL_HRA,
                              E_SPE_ALL = wfs.ACTUAL_SPECIAL_ALLOWANCES,
                              E_PRO_BONUS = wfs.PRODUCTION_INCENTIVE_BONUS,
                              E_GROSS_SALARY = wfs.TOTAL_WAGES,
                              ACTUAL_WAGES_PAID = wfs.TOTAL_WAGES_AFTER_DEDUCTION,
                              WF_EPF = wfs.PF,
                              WF_ESI = wfs.ESI,
                              WF_TDS = wfs.TDS,
                              WF_FINE = wfs.SHOP_FLOOR_FINE,
                              WF_ADVANCE = wfs.ADVANCE,
                              ADMIN_CHARGE = wfs.ADMIN_CHARGES,
                              LEAVE_BALANCE = wfs.LEAVE_BALANCE,
                              LEAVE_TAKEN = wfs.LEAVE_DEDUCTION,

                              CONTRI_EPF = wfs.EMPLOYER_EPF,
                              CONTRI_ESI = wfs.EMPLOYER_EPF,
                              WF_EMP_TYPE = wf.WF_EMP_TYPE,
                              COMPANY_NAME = company.COMPANY_NAME,
                              COMPANY_ADDRESS1 = company.ADDRESS1,
                              COMPANY_ADDRESS2 = company.ADDRESS2,
                              AGENCY_NAME = agency != null ? agency.AGENCY_NAME : null,
                              AGENCY_ADDRESS1 = agency != null ? agency.AGENCY_ADDRESS : null,
                          }).ToList();
            var onAgentSalary = _appEntity.TAB_MAIL_TEMPLATE.Where(x => x.TEMPLATE_FOR == "GenerateOnAgentSalarySlip").FirstOrDefault();
            var onRoleSalary = _appEntity.TAB_MAIL_TEMPLATE.Where(x => x.TEMPLATE_FOR == "GenerateOnRoleSalarySlip").FirstOrDefault();
            List<ExportSalaryMetaData> salaryMetaDatas = new List<ExportSalaryMetaData>();
            #region Templete
            foreach (var item in result)
            {

                string mmmm_yy = lastDay.ToString("MMMM,yy");

                StringBuilder sb = new StringBuilder();
                if (item.WF_EMP_TYPE == (short)Enum_WFEmpType.Contract)
                {
                    string templete = onAgentSalary.TEMPLATE_CONTANT;
                    sb.Append(templete);
                    sb.Replace("[CMPNAME]", item.AGENCY_NAME);
                    sb.Replace("[CMPNAME_ADDRESS]", item.AGENCY_ADDRESS1);
                    sb.Replace("[WAGES_RATE]", (item.W_ACTUAL_GROSS).ToString());
                    sb.Replace("[WF_FINE]", (item.WF_FINE + item.WF_ADVANCE).ToString());

                }
                else
                {
                    string templete = onRoleSalary.TEMPLATE_CONTANT;
                    sb.Append(templete);
                    sb.Replace("[CMPNAME]", item.COMPANY_NAME);
                    sb.Replace("[CMPNAME_ADDRESS]", item.COMPANY_ADDRESS1 + " " + item.COMPANY_ADDRESS2);
                    sb.Replace("[W_BASIC_DA]", item.W_BASIC_DA.ToString());
                    sb.Replace("[W_HR_ALL]", item.W_HR_ALL.ToString());
                    sb.Replace("[W_SPE_ALL]", item.W_SPE_ALL.ToString());
                    sb.Replace("[W_ACTUAL_GROSS]", item.W_ACTUAL_GROSS.ToString());
                    sb.Replace("[E_SPE_ALL]", item.E_SPE_ALL.ToString());
                    sb.Replace("[DD_MM_YYYY]", dt.ToShortDateString());


                    sb.Replace("[E_PRO_BONUS]", item.E_PRO_BONUS.ToString());
                    sb.Replace("[WF_TDS]", item.WF_TDS.ToString());
                    sb.Replace("[WF_FINE]", item.WF_FINE.ToString());
                    sb.Replace("[WF_ADVANCE]", item.WF_ADVANCE.ToString());

                    sb.Replace("[ADMIN_CHARGE]", item.ADMIN_CHARGE.ToString());
                    sb.Replace("[LEAVE_TAKEN]", item.LEAVE_TAKEN.ToString());
                    sb.Replace("[LEAVE_BALANCE]", item.LEAVE_BALANCE.ToString());

                }


                sb.Replace("[MONTH_YY]", mmmm_yy);
                sb.Replace("[EMP_NAME]", item.EMP_NAME);
                sb.Replace("[EMP_CODE]", item.EMP_CODE);
                sb.Replace("[DEPARTMENT]", item.DEPARTMENT);
                sb.Replace("[UAN]", item.UAN);
                sb.Replace("[ESIC]", item.ESIC);
                sb.Replace("[ACTUAL_PAID_DAY]", item.ACTUAL_PAID_DAY.ToString());
                sb.Replace("[E_BASIC_DA]", item.E_BASIC_DA.ToString());
                sb.Replace("[E_HR_ALL]", item.E_HR_ALL.ToString());
                sb.Replace("[E_GROSS_SALARY]", item.E_GROSS_SALARY.ToString());
                sb.Replace("[WF_EPF]", item.WF_EPF.ToString());
                sb.Replace("[WF_ESI]", item.WF_ESI.ToString());
                sb.Replace("[ACTUAL_WAGES_PAID]", item.ACTUAL_WAGES_PAID.ToString());
                sb.Replace("[CONTRI_EPF]", item.CONTRI_EPF.ToString());
                sb.Replace("[CONTRI_ESI]", item.CONTRI_ESI.ToString());

                ExportSalaryMetaData salaryMetaData = new ExportSalaryMetaData
                {
                    SalarySlip = sb.ToString()
                };
                salaryMetaDatas.Add(salaryMetaData);

            }
            #endregion
            return salaryMetaDatas;
        }

        public SpecialAllowanceMetaData GetSpecialAllowanceById(Guid id)
        {
            Guid cmp_id = SessionHelper.Get<Guid>("CompanyId");
            var result = (from sp in _appEntity.TAB_SPECIAL_ALLOWANCE
                          join wf in _appEntity.TAB_WORKFORCE_MASTER on sp.WF_ID equals wf.WF_ID
                          where wf.COMPANY_ID == cmp_id && sp.SPECIAL_ALLOWANCE_ID == id
                          select new SpecialAllowanceMetaData
                          {
                              MONTH_ID = sp.MONTH_ID,
                              Amount = sp.AMOUNT,
                              WorkforceCode = wf.EMP_ID,
                              WorkforceName = wf.EMP_NAME,
                              DEPARTMENT_ID = wf.DEPT_ID.Value,
                              WF_EMP_TYPE = wf.WF_EMP_TYPE,
                              YEAR_ID = sp.YEAR_ID,
                              SUBDEPT_ID = wf.SUBDEPT_ID,
                              SpecialAllowanceId = sp.SPECIAL_ALLOWANCE_ID,
                              BUILDING_ID = wf.BUILDING_ID,
                              Remarks = sp.Remark
                          }
                         ).FirstOrDefault();
            return result;
        }

        public AddWorkForceItem GetOperationsById(Guid id, Guid? WF_ID)
        {
            Guid cmd_id = SessionHelper.Get<Guid>("CompanyId");
            var wfSalary = _appEntity.TAB_WORKFORCE_SALARY_MASTER.Where(x => x.WF_ID == WF_ID).FirstOrDefault();
            var dailySalary = (double)(wfSalary.BASIC_DA + wfSalary.HRA + wfSalary.SPECIAL_ALLOWANCES) / 26;

            var PerHoursSalary = dailySalary / 8;

            var opdetails = _appEntity.TAB_ITEM_OPERATION_MASTER.Where(x => x.UNIQUE_OPERATION_ID == id
            && x.COMPANY_ID == cmd_id).FirstOrDefault();
            if (opdetails.OPERATION == "SAMPLE MANUFACTURING" || opdetails.OPERATION == "DUTY")
            {
                return (from op in _appEntity.TAB_ITEM_OPERATION_MASTER
                        where op.UNIQUE_OPERATION_ID == id && op.COMPANY_ID == cmd_id
                        select new AddWorkForceItem
                        {
                            UNIQUE_OPERATION_ID = op.UNIQUE_OPERATION_ID,
                            ITEM_CODE = op.ITEM_CODE,
                            RATE = PerHoursSalary,
                            ITEM_ID = op.ITEM_ID,
                        }).FirstOrDefault();
            }
            else
            {
                return (from op in _appEntity.TAB_ITEM_OPERATION_MASTER
                        where op.UNIQUE_OPERATION_ID == id && op.COMPANY_ID == cmd_id
                        select new AddWorkForceItem
                        {
                            UNIQUE_OPERATION_ID = op.UNIQUE_OPERATION_ID,
                            ITEM_CODE = op.ITEM_CODE,
                            RATE = op.RATE,
                            ITEM_ID = op.ITEM_ID,
                        }).FirstOrDefault();
            }

        }


        public bool AddSpecialAllowance(SpecialAllowanceMetaData specialAllowance)
        {
            try
            {
                if (specialAllowance.ALLOWANCE_TYPE == 2)
                {
                    specialAllowance.Amount = -specialAllowance.Amount.Value;
                }
                if (specialAllowance.SpecialAllowanceId == new Guid())
                {


                    TAB_SPECIAL_ALLOWANCE model = new TAB_SPECIAL_ALLOWANCE
                    {
                        AMOUNT = specialAllowance.Amount.Value,
                        CREATED_BY = SessionHelper.Get<Guid>("UserId"),
                        CREATED_DATE = DateTime.Now,
                        MONTH_ID = specialAllowance.MONTH_ID,
                        YEAR_ID = specialAllowance.YEAR_ID,
                        WF_ID = specialAllowance.WF_ID,
                        SPECIAL_ALLOWANCE_ID = Guid.NewGuid(),
                        BUILDING_ID = specialAllowance.BUILDING_ID,
                        Remark = specialAllowance.Remarks
                    };
                    _appEntity.TAB_SPECIAL_ALLOWANCE.Add(model);
                }
                else
                {
                    var result = _appEntity.TAB_SPECIAL_ALLOWANCE.Where(x => x.SPECIAL_ALLOWANCE_ID == specialAllowance.SpecialAllowanceId).FirstOrDefault();
                    if (result != null)
                    {
                        result.AMOUNT = specialAllowance.Amount.Value;
                        result.UPDATED_BY = SessionHelper.Get<Guid>("UserId");
                        result.UPDATED_DATE = DateTime.Now;
                        result.MONTH_ID = specialAllowance.MONTH_ID;
                        result.YEAR_ID = specialAllowance.YEAR_ID;
                        result.Remark = specialAllowance.Remarks;
                    }
                }
                _appEntity.SaveChanges();
            }
            catch (Exception ex)
            {

                return false;
            }


            return true;
        }

        public List<SpecialAllowanceMetaData> GetSpecialAllowanceByDept(Guid? deptId, Guid? sub_dept_id, int? emptype_id, int? year_id, Guid? BUILDING_ID2)
        {
            Guid cmp_id = SessionHelper.Get<Guid>("CompanyId");
            var result = (from sp in _appEntity.TAB_SPECIAL_ALLOWANCE
                          join wf in _appEntity.TAB_WORKFORCE_MASTER on sp.WF_ID equals wf.WF_ID
                          where wf.COMPANY_ID == cmp_id &&
                          wf.DEPT_ID == (deptId != null ? deptId : wf.DEPT_ID)
                          && wf.WF_EMP_TYPE == (emptype_id != null ? emptype_id : wf.WF_EMP_TYPE)
                          && wf.SUBDEPT_ID == (sub_dept_id != null ? sub_dept_id : wf.SUBDEPT_ID)
                          && sp.YEAR_ID == (year_id != null ? year_id : sp.YEAR_ID)
                          && sp.BUILDING_ID == (BUILDING_ID2 != null ? BUILDING_ID2 : sp.BUILDING_ID)
                          select new SpecialAllowanceMetaData
                          {
                              MONTH_ID = sp.MONTH_ID,
                              Amount = sp.AMOUNT,
                              WorkforceCode = wf.EMP_ID,
                              WorkforceName = wf.EMP_NAME,
                              YEAR_ID = sp.YEAR_ID,
                              SpecialAllowanceId = sp.SPECIAL_ALLOWANCE_ID,
                              BUILDING_ID = wf.BUILDING_ID,
                              Remarks = sp.Remark
                          }
                         ).ToList();
            return result;
        }

        public WorkforceSalaryMetaData GetworkforceSalaryByWFId(Guid wfid)
        {
            var result = (from wf_sal in _appEntity.TAB_WORKFORCE_SALARY_MASTER
                          join wf in _appEntity.TAB_WORKFORCE_MASTER on wf_sal.WF_ID equals wf.WF_ID
                          where wf_sal.WF_ID == wfid
                          select new WorkforceSalaryMetaData
                          {
                              ESIC_NO = wf_sal.ESIC_NO,
                              WF_ID = wf_sal.WF_ID,
                              BANK_ACCOUNT_NO = wf_sal.BANK_ACCOUNT_NO,
                              BANK_BRANCH = wf_sal.BANK_BRANCH,
                              BANK_ID = wf_sal.BANK_ID,
                              BANK_IFSC = wf_sal.BANK_IFSC,
                              BASIC_DA = wf_sal.BASIC_DA,
                              COMPANY_ID = wf_sal.COMPANY_ID,
                              EMP_ID = wf.EMP_ID,
                              EMP_NAME = wf.EMP_NAME,
                              EPF_NO = wf_sal.EPF_NO,
                              GROSS = wf_sal.GROSS,
                              PAN_CARD = wf_sal.PAN_CARD,
                              HRA = wf_sal.HRA,
                              UAN_NO = wf_sal.UAN_NO,
                              SPECIAL_ALLOWANCES = wf_sal.SPECIAL_ALLOWANCES,
                          }).FirstOrDefault();
            return result;
        }

        public bool IsBiometricAvailable(string bIOMETRIC_CODE, Int64? AADHAR_NO)
        {
            var result = _appEntity.TAB_WORKFORCE_MASTER.Where(x => x.BIOMETRIC_CODE == bIOMETRIC_CODE.Trim()).FirstOrDefault();
            if (result != null && result.AADHAR_NO != AADHAR_NO)
            {
                return true;
            }
            return false;
        }

        public WorkforceMasterMetaData GetWorkforcByAadharNo(long aadharNo)
        {
            WorkforceMasterMetaData empdetail = null;
            try
            {
                empdetail = (from emp in _appEntity.TAB_WORKFORCE_MASTER
                             join desig in _appEntity.TAB_WF_DESIGNATION_MASTER on emp.WF_DESIGNATION_ID equals desig.WF_DESIGNATION_ID
                             join skil in _appEntity.TAB_SKILL_MASTER on emp.SKILL_ID equals skil.SKILL_ID
                             where emp.AADHAR_NO == aadharNo
                             select new WorkforceMasterMetaData
                             {
                                 EMP_ID = emp.EMP_ID,
                                 EMP_NAME = emp.EMP_NAME,
                                 WF_ID = emp.WF_ID,
                                 COMPANY_ID = emp.COMPANY_ID,
                                 FATHER_NAME = emp.FATHER_NAME,
                                 MOBILE_NO = emp.MOBILE_NO,
                                 DOJ = emp.DOJ,
                                 WF_EMP_TYPE = emp.WF_EMP_TYPE,
                                 AGENCY_ID = emp.AGENCY_ID,
                                 BIOMETRIC_CODE = emp.BIOMETRIC_CODE,
                                 GENDER = emp.GENDER,
                                 DEPT_ID = emp.DEPT_ID,
                                 DATE_OF_BIRTH = emp.DATE_OF_BIRTH,
                                 NATIONALITY = emp.NATIONALITY,
                                 WF_EDUCATION_ID = emp.WF_EDUCATION_ID,
                                 MARITAL_ID = emp.MARITAL_ID,
                                 DOJ_AS_PER_EPF = emp.DOJ_AS_PER_EPF,
                                 WF_DESIGNATION_NAME = desig.WF_DESIGNATION_NAME,
                                 SKILL_ID = emp.SKILL_ID,
                                 SKILL_NAME = skil.SKILL_NAME,
                                 EXIT_DATE = emp.EXIT_DATE,
                                 EMP_TYPE_ID = emp.EMP_TYPE_ID,
                                 ALTERNATE_NO = emp.ALTERNATE_NO,
                                 EMAIL_ID = emp.EMAIL_ID,
                                 PRESENT_ADDRESS = emp.PRESENT_ADDRESS,
                                 PRESENT_ADDRESS_CITY = emp.PRESENT_ADDRESS_CITY,
                                 PRESENT_ADDRESS_STATE = emp.PRESENT_ADDRESS_STATE,
                                 PRESENT_ADDRESS_PIN = emp.PRESENT_ADDRESS_PIN,
                                 PERMANENT_ADDRESS = emp.PERMANENT_ADDRESS,
                                 PERMANENT_ADDRESS_CITY = emp.PERMANENT_ADDRESS_CITY,
                                 PERMANENT_ADDRESS_STATE = emp.PERMANENT_ADDRESS_STATE,
                                 PERMANENT_ADDRESS_PIN = emp.PERMANENT_ADDRESS_PIN,
                                 AADHAR_NO = emp.AADHAR_NO,
                                 STATUS = (emp.STATUS == "Y" ? "Active" : "Unactive"),
                                 EMP_STATUS_ID = emp.EMP_STATUS_ID,
                                 IDENTIFICATION_MARK = emp.IDENTIFICATION_MARK,

                             }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "WorkforceRepository", "FindWorkforceIWithFullDetailByWFId", "", ex.ToString());
            }

            return empdetail;
        }
        public bool IS_EMPID_EXIST(string EMP_ID, Int64? AADHAR_NO)
        {
            var result = _appEntity.TAB_WORKFORCE_MASTER.Where(x => x.EMP_ID == EMP_ID.Trim()).FirstOrDefault();
            if (result != null && result.AADHAR_NO != AADHAR_NO)
            {
                return true;
            }
            return false;
        }

        public List<MRFDLL> GetApprovedAndNotHireMRF()
        {
            List<MRFDLL> listMRF = new List<MRFDLL>();
            Guid companyid = SessionHelper.Get<Guid>("CompanyId");
            string LoginUserId = SessionHelper.Get<string>("LoginUserId");
            string LoginType = SessionHelper.Get<string>("LoginType");

            var result = (from mrf in _appEntity.TAB_MRF
                          join mrfd in _appEntity.TAB_MRF_DETAILS on mrf.MRP_INETRNAL_ID equals mrfd.MRP_INETRNAL_ID
                          join dm in _appEntity.TAB_DEPARTMENT_MASTER on mrfd.FLOOR_ID equals dm.DEPT_ID
                          where mrf.MRF_STATUS == "Approved"
                          && mrfd.HIRING_QUANTITY != mrfd.WOKFORCE_QUANTITY
                          && mrf.COMPANY_ID == companyid
                          && (mrfd.CREATED_BY == LoginUserId || LoginType == "ADMIN - IT")
                          select new
                          {
                              mrf.MRP_INETRNAL_ID,
                              mrf.MRF_CODE,
                              dm.DEPT_NAME,
                              mrfd.HIRING_QUANTITY,
                              mrfd.WOKFORCE_QUANTITY
                          }).ToList();
            listMRF = result.Select(x => new MRFDLL
            {
                MRF_ID = x.MRP_INETRNAL_ID,
                MRF_CODE = x.MRF_CODE.ToString("00000") + "_" + x.DEPT_NAME.ToString() + " (" + Convert.ToInt32(x.WOKFORCE_QUANTITY - x.HIRING_QUANTITY).ToString() + ")",

            }).ToList();
            return listMRF;
        }

        public List<WorkforceMasterMetaData> GetEmployeesBydeptIdAutoComplete(Guid departId, string emp)
        {
            List<WorkforceMasterMetaData> employeeMasterMetaData = _appEntity.TAB_WORKFORCE_MASTER.Where(x => x.STATUS == "Y" && x.DEPT_ID == departId && (x.EMP_ID.StartsWith(emp) || x.EMP_NAME.StartsWith(emp)))
                 .OrderBy(x => x.EMP_NAME)
                 .Select(x => new WorkforceMasterMetaData { EMP_NAME = x.EMP_NAME, WF_ID = x.WF_ID, EMP_ID = x.EMP_ID }).ToList();
            return employeeMasterMetaData;
        }

        public void Create(WorkforceMetaData workforce)
        {
            try
            {
                var mrf_detail = _appEntity.TAB_MRF_DETAILS.Where(x => x.MRP_INETRNAL_ID == workforce.MRF_INTERNAL_ID).FirstOrDefault();
                mrf_detail.HIRING_QUANTITY = mrf_detail.HIRING_QUANTITY + 1;
                workforce.WF_DESIGNATION_ID = mrf_detail.WF_DESIGNATION_ID.Value;
                workforce.SKILL_ID = mrf_detail.SKILL_ID.Value;
                workforce.WF_EMP_TYPE = mrf_detail.WF_EMP_TYPE.Value;

                if (workforce.WF_ID == Guid.Empty)
                {
                    workforce.WF_ID = Guid.NewGuid();
                    string cmp_name = string.Empty;
                    if (workforce.WF_EMP_TYPE == Convert.ToInt32(WF_EMP_TYPE.OnRoll))
                    {
                        cmp_name = _appEntity.TAB_COMPANY_MASTER.Where(x => x.COMPANY_ID == workforce.COMPANY_ID).FirstOrDefault().COMPANY_NAME;
                    }
                    else
                    {
                        cmp_name = _appEntity.TAB_AGENCY_MASTER.Where(x => x.AGENCY_ID == workforce.AGENCY_ID).FirstOrDefault().AGENCY_NAME;
                    }
                    var empCount = _appEntity.TAB_CONSTANT_DATA.FirstOrDefault();
                    if (workforce.EMP_ID == "" || workforce.EMP_ID == null)
                    {
                        workforce.EMP_ID = cmp_name.Substring(0, 3).Trim() + empCount.WORKFORC_COUNT.ToString("000000");
                    }

                    empCount.WORKFORC_COUNT = empCount.WORKFORC_COUNT + 1;
                    List<TAB_WORKFORCE_PHOTO_MASTER> images = new List<TAB_WORKFORCE_PHOTO_MASTER>() {
                            new TAB_WORKFORCE_PHOTO_MASTER
                            {
                                PHOTO_ID = Guid.NewGuid(),
                                PHOTO = workforce.PHOTO,
                                EMP_SIGNATURE = workforce.EMP_SIGNATURE,
                                CREATED_BY = SessionHelper.Get<String>("Username"),
                                CREATED_DATE = DateTime.Now,
                            }
                    };
                    Wfm.App.Core.TAB_WORKFORCE_MASTER obj = new Wfm.App.Core.TAB_WORKFORCE_MASTER
                    {
                        WF_ID = workforce.WF_ID,
                        EMP_ID = workforce.EMP_ID,
                        WF_EMP_TYPE = workforce.WF_EMP_TYPE,
                        COMPANY_ID = workforce.COMPANY_ID,
                        AGENCY_ID = workforce.AGENCY_ID,
                        BIOMETRIC_CODE = workforce.BIOMETRIC_CODE,
                        REFERENCE_ID = workforce.REFERENCE_ID,
                        EMP_NAME = workforce.EMP_NAME.ToUpper(),
                        FATHER_NAME = workforce.FATHER_NAME.ToUpper(),
                        GENDER = workforce.GENDER,
                        DEPT_ID = workforce.DEPT_ID,
                        SUBDEPT_ID = workforce.SUBDEPT_ID,

                        DATE_OF_BIRTH = workforce.DATE_OF_BIRTH,
                        NATIONALITY = workforce.NATIONALITY,
                        WF_EDUCATION_ID = workforce.WF_EDUCATION_ID,
                        MARITAL_ID = workforce.MARITAL_ID,
                        DOJ = workforce.DOJ,
                        DOJ_AS_PER_EPF = workforce.DOJ_AS_PER_EPF,
                        WF_DESIGNATION_ID = workforce.WF_DESIGNATION_ID,
                        SKILL_ID = workforce.SKILL_ID,
                        EMP_TYPE_ID = workforce.EMP_TYPE_ID,
                        MOBILE_NO = workforce.MOBILE_NO,
                        ALTERNATE_NO = workforce.ALTERNATE_NO,
                        EMAIL_ID = workforce.EMAIL_ID,
                        PRESENT_ADDRESS = workforce.PRESENT_ADDRESS.ToUpper(),
                        PRESENT_ADDRESS_CITY = workforce.PRESENT_ADDRESS_CITY,
                        PRESENT_ADDRESS_STATE = workforce.PRESENT_ADDRESS_STATE,
                        PRESENT_ADDRESS_PIN = workforce.PRESENT_ADDRESS_PIN,
                        PERMANENT_ADDRESS = workforce.PERMANENT_ADDRESS.ToUpper(),
                        PERMANENT_ADDRESS_CITY = workforce.PERMANENT_ADDRESS_CITY,
                        PERMANENT_ADDRESS_STATE = workforce.PERMANENT_ADDRESS_STATE,
                        PERMANENT_ADDRESS_PIN = workforce.PERMANENT_ADDRESS_PIN,
                        AADHAR_NO = workforce.AADHAR_NO,
                        EMP_STATUS_ID = workforce.EMP_STATUS_ID,
                        IDENTIFICATION_MARK = workforce.IDENTIFICATION_MARK,
                        MRF_INTERNAL_ID = workforce.MRF_INTERNAL_ID,
                        STATUS = "Y",
                        TAB_WORKFORCE_PHOTO_MASTER = images,
                        CREATED_DATE = DateTime.Now,
                        CREATED_BY = SessionHelper.Get<String>("Username"),
                        BUILDING_ID = workforce.BUILDING_ID
                    };
                    _appEntity.TAB_WORKFORCE_MASTER.Add(obj);
                    _appEntity.SaveChanges();
                }
                else
                {
                    Edit(workforce);
                }
            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "WorkforceReporsitory", "Create", "", ex.InnerException.ToString());
            }
        }

        public List<WorkforceSalaryData> GetEmpSalary(Guid deptId, Guid? sub_dept_id, int month, int year, short wfEmpType)
        {
            List<WorkforceSalaryData> salList = new List<WorkforceSalaryData>();
            try
            {
                Guid loggedInUserId = Utility.GetLoggedInUserId();

                //select all department salary
                if (deptId == Guid.Parse("BD5E4EF7-DC4F-43C9-A223-8954B4CB6455"))
                {
                    salList = (from emp in _appEntity.TAB_WORKFORCE_MASTER
                               join dept in _appEntity.TAB_DEPARTMENT_MASTER on emp.DEPT_ID equals dept.DEPT_ID
                               join desig in _appEntity.TAB_WF_DESIGNATION_MASTER on emp.WF_DESIGNATION_ID equals desig.WF_DESIGNATION_ID
                               join salary in _appEntity.TAB_WORKFORCE_SALARY on emp.WF_ID equals salary.WF_ID
                               join user_depart_map in _appEntity.TAB_USER_DEPARTMENT_MAPPING on dept.DEPT_ID equals user_depart_map.DEPT_ID
                               join login_master in _appEntity.TAB_LOGIN_MASTER on user_depart_map.USER_ID equals login_master.USER_ID
                               where login_master.USER_ID == loggedInUserId
                               && emp.WF_EMP_TYPE == wfEmpType
                               && salary.SALARY_MONTH == month && salary.STARTDATE.Value.Year == year
                               && emp.SUBDEPT_ID == (sub_dept_id != null ? sub_dept_id : emp.SUBDEPT_ID)
                               select new WorkforceSalaryData
                               {
                                   EMP_ID = emp.EMP_ID,
                                   EMP_NAME = emp.EMP_NAME,
                                   DEPT = dept.DEPT_NAME,
                                   DESIGNATION = desig.WF_DESIGNATION_NAME,
                                   SALARY_MONTH = salary.SALARY_MONTH,
                                   STARTDATE = salary.STARTDATE,
                                   ENDDATE = salary.ENDDATE,
                                   PAID_DAYS = salary.PAID_DAYS,
                                   OVERTIME_DAYS = salary.OVERTIME_HOURS,
                                   TOTAL_LEAVE_TAKEN = salary.TOTAL_LEAVE_TAKEN_CURRENT_MONTH,
                                   TOTAL_LEAVE_AVAILABLE = salary.TOTAL_LEAVE_AVAILABLE,
                                   LEAVE_ADJUSTED = salary.LEAVE_ADJUSTED,
                                   TOTAL_LEAVE_BALANCE = salary.LEAVE_BALANCE,
                                   BASIC_DA = salary.BASIC_DA,
                                   HRA = salary.HRA,
                                   SPECIAL_ALLOWANCES = salary.SPECIAL_ALLOWANCES,
                                   PRODUCTION_INCENTIVE_BONUS = salary.PRODUCTION_INCENTIVE_BONUS,
                                   PF = salary.PF,
                                   ESI = salary.ESI,
                                   TDS = salary.TDS,
                                   SHOP_FLOOR_FINE = salary.SHOP_FLOOR_FINE,
                                   OTHER_DEDUCTION = salary.OTHER_DEDUCTION,
                                   ADVANCE = salary.ADVANCE,
                                   OVERTIME_WAGES = salary.OVERTIME_WAGES,
                                   WORKINGDAY_WAGES = salary.WORKINGDAY_WAGES,
                                   TOTAL_WAGES = salary.TOTAL_WAGES,
                                   TOTAL_WAGES_AFTER_DEDUCTION = salary.TOTAL_WAGES_AFTER_DEDUCTION,
                                   EMPLOYER_EPF = salary.EMPLOYER_EPF,
                                   EMPLOYER_ESI = salary.EMPLOYER_ESI,
                                   ADMIN_CHARGES = salary.ADMIN_CHARGES,
                                   EDLI_CHARGES = salary.EDLI_CHARGES,
                                   CREATED_ON = salary.CREATED_ON,
                                   MODE_OF_PAYMENT = salary.MODE_OF_PAYMENT,
                                   PAID_STATUS = salary.PAID_STATUS
                               }).ToList();
                }
                else
                {
                    //// Put Current Month Check
                    salList = (from emp in _appEntity.TAB_WORKFORCE_MASTER
                               join dept in _appEntity.TAB_DEPARTMENT_MASTER on emp.DEPT_ID equals dept.DEPT_ID
                               join desig in _appEntity.TAB_WF_DESIGNATION_MASTER on emp.WF_DESIGNATION_ID equals desig.WF_DESIGNATION_ID
                               join salary in _appEntity.TAB_WORKFORCE_SALARY on emp.WF_ID equals salary.WF_ID
                               where emp.WF_EMP_TYPE == wfEmpType && dept.DEPT_ID == deptId && salary.SALARY_MONTH == month && salary.STARTDATE.Value.Year == year
                               select new WorkforceSalaryData
                               {
                                   EMP_ID = emp.EMP_ID,
                                   EMP_NAME = emp.EMP_NAME,
                                   DEPT = dept.DEPT_NAME,
                                   DESIGNATION = desig.WF_DESIGNATION_NAME,
                                   SALARY_MONTH = salary.SALARY_MONTH,
                                   STARTDATE = salary.STARTDATE,
                                   ENDDATE = salary.ENDDATE,
                                   PAID_DAYS = salary.PAID_DAYS,
                                   OVERTIME_DAYS = salary.OVERTIME_HOURS,
                                   TOTAL_LEAVE_TAKEN = salary.TOTAL_LEAVE_TAKEN_CURRENT_MONTH,
                                   TOTAL_LEAVE_AVAILABLE = salary.TOTAL_LEAVE_AVAILABLE,
                                   LEAVE_ADJUSTED = salary.LEAVE_ADJUSTED,
                                   TOTAL_LEAVE_BALANCE = salary.LEAVE_BALANCE,
                                   BASIC_DA = salary.BASIC_DA,
                                   HRA = salary.HRA,
                                   SPECIAL_ALLOWANCES = salary.SPECIAL_ALLOWANCES,
                                   PRODUCTION_INCENTIVE_BONUS = salary.PRODUCTION_INCENTIVE_BONUS,
                                   PF = salary.PF,
                                   ESI = salary.ESI,
                                   TDS = salary.TDS,
                                   SHOP_FLOOR_FINE = salary.SHOP_FLOOR_FINE,
                                   OTHER_DEDUCTION = salary.OTHER_DEDUCTION,
                                   ADVANCE = salary.ADVANCE,
                                   OVERTIME_WAGES = salary.OVERTIME_WAGES,
                                   WORKINGDAY_WAGES = salary.WORKINGDAY_WAGES,
                                   TOTAL_WAGES = salary.TOTAL_WAGES,
                                   TOTAL_WAGES_AFTER_DEDUCTION = salary.TOTAL_WAGES_AFTER_DEDUCTION,
                                   EMPLOYER_EPF = salary.EMPLOYER_EPF,
                                   EMPLOYER_ESI = salary.EMPLOYER_ESI,
                                   ADMIN_CHARGES = salary.ADMIN_CHARGES,
                                   EDLI_CHARGES = salary.EDLI_CHARGES,
                                   CREATED_ON = salary.CREATED_ON,
                                   MODE_OF_PAYMENT = salary.MODE_OF_PAYMENT,
                                   PAID_STATUS = salary.PAID_STATUS
                               }).ToList();
                }
            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "WorkforceReporsitory", "GetEmpSalary", "", ex.InnerException.ToString());
            }

            return salList;

        }

        public List<WorkforceMetaDataList> GetEmpAllItems(Guid? deptId, Guid? sub_dept_id, int? emptype_id, Guid? BUILDING_ID)
        {
            string deptIds = deptId.ToString();
            string sub_dept_ids = sub_dept_id.ToString();
            if (deptIds == "00000000-0000-0000-0000-000000000000")
            {
                deptIds = "";
            }
            if (sub_dept_ids == "00000000-0000-0000-0000-000000000000")
            {
                sub_dept_ids = "";
            }
            Guid cmp_id = SessionHelper.Get<Guid>("CompanyId");

            List<WorkforceMetaDataList> empMetaDataLists = new List<WorkforceMetaDataList>();

            try
            {
                empMetaDataLists = (from emp in _appEntity.TAB_WORKFORCE_MASTER
                                    join ut in _appEntity.TAB_BUILDING_MASTER on emp.BUILDING_ID equals ut.BUILDING_ID
                                    join dept in _appEntity.TAB_DEPARTMENT_MASTER on emp.DEPT_ID equals dept.DEPT_ID
                                    join Sdept in _appEntity.TAB_SUBDEPARTMENT_MASTER on emp.SUBDEPT_ID equals Sdept.SUBDEPT_ID
                                    join desig in _appEntity.TAB_WF_DESIGNATION_MASTER on emp.WF_DESIGNATION_ID equals
                                    desig.WF_DESIGNATION_ID
                                    join wt in _appEntity.TAB_WORFORCE_TYPE on emp.WF_EMP_TYPE equals wt.WF_EMP_TYPE
                                    join esm in _appEntity.TAB_EMP_STATUS_MASTER on emp.EMP_STATUS_ID equals esm.EMP_STATUS_ID
                                    join skm in _appEntity.TAB_SKILL_MASTER on emp.SKILL_ID equals skm.SKILL_ID
                                    join et in _appEntity.TAB_EMPL_TYPE_MASTER on emp.EMP_TYPE_ID equals et.EMP_TYPE_ID
                                    join edu in _appEntity.TAB_WORKFORCE_EDU_MASTER on emp.WF_EDUCATION_ID equals edu.WF_EDUCATION_ID
                                    //join sm1 in _appEntity.TAB_WORKFORCE_SALARY_MASTER on emp.WF_ID equals sm1.WF_ID
                                    //join bnl in _appEntity.TAB_BANK_MASTER on sm1.BANK_ID equals bnl.BANK_ID
                                    join sm in _appEntity.TAB_WORKFORCE_SALARY_MASTER on emp.WF_ID equals sm.WF_ID into salM
                                    from sm in salM.DefaultIfEmpty()

                                    where emp.COMPANY_ID == cmp_id
                                    && (emp.DEPT_ID == deptId || deptIds == "")
                                    && (emp.SUBDEPT_ID == (sub_dept_id != null ? sub_dept_id : emp.SUBDEPT_ID) || sub_dept_ids == "")
                                    && emp.WF_EMP_TYPE == (emptype_id != null ? emptype_id : emp.WF_EMP_TYPE)
                                    && emp.BUILDING_ID == (BUILDING_ID != null ? BUILDING_ID : emp.BUILDING_ID)
                                    && emp.STATUS == "Y"
                                    select new WorkforceMetaDataList
                                    {
                                        EMP_ID = emp.EMP_ID,
                                        WFEMPTYPE = wt.EMP_TYPE,
                                        EMP_Status = esm.EMP_STATUS,
                                        BIOMETRIC_CODE = emp.BIOMETRIC_CODE,
                                        EMP_NAME = emp.EMP_NAME,
                                        FATHER_NAME = emp.FATHER_NAME,
                                        GENDER = (emp.GENDER == "M" ? "MALE" : "FEMALE"),
                                        DATE_OF_BIRTH = emp.DATE_OF_BIRTH,
                                        UNIT_NAME = ut.BUILDING_NAME,
                                        DEPT_NAME = dept.DEPT_NAME,
                                        SUBDEPT_NAME = Sdept.SUBDEPT_NAME,
                                        NATIONALITY = emp.NATIONALITY,
                                        DEPT_ID = dept.DEPT_ID,
                                        DEPT = dept.DEPT_NAME,
                                        WF_DESIGNATION = desig.WF_DESIGNATION_NAME,
                                        DOJ = emp.DOJ,
                                        MOBILE_NO = emp.MOBILE_NO,
                                        AADHAR_NO = emp.AADHAR_NO,
                                        STATUS = emp.STATUS,
                                        WF_ID = emp.WF_ID,
                                        WF_EMP_TYPE = emp.WF_EMP_TYPE,
                                        SKILL_NAME = skm.SKILL_NAME,
                                        ETYPE = et.EMP_TYPE,
                                        ALTERNATE_NO = emp.ALTERNATE_NO,
                                        PRESENT_ADDRESS = emp.PRESENT_ADDRESS,
                                        PERMANENT_ADDRESS = emp.PERMANENT_ADDRESS,
                                        EMAIL_ID = emp.EMAIL_ID,
                                        Education = edu.WF_COURSE_NAME,
                                        HRA = sm.HRA.ToString(),
                                        GROSS = sm.GROSS.ToString(),
                                        SPECIAL_ALLOWANCE = sm.SPECIAL_ALLOWANCES.ToString(),
                                        BASIC_DA = sm.BASIC_DA.ToString(),
                                        UAN = sm.UAN_NO.ToString(),
                                        PAN = sm.PAN_CARD.ToString(),
                                        EPF = sm.EPF_NO.ToString(),
                                        ESIC = sm.ESIC_NO.ToString(),
                                        ACCNO = sm.BANK_ACCOUNT_NO.ToString(),
                                        IFSC = sm.BANK_IFSC.ToString(),
                                        BranchName = sm.BANK_BRANCH.ToString(),

                                    }).ToList();
            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "WorkforceReporsitory", "GetEmpAllItems", "", ex.InnerException.ToString());
            }

            return empMetaDataLists;
        }
        public List<WorkforceMetaDataList> GetEmpAllItems_PIB(Guid? deptId, Guid? sub_dept_id, int? emptype_id, Guid? BUILDING_ID)
        {
            string deptIds = deptId.ToString();
            string sub_dept_ids = sub_dept_id.ToString();
            if (deptIds == "00000000-0000-0000-0000-000000000000")
            {
                deptIds = "";
            }
            if (sub_dept_ids == "00000000-0000-0000-0000-000000000000")
            {
                sub_dept_ids = "";
            }
            Guid cmp_id = SessionHelper.Get<Guid>("CompanyId");

            List<WorkforceMetaDataList> empMetaDataLists = new List<WorkforceMetaDataList>();

            try
            {
                empMetaDataLists = (from emp in _appEntity.TAB_WORKFORCE_MASTER
                                    join ut in _appEntity.TAB_BUILDING_MASTER on emp.BUILDING_ID equals ut.BUILDING_ID
                                    join dept in _appEntity.TAB_DEPARTMENT_MASTER on emp.DEPT_ID equals dept.DEPT_ID
                                    join Sdept in _appEntity.TAB_SUBDEPARTMENT_MASTER on emp.SUBDEPT_ID equals Sdept.SUBDEPT_ID
                                    join desig in _appEntity.TAB_WF_DESIGNATION_MASTER on emp.WF_DESIGNATION_ID equals
                                    desig.WF_DESIGNATION_ID
                                    join wt in _appEntity.TAB_WORFORCE_TYPE on emp.WF_EMP_TYPE equals wt.WF_EMP_TYPE
                                    join esm in _appEntity.TAB_EMP_STATUS_MASTER on emp.EMP_STATUS_ID equals esm.EMP_STATUS_ID
                                    join skm in _appEntity.TAB_SKILL_MASTER on emp.SKILL_ID equals skm.SKILL_ID
                                    join et in _appEntity.TAB_EMPL_TYPE_MASTER on emp.EMP_TYPE_ID equals et.EMP_TYPE_ID
                                    join edu in _appEntity.TAB_WORKFORCE_EDU_MASTER on emp.WF_EDUCATION_ID equals edu.WF_EDUCATION_ID
                                    //join sm1 in _appEntity.TAB_WORKFORCE_SALARY_MASTER on emp.WF_ID equals sm1.WF_ID
                                    //join bnl in _appEntity.TAB_BANK_MASTER on sm1.BANK_ID equals bnl.BANK_ID
                                    join sm in _appEntity.TAB_WORKFORCE_SALARY_MASTER on emp.WF_ID equals sm.WF_ID into salM
                                    from sm in salM.DefaultIfEmpty()

                                    where emp.COMPANY_ID == cmp_id
                                    && (emp.DEPT_ID == deptId || deptIds == "")
                                    && (emp.SUBDEPT_ID == (sub_dept_id != null ? sub_dept_id : emp.SUBDEPT_ID) || sub_dept_ids == "")
                                    && emp.WF_EMP_TYPE == (emptype_id != null ? emptype_id : emp.WF_EMP_TYPE)
                                    && emp.BUILDING_ID == (BUILDING_ID != null ? BUILDING_ID : emp.BUILDING_ID)
                                    && emp.STATUS == "Y" && emp.EMP_TYPE_ID == 2
                                    select new WorkforceMetaDataList
                                    {
                                        EMP_ID = emp.EMP_ID,
                                        WFEMPTYPE = wt.EMP_TYPE,
                                        EMP_Status = esm.EMP_STATUS,
                                        BIOMETRIC_CODE = emp.BIOMETRIC_CODE,
                                        EMP_NAME = emp.EMP_NAME,
                                        FATHER_NAME = emp.FATHER_NAME,
                                        GENDER = (emp.GENDER == "M" ? "MALE" : "FEMALE"),
                                        DATE_OF_BIRTH = emp.DATE_OF_BIRTH,
                                        UNIT_NAME = ut.BUILDING_NAME,
                                        DEPT_NAME = dept.DEPT_NAME,
                                        SUBDEPT_NAME = Sdept.SUBDEPT_NAME,
                                        NATIONALITY = emp.NATIONALITY,
                                        DEPT_ID = dept.DEPT_ID,
                                        DEPT = dept.DEPT_NAME,
                                        WF_DESIGNATION = desig.WF_DESIGNATION_NAME,
                                        DOJ = emp.DOJ,
                                        MOBILE_NO = emp.MOBILE_NO,
                                        AADHAR_NO = emp.AADHAR_NO,
                                        STATUS = emp.STATUS,
                                        WF_ID = emp.WF_ID,
                                        WF_EMP_TYPE = emp.WF_EMP_TYPE,
                                        SKILL_NAME = skm.SKILL_NAME,
                                        ETYPE = et.EMP_TYPE,
                                        ALTERNATE_NO = emp.ALTERNATE_NO,
                                        PRESENT_ADDRESS = emp.PRESENT_ADDRESS,
                                        PERMANENT_ADDRESS = emp.PERMANENT_ADDRESS,
                                        EMAIL_ID = emp.EMAIL_ID,
                                        Education = edu.WF_COURSE_NAME,
                                        HRA = sm.HRA.ToString(),
                                        GROSS = sm.GROSS.ToString(),
                                        SPECIAL_ALLOWANCE = sm.SPECIAL_ALLOWANCES.ToString(),
                                        BASIC_DA = sm.BASIC_DA.ToString(),
                                        UAN = sm.UAN_NO.ToString(),
                                        PAN = sm.PAN_CARD.ToString(),
                                        EPF = sm.EPF_NO.ToString(),
                                        ESIC = sm.ESIC_NO.ToString(),
                                        ACCNO = sm.BANK_ACCOUNT_NO.ToString(),
                                        IFSC = sm.BANK_IFSC.ToString(),
                                        BranchName = sm.BANK_BRANCH.ToString(),

                                    }).ToList();
            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "WorkforceReporsitory", "GetEmpAllItems", "", ex.InnerException.ToString());
            }

            return empMetaDataLists;
        }

        public List<WorkforceMetaDataList> GetEmployessforAddSalary(Guid? deptId, Guid? sub_dept_id, int? emptype_id, Guid? BUILDING_ID)
        {
            string deptIds = deptId.ToString();
            string sub_dept_ids = sub_dept_id.ToString();
            if (deptIds == "00000000-0000-0000-0000-000000000000")
            {
                deptIds = "";
            }
            if (sub_dept_ids == "00000000-0000-0000-0000-000000000000")
            {
                sub_dept_ids = "";
            }
            Guid cmp_id = SessionHelper.Get<Guid>("CompanyId");

            List<WorkforceMetaDataList> empMetaDataLists = new List<WorkforceMetaDataList>();

            try
            {
                empMetaDataLists = (from emp in _appEntity.TAB_WORKFORCE_MASTER
                                    join dept in _appEntity.TAB_DEPARTMENT_MASTER on emp.DEPT_ID equals dept.DEPT_ID
                                    join desig in _appEntity.TAB_WF_DESIGNATION_MASTER on emp.WF_DESIGNATION_ID equals desig.WF_DESIGNATION_ID
                                    join sm in _appEntity.TAB_WORKFORCE_SALARY_MASTER on emp.WF_ID equals sm.WF_ID into salaries
                                    from sm in salaries.DefaultIfEmpty()
                                    where emp.COMPANY_ID == cmp_id
                                    && (emp.DEPT_ID == deptId || deptIds == "")
                                    && (emp.SUBDEPT_ID == (sub_dept_id != null ? sub_dept_id : emp.SUBDEPT_ID) || sub_dept_ids == "")
                                    && emp.WF_EMP_TYPE == (emptype_id != null ? emptype_id : emp.WF_EMP_TYPE)
                                    && emp.BUILDING_ID == (BUILDING_ID != null ? BUILDING_ID : emp.BUILDING_ID)
                                    && sm.WF_ID != emp.WF_ID
                                    //&& (emp.CREATED_BY == UserId || UserId=="admin")
                                    select new WorkforceMetaDataList
                                    {
                                        EMP_ID = emp.EMP_ID,
                                        EMP_NAME = emp.EMP_NAME,
                                        DEPT_ID = dept.DEPT_ID,
                                        DEPT = dept.DEPT_NAME,
                                        WF_DESIGNATION = desig.WF_DESIGNATION_NAME,
                                        DOJ = emp.DOJ,
                                        MOBILE_NO = emp.MOBILE_NO,
                                        AADHAR_NO = emp.AADHAR_NO,
                                        STATUS = emp.STATUS,
                                        WF_ID = emp.WF_ID,
                                        WF_EMP_TYPE = emp.WF_EMP_TYPE
                                    }).ToList();
            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "WorkforceReporsitory", "GetEmpAllItems", "", ex.InnerException.ToString());
            }

            return empMetaDataLists;
        }



        public List<WorkforceMetaDataList> BindWorkforceByWFType(Guid? deptId, Guid? sub_dept_id, int? emptype_id, Guid? BUILDING_ID)
        {
            Guid cmp_id = SessionHelper.Get<Guid>("CompanyId");
            List<WorkforceMetaDataList> empMetaDataLists = new List<WorkforceMetaDataList>(); 
            try
            {
                empMetaDataLists = (from emp in _appEntity.TAB_WORKFORCE_MASTER
                                    join dept in _appEntity.TAB_DEPARTMENT_MASTER on emp.DEPT_ID equals dept.DEPT_ID
                                    join desig in _appEntity.TAB_WF_DESIGNATION_MASTER on emp.WF_DESIGNATION_ID equals desig.WF_DESIGNATION_ID
                                    where emp.COMPANY_ID == cmp_id
                                    && emp.DEPT_ID == deptId
                                    && (emp.SUBDEPT_ID == sub_dept_id || sub_dept_id.ToString() == "00000000-0000-0000-0000-000000000000")
                                    //&& emp.SUBDEPT_ID == (sub_dept_id != null ? sub_dept_id : emp.SUBDEPT_ID)
                                    //&& emp.WF_EMP_TYPE == (emptype_id != null ? emptype_id : emp.WF_EMP_TYPE)
                                    && emp.WF_EMP_TYPE == emptype_id
                                    && emp.BUILDING_ID == BUILDING_ID
                                    && emp.STATUS == "Y"
                                    select new WorkforceMetaDataList
                                    {
                                        EMP_ID = emp.EMP_ID,
                                        EMP_NAME = emp.EMP_NAME + " - " + emp.EMP_ID,
                                        DEPT_ID = dept.DEPT_ID,
                                        DEPT = dept.DEPT_NAME,
                                        WF_DESIGNATION = desig.WF_DESIGNATION_NAME,
                                        DOJ = emp.DOJ,
                                        MOBILE_NO = emp.MOBILE_NO,
                                        AADHAR_NO = emp.AADHAR_NO,
                                        STATUS = emp.STATUS,
                                        WF_ID = emp.WF_ID,
                                        WF_EMP_TYPE = emp.WF_EMP_TYPE
                                    }).OrderBy(x => x.EMP_NAME).ToList();
            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "WorkforceReporsitory", "GetEmpAllItems", "", ex.InnerException.ToString());
            }

            return empMetaDataLists;
        }


        public List<WorkforceMetaDataList> BindWorkforceByWFType_New(Guid BUILDING_ID, Guid deptId, Guid? sub_dept_id, int? emp_type_id, int? EMPLOYMENT_TYPE)
        {
            Guid cmp_id = SessionHelper.Get<Guid>("CompanyId");
            List<WorkforceMetaDataList> empMetaDataLists = new List<WorkforceMetaDataList>();
            //if(sub_dept_id== new Guid("00000000-0000-0000-0000-000000000000"))

            try
            {
                empMetaDataLists = (from emp in _appEntity.TAB_WORKFORCE_MASTER
                                    join dept in _appEntity.TAB_DEPARTMENT_MASTER on emp.DEPT_ID equals dept.DEPT_ID
                                    join desig in _appEntity.TAB_WF_DESIGNATION_MASTER on emp.WF_DESIGNATION_ID equals desig.WF_DESIGNATION_ID
                                    where emp.COMPANY_ID == cmp_id
                                    && emp.BUILDING_ID == BUILDING_ID
                                    && emp.DEPT_ID == deptId
                                    && emp.SUBDEPT_ID == (sub_dept_id != new Guid("00000000-0000-0000-0000-000000000000") ? sub_dept_id : emp.SUBDEPT_ID)
                                    && emp.WF_EMP_TYPE == (emp_type_id != 0 ? emp_type_id : emp.WF_EMP_TYPE)
                                    && emp.EMP_TYPE_ID == (EMPLOYMENT_TYPE != 0 ? EMPLOYMENT_TYPE : emp.EMP_TYPE_ID)
                                      && emp.STATUS == "Y"
                                    select new WorkforceMetaDataList
                                    {
                                        EMP_ID = emp.EMP_ID,
                                        EMP_NAME = emp.EMP_NAME,
                                        DEPT_ID = dept.DEPT_ID,
                                        DEPT = dept.DEPT_NAME,
                                        WF_DESIGNATION = desig.WF_DESIGNATION_NAME,
                                        DOJ = emp.DOJ,
                                        MOBILE_NO = emp.MOBILE_NO,
                                        AADHAR_NO = emp.AADHAR_NO,
                                        STATUS = emp.STATUS,
                                        WF_ID = emp.WF_ID,
                                        WF_EMP_TYPE = emp.WF_EMP_TYPE
                                    }).OrderBy(x => x.EMP_NAME).ToList();
            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "WorkforceReporsitory", "GetEmpAllItems", "", ex.InnerException.ToString());
            }

            return empMetaDataLists;
        }

        public List<WorkforceMetaDataList> BindEmployeeData(Guid BUILDING_ID, int? emp_type_id)
        {
            Guid cmp_id = SessionHelper.Get<Guid>("CompanyId");
            List<WorkforceMetaDataList> empMetaDataLists = new List<WorkforceMetaDataList>();

            try
            {
                empMetaDataLists = (from emp in _appEntity.TAB_WORKFORCE_MASTER
                                    join dept in _appEntity.TAB_DEPARTMENT_MASTER on emp.DEPT_ID equals dept.DEPT_ID
                                    join Sdept in _appEntity.TAB_SUBDEPARTMENT_MASTER on emp.SUBDEPT_ID equals Sdept.SUBDEPT_ID
                                    join desig in _appEntity.TAB_WF_DESIGNATION_MASTER on emp.WF_DESIGNATION_ID equals desig.WF_DESIGNATION_ID
                                    where emp.COMPANY_ID == cmp_id
                                    && emp.BUILDING_ID == BUILDING_ID
                                    && emp.WF_EMP_TYPE == emp_type_id
                                      && emp.STATUS == "Y"
                                    select new WorkforceMetaDataList
                                    {
                                        EMP_ID = emp.EMP_ID,
                                        EMP_NAME = emp.EMP_NAME,
                                        DEPT = dept.DEPT_NAME,
                                        REFERENCE_NAME = Sdept.SUBDEPT_NAME,
                                        WF_ID = emp.WF_ID,
                                        IDENTIFICATION_MARK = (emp.EMP_TYPE_ID == 1 ? "SALARIED" : "PIECE WAGER")
                                    }).OrderBy(x => x.EMP_NAME).ToList();
            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "WorkforceReporsitory", "GetEmpAllItems", "", ex.InnerException.ToString());
            }

            return empMetaDataLists;
        }



        public List<WorkforceMetaDataList> GetPiecWagerWorkforceByWFTypes(Guid? deptId, Guid? sub_dept_id, int? emptype_id)
        {
            Guid cmp_id = SessionHelper.Get<Guid>("CompanyId");
            List<WorkforceMetaDataList> empMetaDataLists = new List<WorkforceMetaDataList>();
            try
            {
                empMetaDataLists = (from emp in _appEntity.TAB_WORKFORCE_MASTER
                                    where emp.COMPANY_ID == cmp_id && emp.DEPT_ID == deptId
                                    && emp.WF_EMP_TYPE == (emptype_id != null ? emptype_id : emp.WF_EMP_TYPE)
                                    && emp.SUBDEPT_ID == (sub_dept_id != null ? sub_dept_id : emp.SUBDEPT_ID)
                                    && emp.EMP_TYPE_ID == (short)Enum_EMPL_TYPE_MASTER.PIECE_WAGER
                                    select new WorkforceMetaDataList
                                    {
                                        EMP_ID = emp.EMP_ID,
                                        EMP_NAME = emp.EMP_NAME,
                                        WF_ID = emp.WF_ID,
                                    }).OrderBy(x => x.EMP_NAME).ToList();
            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "WorkforceReporsitory", "GetEmpAllItems", "", ex.InnerException.ToString());
            }

            return empMetaDataLists;
        }

        public List<WorkforceMetaDataList> GetEmpAllItems()
        {
            List<WorkforceMetaDataList> empMetaDataLists = new List<WorkforceMetaDataList>();

            Guid loggedInUserId = Utility.GetLoggedInUserId();

            try
            {
                empMetaDataLists = (from emp in _appEntity.TAB_WORKFORCE_MASTER
                                    join dept in _appEntity.TAB_DEPARTMENT_MASTER on emp.DEPT_ID equals dept.DEPT_ID
                                    join desig in _appEntity.TAB_WF_DESIGNATION_MASTER on emp.WF_DESIGNATION_ID equals desig.WF_DESIGNATION_ID
                                    join user_depart_map in _appEntity.TAB_USER_DEPARTMENT_MAPPING on dept.DEPT_ID equals user_depart_map.DEPT_ID
                                    join login_master in _appEntity.TAB_LOGIN_MASTER on user_depart_map.USER_ID equals login_master.USER_ID
                                    where login_master.USER_ID == loggedInUserId
                                    select new WorkforceMetaDataList
                                    {
                                        EMP_ID = emp.EMP_ID,
                                        EMP_NAME = emp.EMP_NAME,
                                        DEPT_ID = dept.DEPT_ID,
                                        DEPT = dept.DEPT_NAME,
                                        WF_DESIGNATION = desig.WF_DESIGNATION_NAME,
                                        DOJ = emp.DOJ,
                                        MOBILE_NO = emp.MOBILE_NO,
                                        AADHAR_NO = emp.AADHAR_NO,
                                        STATUS = emp.STATUS,
                                        WF_ID = emp.WF_ID
                                    }).ToList();
            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "WorkforceReporsitory", "GetEmpAllItems", "", ex.InnerException.ToString());
            }

            return empMetaDataLists;
        }

        public WorkforceSalaryMetaData Find(string emp_id)
        {
            WorkforceSalaryMetaData empsal = null;
            try
            {

                empsal = (from emp in _appEntity.TAB_WORKFORCE_MASTER
                          join emps in _appEntity.TAB_WORKFORCE_SALARY_MASTER on emp.WF_ID equals emps.WF_ID
                          join bank in _appEntity.TAB_BANK_MASTER on emps.BANK_ID equals bank.BANK_ID
                          select new WorkforceSalaryMetaData
                          {
                              EMP_ID = emp.EMP_ID,
                              EMP_NAME = emp.EMP_NAME,
                              WF_ID = emp.WF_ID,
                              COMPANY_ID = emp.COMPANY_ID,
                              UAN_NO = emps.UAN_NO,
                              PAN_CARD = emps.PAN_CARD,
                              EPF_NO = emps.EPF_NO,
                              ESIC_NO = emps.ESIC_NO,
                              BANK_ID = emps.BANK_ID,
                              BANK_IFSC = emps.BANK_IFSC,
                              BANK_BRANCH = emps.BANK_BRANCH,
                              BANK_ACCOUNT_NO = emps.BANK_ACCOUNT_NO,
                              BASIC_DA = emps.BASIC_DA,
                              HRA = emps.HRA,
                              SPECIAL_ALLOWANCES = emps.SPECIAL_ALLOWANCES,
                              GROSS = emps.GROSS,
                              ACTION = "UPDATE",
                              SELECTEDBANKNAME = bank.BANK_NAME,
                              SELECTEDBANKID = bank.BANK_ID

                          }).Where(f => f.EMP_ID == emp_id).FirstOrDefault();

                if (empsal == null)
                {
                    var wfobj = _appEntity.TAB_WORKFORCE_MASTER.Where(x => x.EMP_ID == emp_id).FirstOrDefault();

                    if (wfobj != null)
                    {
                        empsal = new WorkforceSalaryMetaData
                        {
                            WF_ID = wfobj.WF_ID,
                            EMP_NAME = wfobj.EMP_NAME,
                            COMPANY_ID = wfobj.COMPANY_ID,
                            ACTION = "ADD"
                        };
                    }
                    else
                    {
                        empsal = new WorkforceSalaryMetaData
                        {
                            ACTION = "NO DATA"
                        };
                    }
                }

            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "WorkforceRepository", "Find", "", ex.ToString());
            }

            return empsal;
        }

        public WorkforceMetaData FindWorkforceByWFId(Guid wf_id)
        {
            WorkforceMetaData empdetail = null;
            try
            {
                empdetail = (from emp in _appEntity.TAB_WORKFORCE_MASTER

                             select new WorkforceMetaData
                             {
                                 EMP_ID = emp.EMP_ID,
                                 EMP_NAME = emp.EMP_NAME + " - " + emp.EMP_ID,
                                 WF_ID = emp.WF_ID,
                                 COMPANY_ID = emp.COMPANY_ID,
                                 FATHER_NAME = emp.FATHER_NAME,
                                 MOBILE_NO = emp.MOBILE_NO,
                                 DOJ = emp.DOJ
                             }).Where(f => f.WF_ID == wf_id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "WorkforceRepository", "Find", "", ex.ToString());
            }

            return empdetail;
        }

        public WorkforceMasterMetaData FindWorkforceIWithFullDetailByWFId(Guid wf_id)
        {
            WorkforceMasterMetaData empdetail = null;
            try
            {
                empdetail = (from emp in _appEntity.TAB_WORKFORCE_MASTER
                             join photo in _appEntity.TAB_WORKFORCE_PHOTO_MASTER on emp.WF_ID equals photo.WF_ID into poj
                             from photo in poj.DefaultIfEmpty()
                             select new WorkforceMasterMetaData
                             {
                                 EMP_ID = emp.EMP_ID,
                                 EMP_NAME = emp.EMP_NAME,
                                 WF_ID = emp.WF_ID,
                                 COMPANY_ID = emp.COMPANY_ID,
                                 FATHER_NAME = emp.FATHER_NAME,
                                 MOBILE_NO = emp.MOBILE_NO,
                                 DOJ = emp.DOJ,
                                 WF_EMP_TYPE = emp.WF_EMP_TYPE,
                                 AGENCY_ID = emp.AGENCY_ID,
                                 BIOMETRIC_CODE = emp.BIOMETRIC_CODE,
                                 GENDER = emp.GENDER,
                                 DEPT_ID = emp.DEPT_ID,
                                 DATE_OF_BIRTH = emp.DATE_OF_BIRTH,
                                 NATIONALITY = emp.NATIONALITY,
                                 WF_EDUCATION_ID = emp.WF_EDUCATION_ID,
                                 MARITAL_ID = emp.MARITAL_ID,
                                 DOJ_AS_PER_EPF = emp.DOJ_AS_PER_EPF,
                                 WF_DESIGNATION_ID = emp.WF_DESIGNATION_ID,
                                 SKILL_ID = emp.SKILL_ID,
                                 REFERENCE_ID = emp.REFERENCE_ID,
                                 SUBDEPT_ID = emp.SUBDEPT_ID,
                                 EMP_TYPE_ID = emp.EMP_TYPE_ID,
                                 ALTERNATE_NO = emp.ALTERNATE_NO,
                                 EMAIL_ID = emp.EMAIL_ID,
                                 PRESENT_ADDRESS = emp.PRESENT_ADDRESS,
                                 PRESENT_ADDRESS_CITY = emp.PRESENT_ADDRESS_CITY,
                                 PRESENT_ADDRESS_STATE = emp.PRESENT_ADDRESS_STATE,
                                 PRESENT_ADDRESS_PIN = emp.PRESENT_ADDRESS_PIN,
                                 PERMANENT_ADDRESS = emp.PERMANENT_ADDRESS,
                                 PERMANENT_ADDRESS_CITY = emp.PERMANENT_ADDRESS_CITY,
                                 PERMANENT_ADDRESS_STATE = emp.PERMANENT_ADDRESS_STATE,
                                 PERMANENT_ADDRESS_PIN = emp.PERMANENT_ADDRESS_PIN,
                                 AADHAR_NO = emp.AADHAR_NO,
                                 EMP_STATUS_ID = emp.EMP_STATUS_ID,
                                 IDENTIFICATION_MARK = emp.IDENTIFICATION_MARK,
                                 PHOTO = photo == null ? null : photo.PHOTO,
                                 EMP_SIGNATURE = photo == null ? null : photo.EMP_SIGNATURE,
                                 BUILDING_ID = emp.BUILDING_ID
                             }).Where(f => f.WF_ID == wf_id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "WorkforceRepository", "FindWorkforceIWithFullDetailByWFId", "", ex.ToString());
            }

            return empdetail;
        }

        public WorkforceDailyWorkMetaData EditDailyWork(Guid ID)
        {
            var result = (from dw in _appEntity.TAB_WORKFORCE_DAILYWORK
                          join op in _appEntity.TAB_ITEM_OPERATION_MASTER on dw.UNIQUE_OPERATION_ID equals op.UNIQUE_OPERATION_ID
                          join wf in _appEntity.TAB_WORKFORCE_MASTER on dw.WF_ID equals wf.WF_ID
                          where dw.ID == ID
                          select new WorkforceDailyWorkMetaData
                          {
                              BUILDING_ID = wf.BUILDING_ID,
                              DEPARTMENT_ID = (Guid)wf.DEPT_ID,
                              SUBDEPT_ID = wf.SUBDEPT_ID,
                              WF_EMP_TYPE = wf.WF_EMP_TYPE,
                              WF_ID = dw.WF_ID,
                              WORK_DATE = dw.WORK_DATE,
                              ITEM_NAME = op.ITEM_CODE + "_" + op.ITEM_NAME,
                              UNIQUE_OPERATION_ID = dw.UNIQUE_OPERATION_ID,
                              QTY = dw.QTY,
                              MachineNo=dw.MachineNo,
                              AvgPercentage = dw.AvgPercentage.ToString(),
                              WASTE =  dw.WASTE.ToString(),
                              REJECTION_ON_LOOM = dw.REJECTION_ON_LOOM.ToString(),
                              REJECTION_ON_FINISHING = dw.REJECTION_ON_FINISHING.ToString(),
                          }).FirstOrDefault();
            return result;
        }

        public void UpdateDailyWork(Guid? UNIQUE_OPERATION_ID, string QTY, Guid? ID, string MachineNo, string AvgPercentage, string WASTE, string REJECTION_ON_LOOM, string REJECTION_ON_FINISHING)
        {

            Core.TAB_WORKFORCE_DAILYWORK Obj = _appEntity.TAB_WORKFORCE_DAILYWORK.Where(x => x.ID == ID).FirstOrDefault();
            var GetOperation = _appEntity.TAB_WORKFORCE_DAILYWORK.Where(x => x.ID == ID).FirstOrDefault();
            var GetItemsName = _appEntity.TAB_ITEM_OPERATION_MASTER.Where(x => x.UNIQUE_OPERATION_ID == UNIQUE_OPERATION_ID).FirstOrDefault();
            DateTime workingDate = Convert.ToDateTime(Obj.WORK_DATE);
            var wfReord = _appEntity.TAB_WORKFORCE_MASTER.Where(x => x.WF_ID == Obj.WF_ID).FirstOrDefault();
            var BioMetricDetails = _appEntity.TAB_BIOMETRIC.Where(x => x.BIOMETRIC_CODE == wfReord.BIOMETRIC_CODE && x.ATTENDANCE_DATE == workingDate).FirstOrDefault();
            try
            {
                if (Obj != null)
                {
                    var IS_DUTYHRMS = _appEntity.TAB_WORKFORCE_DAILYWORK.Where(x => x.WORK_DATE == Obj.WORK_DATE
                     && x.WF_ID == Obj.WF_ID && x.Operation == "DUTY_HRMS").FirstOrDefault();

                    var wfSalary = _appEntity.TAB_WORKFORCE_SALARY_MASTER.Where(x => x.WF_ID == Obj.WF_ID).FirstOrDefault();
                    var dailySalary = (double)(wfSalary.BASIC_DA + wfSalary.HRA + wfSalary.SPECIAL_ALLOWANCES) / 26;

                    double PerHoursSalary = dailySalary / 8;
                    double PerMinSalary = PerHoursSalary / 60;

                    Obj.UNIQUE_OPERATION_ID = (Guid)UNIQUE_OPERATION_ID;
                    Obj.Operation = GetItemsName.OPERATION;
                    Obj.QTY = Convert.ToDecimal(QTY);
                    Obj.WORK_DATE = Convert.ToDateTime(GetOperation.WORK_DATE);
                    if (GetOperation.Operation == "SAMPLE MANUFACTURING" || GetOperation.Operation == "DUTY")
                    {
                        Obj.RATE = PerHoursSalary;
                        Obj.TotalPrice = Convert.ToDecimal(PerHoursSalary) * Convert.ToDecimal(QTY);
                    }
                    else
                    {
                        Obj.RATE = (double)GetItemsName.RATE;
                        Obj.TotalPrice = Convert.ToDecimal(QTY) * Convert.ToDecimal(GetItemsName.RATE);
                    }
                    Obj.UPDATED_DATE = DateTime.Now;
                    Obj.UPDATED_BY = SessionHelper.Get<String>("Username");
                    Obj.MachineNo = MachineNo;
                    Obj.AvgPercentage = Convert.ToDecimal(AvgPercentage);
                    Obj.WASTE = Convert.ToDecimal(WASTE);
                    Obj.REJECTION_ON_LOOM = Convert.ToDecimal(REJECTION_ON_LOOM);
                    Obj.REJECTION_ON_FINISHING = Convert.ToDecimal(REJECTION_ON_FINISHING);
                    _appEntity.SaveChanges();


                    List<TAB_WORKFORCE_DAILYWORK> lists = new List<TAB_WORKFORCE_DAILYWORK>();

                    var Alloperations = _appEntity.TAB_WORKFORCE_DAILYWORK.Where(x => x.WORK_DATE == Obj.WORK_DATE
                    && x.WF_ID == Obj.WF_ID && x.Operation != "DUTY_HRMS").ToList();

                    double todaySalary = (from ds in Alloperations group ds by 1 into g select new { dailySalary = g.Sum(x => ((double)x.QTY * x.RATE)) }).FirstOrDefault().dailySalary;

                    if (IS_DUTYHRMS != null)
                    {
                        _appEntity.TAB_WORKFORCE_DAILYWORK.Remove(IS_DUTYHRMS);
                    }

                    //-----------------------------Get Duration-----------------------------------

                    double Duration = 0; double diff = 0;
                    DataSet ds1 = new DataSet("WorkingHours");
                    using (SqlConnection conn = new SqlConnection(Configurations.SqlConnectionString))
                    {
                        string attDate = workingDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                        string sqlQuery = $"Select Duration as WorkInMinutes from TAB_BIOMETRIC  where BIOMETRIC_CODE = '{wfReord.BIOMETRIC_CODE}' and ATTENDANCE_DATE='{attDate}'AND STATUS_CODE in ('P','½P','HP','WOP','WO½P','H½P')";

                        SqlCommand sqlComm = new SqlCommand(sqlQuery, conn);
                        sqlComm.CommandType = CommandType.Text;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlComm;
                        da.Fill(ds1);

                        if (ds1 != null)
                        {
                            foreach (DataRow dr in ds1.Tables[0].Rows)
                            {
                                Duration = Convert.ToDouble(dr["WorkInMinutes"]);

                                diff = Duration - 480;
                                if (diff <= 0)
                                {
                                    diff = 0;
                                }
                            }
                        }
                    }


                    //------------------------------Permission------------------------------------

                    int Permission = 0; double PermissionAmt = 0;
                    DataSet ds3 = new DataSet("WorkingHours");
                    using (SqlConnection conn = new SqlConnection(Configurations.SqlConnectionString))
                    {
                        string attDate = workingDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                        //string sqlQuery = $"select Permission from [dbo].[fn_CalPermission] dbo.CalculateWorkingAndOverTimeMinutes('{wfReord.BIOMETRIC_CODE}','{attDate}','{attDate}')";

                        string sqlQuery = $"select(select Permission from[dbo].[fn_CalPermission](InTime, OutTime, BreakStartTime, BreakEndTime, OverTime, EarlyBy, LateBy, PunchRecords)) as p1 from TAB_BIOMETRIC as bm inner join TAB_SHIFT_MASTER as sm on sm.ID = bm.SHIFT_ID where BIOMETRIC_CODE = '{wfReord.BIOMETRIC_CODE}' and ATTENDANCE_DATE='{attDate}'AND STATUS_CODE in ('P','½P','HP','WOP','WO½P','H½P')";

                        SqlCommand sqlComm = new SqlCommand(sqlQuery, conn);
                        sqlComm.CommandType = CommandType.Text;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlComm;
                        da.Fill(ds3);

                        if (ds3 != null)
                        {
                            foreach (DataRow dr in ds3.Tables[0].Rows)
                            {
                                Permission = Convert.ToInt32(dr["p1"]);
                            }
                        }
                    }

                    if (Permission > 0)
                    {
                        PermissionAmt = Permission * PerMinSalary;
                    }

                    //if (wfReord.DEPT_ID == new Guid("cf268f59-ec4f-4149-bf79-c22c0b263a58")) //Condititon For only Machine Shop

                    if (wfReord.DEPT_ID == new Guid("721ffaf4-4c48-4412-a092-7fb487ad2019"))//Forging Shop
                    {
                        double TotalWorkDur = Convert.ToInt32(BioMetricDetails.OverTime) + Duration;
                        dailySalary = Convert.ToDouble(TotalWorkDur * PerMinSalary);

                        if (todaySalary < dailySalary)
                        {
                            var DUTYHRMS_Amount = Convert.ToDecimal(dailySalary - todaySalary);
                            var Rate = 1;
                            var Qty = Convert.ToDecimal(DUTYHRMS_Amount);
                            if (DUTYHRMS_Amount > 0)
                            {
                                var otherOperation = _appEntity.TAB_ITEM_OPERATION_MASTER.Where(x => x.OPERATION == "DUTY_HRMS").FirstOrDefault();
                                TAB_WORKFORCE_DAILYWORK model = new TAB_WORKFORCE_DAILYWORK
                                {
                                    ID = Guid.NewGuid(),
                                    WF_ID = Obj.WF_ID,
                                    UNIQUE_OPERATION_ID = otherOperation.UNIQUE_OPERATION_ID,
                                    QTY = Qty,
                                    RATE = Rate,
                                    WORK_DATE = Obj.WORK_DATE,
                                    TotalPrice = Convert.ToDecimal(DUTYHRMS_Amount),
                                    CREATED_DATE = DateTime.Now,
                                    CREATED_BY = SessionHelper.Get<String>("Username"),
                                    Operation = otherOperation.OPERATION,
                                };
                                lists.Add(model);
                            }
                        }
                    }
                    else
                    {
                        double TotalWorkDur = Convert.ToInt32(BioMetricDetails.OverTime) + Duration;

                        if (TotalWorkDur > 270 && TotalWorkDur < 690)
                        {
                            if (Duration > 0)
                            {
                                TotalWorkDur = TotalWorkDur - 30;
                            }
                            else
                            {
                                if (TotalWorkDur >= 480)
                                {
                                    TotalWorkDur = 480;
                                }
                                else
                                {
                                    TotalWorkDur = TotalWorkDur;
                                }

                            }
                        }

                        if (TotalWorkDur >= 690)
                        {
                            TotalWorkDur = 690;
                        }


                        dailySalary = Convert.ToDouble(TotalWorkDur * PerMinSalary);

                        if (todaySalary < dailySalary)
                        {
                            var DUTYHRMS_Amount = Convert.ToDecimal(dailySalary - todaySalary);
                            var Rate = 1;
                            var Qty = Convert.ToDecimal(DUTYHRMS_Amount);

                            if (DUTYHRMS_Amount > 0)
                            {
                                var otherOperation = _appEntity.TAB_ITEM_OPERATION_MASTER.Where(x => x.OPERATION == "DUTY_HRMS").FirstOrDefault();
                                TAB_WORKFORCE_DAILYWORK model = new TAB_WORKFORCE_DAILYWORK
                                {
                                    ID = Guid.NewGuid(),
                                    WF_ID = wfReord.WF_ID,
                                    //ITEM_ID = otherOperation.ITEM_ID,
                                    WORK_DATE = workingDate,
                                    UNIQUE_OPERATION_ID = otherOperation.UNIQUE_OPERATION_ID,
                                    QTY = Qty,
                                    RATE = Rate,
                                    TotalPrice = Convert.ToDecimal(DUTYHRMS_Amount),
                                    CREATED_DATE = DateTime.Now,
                                    CREATED_BY = SessionHelper.Get<String>("Username"),
                                    Operation = otherOperation.OPERATION,
                                };
                                lists.Add(model);
                            }
                        }
                    }
                    _appEntity.TAB_WORKFORCE_DAILYWORK.AddRange(lists);
                    _appEntity.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "ToolTalkRepository", "Update", "", ex.ToString());
            }
        }

        public WorkforceMetaData FindWorkforce(string emp_id)
        {
            WorkforceMetaData empdetail = null;
            try
            {
                empdetail = (from emp in _appEntity.TAB_WORKFORCE_MASTER

                             select new WorkforceMetaData
                             {
                                 EMP_ID = emp.EMP_ID,
                                 EMP_NAME = emp.EMP_NAME,
                                 WF_ID = emp.WF_ID,
                                 COMPANY_ID = emp.COMPANY_ID,
                                 FATHER_NAME = emp.FATHER_NAME,
                                 MOBILE_NO = emp.MOBILE_NO,
                                 DOJ = emp.DOJ
                             }).Where(f => f.EMP_ID == emp_id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "WorkforceRepository", "Find", "", ex.ToString());
            }

            return empdetail;
        }

        public WorkforceDailyWorkData DWFind(string emp_id)
        {
            WorkforceDailyWorkData empDW = null;
            try
            {

                empDW = (from emp in _appEntity.TAB_WORKFORCE_MASTER
                         join emps in _appEntity.TAB_WORKFORCE_SALARY_MASTER on emp.WF_ID equals emps.WF_ID
                         join dept in _appEntity.TAB_DEPARTMENT_MASTER on emp.DEPT_ID equals dept.DEPT_ID
                         select new WorkforceDailyWorkData
                         {
                             EMP_ID = emp.EMP_ID,
                             EMP_NAME = emp.EMP_NAME,
                             WF_ID = emp.WF_ID,
                             COMPANY_ID = emp.COMPANY_ID,
                             DEPT_ID = emp.DEPT_ID,
                             DEPT_NAME = dept.DEPT_NAME,
                             EMP_TYPE_ID = emp.EMP_TYPE_ID

                         }).Where(f => f.EMP_ID == emp_id && f.EMP_TYPE_ID == 2).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "WorkforceRepository", "DWFind", "", ex.ToString());
            }

            return empDW;
        }

        public void Salary(WorkforceSalaryMetaData salary)
        {
            try
            {
                if (salary.ACTION == "ADD")
                {
                    Core.TAB_BANK_MASTER bm = _appEntity.TAB_BANK_MASTER.Where(x => x.BANK_ID == salary.BANK_ID).FirstOrDefault();
                    Wfm.App.Core.TAB_WORKFORCE_SALARY_MASTER obj = new Wfm.App.Core.TAB_WORKFORCE_SALARY_MASTER
                    {
                        WF_ID = salary.WF_ID,
                        COMPANY_ID = salary.COMPANY_ID,
                        UAN_NO = salary.UAN_NO,
                        PAN_CARD = salary.PAN_CARD,
                        EPF_NO = salary.EPF_NO,
                        ESIC_NO = salary.ESIC_NO,
                        BANK_ID = salary.BANK_ID,
                        BANK_IFSC = bm.IFSC_CODE,
                        BANK_BRANCH = bm.BRANCH_NAME,
                        BANK_ACCOUNT_NO = salary.BANK_ACCOUNT_NO,
                        BASIC_DA = salary.BASIC_DA,
                        HRA = salary.HRA,
                        SPECIAL_ALLOWANCES = salary.SPECIAL_ALLOWANCES,
                        GROSS = salary.BASIC_DA + salary.HRA + salary.SPECIAL_ALLOWANCES,
                        CREATED_DATE = DateTime.Now,
                        CREATED_BY = SessionHelper.Get<String>("Username"),
                        STATUS = "Y"
                    };
                    _appEntity.TAB_WORKFORCE_SALARY_MASTER.Add(obj);
                    _appEntity.SaveChanges();
                }

                if (salary.ACTION == "UPDATE")
                {
                    Core.TAB_WORKFORCE_SALARY_MASTER obj = _appEntity.TAB_WORKFORCE_SALARY_MASTER.Where(x => x.WF_ID == salary.WF_ID).FirstOrDefault();

                    Core.TAB_BANK_MASTER bm = _appEntity.TAB_BANK_MASTER.Where(x => x.BANK_ID == salary.BANK_ID).FirstOrDefault();
                    if (obj != null)
                    {
                        obj.WF_ID = salary.WF_ID;
                        obj.COMPANY_ID = salary.COMPANY_ID;
                        obj.UAN_NO = salary.UAN_NO;
                        obj.PAN_CARD = salary.PAN_CARD;
                        obj.EPF_NO = salary.EPF_NO;
                        obj.ESIC_NO = salary.ESIC_NO;
                        obj.BANK_ID = salary.BANK_ID;
                        obj.BANK_IFSC = bm.IFSC_CODE;
                        obj.BANK_BRANCH = bm.BRANCH_NAME;
                        obj.BANK_ACCOUNT_NO = salary.BANK_ACCOUNT_NO;
                        obj.BASIC_DA = salary.BASIC_DA;
                        obj.HRA = salary.HRA;
                        obj.SPECIAL_ALLOWANCES = salary.SPECIAL_ALLOWANCES;
                        obj.GROSS = salary.BASIC_DA + salary.HRA + salary.SPECIAL_ALLOWANCES;
                        obj.UPDATED_DATE = DateTime.Now;
                        obj.UPDATED_BY = SessionHelper.Get<String>("Username");
                        obj.STATUS = "Y";
                        _appEntity.Entry(obj).State = EntityState.Modified;
                        _appEntity.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "WorkforceReporsitory", "Salary", "", ex.InnerException.ToString());
            }
        }

        public List<ItemMasterMetaData> GetItemByDeptId(Guid DeptId)
        {
            List<ItemMasterMetaData> itemMasterMetaData = (from item in _appEntity.TAB_ITEM_OPERATION_MASTER

                                                           where item.STATUS == "Y"
                                                           && (item.DEPT_ID == DeptId || item.ITEM_CODE == "DUTY" || item.ITEM_CODE == "SAMPLE MANUFACTURING")

                                                           select new ItemMasterMetaData
                                                           {
                                                               DEPT_ID = (Guid)item.DEPT_ID,
                                                               //ITEM_ID = item.UNIQUE_OPERATION_ID,
                                                               ITEM_CODE_NAME = item.ITEM_CODE,
                                                               ITEM_NAME = item.ITEM_NAME,

                                                           }).ToList();
            return itemMasterMetaData;
        }

        public List<ItemOperationsMasterMetaData> GetItemOperationsByItemId(string ItemId, string ItemName, Guid DEPARTMENT_ID)
        {
            List<ItemOperationsMasterMetaData> itemOperationsMasterMetaData = _appEntity.TAB_ITEM_OPERATION_MASTER.Where(x => x.STATUS == "Y"
            && x.ITEM_CODE == ItemId && x.ITEM_NAME == ItemName
            && (x.DEPT_ID == DEPARTMENT_ID || x.ITEM_CODE == "DUTY" || x.ITEM_CODE == "SAMPLE MANUFACTURING")
            )
               .Select(x => new ItemOperationsMasterMetaData
               {
                   //ITEM_NAME = x.ITEM_NAME,
                   UNIQUE_OPERATION_ID = x.UNIQUE_OPERATION_ID,
                   OPERATION = x.OPERATION,
                   RATE = x.RATE
               }).ToList();
            return itemOperationsMasterMetaData;
        }

        public void SaveDailyWorkMaster(WorkforceDailyWorkData dailywork)
        {
            try
            {
                Wfm.App.Core.TAB_WORKFORCE_DAILYWORK_MASTER obj = new Wfm.App.Core.TAB_WORKFORCE_DAILYWORK_MASTER
                {
                    ID = Guid.NewGuid(),
                    DEPT_ID = dailywork.DEPT_ID.Value,
                    EMP_ID = dailywork.EMP_ID,
                    WORK_DATE = dailywork.WORK_DATE.Value,
                    OVETIME = true,
                    CREATED_DATE = DateTime.Now,
                    CREATED_BY = SessionHelper.Get<String>("Username")
                };

                _appEntity.TAB_WORKFORCE_DAILYWORK_MASTER.Add(obj);
                _appEntity.SaveChanges();
            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "WorkforceReporsitory", "SaveDailyWorkMaster", "", ex.InnerException.ToString());
            }
        }

        public void SaveDailyWork(WorkforceDailyWorkData dailywork)
        {
            try
            {
                int quantity = Convert.ToInt16(dailywork.QTY);
                Guid wfid = Guid.Parse(dailywork.EMP_ID);
                var wfReord = _appEntity.TAB_WORKFORCE_MASTER.Where(x => x.WF_ID == wfid).FirstOrDefault();

                Wfm.App.Core.TAB_WORKFORCE_DAILYWORK obj = new Wfm.App.Core.TAB_WORKFORCE_DAILYWORK
                {
                    ID = Guid.NewGuid(),
                    WF_ID = wfReord.WF_ID,
                    WORK_DATE = dailywork.WORK_DATE,
                    UNIQUE_OPERATION_ID = dailywork.UNIQUE_OPERATION_ID,
                    ITEM_ID = dailywork.ITEM_ID,
                    QTY = Convert.ToInt16(dailywork.QTY),
                    CREATED_DATE = DateTime.Now,
                    CREATED_BY = SessionHelper.Get<String>("Username")
                };

                _appEntity.TAB_WORKFORCE_DAILYWORK.Add(obj);
                _appEntity.SaveChanges();
            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "WorkforceReporsitory", "SaveDailyWork", "", ex.InnerException.ToString());
            }
        }

        public object AddDailyWork(AddWorkForceItemMetaData dailywork)
        {
            DateTime workingDate = Convert.ToDateTime(dailywork.WORK_DATE);
            try
            {
                Guid cmd_id = SessionHelper.Get<Guid>("CompanyId");
                var wfReord = _appEntity.TAB_WORKFORCE_MASTER.Where(x => x.WF_ID == dailywork.WF_ID).FirstOrDefault();
                var wfSalary = _appEntity.TAB_WORKFORCE_SALARY_MASTER.Where(x => x.WF_ID == dailywork.WF_ID).FirstOrDefault();
                if (wfSalary == null)
                {
                    return new { Status = false, Message = $"Salary not reated for {wfReord.EMP_NAME}({wfReord.EMP_ID})" };
                }

                var dailySalary = (double)(wfSalary.BASIC_DA + wfSalary.HRA + wfSalary.SPECIAL_ALLOWANCES) / 26;
                double PerHoursSalary = dailySalary / 8;

                double PerMinSalary = PerHoursSalary / 60;

                var arraysOperationId = dailywork.OPERATIONs.Select(x => x.UNIQUE_OPERATION_ID).ToArray();
                var operations = _appEntity.TAB_ITEM_OPERATION_MASTER.Where(x => arraysOperationId.Contains(x.UNIQUE_OPERATION_ID) && x.COMPANY_ID == cmd_id).ToList();
                List<TAB_WORKFORCE_DAILYWORK> lists = new List<TAB_WORKFORCE_DAILYWORK>();
                foreach (var item in operations)
                {
                    var qty = dailywork.OPERATIONs.Where(x => x.UNIQUE_OPERATION_ID == item.UNIQUE_OPERATION_ID).FirstOrDefault().QTY;

                    var Sandila_DT = dailywork.OPERATIONs.Where(x => x.UNIQUE_OPERATION_ID == item.UNIQUE_OPERATION_ID).FirstOrDefault();


                    if (item.OPERATION == "SAMPLE MANUFACTURING" || item.OPERATION == "DUTY")
                    {
                        TAB_WORKFORCE_DAILYWORK model = new TAB_WORKFORCE_DAILYWORK
                        {
                            ID = Guid.NewGuid(),
                            WF_ID = wfReord.WF_ID,
                            //ITEM_ID = item.ITEM_ID,
                            WORK_DATE = workingDate,
                            UNIQUE_OPERATION_ID = item.UNIQUE_OPERATION_ID,
                            QTY = qty,
                            RATE = PerHoursSalary,
                            TotalPrice = Convert.ToDecimal(PerHoursSalary) * (qty),
                            Operation = item.OPERATION,
                            CREATED_DATE = DateTime.Now,
                            CREATED_BY = SessionHelper.Get<String>("Username"),
                            MachineNo = Sandila_DT.MachineNo,
                            AvgPercentage = Sandila_DT.AvgPercentage,
                            WASTE = Sandila_DT.WASTE,
                            REJECTION_ON_LOOM = Sandila_DT.REJECTION_ON_LOOM,
                            REJECTION_ON_FINISHING = Sandila_DT.REJECTION_ON_FINISHING
                        };
                        lists.Add(model);
                    }
                    else
                    {
                        TAB_WORKFORCE_DAILYWORK model = new TAB_WORKFORCE_DAILYWORK
                        {
                            ID = Guid.NewGuid(),
                            WF_ID = wfReord.WF_ID,
                            //ITEM_ID = item.ITEM_ID,
                            WORK_DATE = workingDate,
                            UNIQUE_OPERATION_ID = item.UNIQUE_OPERATION_ID,
                            QTY = qty,
                            RATE = item.RATE.Value,
                            TotalPrice = Convert.ToDecimal(item.RATE.Value) * (qty),
                            Operation = item.OPERATION,
                            CREATED_DATE = DateTime.Now,
                            CREATED_BY = SessionHelper.Get<String>("Username"),
                            MachineNo = Sandila_DT.MachineNo,
                            AvgPercentage = Sandila_DT.AvgPercentage,
                            WASTE = Sandila_DT.WASTE,
                            REJECTION_ON_LOOM = Sandila_DT.REJECTION_ON_LOOM,
                            REJECTION_ON_FINISHING = Sandila_DT.REJECTION_ON_FINISHING
                        };
                        lists.Add(model);
                    }

                }
                double todaySalary = (from ds in lists group ds by 1 into g select new { dailySalary = g.Sum(x => ((double)x.QTY * x.RATE)) }).FirstOrDefault().dailySalary;

                var DUTY = _appEntity.TAB_ITEM_OPERATION_MASTER.Where(x => x.OPERATION == "DUTY").FirstOrDefault();

                //-----------------------------Get Duration-----------------------------------

                double Duration = 0; double diff = 0;
                DataSet ds1 = new DataSet("WorkingHours");
                using (SqlConnection conn = new SqlConnection(Configurations.SqlConnectionString))
                {
                    string attDate = workingDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                    string sqlQuery = $"Select Duration as WorkInMinutes from TAB_BIOMETRIC  where BIOMETRIC_CODE = '{wfReord.BIOMETRIC_CODE}' and ATTENDANCE_DATE='{attDate}'AND STATUS_CODE in ('P','½P','HP','WOP','WO½P','H½P')";

                    SqlCommand sqlComm = new SqlCommand(sqlQuery, conn);
                    sqlComm.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;
                    da.Fill(ds1);

                    if (ds1 != null)
                    {
                        foreach (DataRow dr in ds1.Tables[0].Rows)
                        {
                            Duration = Convert.ToDouble(dr["WorkInMinutes"]);

                            diff = Duration - 480;
                            if (diff <= 0)
                            {
                                diff = 0;
                            }
                        }
                    }
                }


                //------------------------------Permission------------------------------------

                int Permission = 0; double PermissionAmt = 0;
                DataSet ds3 = new DataSet("WorkingHours");
                using (SqlConnection conn = new SqlConnection(Configurations.SqlConnectionString))
                {
                    string attDate = workingDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    //string sqlQuery = $"select Permission from [dbo].[fn_CalPermission] dbo.CalculateWorkingAndOverTimeMinutes('{wfReord.BIOMETRIC_CODE}','{attDate}','{attDate}')";

                    string sqlQuery = $"select(select Permission from[dbo].[fn_CalPermission](InTime, OutTime, BreakStartTime, BreakEndTime, OverTime, EarlyBy, LateBy, PunchRecords)) as p1 from TAB_BIOMETRIC as bm inner join TAB_SHIFT_MASTER as sm on sm.ID = bm.SHIFT_ID where BIOMETRIC_CODE = '{wfReord.BIOMETRIC_CODE}' and ATTENDANCE_DATE='{attDate}'AND STATUS_CODE in ('P','½P','HP','WOP','WO½P','H½P')";

                    SqlCommand sqlComm = new SqlCommand(sqlQuery, conn);
                    sqlComm.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;
                    da.Fill(ds3);

                    if (ds3 != null)
                    {
                        foreach (DataRow dr in ds3.Tables[0].Rows)
                        {
                            Permission = Convert.ToInt32(dr["p1"]);
                        }
                    }
                }

                if (Permission > 0)
                {
                    PermissionAmt = Permission * PerMinSalary;
                }

                //if (wfReord.DEPT_ID == new Guid("cf268f59-ec4f-4149-bf79-c22c0b263a58")) //Condititon For only Machine Shop

                if (wfReord.DEPT_ID == new Guid("721ffaf4-4c48-4412-a092-7fb487ad2019"))   // Condition For Other department except Forging Shop
                {
                    double TotalWorkDur = Convert.ToInt32(dailywork.OverTime) + Duration;
                    dailySalary = Convert.ToDouble(TotalWorkDur * PerMinSalary);
                    if (todaySalary < dailySalary)
                    {
                        var DUTYHRMS_Amount = Convert.ToDecimal(dailySalary - todaySalary);

                        var Rate = 1;
                        var Qty = Convert.ToDecimal(DUTYHRMS_Amount);

                        if (DUTYHRMS_Amount > 0)
                        {
                            var otherOperation = _appEntity.TAB_ITEM_OPERATION_MASTER.Where(x => x.OPERATION == "DUTY_HRMS").FirstOrDefault();
                            TAB_WORKFORCE_DAILYWORK model = new TAB_WORKFORCE_DAILYWORK
                            {
                                ID = Guid.NewGuid(),
                                WF_ID = wfReord.WF_ID,
                                //ITEM_ID = otherOperation.ITEM_ID,
                                WORK_DATE = workingDate,
                                UNIQUE_OPERATION_ID = otherOperation.UNIQUE_OPERATION_ID,
                                QTY = Qty,
                                RATE = Rate,
                                TotalPrice = Convert.ToDecimal(DUTYHRMS_Amount),
                                CREATED_DATE = DateTime.Now,
                                CREATED_BY = SessionHelper.Get<String>("Username"),
                                Operation = otherOperation.OPERATION,
                            };
                            lists.Add(model);
                        }
                    }
                }
                else
                {
                    double TotalWorkDur = Convert.ToInt32(dailywork.OverTime) + Duration;

                    if (TotalWorkDur > 270 && TotalWorkDur < 690)
                    {
                        if (Duration > 0)
                        {
                            TotalWorkDur = TotalWorkDur - 30;
                        }
                        else
                        {
                            if (TotalWorkDur >= 480)
                            {
                                TotalWorkDur = 480;
                            }
                            else
                            {
                                TotalWorkDur = TotalWorkDur;
                            }

                        }
                    }

                    if (TotalWorkDur >= 690)
                    {
                        TotalWorkDur = 690;
                    }

                    dailySalary = Convert.ToDouble(TotalWorkDur * PerMinSalary);

                    if (todaySalary < dailySalary)
                    {
                        var DUTYHRMS_Amount = Convert.ToDecimal(dailySalary - todaySalary);
                        var Rate = 1;
                        var Qty = Convert.ToDecimal(DUTYHRMS_Amount);

                        if (DUTYHRMS_Amount > 0)
                        {
                            var otherOperation = _appEntity.TAB_ITEM_OPERATION_MASTER.Where(x => x.OPERATION == "DUTY_HRMS").FirstOrDefault();
                            TAB_WORKFORCE_DAILYWORK model = new TAB_WORKFORCE_DAILYWORK
                            {
                                ID = Guid.NewGuid(),
                                WF_ID = wfReord.WF_ID,
                                //ITEM_ID = otherOperation.ITEM_ID,
                                WORK_DATE = workingDate,
                                UNIQUE_OPERATION_ID = otherOperation.UNIQUE_OPERATION_ID,
                                QTY = Qty,
                                RATE = Rate,
                                TotalPrice = Convert.ToDecimal(DUTYHRMS_Amount),
                                CREATED_DATE = DateTime.Now,
                                CREATED_BY = SessionHelper.Get<String>("Username"),
                                Operation = otherOperation.OPERATION,
                            };
                            lists.Add(model);
                        }
                    }

                }


                _appEntity.TAB_WORKFORCE_DAILYWORK.AddRange(lists);
                _appEntity.SaveChanges();
                return new { Status = true, Message = $"Record saved for daily work" };
            }
            catch (Exception ex)
            {
                return new { Status = false, Message = $"Error while saving the daily work" };
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "WorkforceReporsitory", "SaveDailyWork", "", ex.InnerException.ToString());
            }
        }

        public List<SerchDailyWorkMetaData> GetDailyWorks(FilterDailyWork filterDailyWork)
        {
            DateTime workingStartDate = Convert.ToDateTime(filterDailyWork.StartDate);
            DateTime workingEndDate = Convert.ToDateTime(filterDailyWork.EndDate);
            Guid cmd_id = SessionHelper.Get<Guid>("CompanyId");
            List<SerchDailyWorkMetaData> result = (from dw in _appEntity.TAB_WORKFORCE_DAILYWORK
                                                   join op in _appEntity.TAB_ITEM_OPERATION_MASTER on dw.UNIQUE_OPERATION_ID equals op.UNIQUE_OPERATION_ID
                                                   join wf in _appEntity.TAB_WORKFORCE_MASTER on dw.WF_ID equals wf.WF_ID
                                                   where wf.COMPANY_ID == cmd_id
                                                   && wf.DEPT_ID == filterDailyWork.DEPARTMENT_ID
                                                   && wf.WF_ID == (filterDailyWork.WF_ID != null ? filterDailyWork.WF_ID : wf.WF_ID)
                                                   && wf.WF_EMP_TYPE == (filterDailyWork.WF_EMP_TYPE != null ? filterDailyWork.WF_EMP_TYPE : wf.WF_EMP_TYPE)
                                                   && (dw.WORK_DATE >= workingStartDate && dw.WORK_DATE <= workingEndDate)
                                                   group new { dw, wf, op } by new { dw.WORK_DATE, wf.WF_ID, wf.EMP_NAME } into grp
                                                   select new SerchDailyWorkMetaData
                                                   {
                                                       Total = grp.Sum(x => ((double)x.dw.TotalPrice)),
                                                       WorkingDate = grp.Key.WORK_DATE,
                                                       WF_ID = grp.Key.WF_ID,
                                                       WorkforceName = grp.Key.EMP_NAME,
                                                       partialDailyWorkMetaDatas = grp.
                                                       Select(x => new PartialDailyWorkMetaData
                                                       {
                                                           ITEM = x.op.ITEM_NAME,
                                                           ITEM_CODE = x.op.ITEM_CODE,
                                                           OPERATION_NAME = x.op.OPERATION,
                                                           QTY = x.dw.QTY,
                                                           RATE = x.dw.RATE,
                                                           TotalPrice = x.dw.TotalPrice
                                                       }).ToList()
                                                   }
                        ).ToList();
            return result.OrderBy(x => x.WorkforceName).OrderBy(x => x.WorkingDate).ToList();
        }

        public List<SerchDailyWorkMetaData> SearchDailyWorkList(FilterDailyWork filterDailyWork)
        {
            DateTime workingStartDate = Convert.ToDateTime(filterDailyWork.StartDate);
            DateTime workingEndDate = Convert.ToDateTime(filterDailyWork.EndDate);
            Guid cmd_id = SessionHelper.Get<Guid>("CompanyId");
            List<SerchDailyWorkMetaData> result = (from dw in _appEntity.TAB_WORKFORCE_DAILYWORK
                                                   join op in _appEntity.TAB_ITEM_OPERATION_MASTER on dw.UNIQUE_OPERATION_ID equals op.UNIQUE_OPERATION_ID
                                                   join wf in _appEntity.TAB_WORKFORCE_MASTER on dw.WF_ID equals wf.WF_ID
                                                   where wf.COMPANY_ID == cmd_id
                                                   && wf.DEPT_ID == filterDailyWork.DEPARTMENT_ID
                                                   && wf.WF_ID == (filterDailyWork.WF_ID != null ? filterDailyWork.WF_ID : wf.WF_ID)
                                                   && wf.WF_EMP_TYPE == (filterDailyWork.WF_EMP_TYPE != null ? filterDailyWork.WF_EMP_TYPE : wf.WF_EMP_TYPE)
                                                   //&& (dw.WORK_DATE>=workingStartDate && dw.WORK_DATE<=workingEndDate)
                                                   && dw.WORK_DATE == workingStartDate
                                                   group new { dw, wf, op } by new { dw.WORK_DATE, wf.WF_ID, wf.EMP_NAME } into grp
                                                   select new SerchDailyWorkMetaData
                                                   {
                                                       Total = grp.Sum(x => ((double)x.dw.QTY * x.dw.RATE)),
                                                       WorkingDate = grp.Key.WORK_DATE,
                                                       WF_ID = grp.Key.WF_ID,
                                                       WorkforceName = grp.Key.EMP_NAME,
                                                       partialDailyWorkMetaDatas = grp.
                                                       Select(x => new PartialDailyWorkMetaData
                                                       {
                                                           UNIQUE_OPERATION_ID = x.dw.ID,
                                                           ITEM = x.op.ITEM_NAME,
                                                           ITEM_CODE = x.op.ITEM_CODE,
                                                           OPERATION_NAME = x.op.OPERATION,
                                                           QTY = x.dw.QTY,
                                                           RATE = x.dw.RATE,
                                                           DW_ID = x.dw.ID
                                                       }).ToList()
                                                   }
                        ).ToList();
            return result.OrderBy(x => x.WorkforceName).OrderByDescending(x => x.WorkingDate).ToList();
        }

        public int Delete_DailyWork(Guid id, string Date)
        {
            try
            {
                DateTime workingStartDate = Convert.ToDateTime(Date);

                var configuredItem = _appEntity.TAB_WORKFORCE_DAILYWORK.Where(x => x.WF_ID == id && x.WORK_DATE == workingStartDate).ToList();

                if (configuredItem == null) return 0;
                foreach (var item in configuredItem)
                {

                    _appEntity.TAB_WORKFORCE_DAILYWORK.Remove(item);
                }
                _appEntity.SaveChanges();
            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "WorkforceReporsitory", "SearchDailyWorkList", "", ex.ToString());
            }

            return 1;
        }


        public List<WorkforceFaultyData> GetFaultyData(Guid dept_id, int month, int year)
        {
            List<WorkforceFaultyData> faultyrecords = null;
            try
            {

                faultyrecords = (from faultypunches in _appEntity.TAB_FAULTY_PUNCHES
                                 join emps in _appEntity.TAB_WORKFORCE_MASTER on faultypunches.BIOMETRIC_CODE equals emps.BIOMETRIC_CODE
                                 join dept in _appEntity.TAB_DEPARTMENT_MASTER on emps.DEPT_ID equals dept.DEPT_ID
                                 where dept.DEPT_ID == dept_id && faultypunches.ATTENDANCE_DATE.Value.Month == month && faultypunches.ATTENDANCE_DATE.Value.Year == year
                                 select new WorkforceFaultyData
                                 {
                                     EMP_ID = emps.EMP_ID,
                                     EMP_NAME = emps.EMP_NAME,
                                     WF_ID = emps.WF_ID,
                                     DEPT_ID = dept.DEPT_ID,
                                     DEPT_NAME = dept.DEPT_NAME,
                                     BIO_CODE = emps.BIOMETRIC_CODE,
                                     ATTENDANCE_DATE = faultypunches.ATTENDANCE_DATE,
                                     PUNCH_RECORD = faultypunches.PUNCH_RECORD,
                                     REMARKS = faultypunches.REMARKS
                                 }).ToList();

            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "WorkforceRepository", "GetFaultyData", "", ex.ToString());
            }

            return faultyrecords;
        }

        public void Edit(WorkforceMetaData workforce)
        {
            try
            {
                Wfm.App.Core.TAB_WORKFORCE_MASTER obj = _appEntity.TAB_WORKFORCE_MASTER.Where(x => x.WF_ID == workforce.WF_ID).FirstOrDefault();

                if (obj != null)
                {
                    obj.WF_ID = workforce.WF_ID;
                    obj.EMP_ID = workforce.EMP_ID;
                    obj.WF_EMP_TYPE = workforce.WF_EMP_TYPE;
                    obj.COMPANY_ID = workforce.COMPANY_ID;
                    obj.AGENCY_ID = workforce.AGENCY_ID;
                    obj.BIOMETRIC_CODE = workforce.BIOMETRIC_CODE;
                    obj.REFERENCE_ID = workforce.REFERENCE_ID;
                    obj.EMP_NAME = workforce.EMP_NAME;
                    obj.FATHER_NAME = workforce.FATHER_NAME;
                    obj.GENDER = workforce.GENDER;
                    obj.DEPT_ID = workforce.DEPT_ID;
                    obj.SUBDEPT_ID = workforce.SUBDEPT_ID;
                    obj.DATE_OF_BIRTH = workforce.DATE_OF_BIRTH;
                    obj.NATIONALITY = workforce.NATIONALITY;
                    obj.WF_EDUCATION_ID = workforce.WF_EDUCATION_ID;
                    obj.MARITAL_ID = workforce.MARITAL_ID;
                    obj.DOJ = workforce.DOJ;
                    obj.DOJ_AS_PER_EPF = workforce.DOJ_AS_PER_EPF;
                    obj.WF_DESIGNATION_ID = workforce.WF_DESIGNATION_ID;
                    obj.SKILL_ID = workforce.SKILL_ID;
                    obj.EMP_TYPE_ID = workforce.EMP_TYPE_ID;
                    obj.MOBILE_NO = workforce.MOBILE_NO;
                    obj.ALTERNATE_NO = workforce.ALTERNATE_NO;
                    obj.EMAIL_ID = workforce.EMAIL_ID;
                    obj.PRESENT_ADDRESS = workforce.PRESENT_ADDRESS;
                    obj.PRESENT_ADDRESS_CITY = workforce.PRESENT_ADDRESS_CITY;
                    obj.PRESENT_ADDRESS_STATE = workforce.PRESENT_ADDRESS_STATE;
                    obj.PRESENT_ADDRESS_PIN = workforce.PRESENT_ADDRESS_PIN;
                    obj.PERMANENT_ADDRESS = workforce.PERMANENT_ADDRESS;
                    obj.PERMANENT_ADDRESS_CITY = workforce.PERMANENT_ADDRESS_CITY;
                    obj.PERMANENT_ADDRESS_STATE = workforce.PERMANENT_ADDRESS_STATE;
                    obj.PERMANENT_ADDRESS_PIN = workforce.PERMANENT_ADDRESS_PIN;
                    obj.AADHAR_NO = workforce.AADHAR_NO;
                    obj.EMP_STATUS_ID = workforce.EMP_STATUS_ID;
                    obj.IDENTIFICATION_MARK = workforce.IDENTIFICATION_MARK;
                    //obj.MRF_INTERNAL_ID = workforce.MRF_INTERNAL_ID;
                    obj.STATUS = "Y";
                    obj.UPDATED_DATE = DateTime.Now;
                    obj.UPDATED_BY = SessionHelper.Get<String>("Username");
                    obj.BUILDING_ID = workforce.BUILDING_ID;
                    var wf_photos = _appEntity.TAB_WORKFORCE_PHOTO_MASTER.Where(x => x.WF_ID == obj.WF_ID).FirstOrDefault();
                    if (wf_photos != null)
                    {
                        wf_photos.PHOTO = workforce.PHOTO != null ? workforce.PHOTO : wf_photos.PHOTO;
                        wf_photos.EMP_SIGNATURE = workforce.EMP_SIGNATURE != null ? workforce.EMP_SIGNATURE : wf_photos.EMP_SIGNATURE;
                        wf_photos.UPDATED_BY = SessionHelper.Get<String>("Username");
                    }
                    else
                    {
                        List<TAB_WORKFORCE_PHOTO_MASTER> images = new List<TAB_WORKFORCE_PHOTO_MASTER>() {
                             new TAB_WORKFORCE_PHOTO_MASTER
                            {
                                PHOTO_ID = Guid.NewGuid(),
                                //WF_ID = workforce.WF_ID,
                                PHOTO = workforce.PHOTO,
                                EMP_SIGNATURE = workforce.EMP_SIGNATURE,
                                CREATED_BY = SessionHelper.Get<String>("Username"),
                                CREATED_DATE = DateTime.Now,
                            }
                        };
                        obj.TAB_WORKFORCE_PHOTO_MASTER = images;
                        //TAB_WORKFORCE_PHOTO_MASTER images = new TAB_WORKFORCE_PHOTO_MASTER
                        //     {
                        //         PHOTO_ID = Guid.NewGuid(),
                        //         WF_ID = workforce.WF_ID,
                        //         PHOTO = workforce.PHOTO,
                        //         EMP_SIGNATURE = workforce.EMP_SIGNATURE,
                        //         CREATED_BY = workforce.CREATED_BY,
                        //         CREATED_DATE = DateTime.Now,

                        // };
                        // _appEntity.TAB_WORKFORCE_PHOTO_MASTER.Add(images);

                    }
                    _appEntity.SaveChanges();
                    Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.INFO, "WFMUI", "WFM.App", "WorkforceReporsitory", "Edit", "", workforce.WF_ID.ToString() + " record has been updated successfully.");
                }
            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "WorkforceReporsitory", "Edit", "", ex.InnerException.ToString());
            }
        }



        public BiometricAndAttendance GetWorkingHoursBywfId(Guid wfid, DateTime attdate)
        {
            BiometricAndAttendance empAtt = new BiometricAndAttendance();
            try
            {
                //WorkforceMetaData empdetail = null;
                var dailyWorkCheck = _appEntity.TAB_WORKFORCE_DAILYWORK.Where(x => x.WF_ID == wfid && x.WORK_DATE == attdate).FirstOrDefault();
                if (dailyWorkCheck != null)
                {
                    empAtt.START_DATE = $"Daily work item added for {attdate.ToShortDateString()}";
                    return empAtt;
                }

                var empdetail = (from wf in _appEntity.TAB_WORKFORCE_MASTER
                                 join bio in _appEntity.TAB_BIOMETRIC on wf.BIOMETRIC_CODE equals bio.BIOMETRIC_CODE
                                 join dept in _appEntity.TAB_DEPARTMENT_MASTER on wf.DEPT_ID equals dept.DEPT_ID
                                 where wf.WF_ID == wfid && bio.ATTENDANCE_DATE == attdate

                                 select new
                                 {
                                     wf.BIOMETRIC_CODE,
                                     wf.EMP_NAME,
                                     wf.WF_ID,
                                     bio.START_DATE,
                                     bio.END_DATE,
                                     bio.ATTENDANCE_DATE,
                                     dept.DEPT_NAME,
                                 }).FirstOrDefault();
                if (empdetail != null)
                {
                    var worforce = new BiometricAndAttendance
                    {
                        BIOMETRIC_CODE = empdetail.BIOMETRIC_CODE,
                        EMP_NAME = empdetail.EMP_NAME,
                        WF_ID = empdetail.WF_ID,
                        START_DATE = $"{empdetail.START_DATE.ToShortDateString()} {empdetail.START_DATE.Hour.ToString("00")}:{empdetail.START_DATE.Minute.ToString("00")}",
                        END_DATE = empdetail.END_DATE != null ? $"{empdetail.END_DATE.Value.ToShortDateString()} {empdetail.END_DATE.Value.Hour.ToString("00")}:{empdetail.END_DATE.Value.Minute.ToString("00")}" : "",
                        ATTENDANCE_DATE = empdetail.ATTENDANCE_DATE.Value.ToShortDateString()
                    };
                    empAtt = worforce;

                    DataSet ds = new DataSet("WorkingHours");
                    using (SqlConnection conn = new SqlConnection(Configurations.SqlConnectionString))
                    {
                        string attDate = attdate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                        //string sqlQuery = $"Select WorkInMinutes,OverTimeInMinutes,NoOfDaysPresent from dbo.CalculateWorkingAndOverTimeMinutes('{empdetail.BIOMETRIC_CODE}','{attDate}','{attDate}')";

                        string sqlQuery = $"Select Duration as WorkInMinutes,OverTime as OverTimeInMinutes from TAB_BIOMETRIC  where BIOMETRIC_CODE = '{empdetail.BIOMETRIC_CODE}' and ATTENDANCE_DATE='{attDate}'AND STATUS_CODE in ('P','½P','HP','WOP','WO½P','H½P')";

                        SqlCommand sqlComm = new SqlCommand(sqlQuery, conn);
                        sqlComm.CommandType = CommandType.Text;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlComm;
                        da.Fill(ds);

                        if (ds != null)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                var WorkINMin = Convert.ToInt32(dr["WorkInMinutes"]);
                                int dur = 480;
                                if (empdetail.DEPT_NAME == "FORGING SHOP")
                                {
                                    dur = 510;
                                }

                                var workHour = 0; decimal workMinute = 0; var overHour = 0; decimal overMinute = 0;
                                if (WorkINMin >= dur)
                                {
                                    //var OT = Convert.ToInt32(dr["WorkInMinutes"]) - 480;
                                    workHour = dur / 60;
                                    workMinute = dur % 60;

                                    //overHour = OT / 60;
                                    //overMinute = OT % 60;
                                }
                                else
                                {
                                    workHour = WorkINMin / 60;
                                    workMinute = WorkINMin % 60;
                                }
                                if (dr["OverTimeInMinutes"].ToString() != "")
                                {
                                    overHour = Convert.ToInt32(dr["OverTimeInMinutes"]) / 60;
                                    overMinute = Convert.ToDecimal(dr["OverTimeInMinutes"]) % 60;
                                }

                                //}

                                empAtt.DUTY_HOURS = $"{workHour}:{workMinute}";
                                empAtt.OVERTIME_HOURS = $"{overHour}:{overMinute}";
                                empAtt.OT_Min = dr["OverTimeInMinutes"].ToString() == "" ? 0 : Convert.ToInt32(dr["OverTimeInMinutes"]);

                            }
                        }
                    }


                }

            }
            catch (SqlException ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "WorkforceReporsitory", "GetAttendanceBywfId", "", "SQL Error : " + ex.InnerException.ToString());
            }
            return empAtt;
        }

        public List<WorkforceAttendance> GetAttendanceBywfid(Guid wfid, Guid deptid, int month, int year)
        {
            List<WorkforceAttendance> attrecords = null;
            try
            {
                attrecords = (from att in _appEntity.TAB_BIOMETRIC
                              join emp in _appEntity.TAB_WORKFORCE_MASTER on att.BIOMETRIC_CODE equals emp.BIOMETRIC_CODE
                              //join db in _appEntity.TAB_BIOMETRIC_DATABASE on att.MDBFILEID equals db.ID
                              where emp.DEPT_ID == deptid && att.ATTENDANCE_DATE.Value.Month == month && att.ATTENDANCE_DATE.Value.Year == year && emp.WF_ID == wfid
                              orderby att.ATTENDANCE_DATE, att.START_DATE
                              select new WorkforceAttendance
                              {
                                  WF_ID = emp.WF_ID,
                                  BIOMETRIC_CODE = emp.BIOMETRIC_CODE,
                                  ATTENDANCE_DATE = att.ATTENDANCE_DATE,
                                  START_DATE = att.START_DATE,
                                  END_DATE = att.END_DATE,
                                  STATUS_CODE = att.STATUS_CODE,
                                  SHIFT_STARTTIME = DbFunctions.CreateTime(att.START_DATE.Hour, att.START_DATE.Minute, att.START_DATE.Second),
                                  SHIFT_ENDTIME = DbFunctions.CreateTime(att.END_DATE.Value.Hour, att.END_DATE.Value.Minute, att.END_DATE.Value.Second),
                                  //mdbfilename = db.MDBFILENAME

                              }).ToList();

            }
            catch (Exception ex)
            {
                Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.ERROR, "WFMUI", "WFM.App", "WorkforceRepository", "GetFaultyData", "", ex.ToString());
            }

            return attrecords;
        }
    }
    public enum Enum_EMPL_TYPE_MASTER
    {
        SALARIED = 1,
        PIECE_WAGER
    }
}
