using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wfm.App.Common;
using Wfm.App.Core;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Interfaces;
using TAB_SUBDEPARTMENT_MASTER = Wfm.App.Core.TAB_SUBDEPARTMENT_MASTER;

namespace Wfm.App.Infrastructure.Repositories
{
    public class SubDepartmentRepository : ISubDepartmentRepository
    {
        private ApplicationEntities _appEntity;

        public SubDepartmentRepository()
        {
            _appEntity = new ApplicationEntities();
        }
        public List<SubDepartmentMasterMetaData> GetAllSubDepartment()
        {
            List<SubDepartmentMasterMetaData> subdepartments = null;
            try
            {
                Guid loggedInUserId = Utility.GetLoggedInUserId();

                subdepartments = (from DM in _appEntity.TAB_DEPARTMENT_MASTER
                                  join UDM in _appEntity.TAB_USER_DEPARTMENT_MAPPING on DM.DEPT_ID equals UDM.DEPT_ID
                                  join SDM in _appEntity.TAB_SUBDEPARTMENT_MASTER on DM.DEPT_ID equals SDM.DEPT_ID
                                  join LM in _appEntity.TAB_LOGIN_MASTER on UDM.USER_ID equals LM.USER_ID
                                  where LM.USER_ID == loggedInUserId && DM.status == "Y"
                                  select new SubDepartmentMasterMetaData
                                  {
                                      SUBDEPT_ID = SDM.SUBDEPT_ID,
                                      DEPT_ID = DM.DEPT_ID,
                                      SUBDEPT_NAME = DM.DEPT_NAME
                                  })
                                  .OrderBy(x => x.SUBDEPT_NAME)
                                  .Distinct()
                                  .ToList();

                return subdepartments;
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - SubDepartmentRepository.cs, Method - GetAllSubDepartment", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }

            return subdepartments;
        }

        public bool IsSubDepartmentNameAvailable(string subDept_Name, Guid dEPT_ID, Guid sUBDEPT_ID, Guid BUILDING_ID)
        {
            Guid companyid = SessionHelper.Get<Guid>("CompanyId");
            if (sUBDEPT_ID != new Guid())
            {
                var detp = _appEntity.TAB_SUBDEPARTMENT_MASTER.Where(x => x.SUBDEPT_NAME == subDept_Name
                && x.BUILDING_ID == BUILDING_ID
                && x.DEPT_ID != dEPT_ID
                && x.COMPANY_ID == companyid
                && x.SUBDEPT_ID == sUBDEPT_ID).FirstOrDefault();
                if (detp != null)
                {
                    return true;
                }
                return false;
            }
            else
            {
                var detp = _appEntity.TAB_SUBDEPARTMENT_MASTER.Where(x => x.SUBDEPT_NAME == subDept_Name
                && x.BUILDING_ID == BUILDING_ID
                && x.DEPT_ID == dEPT_ID
                && x.COMPANY_ID == companyid).FirstOrDefault();
                if (detp != null)
                {
                    return true;
                }
            }
            return false;
        }

        public SubDepartmentMasterMetaData GetSubDepartmentById(Guid id)
        {
            Guid companyid = SessionHelper.Get<Guid>("CompanyId");
            return _appEntity.TAB_SUBDEPARTMENT_MASTER.Where(x => x.SUBDEPT_ID == id).Select(
                  x => new SubDepartmentMasterMetaData
                  {
                      BUILDING_ID = (Guid)x.BUILDING_ID,
                      SUBDEPT_ID = x.SUBDEPT_ID,
                      DEPT_ID = x.DEPT_ID,
                      SUBDEPT_NAME = x.SUBDEPT_NAME,
                      FreezingStrength = x.FreezingStrength
                  }).FirstOrDefault();
            //return (from d in _appEntity.TAB_SUBDEPARTMENT_MASTER
            //              join DM in _appEntity.TAB_DEPARTMENT_MASTER on d.DEPT_ID equals DM.DEPT_ID
            //              where d.COMPANY_ID == companyid
            //              select new SubDepartmentMasterMetaData
            //              {
            //                  SUBDEPT_ID = d.SUBDEPT_ID,
            //                  DEPT_ID = d.DEPT_ID,
            //                  SUBDEPT_NAME = d.SUBDEPT_NAME,
            //                  FreezingStrength = d.FreezingStrength,
            //                  BUILDING_ID=(Guid)d.BUILDING_ID,
            //              }).OrderBy(x => x.DEPT_NAME).FirstOrDefault();

        }

        public bool AddSubDepartment(SubDepartmentMasterMetaData metaData)
        {
            DateTime dt = DateTime.Now;
            Guid companyid = SessionHelper.Get<Guid>("CompanyId");
            var dept = _appEntity.TAB_DEPARTMENT_MASTER.Where(x => x.DEPT_ID == metaData.DEPT_ID
            && x.COMPANY_ID == companyid && x.BUILDING_ID == metaData.BUILDING_ID).FirstOrDefault();
            if (metaData.SUBDEPT_ID != new Guid())
            {
                var subdept = _appEntity.TAB_SUBDEPARTMENT_MASTER.Where(x => x.SUBDEPT_ID == metaData.SUBDEPT_ID
                && x.BUILDING_ID == metaData.BUILDING_ID).FirstOrDefault();
                subdept.SUBDEPT_NAME = metaData.SUBDEPT_NAME;
                subdept.DEPT_ID = metaData.DEPT_ID;
                subdept.BUILDING_ID = metaData.BUILDING_ID;
                subdept.DEPT_HEAD_ID = dept.DEPT_HEAD_ID;
                subdept.DEPT_HEAD_NAME = dept.DEPT_HEAD_NAME;
                subdept.FreezingStrength = metaData.FreezingStrength;
                subdept.UPDATED_BY = SessionHelper.Get<String>("Username");
                subdept.UPDATED_DATE = dt;
            }
            else
            {
                TAB_SUBDEPARTMENT_MASTER sub_dept = new TAB_SUBDEPARTMENT_MASTER()
                {
                    DEPT_HEAD_ID = dept.DEPT_HEAD_ID,
                    DEPT_HEAD_NAME = dept.DEPT_HEAD_NAME,
                    BUILDING_ID = metaData.BUILDING_ID,
                    DEPT_ID = metaData.DEPT_ID,
                    COMPANY_ID = companyid,
                    SUBDEPT_ID = Guid.NewGuid(),
                    SUBDEPT_NAME = metaData.SUBDEPT_NAME,
                    FreezingStrength = metaData.FreezingStrength,
                    CREATED_DATE = dt,
                    CREATED_BY = SessionHelper.Get<String>("Username"),
                    STATUS = "Y"
                };
                _appEntity.TAB_SUBDEPARTMENT_MASTER.Add(sub_dept);
            }
            _appEntity.SaveChanges();
            return true;
        }

        public List<SubDepartmentMasterMetaData> GetAllSubDepartmentOnlyForAdmin(Guid companyId)
        {
            List<SubDepartmentMasterMetaData> subdepartments = null;
            try
            {
                subdepartments = (from SDM in _appEntity.TAB_SUBDEPARTMENT_MASTER
                                  join DM in _appEntity.TAB_DEPARTMENT_MASTER on SDM.DEPT_ID equals DM.DEPT_ID
                                  where SDM.STATUS == "Y" && SDM.COMPANY_ID == companyId
                                  select new SubDepartmentMasterMetaData
                                  {
                                      DEPT_ID = DM.DEPT_ID,
                                      DEPT_NAME = DM.DEPT_NAME,
                                      SUBDEPT_ID = SDM.SUBDEPT_ID,
                                      SUBDEPT_NAME = SDM.SUBDEPT_NAME
                                  })
                                  .OrderBy(x => x.SUBDEPT_NAME)
                                  .Distinct()
                                  .ToList();

                return subdepartments;
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - SubDepartmentRepository.cs, Method - GetAllSubDepartmentOnlyForAdmin", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }

            return subdepartments;
        }

        public List<SubDepartmentMasterMetaData> GetSubDepartmentByCompanyId(Guid companyId)
        {
            List<SubDepartmentMasterMetaData> subDepartmentMasterMetaData = null;
            try
            {
                Guid loggedInUserId = Utility.GetLoggedInUserId();

                subDepartmentMasterMetaData = (from DM in _appEntity.TAB_DEPARTMENT_MASTER
                                               join UDM in _appEntity.TAB_USER_DEPARTMENT_MAPPING on DM.DEPT_ID equals UDM.DEPT_ID
                                               join SDM in _appEntity.TAB_SUBDEPARTMENT_MASTER on DM.DEPT_ID equals SDM.DEPT_ID
                                               join LM in _appEntity.TAB_LOGIN_MASTER on UDM.USER_ID equals LM.USER_ID
                                               where LM.USER_ID == loggedInUserId && DM.status == "Y" && DM.COMPANY_ID == companyId
                                               select new SubDepartmentMasterMetaData
                                               {
                                                   SUBDEPT_ID = SDM.SUBDEPT_ID,
                                                   DEPT_ID = SDM.DEPT_ID,
                                                   SUBDEPT_NAME = SDM.SUBDEPT_NAME,
                                                   COMPANY_ID = SDM.COMPANY_ID,
                                                   DEPT_HEAD_ID = SDM.DEPT_HEAD_ID,
                                                   DEPT_HEAD_NAME = SDM.DEPT_HEAD_NAME
                                               })
                                               .OrderBy(x => x.SUBDEPT_NAME)
                                               .Distinct()
                                               .ToList();
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - SubDepartmentRepository.cs, Method - GetSubDepartmentByCompanyId", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
            return subDepartmentMasterMetaData;
        }

        public List<SubDepartmentMasterMetaData> GetSubDepartmentsByUserId(Guid comp_id, Guid user_id)
        {
            List<SubDepartmentMasterMetaData> subdepartments = null;
            try
            {
                string Role = SessionHelper.Get<string>("LoginUserId");

                if (Role == "admin")
                {
                    subdepartments = (from dept in _appEntity.TAB_DEPARTMENT_MASTER
                                      join dept_user in _appEntity.TAB_USER_DEPARTMENT_MAPPING on dept.DEPT_ID equals dept_user.DEPT_ID
                                      join subdept in _appEntity.TAB_SUBDEPARTMENT_MASTER on dept.DEPT_ID equals subdept.DEPT_ID
                                      where dept.COMPANY_ID == comp_id && dept_user.USER_ID == user_id
                                      select new SubDepartmentMasterMetaData
                                      {
                                          SUBDEPT_ID = subdept.SUBDEPT_ID,
                                          DEPT_ID = subdept.DEPT_ID,
                                          SUBDEPT_NAME = subdept.SUBDEPT_NAME
                                      })
                                  .OrderBy(x => x.SUBDEPT_NAME)
                                  .Distinct()
                                  .ToList();
                }
                else
                {
                    subdepartments = (from dept in _appEntity.TAB_DEPARTMENT_MASTER
                                      join dept_user in _appEntity.TAB_USER_DEPARTMENT_MAPPING on dept.DEPT_ID equals dept_user.DEPT_ID
                                      join subdept in _appEntity.TAB_SUBDEPARTMENT_MASTER on dept.DEPT_ID equals subdept.DEPT_ID
                                      where dept.COMPANY_ID == comp_id && dept_user.USER_ID == user_id
                                      select new SubDepartmentMasterMetaData
                                      {
                                          SUBDEPT_ID = subdept.SUBDEPT_ID,
                                          DEPT_ID = subdept.DEPT_ID,
                                          SUBDEPT_NAME = subdept.SUBDEPT_NAME
                                      })
                                  .OrderBy(x => x.SUBDEPT_NAME)
                                  .Distinct()
                                  .ToList();
                }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - SubDepartmentRepository.cs, Method - GetSubDepartmentsByUserId", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }

            return subdepartments;
        }

        public List<SubDepartmentMasterMetaData> GetSubDepartmentsByDeptId(Guid? dept_id)
        {
            Guid companyid = SessionHelper.Get<Guid>("CompanyId");
            List<SubDepartmentMasterMetaData> subdepartments = null;
            try
            {
                subdepartments = (from dept in _appEntity.TAB_DEPARTMENT_MASTER
                                  join subdept in _appEntity.TAB_SUBDEPARTMENT_MASTER on dept.DEPT_ID equals subdept.DEPT_ID
                                  join BM in _appEntity.TAB_BUILDING_MASTER on subdept.BUILDING_ID equals BM.BUILDING_ID
                                  where dept.DEPT_ID == (dept_id != null ? dept_id : dept.DEPT_ID)
                                  select new SubDepartmentMasterMetaData
                                  {
                                      DEPT_HEAD_NAME = BM.BUILDING_NAME,
                                      SUBDEPT_ID = subdept.SUBDEPT_ID,
                                      DEPT_NAME = dept.DEPT_NAME,
                                      SUBDEPT_NAME = subdept.SUBDEPT_NAME,
                                      CREATED_BY = subdept.CREATED_BY
                                  })
                                  .OrderBy(x => x.SUBDEPT_NAME)
                                  .Distinct()
                                  .ToList();
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - SubDepartmentRepository.cs, Method - GetSubDepartmentsByDeptId", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }

            return subdepartments;
        }

        public List<SubDepartmentMasterMetaData> GetSubDepartmentsByDeptId_N(Guid? dept_id, Guid? BUILDING_ID2)
        {
            Guid companyid = SessionHelper.Get<Guid>("CompanyId");
            List<SubDepartmentMasterMetaData> subdepartments = null;
            try
            {
                subdepartments = (from dept in _appEntity.TAB_DEPARTMENT_MASTER
                                  join subdept in _appEntity.TAB_SUBDEPARTMENT_MASTER on dept.DEPT_ID equals subdept.DEPT_ID
                                  join BM in _appEntity.TAB_BUILDING_MASTER on subdept.BUILDING_ID equals BM.BUILDING_ID
                                  where dept.DEPT_ID == (dept_id != null ? dept_id : dept.DEPT_ID)
                                  && dept.BUILDING_ID == (BUILDING_ID2 != null ? BUILDING_ID2 : dept.BUILDING_ID)
                                  select new SubDepartmentMasterMetaData
                                  {
                                      DEPT_HEAD_NAME = BM.BUILDING_NAME,
                                      SUBDEPT_ID = subdept.SUBDEPT_ID,
                                      DEPT_NAME = dept.DEPT_NAME,
                                      SUBDEPT_NAME = subdept.SUBDEPT_NAME,
                                      CREATED_BY = subdept.CREATED_BY,
                                      BUILDING_ID = (Guid)dept.BUILDING_ID
                                  })
                                  .OrderBy(x => x.SUBDEPT_NAME)
                                  .Distinct()
                                  .ToList();
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - SubDepartmentRepository.cs, Method - GetSubDepartmentsByDeptId_N", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }

            return subdepartments;
        }


        public List<SubDepartmentMasterMetaData> GetSubDepartmentsByDeptId(Guid comp_id, Guid user_id, Guid dept_id)
        {
            List<SubDepartmentMasterMetaData> subdepartments = null;
            try
            {
                Guid loggedInUserId = Utility.GetLoggedInUserId();
                string Role = SessionHelper.Get<string>("LoginUserId");

                if (Role == "admin")
                {
                    subdepartments = (from dept in _appEntity.TAB_DEPARTMENT_MASTER
                                          //join dept_user in _appEntity.TAB_USER_DEPARTMENT_MAPPING on dept.DEPT_ID equals dept_user.DEPT_ID
                                      join subdept in _appEntity.TAB_SUBDEPARTMENT_MASTER on dept.DEPT_ID equals subdept.DEPT_ID
                                      where dept.COMPANY_ID == comp_id 
                                      && dept.DEPT_ID == dept_id
                                      && dept.status=="Y"
                                      select new SubDepartmentMasterMetaData
                                      {
                                          SUBDEPT_ID = subdept.SUBDEPT_ID,
                                          DEPT_ID = subdept.DEPT_ID,
                                          SUBDEPT_NAME = subdept.SUBDEPT_NAME
                                      })
                                  .Distinct()
                                  .OrderBy(x => x.SUBDEPT_NAME)
                                  .ToList();
                }
                else
                {
                    subdepartments = (from subdept in _appEntity.TAB_SUBDEPARTMENT_MASTER
                                      join udm in _appEntity.TAB_USER_DEPARTMENT_MAPPING on subdept.SUBDEPT_ID equals udm.SUBDEPT_ID
                                      where subdept.COMPANY_ID == comp_id 
                                      && udm.USER_ID == loggedInUserId 
                                      &&  subdept.DEPT_ID == dept_id
                                      && subdept.STATUS == "Y"
                                      select new SubDepartmentMasterMetaData
                                      {
                                          SUBDEPT_ID = subdept.SUBDEPT_ID,
                                          DEPT_ID = subdept.DEPT_ID,
                                          SUBDEPT_NAME = subdept.SUBDEPT_NAME
                                      })
                                  .Distinct()
                                  .OrderBy(x => x.SUBDEPT_NAME)
                                  .ToList();
                }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - SubDepartmentRepository.cs, Method - GetSubDepartmentsByDeptId", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }

            return subdepartments;
        }
        public List<SubDepartmentMasterMetaData> GetAllSubDepartmentByDepartmentId(Guid comp_id, Guid user_id, Guid dept_id)
        {
            List<SubDepartmentMasterMetaData> subdepartments = null;
            try
            {
               
                    subdepartments = (from dept in _appEntity.TAB_DEPARTMENT_MASTER
                                          //join dept_user in _appEntity.TAB_USER_DEPARTMENT_MAPPING on dept.DEPT_ID equals dept_user.DEPT_ID
                                      join subdept in _appEntity.TAB_SUBDEPARTMENT_MASTER on dept.DEPT_ID equals subdept.DEPT_ID
                                      where dept.COMPANY_ID == comp_id 
                                      && dept.DEPT_ID == dept_id
                                      && dept.status=="Y"
                                      select new SubDepartmentMasterMetaData
                                      {
                                          SUBDEPT_ID = subdept.SUBDEPT_ID,
                                          DEPT_ID = subdept.DEPT_ID,
                                          SUBDEPT_NAME = subdept.SUBDEPT_NAME
                                      })
                                  .Distinct()
                                  .OrderBy(x => x.SUBDEPT_NAME)
                                  .ToList();
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - SubDepartmentRepository.cs, Method - GetSubDepartmentsByDeptId", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }

            return subdepartments;
        }

        public List<SubDepartmentMasterMetaData> GetCityByState(Guid State_ID)
        {
            List<SubDepartmentMasterMetaData> subdepartments = null;
            try
            {
                subdepartments = (from sc in _appEntity.TAB_STATE_CITY_MASTER

                                  where sc.STATE_ID == State_ID
                                  select new SubDepartmentMasterMetaData
                                  {
                                      SUBDEPT_ID = sc.CITY_ID,
                                      DEPT_ID = sc.STATE_ID,
                                      SUBDEPT_NAME = sc.CITY_NAME
                                  })
                                  .Distinct()
                                  .OrderBy(x => x.SUBDEPT_NAME)
                                  .ToList();
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - SubDepartmentRepository.cs, Method - GetCityByState", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }

            return subdepartments;
        }

    }
}
