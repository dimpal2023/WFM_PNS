using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Wfm.App.Common;
using Wfm.App.Core;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Interfaces;

namespace Wfm.App.Infrastructure.Repositories
{
    public class MasterDataRepository : IMasterDataRepository
    {
        private ApplicationEntities _appEntity;

        public MasterDataRepository()
        {
            _appEntity = new ApplicationEntities();
        }

        public List<CompanyMasterMetaData> GetCompanyList()
        {
            List<Core.TAB_COMPANY_MASTER> companies = _appEntity.TAB_COMPANY_MASTER.Where(x => x.Status == "Y").ToList();
            List<CompanyMasterMetaData> companiesMaster = new List<CompanyMasterMetaData>();
            try
            {
                if (companies != null && companies.Count > 0)
                {
                    foreach (Core.TAB_COMPANY_MASTER comp in companies)
                    {
                        CompanyMasterMetaData companyMaster = new CompanyMasterMetaData
                        {
                            COMPANY_ID = comp.COMPANY_ID,
                            COMPANY_NAME = comp.COMPANY_NAME,
                            Created_by = SessionHelper.Get<string>("LoginUserId"),
                            Created_date = comp.Created_date,
                            Status = comp.Status
                        };

                        companiesMaster.Add(companyMaster);
                    }
                }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - MasterDataRepository.cs, Method - GetCompanyList", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
            return companiesMaster;
        }

        public List<BuildingMasterMetaData> GetBuildingList()
        {
            List<Core.TAB_BUILDING_MASTER> buildings = _appEntity.TAB_BUILDING_MASTER.Where(x => x.status == "Y").OrderBy(x => x.BUILDING_NAME).ToList();
            List<BuildingMasterMetaData> buildingsMaster = new List<BuildingMasterMetaData>();

            try
            {
                if (buildings != null && buildings.Count > 0)
                {
                    foreach (Core.TAB_BUILDING_MASTER building in buildings)
                    {
                        BuildingMasterMetaData objbuilding = new BuildingMasterMetaData
                        {
                            BUILDING_ID = building.BUILDING_ID,
                            BUILDING_NAME = building.BUILDING_NAME,
                            COMPANY_ID = building.COMPANY_ID,
                            BUILDING_ADDRESS = building.BUILDING_ADDRESS
                        };

                        buildingsMaster.Add(objbuilding);
                    }
                }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - MasterDataRepository.cs, Method - GetBuildingList", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
            return buildingsMaster;
        }

        public List<DepartmentMasterMetaData> GetDepartmentList()
        {
            var companyid = SessionHelper.Get<Guid>("CompanyId");

            Guid loggedInUserId = Utility.GetLoggedInUserId();

            List<Core.TAB_DEPARTMENT_MASTER> departments = (from DM in _appEntity.TAB_DEPARTMENT_MASTER
                                                            join UDM in _appEntity.TAB_USER_DEPARTMENT_MAPPING on DM.DEPT_ID equals UDM.DEPT_ID
                                                            join LM in _appEntity.TAB_LOGIN_MASTER on UDM.USER_ID equals LM.USER_ID
                                                            where LM.USER_ID == loggedInUserId && DM.status == "Y" && DM.COMPANY_ID == companyid
                                                            select new Core.TAB_DEPARTMENT_MASTER
                                                            {
                                                                DEPT_ID = DM.DEPT_ID,
                                                                DEPT_NAME = DM.DEPT_NAME,
                                                                COMPANY_ID = DM.COMPANY_ID,
                                                                DEPT_HEAD_ID = DM.DEPT_HEAD_ID,
                                                                DEPT_HEAD_NAME = DM.DEPT_HEAD_NAME
                                                            }).OrderBy(x => x.DEPT_NAME).ToList();
            List<DepartmentMasterMetaData> departmentMaster = new List<DepartmentMasterMetaData>();

            if (departments != null && departments.Count > 0)
            {
                foreach (Core.TAB_DEPARTMENT_MASTER dept in departments)
                {
                    DepartmentMasterMetaData deptMaster = new DepartmentMasterMetaData
                    {
                        DEPT_ID = dept.DEPT_ID,
                        DEPT_NAME = dept.DEPT_NAME,
                        COMPANY_ID = dept.COMPANY_ID,
                        DEPT_HEAD_ID = dept.DEPT_HEAD_ID,
                        DEPT_HEAD_NAME = dept.DEPT_HEAD_NAME,
                        created_date = dept.created_date,
                        Created_by = SessionHelper.Get<string>("LoginUserId"),
                        status = dept.status
                    };

                    departmentMaster.Add(deptMaster);
                }
            }

            return departmentMaster;
        }


        public List<AgencyMasterMetaData> GetAgencyList()
        {
            List<Core.TAB_AGENCY_MASTER> agencies = _appEntity.TAB_AGENCY_MASTER.Where(x => x.status == "Y").OrderBy(x => x.AGENCY_NAME).ToList();
            List<AgencyMasterMetaData> agencyMaster = new List<AgencyMasterMetaData>();
            try
            {
                if (agencies != null && agencies.Count > 0)
                {
                    foreach (Core.TAB_AGENCY_MASTER ag in agencies)
                    {
                        AgencyMasterMetaData agency = new AgencyMasterMetaData
                        {
                            AGENCY_ID = ag.AGENCY_ID,
                            AGENCY_NAME = ag.AGENCY_NAME,
                            COMPANY_ID = ag.COMPANY_ID,
                            AGENCY_ADDRESS = ag.AGENCY_ADDRESS,
                            AUTTHENTICATED_PERSON_NAME = ag.AUTTHENTICATED_PERSON_NAME,
                            AUTHENTICATED_PERSON_NUM = ag.AUTHENTICATED_PERSON_NUM,
                            created_date = ag.created_date,
                            Created_by = SessionHelper.Get<string>("LoginUserId"),
                            status = ag.status
                        };

                        agencyMaster.Add(agency);
                    }
                }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - MasterDataRepository.cs, Method - GetAgencyList", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
            return agencyMaster;
        }
        public List<BankMasterMetaData> GetAllBank()
        {
            List<Core.TAB_BANK_MASTER> banklist = _appEntity.TAB_BANK_MASTER.Where(x => x.STATUS == "Y").OrderBy(x => x.BANK_NAME).ToList();
            List<BankMasterMetaData> bank = new List<BankMasterMetaData>();
            try
            {
                if (banklist != null && banklist.Count > 0)
                {
                    foreach (Core.TAB_BANK_MASTER ag in banklist)
                    {
                        BankMasterMetaData Bank = new BankMasterMetaData
                        {
                            BANK_ID = ag.BANK_ID,
                            BANK_NAME = ag.BANK_NAME,
                            IFSC_CODE = ag.IFSC_CODE,
                            BRANCH_NAME = ag.BRANCH_NAME,
                            STATUS = ag.STATUS
                        };

                        bank.Add(Bank);
                    }
                }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - MasterDataRepository.cs, Method - GetAllBank", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
            return bank;
        }

        public List<BankMasterMetaData> SelectBankData(Guid Bank_ID)
        {
            List<Core.TAB_BANK_MASTER> banklist = _appEntity.TAB_BANK_MASTER.Where(x => x.STATUS == "Y" && x.BANK_ID == Bank_ID).OrderBy(x => x.BANK_NAME).ToList();
            List<BankMasterMetaData> bank = new List<BankMasterMetaData>();
            try
            {
                if (banklist != null && banklist.Count > 0)
                {
                    foreach (Core.TAB_BANK_MASTER ag in banklist)
                    {
                        BankMasterMetaData Bank = new BankMasterMetaData
                        {
                            BANK_ID = ag.BANK_ID,
                            BANK_NAME = ag.BANK_NAME,
                            IFSC_CODE = ag.IFSC_CODE,
                            BRANCH_NAME = ag.BRANCH_NAME,
                            STATUS = ag.STATUS
                        };

                        bank.Add(Bank);
                    }
                }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - MasterDataRepository.cs, Method - SelectBankData", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
            return bank;
        }

        public ExportSalaryMetaData CheckUAN(string UAN_NO)
        {
            ExportSalaryMetaData detail = null;
            try
            {
                detail = (from emp in _appEntity.TAB_WORKFORCE_SALARY_MASTER
                          where emp.UAN_NO == UAN_NO
                          select new ExportSalaryMetaData
                          {
                              SalarySlip = emp.UAN_NO
                          }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - MasterDataRepository.cs, Method - CheckUAN", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
            return detail;
        }
        public ExportSalaryMetaData CheckPAN(string PAN)
        {
            ExportSalaryMetaData detail = null;
            try
            {
                detail = (from emp in _appEntity.TAB_WORKFORCE_SALARY_MASTER
                          where emp.PAN_CARD == PAN
                          select new ExportSalaryMetaData
                          {
                              SalarySlip = emp.PAN_CARD
                          }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - MasterDataRepository.cs, Method - CheckPAN", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
            return detail;
        }
        public ExportSalaryMetaData CheckEPF(string EPF)
        {
            ExportSalaryMetaData detail = null;
            try
            {
                detail = (from emp in _appEntity.TAB_WORKFORCE_SALARY_MASTER
                          where emp.EPF_NO == EPF
                          select new ExportSalaryMetaData
                          {
                              SalarySlip = emp.EPF_NO
                          }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - MasterDataRepository.cs, Method - CheckEPF", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
            return detail;
        }
        public ExportSalaryMetaData CheckESIC(string ESIC)
        {
            ExportSalaryMetaData detail = null;
            try
            {
                detail = (from emp in _appEntity.TAB_WORKFORCE_SALARY_MASTER
                          where emp.ESIC_NO == ESIC
                          select new ExportSalaryMetaData
                          {
                              SalarySlip = emp.ESIC_NO
                          }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - MasterDataRepository.cs, Method - CheckESIC", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
            return detail;
        }
        public ExportSalaryMetaData CheckAccountNo(string ACC)
        {
            ExportSalaryMetaData detail = null;
            try
            {
                detail = (from emp in _appEntity.TAB_WORKFORCE_SALARY_MASTER
                          where emp.BANK_ACCOUNT_NO == ACC
                          select new ExportSalaryMetaData
                          {
                              SalarySlip = emp.BANK_ACCOUNT_NO
                          }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - MasterDataRepository.cs, Method - CheckAccountNo", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
            return detail;
        }


        public List<WorkforceEduMasterMetaData> GetEducationList()
        {
            List<Core.TAB_WORKFORCE_EDU_MASTER> wfedulist = _appEntity.TAB_WORKFORCE_EDU_MASTER.Where(x => x.STATUS == "Y").OrderBy(x => x.WF_COURSE_NAME).ToList();
            List<WorkforceEduMasterMetaData> wfeduMaster = new List<WorkforceEduMasterMetaData>();
            try
            {
                if (wfedulist != null && wfedulist.Count > 0)
                {
                    foreach (Core.TAB_WORKFORCE_EDU_MASTER ed in wfedulist)
                    {
                        WorkforceEduMasterMetaData wfedu = new WorkforceEduMasterMetaData
                        {
                            WF_EDUCATION_ID = ed.WF_EDUCATION_ID,
                            WF_COURSE_NAME = ed.WF_COURSE_NAME,
                            COMPANY_ID = ed.COMPANY_ID,
                            CREATED_DATE = ed.CREATED_DATE,
                            STATUS = ed.STATUS
                        };

                        wfeduMaster.Add(wfedu);
                    }
                }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - MasterDataRepository.cs, Method - GetEducationList", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
            return wfeduMaster;
        }

        public List<MartialStatusMasterMetaData> GetMartialStatusList()
        {
            List<Core.TAB_MARITAL_STATUS_MASTER> mStatuslist = _appEntity.TAB_MARITAL_STATUS_MASTER.Where(x => x.STATUS == "Y").OrderBy(x => x.MARITAL_NAME).ToList();
            List<MartialStatusMasterMetaData> martialStatusMaster = new List<MartialStatusMasterMetaData>();
            try
            {
                if (mStatuslist != null && mStatuslist.Count > 0)
                {
                    foreach (Core.TAB_MARITAL_STATUS_MASTER ms in mStatuslist)
                    {
                        MartialStatusMasterMetaData mStatus = new MartialStatusMasterMetaData
                        {
                            MARITAL_ID = ms.MARITAL_ID,
                            MARITAL_NAME = ms.MARITAL_NAME,
                            COMPANY_ID = ms.COMPANY_ID,
                            CREATED_DATE = ms.CREATED_DATE,
                            STATUS = ms.STATUS
                        };

                        martialStatusMaster.Add(mStatus);
                    }
                }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - MasterDataRepository.cs, Method - GetMartialStatusList", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
            return martialStatusMaster;
        }

        public List<EmpStatusMasterMetaData> GetEmpStatusList()
        {
            List<Core.TAB_EMP_STATUS_MASTER> empStatuslist = _appEntity.TAB_EMP_STATUS_MASTER.Where(x => x.STATUS == "Y").OrderBy(x => x.EMP_STATUS).ToList();
            List<EmpStatusMasterMetaData> empStatusMaster = new List<EmpStatusMasterMetaData>();
            try
            {
                if (empStatuslist != null && empStatuslist.Count > 0)
                {
                    foreach (Core.TAB_EMP_STATUS_MASTER es in empStatuslist)
                    {
                        EmpStatusMasterMetaData empStatus = new EmpStatusMasterMetaData
                        {
                            EMP_STATUS_ID = es.EMP_STATUS_ID,
                            EMP_STATUS = es.EMP_STATUS,
                            COMPANY_ID = es.COMPANY_ID,
                            CREATED_DATE = es.CREATED_DATE,
                            STATUS = es.STATUS
                        };

                        empStatusMaster.Add(empStatus);
                    }
                }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - MasterDataRepository.cs, Method - GetEmpStatusList", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
            return empStatusMaster;
        }

        public List<StateCityMetaData> GetStateCityList()
        {
            List<Core.TAB_STATE_CITY_MASTER> stateCitylist = _appEntity.TAB_STATE_CITY_MASTER.Where(x => x.STATUS == "Y").OrderBy(x => x.CITY_NAME).ToList();

            List<StateCityMetaData> stateCityMaster = new List<StateCityMetaData>();
            try
            {
                if (stateCitylist != null && stateCitylist.Count > 0)
                {
                    foreach (Core.TAB_STATE_CITY_MASTER sc in stateCitylist)
                    {
                        StateCityMetaData StateCity = new StateCityMetaData
                        {
                            STATE_ID = sc.STATE_ID,
                            CITY_ID = sc.CITY_ID,
                            CITY_NAME = sc.CITY_NAME,
                            CREATED_DATE = sc.CREATED_DATE,
                            STATUS = sc.STATUS
                        };

                        stateCityMaster.Add(StateCity);
                    }
                }
            }
            catch(Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - MasterDataRepository.cs, Method - GetStateCityList", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
            return stateCityMaster;
        }

        public List<StateMetaData> GetStateList()
        {
            List<Core.TAB_STATE_MASTER> statelist = _appEntity.TAB_STATE_MASTER.Where(x => x.STATUS == "Y").OrderBy(x => x.STATE_NAME).ToList();

            List<StateMetaData> stateMaster = new List<StateMetaData>();
            try
            {
                if (statelist != null && statelist.Count > 0)
                {
                    foreach (Core.TAB_STATE_MASTER st in statelist)
                    {
                        StateMetaData State = new StateMetaData
                        {
                            STATE_ID = st.STATE_ID,
                            STATE_NAME = st.STATE_NAME,
                            CREATED_DATE = st.CREATED_DATE
                        };

                        stateMaster.Add(State);
                    }
                }
            }
            catch(Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - MasterDataRepository.cs, Method - GetStateList", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
            return stateMaster;
        }


        public List<WFDesignationMasterMetaData> GetDesignationList()
        {
            List<Core.TAB_WF_DESIGNATION_MASTER> designationlist = _appEntity.TAB_WF_DESIGNATION_MASTER.Where(x => x.STATUS == "Y").OrderBy(x => x.WF_DESIGNATION_NAME).ToList();

            List<WFDesignationMasterMetaData> designationMaster = new List<WFDesignationMasterMetaData>();
            try
            {
                if (designationlist != null && designationlist.Count > 0)
                {
                    foreach (Core.TAB_WF_DESIGNATION_MASTER designation in designationlist)
                    {
                        WFDesignationMasterMetaData wfd = new WFDesignationMasterMetaData
                        {
                            WF_DESIGNATION_ID = designation.WF_DESIGNATION_ID,
                            WF_DESIGNATION_NAME = designation.WF_DESIGNATION_NAME,
                            COMPANY_ID = designation.COMPANY_ID
                        };

                        designationMaster.Add(wfd);
                    }
                }
            }
            catch(Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - MasterDataRepository.cs, Method - GetStateList", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
            return designationMaster;
        }

        public List<SkillMasterMetaData> GetSkillList()
        {
            List<Core.TAB_SKILL_MASTER> skilllist = _appEntity.TAB_SKILL_MASTER.Where(x => x.status == "Y").OrderBy(x => x.SKILL_NAME).ToList();

            List<SkillMasterMetaData> skillMaster = new List<SkillMasterMetaData>();
            try
            {
                if (skilllist != null && skilllist.Count > 0)
            {
                foreach (Core.TAB_SKILL_MASTER skill in skilllist)
                {
                    SkillMasterMetaData sk = new SkillMasterMetaData
                    {
                        SKILL_ID = skill.SKILL_ID,
                        SKILL_NAME = skill.SKILL_NAME,
                        COMPANY_ID = skill.COMPANY_ID,
                        BASIC_SALARY = skill.BASIC_SALARY.ToString()
                    };

                    skillMaster.Add(sk);
                }
            }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - MasterDataRepository.cs, Method - GetSkillList", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
            return skillMaster;
        }

        public List<EmplTypeMasterMetaData> GetEmpTypeList()
        {
            List<Core.TAB_EMPL_TYPE_MASTER> empTypelist = _appEntity.TAB_EMPL_TYPE_MASTER.Where(x => x.STATUS == "Y").OrderBy(x => x.EMP_TYPE).ToList();

            List<EmplTypeMasterMetaData> empTypeMaster = new List<EmplTypeMasterMetaData>();
            try
            {
                if (empTypelist != null && empTypelist.Count > 0)
            {
                foreach (Core.TAB_EMPL_TYPE_MASTER empType in empTypelist)
                {
                    EmplTypeMasterMetaData et = new EmplTypeMasterMetaData
                    {
                        EMP_TYPE_ID = empType.EMP_TYPE_ID,
                        EMP_TYPE = empType.EMP_TYPE,
                        COMPANY_ID = empType.COMPANY_ID
                    };

                    empTypeMaster.Add(et);
                }
            }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - MasterDataRepository.cs, Method - GetEmpTypeList", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
            return empTypeMaster;
        }

        public List<StateCityMetaData> GetCityList(String stateId)
        {

            List<Core.TAB_STATE_CITY_MASTER> stateCitylist = _appEntity.TAB_STATE_CITY_MASTER.Where(x => x.STATUS == "Y" & x.STATE_ID == Guid.Parse(stateId)).OrderBy(x => x.CITY_NAME).ToList();

            List<StateCityMetaData> stateCityMaster = new List<StateCityMetaData>();
            try
            {
                if (stateCitylist != null && stateCitylist.Count > 0)
            {
                foreach (Core.TAB_STATE_CITY_MASTER sc in stateCitylist)
                {
                    StateCityMetaData StateCity = new StateCityMetaData
                    {
                        STATE_ID = sc.STATE_ID,
                        CITY_ID = sc.CITY_ID,
                        CITY_NAME = sc.CITY_NAME,
                        CREATED_DATE = sc.CREATED_DATE,
                        STATUS = sc.STATUS
                    };

                    stateCityMaster.Add(StateCity);
                }
            }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - MasterDataRepository.cs, Method - GetCityList", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
            return stateCityMaster;
        }

        public List<WorkforceTypeMetaData> GetWF_EMP_TYPE()
        {

            List<Core.TAB_WORFORCE_TYPE> WF_EMP_TYPElist = _appEntity.TAB_WORFORCE_TYPE.OrderBy(x => x.EMP_TYPE).ToList();

            List<WorkforceTypeMetaData> WF_EMP_TYPEMaster = new List<WorkforceTypeMetaData>();
            try
            {
                if (WF_EMP_TYPElist != null && WF_EMP_TYPElist.Count > 0)
            {
                foreach (Core.TAB_WORFORCE_TYPE sc in WF_EMP_TYPElist)
                {
                    WorkforceTypeMetaData WF_EMP_TYPE = new WorkforceTypeMetaData
                    {
                        WF_EMP_TYPE = sc.WF_EMP_TYPE,
                        EMP_TYPE = sc.EMP_TYPE,
                        COMPANY_ID = sc.COMPANY_ID
                    };

                    WF_EMP_TYPEMaster.Add(WF_EMP_TYPE);
                }
            }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - MasterDataRepository.cs, Method - GetWF_EMP_TYPE", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
            return WF_EMP_TYPEMaster;
        }
       
    }
}