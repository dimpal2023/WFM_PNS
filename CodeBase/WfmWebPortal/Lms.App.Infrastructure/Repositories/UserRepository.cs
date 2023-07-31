using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Wfm.App.Common;
using Wfm.App.Core;
using Wfm.App.Core.Model;
using TAB_LOGIN_MASTER = Wfm.App.Core.TAB_LOGIN_MASTER;

namespace Wfm.App.Infrastructure.Repositories
{
    public class UserRepository
    {
        private ApplicationEntities _appEntity;
        DateTime dt;
        public UserRepository()
        {
            dt = DateTime.Now;
            _appEntity = new ApplicationEntities();
        }
        public void AddUser(UserMetaData user)
        {
            try
            {
                string randomUserPass = Utility.RandomString(8);
                user.USER_ID = Guid.NewGuid();
                List<TAB_USER_ROLE_MAPPING> user_roles = new List<TAB_USER_ROLE_MAPPING>();
                List<TAB_USER_DEPARTMENT_MAPPING> user_depts = new List<TAB_USER_DEPARTMENT_MAPPING>();

                TAB_LOGIN_MASTER login_Master = new TAB_LOGIN_MASTER
                {
                    USER_ID = user.USER_ID,
                    Created_by = SessionHelper.Get<string>("LoginUserId"),
                    COMPANY_ID = user.COMPANY_ID,
                    USER_NAME = user.USER_NAME,
                    MAIL_ID = user.MAIL_ID,
                    MOBILE_NO = user.MOBILE_NO,
                    created_date = dt,
                    //CURRENT_PASSWORD = Utility.Base64Encode(randomUserPass),
                    CURRENT_PASSWORD = Utility.Base64Encode(user.CURRENT_PASSWORD),
                    status = "Y",
                    USER_LOGIN_ID = user.USER_LOGIN_ID,
                    BUILDING_ID=user.BUILDING_ID
                };
                foreach (var role_id in user.USER_ROLES)
                {
                    TAB_USER_ROLE_MAPPING user_role = new TAB_USER_ROLE_MAPPING
                    {
                        Created_by = SessionHelper.Get<string>("LoginUserId"),
                        created_date = dt,
                        ROLE_ID = new Guid(role_id),
                        status = "Y",
                        USER_ID = user.USER_ID,
                        ID = Guid.NewGuid()
                    };
                    user_roles.Add(user_role);
                }
                foreach (var dept_id in user.DEPT_IDs)
                {
                    Guid did = Guid.Parse(dept_id);
                    var subdept = _appEntity.TAB_SUBDEPARTMENT_MASTER.Where(x => user.SUBDEPT_IDs.Contains(x.SUBDEPT_ID.ToString()) && x.DEPT_ID == did).ToList();
                    foreach (Core.TAB_SUBDEPARTMENT_MASTER subdeptObj in subdept)
                    {
                        TAB_USER_DEPARTMENT_MAPPING user_dept = new TAB_USER_DEPARTMENT_MAPPING
                        {
                            Created_by = SessionHelper.Get<string>("LoginUserId"),
                            created_date = dt,
                            DEPT_ID = did,
                            SUBDEPT_ID = subdeptObj.SUBDEPT_ID,
                            USER_ID = user.USER_ID,
                            BUILDING_ID=user.BUILDING_ID
                        };
                        user_depts.Add(user_dept);
                    }
                }
                TAB_MAIL_TEMPLATE mail_template = _appEntity.TAB_MAIL_TEMPLATE.Where(x => x.TEMPLATE_FOR == "NewUserMailMsg").FirstOrDefault();
                if (mail_template != null)
                {
                    string mail_content = mail_template.TEMPLATE_CONTANT.Replace("[USERNAME]", user.USER_NAME).Replace("[USER_LOGIN_ID]", user.USER_LOGIN_ID).Replace("[CURRENT_PASSWORD]", randomUserPass);
                    TAB_ALL_MAIL mail = new TAB_ALL_MAIL
                    {
                        TO_MAIL = user.MAIL_ID,
                        CC_MAIL = mail_template.CC_MAIL,
                        MAIL_CONTENT = mail_content,
                        MAIL_INSERT_DATE = dt,
                        MAIL_REMARK = mail_template.TEMPLATE_FOR,
                        USER_ID = SessionHelper.Get<string>("LoginUserId")
                    };
                    _appEntity.TAB_ALL_MAIL.Add(mail);
                }

                TAB_MAIL_TEMPLATE whatsapp_template = _appEntity.TAB_MAIL_TEMPLATE.Where(x => x.TEMPLATE_FOR == "NewUserSmsMsg").FirstOrDefault();
                if (whatsapp_template != null)
                {
                    string  sms_content = whatsapp_template.TEMPLATE_CONTANT.Replace("[USER_LOGIN_ID]", user.USER_LOGIN_ID).Replace("[CURRENT_PASSWORD]", randomUserPass);
                    TAB_ALL_SMS sms = new TAB_ALL_SMS
                    {
                        MOBILE_NO = user.MOBILE_NO,
                        SMS_TEXT = sms_content,
                        SMS_INSERT_DATE = DateTime.Now,
                        SMS_FLAG = 0,
                        SMS_REMARK = string.Empty,
                        USER_ID = user.USER_ID.ToString()

                    };
                    _appEntity.TAB_ALL_SMS.Add(sms);
                }

                login_Master.TAB_USER_DEPARTMENT_MAPPING = user_depts;
                login_Master.TAB_USER_ROLE_MAPPING = user_roles;
                _appEntity.TAB_LOGIN_MASTER.Add(login_Master);
               // _appEntity.TAB_USER_ROLE_MAPPING.AddRange(user_roles);
                _appEntity.SaveChanges();

            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - UserRepository.cs, Method - AddUser", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
                throw;
            }
        }

