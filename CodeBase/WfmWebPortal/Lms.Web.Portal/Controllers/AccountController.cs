using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Wfm.App.BL;
using Wfm.App.Core;
using Wfm.App.Core.Model;
using Wfm.App.Common;
using Lms.Web.Portal.Authorization;

namespace Lms.Web.Portal.Controllers
{
    [SessionExpire]
    [WfmAuthorize]
    public class AccountController : Controller
    {        
        private IBaseBL baseBL;

        public AccountController(IBaseBL baseBL)
        {
            this.baseBL = baseBL;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MyProfile()
        {
            Guid loggedInUserId = Wfm.App.Common.Utility.GetLoggedInUserId();
            UserMetaData user = baseBL.UserBL.GetUserById(loggedInUserId);
            return View(user);
        }

        [HttpPost]
        [AllowAnonymous]        
        public ActionResult Index(LoginMasterMetaData user)
        {
            if (!ModelState.IsValid) return View(user);

            AccountValidateUser_Result logedinUser = baseBL.AccountBL.ValidateUser(user.USER_LOGIN_ID, user.CURRENT_PASSWORD);

            string message = string.Empty;

            try
            {
                //Previous user has not logged out manually.
                if (HttpContext != null && HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    var enTicket = FormsAuthentication.Decrypt(HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName].Value);
                    string lastLoggedInUserName = enTicket.Name;                                                         
                }
            }
            catch (Exception)
            {
            }

            if (logedinUser != null)
			{
                Dictionary<MenuMetaData, List<SubMenuMetaData>> userMenu = baseBL.MenuBL.GetUserMenu(logedinUser.USER_ID);
				Guid ticketId = Guid.NewGuid();               
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, logedinUser.USER_LOGIN_ID, DateTime.Now, DateTime.Now.AddMinutes(2880), false, logedinUser.ROLE_NAME, FormsAuthentication.FormsCookiePath);
				PortalSettings.Current().Cache().Add("FormTicket:" + ticketId.ToString(), logedinUser);
				string encTicket = FormsAuthentication.Encrypt(ticket);
				HttpCookie appCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);

                if (ticket.IsPersistent)
                {
                    appCookie.Expires = ticket.Expiration;
                }
                Session["USER"] = logedinUser;
                Session["ROLE"] = logedinUser.ROLE_NAME;
                Session["UserName"] = logedinUser.USER_NAME;
                Session["MENU"] = userMenu;
                Session["CompanyName"] = baseBL.CompanyBL.GetCompanyName(logedinUser.COMPANY_ID);
                string BUILDING_ID= baseBL.CompanyBL.GetBUILDING(logedinUser.USER_ID);
                Session["BUILDING_ID"] = BUILDING_ID;

                SessionHelper.Set<string>("Username", logedinUser.USER_NAME);
                SessionHelper.Set<Guid>("CompanyId", logedinUser.COMPANY_ID);
                SessionHelper.Set<Guid>("UserId", logedinUser.USER_ID);
                SessionHelper.Set<string>("LoginUserId", logedinUser.USER_LOGIN_ID);
                SessionHelper.Set<string>("LoginType", logedinUser.ROLE_NAME);
                SessionHelper.Set<string>("BUILDING_ID", BUILDING_ID);

                Response.Cookies.Add(appCookie);
                if (!string.IsNullOrEmpty(Request.Form["ReturnUrl"]))
                {
                    return RedirectToAction(Request.Form["ReturnUrl"].Split('/')[1], Request.Form["ReturnUrl"].Split('/')[0]);
                }
                else
                {
                    if (logedinUser.ROLE_NAME == "SECURITY")
                        return RedirectToAction("AllItems", "GatePass");
                    else
                        return RedirectToAction("Index", "Dashboard");
                }
            }

            message = "Username and/or password is incorrect.";
            ViewBag.Message = message;
            return View(user);
        }
		
		[Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();            
            HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ForgotPassword([Bind(Exclude = "MAIL_ID, CURRENT_PASSWORD, REMEMBER_ME")] LoginMasterMetaData user)
        {
            //if (!ModelState.IsValid) return View();

            bool result = baseBL.AccountBL.SendUserPasswordByLoginId(user.USER_LOGIN_ID);
            ViewBag.Message = result ? "Your password have been sent on registered email." : "User name does not exist.";

            return View();
        }        

        [HttpGet]
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public bool ChangePassword(ResetPasswordLoginMasterMetaData resetPassword)
        {
            if(ModelState.IsValid)
            {
                AccountValidateUser_Result loggedInUser = (AccountValidateUser_Result)Session["USER"];
                if(loggedInUser != null)
                {
                    resetPassword.USER_ID = loggedInUser.USER_ID;
                }
               bool result= baseBL.UserBL.ResetUserPassword(resetPassword);
               
                Logout();
                return result;
            }
            return false;
        }
    }
}