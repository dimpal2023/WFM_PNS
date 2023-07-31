using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Wfm.App.Common;
using Wfm.App.Core;
using Wfm.App.Infrastructure.Interfaces;

namespace Wfm.App.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private ApplicationEntities applicationEntities;

        public AccountRepository()
        {
            applicationEntities = new ApplicationEntities();
        }

        public AccountValidateUser_Result ValidateUser(string username, string password)
        {
            return applicationEntities.ValidateUser(username, password).FirstOrDefault();
        }

        public bool SendUserPasswordByLoginId(string userLoginId)
        {
            var obj = applicationEntities.TAB_LOGIN_MASTER.Where(x => x.USER_LOGIN_ID.ToLower() == userLoginId.ToLower()).FirstOrDefault();

            if (obj == null) return false;

            TAB_MAIL_TEMPLATE mail_template = applicationEntities.TAB_MAIL_TEMPLATE.Where(x => x.TEMPLATE_FOR == "ForgotPasswordMsg").FirstOrDefault();
            if (mail_template != null)
            {
                string mail_content = mail_template.TEMPLATE_CONTANT.Replace("[USERNAME]", obj.USER_NAME).Replace("[USER_LOGIN_ID]", obj.USER_LOGIN_ID).Replace("[CURRENT_PASSWORD]", Utility.Base64Decode(obj.CURRENT_PASSWORD));

                try
                {
                    TAB_ALL_MAIL mail = new TAB_ALL_MAIL
                    {
                        TO_MAIL = obj.MAIL_ID,
                        CC_MAIL = mail_template.CC_MAIL,
                        MAIL_CONTENT = mail_content,
                        MAIL_INSERT_DATE = DateTime.Now,
                        MAIL_REMARK = mail_template.TEMPLATE_FOR,
                        USER_ID = obj.USER_LOGIN_ID
                    };
                    applicationEntities.TAB_ALL_MAIL.Add(mail);
                    applicationEntities.SaveChanges();

                }
                catch (Exception ex)
                {
                    AccountRepository.InsertError_Log(ex.ToString(), "Page - AccountRepository.cs, Method - SendUserPasswordByLoginId", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
                }
        }

            return true;
        }


        public static void InsertError_Log(string ErrorMessage, string Method, string Path, string UserId)
        {
            string constr = ConfigurationManager.ConnectionStrings["db_con"].ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand("Proc_InsertError_Log", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ErrorMessage", ErrorMessage);
                cmd.Parameters.AddWithValue("@Method", Method);
                cmd.Parameters.AddWithValue("@Path", Path);
                cmd.Parameters.AddWithValue("@UserId", UserId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
            }
        }
    }
}
