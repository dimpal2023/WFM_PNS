using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wfm.App.Common;
using Wfm.App.Core;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Interfaces;
using TAB_DEPARTMENT_MASTER = Wfm.App.Core.TAB_DEPARTMENT_MASTER;

namespace Wfm.App.Infrastructure.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private ApplicationEntities _appEntity;

        public DepartmentRepository()
        {
            _appEntity = new ApplicationEntities();
        }

        public List<DepartmentMasterMetaData> GetAllDepartment()
        {
            Guid loggedInUserId = Utility.GetLoggedInUserId();

            List<DepartmentMasterMetaData> departments = null;
            try
            {
                departments = (from DM in _appEntity.TAB_DEPARTMENT_MASTER
                               join UDM in _appEntity.TAB_USER_DEPARTMENT_MAPPING on DM.DEPT_ID equals UDM.DEPT_ID
                               join LM in _appEntity.TAB_LOGIN_MASTER on UDM.USER_ID equals LM.USER_ID
                               join SD in _appEntity.TAB_SUBDEPARTMENT_MASTER on DM.DEPT_ID equals SD.DEPT_ID
                               where LM.USER_ID == loggedInUserId && DM.status == "Y"
                               select new DepartmentMasterMetaData
                               {
                                   DEPT_ID = DM.DEPT_ID,
                                   DEPT_NAME = DM.DEPT_NAME
                               })
                               .Distinct()
                               .OrderBy(x => x.DEPT_NAME)
                               .ToList();
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - DepartmentRepository.cs, Method - GetAllDepartment", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }

            return departments;
        }

        public bool AddDepartment(DepartmentMasterMetaData metaData)
        {
            try
            {
                if (metaData.DEPT_ID != new Guid())
                {
                    return EditDepartment(metaData);
                }
                else
                {
                    Guid companyid = SessionHelper.Get<Guid>("CompanyId");
                    //string dept_head = _appEntity.TAB_DEPARTMENT_MASTER.Where(x => x.DEPT_HEAD_ID == metaData.DEPT_HEAD_ID).FirstOrDefault().DEPT_HEAD_NAME;
                    TAB_DEPARTMENT_MASTER dept = new TAB_DEPARTMENT_MASTER
                    {
                        DEPT_ID = Guid.NewGuid(),
                        DEPT_NAME = metaData.DEPT_NAME,
                        BUILDING_ID = metaData.BUILDING_ID,
                        DEPT_HEAD_NAME = metaData.DEPT_HEAD_ID,
                        COMPANY_ID = companyid,
                        Created_by = SessionHelper.Get<String>("Username"),
                        created_date = DateTime.Now,
                        status = "Y",
                        //DEPT_HEAD_NAME = dept_head
                    };
                    _appEntity.TAB_DEPARTMENT_MASTER.Add(dept);
                    _appEntity.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - DepartmentRepository.cs, Method - AddDepartment", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
                return false;
            }
        }

        public bool EditDepartment(DepartmentMasterMetaData metaData)
        {
            try
            {
                Guid companyid = SessionHelper.Get<Guid>("CompanyId");
                //string dept_head = _appEntity.TAB_DEPARTMENT_MASTER.Where(x => x.DEPT_HEAD_NAME == metaData.DEPT_HEAD_ID).FirstOrDefault().DEPT_HEAD_NAME;
                var department = _appEntity.TAB_DEPARTMENT_MASTER.Where(x => x.DEPT_ID == metaData.DEPT_ID).FirstOrDefault();
                department.BUILDING_ID = metaData.BUILDING_ID;
                department.DEPT_NAME = metaData.DEPT_NAME;
                department.DEPT_HEAD_NAME = metaData.DEPT_HEAD_ID;
                department.COMPANY_ID = companyid;
                department.UPDATED_BY = SessionHelper.Get<String>("Username");
                department.UPDATED_DATE = DateTime.Now;
                //department.DEPT_HEAD_NAME = dept_head;
                _appEntity.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - DepartmentRepository.cs, Method - EditDepartment", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
                return false;
            }
        }
        public bool IsDepartmentNameAvailable(string Dept_Name, Guid DEPT_ID, Guid BUILDING_ID)
        {
            Guid companyid = SessionHelper.Get<Guid>("CompanyId");
            if (DEPT_ID != new Guid())
            {
                var detp = _appEntity.TAB_DEPARTMENT_MASTER.Where(x => x.DEPT_NAME == Dept_Name
                && x.DEPT_ID != DEPT_ID
                && x.BUILDING_ID == BUILDING_ID
                && x.COMPANY_ID == companyid).FirstOrDefault();
                if (detp != null)
                {
                    return true;
                }
                return false;
            }
            else
            {
                var detp = _appEntity.TAB_DEPARTMENT_MASTER.Where(x => x.DEPT_NAME == Dept_Name
                && x.COMPANY_ID == companyid
                && x.BUILDING_ID == BUILDING_ID).FirstOrDefault();
                if (detp != null)
                {
                    return true;
                }
            }
            return false;
        }
        public DepartmentMasterMetaData GetDepartmentById(Guid id)
        {
            Guid companyid = SessionHelper.Get<Guid>("CompanyId");
            var result = (from d in _appEntity.TAB_DEPARTMENT_MASTER
                          where d.DEPT_ID == id && d.COMPANY_ID == companyid
                          select new DepartmentMasterMetaData
                          {
                              DEPT_HEAD_ID = d.DEPT_HEAD_NAME,
                              DEPT_NAME = d.DEPT_NAME,
                              DEPT_ID = d.DEPT_ID,
                              BUILDING_ID = (Guid)d.BUILDING_ID
                          }).FirstOrDefault();
            return result;
        }

        public List<DepartmentMasterMetaData> GetDepartmentsByHeadId(string dept_head_id)
        {
            Guid companyid = SessionHelper.Get<Guid>("CompanyId");
            var result = (from d in _appEntity.TAB_DEPARTMENT_MASTER
                          join BM in _appEntity.TAB_BUILDING_MASTER on d.BUILDING_ID equals BM.BUILDING_ID
                          where d.status == "Y" && d.COMPANY_ID == companyid
                          select new DepartmentMasterMetaData
                          {
                              DEPT_HEAD_NAME = d.DEPT_HEAD_NAME,
                              DEPT_NAME = d.DEPT_NAME,
                              DEPT_ID = d.DEPT_ID,
                              Created_by = d.Created_by,
                              DEPT_HEAD_ID = BM.BUILDING_NAME,
                          }).OrderBy(x => x.DEPT_NAME).ToList();
            return result;
        }

        public List<DepartmentMasterMetaData> GetAllDepartmentOnlyForAdmin(Guid companyId)
        {
            List<DepartmentMasterMetaData> departments = null;
            try
            {
                departments = (from DM in _appEntity.TAB_DEPARTMENT_MASTER
                               where DM.status == "Y"
                               && DM.BUILDING_ID == companyId
                               select new DepartmentMasterMetaData
                               {
                                   DEPT_ID = DM.DEPT_ID,
                                   DEPT_NAME = DM.DEPT_NAME
                               }).OrderBy(x => x.DEPT_NAME).ToList();

                return departments;
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - DepartmentRepository.cs, Method - GetAllDepartmentOnlyForAdmin", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }

            return departments;
        }

        public List<DepartmentMasterMetaData> GetDepartmentByCompanyId(Guid companyId)
        {
            List<DepartmentMasterMetaData> departmentMasterMetaDatas = null;
            try
            {
                Guid loggedInUserId = Utility.GetLoggedInUserId();
                string Role = SessionHelper.Get<string>("LoginUserId");

                if (Role == "admin")
                {
                    departmentMasterMetaDatas = (from DM in _appEntity.TAB_DEPARTMENT_MASTER
                                                     //join UDM in _appEntity.TAB_USER_DEPARTMENT_MAPPING on DM.DEPT_ID equals UDM.DEPT_ID
                                                     //join SDM in _appEntity.TAB_SUBDEPARTMENT_MASTER on DM.DEPT_ID equals SDM.DEPT_ID
                                                     //join LM in _appEntity.TAB_LOGIN_MASTER on UDM.USER_ID equals LM.USER_ID
                                                 where DM.status == "Y" && DM.COMPANY_ID == companyId
                                                 select new DepartmentMasterMetaData
                                                 {
                                                     DEPT_ID = DM.DEPT_ID,
                                                     DEPT_NAME = DM.DEPT_NAME,
                                                     COMPANY_ID = DM.COMPANY_ID,
                                                     DEPT_HEAD_ID = DM.DEPT_HEAD_ID,
                                                     DEPT_HEAD_NAME = DM.DEPT_HEAD_NAME
                                                 })

                                   .Distinct().OrderBy(x => x.DEPT_NAME)
                                   .ToList();
                }
                else
                {
                    departmentMasterMetaDatas = (from DM in _appEntity.TAB_DEPARTMENT_MASTER
                                                 join UDM in _appEntity.TAB_USER_DEPARTMENT_MAPPING on DM.DEPT_ID equals UDM.DEPT_ID
                                                 join SDM in _appEntity.TAB_SUBDEPARTMENT_MASTER on DM.DEPT_ID equals SDM.DEPT_ID
                                                 join LM in _appEntity.TAB_LOGIN_MASTER on UDM.USER_ID equals LM.USER_ID
                                                 where LM.USER_ID == loggedInUserId && DM.status == "Y" && DM.COMPANY_ID == companyId
                                                 select new DepartmentMasterMetaData
                                                 {
                                                     DEPT_ID = DM.DEPT_ID,
                                                     DEPT_NAME = DM.DEPT_NAME,
                                                     COMPANY_ID = DM.COMPANY_ID,
                                                     DEPT_HEAD_ID = DM.DEPT_HEAD_ID,
                                                     DEPT_HEAD_NAME = DM.DEPT_HEAD_NAME
                                                 })

                                  .Distinct().OrderBy(x => x.DEPT_NAME)
                                  .ToList();
                }


            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - DepartmentRepository.cs, Method - GetDepartmentByCompanyId", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
            return departmentMasterMetaDatas;
        }

        public IEnumerable<RoleMasterMetaData> GetRoleByCompanyId(Guid cmp_id)
        {
            List<RoleMasterMetaData> roles = null;
            try
            {
                roles = _appEntity.TAB_ROLE_MASTER.Where(x => x.COMPANY_ID == cmp_id).Select(x => new RoleMasterMetaData
                {
                    ROLE_ID = x.ROLE_ID,
                    ROLE_NAME = x.ROLE_NAME
                }
                    )
                    .OrderBy(x => x.ROLE_NAME)
                    .ToList();
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - DepartmentRepository.cs, Method - GetRoleByCompanyId", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }

            return roles;
        }

        public List<DepartmentMasterMetaData> GetDepartmentsByUserId(Guid comp_id, Guid user_id)
        {
            List<DepartmentMasterMetaData> departments = null;
            try
            {
                departments = (from dept in _appEntity.TAB_DEPARTMENT_MASTER
                               join dept_user in _appEntity.TAB_USER_DEPARTMENT_MAPPING on dept.DEPT_ID equals dept_user.DEPT_ID
                               join subdept in _appEntity.TAB_SUBDEPARTMENT_MASTER on dept.DEPT_ID equals subdept.DEPT_ID
                               where dept.COMPANY_ID == comp_id && dept_user.USER_ID == user_id
                               select new DepartmentMasterMetaData
                               {
                                   DEPT_ID = dept.DEPT_ID,
                                   DEPT_NAME = dept.DEPT_NAME
                               })
                        .Distinct()
                        .OrderBy(x => x.DEPT_NAME)
                        .ToList();
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - DepartmentRepository.cs, Method - GetDepartmentsByUserId", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }

            return departments;
        }

        public List<DepartmentMasterMetaData> GetDepartmentHeads()
        {
            List<DepartmentMasterMetaData> metaDatas = new List<DepartmentMasterMetaData>
            {
                new DepartmentMasterMetaData
                {
                    DEPT_HEAD_ID="4",
                    DEPT_HEAD_NAME="RAJESH"
                },
                new DepartmentMasterMetaData
                {
                    DEPT_HEAD_ID="5",
                    DEPT_HEAD_NAME="NNN"
                },
            };
            return metaDatas;
        }

    }
}