        public void UserLAST_LOGIN_DATE(Guid uSER_ID)
        {
            var result = _appEntity.TAB_LOGIN_MASTER.Find(uSER_ID);
            if (result != null)
            {
                result.LAST_LOGIN_DATE = dt;
                result.WRONG_ATTEMP_COUNT = 0;
                _appEntity.SaveChanges();
            }
        }
        public void UserWRONG_ATTEMP_COUNT(string userlogin_id)
        {
            var result = _appEntity.TAB_LOGIN_MASTER.Where(x=>x.USER_LOGIN_ID==userlogin_id).FirstOrDefault();
            if (result != null)
            {
                result.WRONG_ATTEMP_COUNT =Convert.ToInt32(result.WRONG_ATTEMP_COUNT) +1;
                _appEntity.SaveChanges();
            }
        }

        public bool ResetUserPassword(ResetPasswordLoginMasterMetaData resetPassword)
        {
            var result = _appEntity.TAB_LOGIN_MASTER.Find(resetPassword.USER_ID);
            if (result != null)
            {
                if (Utility.Base64Decode(result.CURRENT_PASSWORD) == resetPassword.CURRENT_PASSWORD)
                {
                    result.CURRENT_PASSWORD = Utility.Base64Encode(resetPassword.New_PASSWORD);
                    result.PASSWORD_CHANGE_DATE = DateTime.Now;
                    result.UPDATED_DATE = DateTime.Now;
                    result.UPDATED_BY = SessionHelper.Get<string>("LoginUserId");
                    _appEntity.SaveChanges();
                }
                else
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public void DeleteUser(Guid? id)
        {
            var result= _appEntity.TAB_LOGIN_MASTER.Where(x => x.USER_ID == id).FirstOrDefault();
            if (result != null){
                if (result.status == "Y")
                {
                    result.status = "N";
                }
                //else
                //{
                //    result.status = "Y";
                //}
                _appEntity.SaveChanges();
            }

        }

        public void MarkActive(Guid? id)
        {
            var result = _appEntity.TAB_LOGIN_MASTER.Where(x => x.USER_ID == id).FirstOrDefault();
            if (result != null)
            {
                if (result.status == "N")
                {
                    result.status = "Y";
                }               
                _appEntity.SaveChanges();
            }
        }

        public bool IsUserEmailAvailableAtEdit(string mAIL_ID, Guid userId)
        {
            var result =_appEntity.TAB_LOGIN_MASTER.Where(x => x.MAIL_ID == mAIL_ID).FirstOrDefault();
            if (result!=null)
            {
                if(result.USER_ID == userId)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public bool IsUserMobileAvailableAtEdit(string mOBILE_NO,Guid userId)
        {
            var result = _appEntity.TAB_LOGIN_MASTER.Where(x => x.MOBILE_NO == mOBILE_NO).FirstOrDefault();
            if (result != null)
            {
                if (result.USER_ID == userId)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public List<UserMetaData> GetUsersByCompanyId(Guid company_id)
        {
            List<UserMetaData> users = new List<UserMetaData>();
            try
            {
                users = _appEntity.TAB_LOGIN_MASTER.Where(x => x.COMPANY_ID == company_id).Select(x=> new UserMetaData { 
                    USER_ID=x.USER_ID,
                    USER_LOGIN_ID=x.USER_LOGIN_ID,
                    USER_NAME=x.USER_NAME,
                    MOBILE_NO=x.MOBILE_NO,
                    MAIL_ID=x.MAIL_ID,
                    status=x.status=="Y"?"Yes":"No"
                }).ToList();
                return users;
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - UserRepository.cs, Method - GetUsersByCompanyId", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
                throw;
            }
        }

        public void EditUser(UserMetaData user)
        {
            try
            {
                var result = _appEntity.TAB_LOGIN_MASTER.Find(user.USER_ID);
                if (result != null)
                {
                    result.UPDATED_BY = SessionHelper.Get<string>("LoginUserId");
                    result.COMPANY_ID = user.COMPANY_ID;
                    result.USER_NAME = user.USER_NAME;
                    result.CURRENT_PASSWORD = Utility.Base64Encode(user.CURRENT_PASSWORD);
                    result.MAIL_ID = user.EditMAIL_ID;
                    result.MOBILE_NO = user.EditMOBILE_NO;
                    result.UPDATED_DATE = dt;
                    result.BUILDING_ID = user.BUILDING_ID;
                    var role_remove = _appEntity.TAB_USER_ROLE_MAPPING.Where(x => x.USER_ID == user.USER_ID).ToList();
                    var dept_remove = _appEntity.TAB_USER_DEPARTMENT_MAPPING.Where(x => x.USER_ID == user.USER_ID).ToList();                    
                    //var subdept_remove = _appEntity.TAB_USER_DEPARTMENT_MAPPING.Where(x => x.USER_ID == user.USER_ID && user.DEPT_IDs.Contains(x.DEPT_ID.ToString())).ToList();
                    _appEntity.TAB_USER_DEPARTMENT_MAPPING.RemoveRange(dept_remove);
                    _appEntity.TAB_USER_ROLE_MAPPING.RemoveRange(role_remove);

                    List<TAB_USER_ROLE_MAPPING> user_roles = new List<TAB_USER_ROLE_MAPPING>();
                    List<TAB_USER_DEPARTMENT_MAPPING> user_depts = new List<TAB_USER_DEPARTMENT_MAPPING>();

                    foreach (var role_id in user.USER_ROLES)
                    {
                        TAB_USER_ROLE_MAPPING user_role = new TAB_USER_ROLE_MAPPING
                        {
                            Created_by = SessionHelper.Get<string>("LoginUserId"),
                            created_date = dt,
                            ROLE_ID = new Guid(role_id),
                            status = "Y",
                            USER_ID = user.USER_ID,
                            ID = Guid.NewGuid()
                        };
                        user_roles.Add(user_role);
                    }
                    foreach (var dept_id in user.DEPT_IDs)
                    {
                        Guid did = Guid.Parse(dept_id);
                        var subdept = _appEntity.TAB_SUBDEPARTMENT_MASTER.Where(x => user.SUBDEPT_IDs.Contains(x.SUBDEPT_ID.ToString()) && x.DEPT_ID == did).ToList();
                        foreach (Core.TAB_SUBDEPARTMENT_MASTER subdeptObj in subdept)
                        {
                            TAB_USER_DEPARTMENT_MAPPING user_dept = new TAB_USER_DEPARTMENT_MAPPING
                            {
                                Created_by = SessionHelper.Get<string>("LoginUserId"),
                                created_date = dt,
                                DEPT_ID = did,
                                SUBDEPT_ID = subdeptObj.SUBDEPT_ID,
                                USER_ID = user.USER_ID,
                                BUILDING_ID=user.BUILDING_ID
                            };
                            user_depts.Add(user_dept);
                        }
                    }
                    result.TAB_USER_DEPARTMENT_MAPPING = user_depts;
                    result.TAB_USER_ROLE_MAPPING = user_roles;
                    _appEntity.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - UserRepository.cs, Method - EditUser", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
                throw;
            }
        }

        public UserMetaData GetUserById(Guid userId)
        {
          
            var user = _appEntity.TAB_LOGIN_MASTER.Include("TAB_USER_ROLE_MAPPING").Include("TAB_USER_DEPARTMENT_MAPPING").Where(x => x.USER_ID == userId)
                         .Select(x => new
                         {
                             USER_ID = x.USER_ID,
                             USER_LOGIN_ID = x.USER_LOGIN_ID,
                             MOBILE_NO = x.MOBILE_NO,
                             MAIL_ID = x.MAIL_ID,
                             USER_NAME = x.USER_NAME,
                             CURRENT_PASSWORD = x.CURRENT_PASSWORD,
                             DEPT_IDs = x.TAB_USER_DEPARTMENT_MAPPING.Select(d => d.DEPT_ID.ToString()).ToList(),
                             SUBDEPT_IDS = x.TAB_USER_DEPARTMENT_MAPPING.Select(d => d.SUBDEPT_ID.ToString()).ToList(),
                             USER_ROLES = x.TAB_USER_ROLE_MAPPING.Select(r => r.ROLE_ID.ToString()).ToList(),
                             BUILDING_ID = x.BUILDING_ID
                         }).ToList().Select(x => new UserMetaData {
                             USER_ROLES = x.USER_ROLES.ToArray(),
                             DEPT_IDs=x.DEPT_IDs.ToArray(),
                             SUBDEPT_IDs = x.SUBDEPT_IDS.ToArray(),
                             USER_LOGIN_ID =x.USER_LOGIN_ID,
                             USER_NAME=x.USER_NAME,
                             CURRENT_PASSWORD = Utility.Base64Decode(x.CURRENT_PASSWORD).ToString(),
                             EditMAIL_ID =x.MAIL_ID,
                             EditMOBILE_NO=x.MOBILE_NO,
                             USER_ID=x.USER_ID,
                             BUILDING_ID=x.BUILDING_ID
                         }).FirstOrDefault();
            
            return user;
        }

        public bool IsUserNameAvailable(string uSER_LOGIN_ID)
        {
            return _appEntity.TAB_LOGIN_MASTER.Any(x => x.USER_LOGIN_ID == uSER_LOGIN_ID);
        }

        public List<RoleMasterMetaData> GetRolesByCompanyId(Guid company_id)
        {
          return  _appEntity.TAB_ROLE_MASTER.Where(x => x.COMPANY_ID == company_id).Select(x=> new RoleMasterMetaData { 
          ROLE_ID=x.ROLE_ID,
          ROLE_NAME=x.ROLE_NAME+" "+ (x.IS_EDITABLE=="Y"?"HR":"")
          }).OrderBy(x=>x.ROLE_NAME).ToList();
        }

        public bool IsUserEmailAvailable(string mAIL_ID)
        {
            return _appEntity.TAB_LOGIN_MASTER.Any(x => x.MAIL_ID == mAIL_ID);
        }

        public bool IsUserMobileAvailable(string mOBILE_NO)
        {
            return _appEntity.TAB_LOGIN_MASTER.Any(x => x.MOBILE_NO == mOBILE_NO);
        }


        public List<UserMetaData> GetUserByRoleId(Guid company_id,Guid role_Id)
        {
            var result = (from user in _appEntity.TAB_LOGIN_MASTER
                          join user_map in _appEntity.TAB_USER_ROLE_MAPPING on user.USER_ID equals user_map.USER_ID
                          where user.COMPANY_ID == company_id && user_map.ROLE_ID == role_Id
                          select new UserMetaData
                          {
                              USER_NAME = user.USER_NAME,
                              USER_ID = user.USER_ID
                          }).ToList();
            return result;
        }

    }
}
