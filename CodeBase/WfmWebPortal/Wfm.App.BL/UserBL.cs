using System;
using System.Collections.Generic;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Interfaces;

namespace Wfm.App.BL
{
    public class UserBL
    {
        public IBaseRepository baseRepository;
        public UserBL(IBaseRepository baseRepo)
        {
            baseRepository = baseRepo;
        }

        public void AddUser(UserMetaData user)
        {
            baseRepository.UserRepo.AddUser(user);
        }

        public void EditUser(UserMetaData user)
        {
            baseRepository.UserRepo.EditUser(user);
        }

        public UserMetaData GetUserById(Guid userId)
        {
            return baseRepository.UserRepo.GetUserById(userId);
        }

        public List<UserMetaData> GetUsersByCompanyId(Guid company_id)
        {
            return baseRepository.UserRepo.GetUsersByCompanyId(company_id);

        }

        public List<RoleMasterMetaData> GetRolesByCompanyId(Guid company_id)
        {
            return baseRepository.UserRepo.GetRolesByCompanyId(company_id);
        }

        public bool IsUserNameAvailable(string uSER_LOGIN_ID)
        {
            return baseRepository.UserRepo.IsUserNameAvailable(uSER_LOGIN_ID);
        }

        public bool IsUserEmailAvailable(string mAIL_ID)
        {
            return baseRepository.UserRepo.IsUserEmailAvailable(mAIL_ID);
        }

        public bool IsUserMobileAvailable(string mOBILE_NO)
        {
            return baseRepository.UserRepo.IsUserMobileAvailable(mOBILE_NO);
        }

        public bool IsUserMobileAvailableAtEdit(string mOBILE_NO, Guid USER_ID)
        {
            return baseRepository.UserRepo.IsUserMobileAvailableAtEdit(mOBILE_NO,USER_ID);
        }

        public bool IsUserEmailAvailableAtEdit(string mAIL_ID, Guid USER_ID)
        {
            return baseRepository.UserRepo.IsUserEmailAvailableAtEdit(mAIL_ID,USER_ID);
        }

        public void DeleteUser(Guid? id)
        {
            baseRepository.UserRepo.DeleteUser(id);
        }

        public bool ResetUserPassword(ResetPasswordLoginMasterMetaData resetPassword)
        {
            return baseRepository.UserRepo.ResetUserPassword(resetPassword);

        }

        public void UserLAST_LOGIN_DATE(Guid uSER_ID)
        {
            baseRepository.UserRepo.UserLAST_LOGIN_DATE(uSER_ID);
        }
        public void UserWRONG_ATTEMP_COUNT(string userlogin_id)
        {
            baseRepository.UserRepo.UserWRONG_ATTEMP_COUNT(userlogin_id);
        }

        public List<UserMetaData> GetUserByRoleId(Guid company_id, Guid role_Id)
        {
           return baseRepository.UserRepo.GetUserByRoleId(company_id,role_Id);
        }

        public void MarkActive(Guid? id)
        {
            baseRepository.UserRepo.MarkActive(id);
        }
    }
}
