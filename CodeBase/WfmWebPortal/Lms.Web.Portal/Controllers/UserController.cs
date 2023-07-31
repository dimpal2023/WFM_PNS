using Lms.Web.Portal.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Wfm.App.BL;
using Wfm.App.Core;
using Wfm.App.Core.Model;

namespace Lms.Web.Portal.Controllers
{
    [SessionExpire]
    [WfmAuthorize]
    public class UserController : Controller
    {      
        private IBaseBL baseBL;
      
        public UserController(IBaseBL baseBL)
        {            
            this.baseBL = baseBL;
        }

        public ActionResult Index()
        {
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result; 
            List<UserMetaData> users = new List<UserMetaData>();
            users = baseBL.UserBL.GetUsersByCompanyId(loggedin_user.COMPANY_ID);
            return View(users);
        }
        [HttpGet]
        public ActionResult Create()
        {
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            UserMetaData userMetaData = new UserMetaData();
            userMetaData.Departments= new SelectList(baseBL.DepartmentBL.GetAllDepartmentOnlyForAdmin(loggedin_user.COMPANY_ID), "DEPT_ID", "DEPT_NAME");
            userMetaData.SubDepartments = new List<SelectListItem>();
            userMetaData.Roles= new SelectList(baseBL.UserBL.GetRolesByCompanyId(loggedin_user.COMPANY_ID), "ROLE_ID", "ROLE_NAME");
            ViewBag.Buildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            return View("Create",userMetaData);
        }
        [HttpPost]
        public ActionResult Create(UserMetaData user)
        {
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            ModelState.Remove("EditMAIL_ID");
            ModelState.Remove("EditMOBILE_NO");
            if (ModelState.IsValid)
            {
                user.COMPANY_ID = loggedin_user.COMPANY_ID;
                user.Created_by = loggedin_user.USER_ID.ToString();
                baseBL.UserBL.AddUser(user);
               return RedirectToAction("Index");
            }
            user.Departments = new SelectList(baseBL.DepartmentBL.GetAllDepartmentOnlyForAdmin((Guid)user.BUILDING_ID), "DEPT_ID", "DEPT_NAME",user.DEPT_IDs);
            user.Roles= new SelectList(baseBL.UserBL.GetRolesByCompanyId(loggedin_user.COMPANY_ID), "ROLE_ID", "ROLE_NAME",user.USER_ROLES);
            return View(user);
        }
        [HttpGet]
        public ActionResult Edit(Guid? id)
        {
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            UserMetaData user = baseBL.UserBL.GetUserById(id.Value);
            if (user != null)
            {
                user.Departments = new SelectList(baseBL.DepartmentBL.GetAllDepartmentOnlyForAdmin((Guid)user.BUILDING_ID), "DEPT_ID", "DEPT_NAME", user.DEPT_IDs);

                //user.Departments = new SelectList(baseBL.ManPowerRequestBL.GetFloorByBuildingId((Guid)user.BUILDING_ID), "DEPT_ID", "DEPT_NAME", user.DEPT_ID);

                //selected subdepartments
                var allsubdepartments = baseBL.SubDepartmentBL.GetAllSubDepartmentOnlyForAdmin(loggedin_user.COMPANY_ID);
                var selectedSubDeptGroup = allsubdepartments.Where(x => user.SUBDEPT_IDs.Contains(x.SUBDEPT_ID.ToString())).GroupBy(x => x.DEPT_NAME).ToList();
                var nonselectedSubDeptGroup = allsubdepartments.Where(x => !user.SUBDEPT_IDs.Contains(x.SUBDEPT_ID.ToString()) && user.DEPT_IDs.Contains(x.DEPT_ID.ToString())).ToList();

                user.SubDepartments = new List<SelectListItem>();
                foreach (IGrouping<string, SubDepartmentMasterMetaData> t in selectedSubDeptGroup)
                {
                    var optionGroup = new SelectListGroup() { Name = t.Key };
                    foreach (var subdept in t)
                    {
                        user.SubDepartments.Add(new SelectListItem() { Value = subdept.SUBDEPT_ID.ToString(), Text = subdept.SUBDEPT_NAME, Group = optionGroup, Selected = true });
                    }
                    var remainingSubDept = nonselectedSubDeptGroup.Where(x => x.DEPT_NAME == optionGroup.Name).ToList();
                    
                    foreach(var subdept in remainingSubDept)
                    {
                        user.SubDepartments.Add(new SelectListItem() { Value = subdept.SUBDEPT_ID.ToString(), Text = subdept.SUBDEPT_NAME, Group = optionGroup, Selected = false });
                    }
                }

                user.Roles = new SelectList(baseBL.UserBL.GetRolesByCompanyId(loggedin_user.COMPANY_ID), "ROLE_ID", "ROLE_NAME", user.USER_ROLES);
                ViewBag.Buildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
                return View(user);
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult Edit(UserMetaData user)
        {
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            ModelState.Remove("USER_LOGIN_ID");
            ModelState.Remove("MAIL_ID");
            ModelState.Remove("MOBILE_NO");
            if (ModelState.IsValid)
            {

                user.COMPANY_ID = loggedin_user.COMPANY_ID;
                user.Created_by = loggedin_user.USER_ID.ToString();
                baseBL.UserBL.EditUser(user);
                return RedirectToAction("Index");
            }
            user.Departments = new SelectList(baseBL.DepartmentBL.GetAllDepartmentOnlyForAdmin((Guid)user.BUILDING_ID), "DEPT_ID", "DEPT_NAME", user.DEPT_IDs);
            var v = baseBL.SubDepartmentBL.GetAllSubDepartmentOnlyForAdmin(loggedin_user.COMPANY_ID).Where(x => user.SUBDEPT_IDs.Contains(x.SUBDEPT_ID.ToString())).GroupBy(x => x.DEPT_NAME).ToList();
            user.SubDepartments = new List<SelectListItem>();
            foreach (IGrouping<string, SubDepartmentMasterMetaData> t in v)
            {
                var optionGroup = new SelectListGroup() { Name = t.Key };
                foreach (var subdept in t)
                {
                    user.SubDepartments.Add(new SelectListItem() { Value = subdept.SUBDEPT_ID.ToString(), Text = subdept.SUBDEPT_NAME, Group = optionGroup, Selected = true });
                }
            }
            user.Roles = new SelectList(baseBL.UserBL.GetRolesByCompanyId(loggedin_user.COMPANY_ID), "ROLE_ID", "ROLE_NAME", user.USER_ROLES);
            return View(user);
        }

        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            baseBL.UserBL.DeleteUser(id);
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("Index", "User");
            return Json(new { Url = redirectUrl });
        }

        [Authorize]
        public ActionResult MarkActive(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            baseBL.UserBL.MarkActive(id);
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("Edit", "User");
            return Json(new { Url = redirectUrl });
        }

        [HttpGet]
        public JsonResult IsUserIdAvailable(string USER_LOGIN_ID)
        {
            return Json(!baseBL.UserBL.IsUserNameAvailable(USER_LOGIN_ID), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult IsUserEmailAvailable(string MAIL_ID)
        {
            return Json(!baseBL.UserBL.IsUserEmailAvailable(MAIL_ID), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult IsUserMobileAvailable(string MOBILE_NO)
        {
            return Json(!baseBL.UserBL.IsUserMobileAvailable(MOBILE_NO), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult IsUserEmailAvailableAtEdit(string EditMAIL_ID, Guid USER_ID)
        {
            return Json(!baseBL.UserBL.IsUserEmailAvailableAtEdit(EditMAIL_ID, USER_ID), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult IsUserMobileAvailableAtEdit(string EditMOBILE_NO, Guid USER_ID)
        {
            return Json(!baseBL.UserBL.IsUserMobileAvailableAtEdit(EditMOBILE_NO, USER_ID), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUserByRoleId(string roleId)
        {
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            return Json(baseBL.UserBL.GetUserByRoleId(loggedin_user.COMPANY_ID, new Guid(roleId)), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSubDepartments(List<Guid> departmentIds)
        {
            if (departmentIds == null) return Json(new UserMetaData(), JsonRequestBehavior.AllowGet);

            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;

            IEnumerable<IGrouping<string, SubDepartmentMasterMetaData>> v = baseBL.SubDepartmentBL.GetAllSubDepartmentOnlyForAdmin(loggedin_user.COMPANY_ID).Where(y => departmentIds.Contains(y.DEPT_ID)).GroupBy(x => x.DEPT_NAME).ToList();
            List<SubDepartmentMasterMetaData> selectedSubDepartments = baseBL.SubDepartmentBL.GetSubDepartmentsByUserId(loggedin_user.COMPANY_ID, loggedin_user.USER_ID);
            UserMetaData userMetaData = new UserMetaData();

            userMetaData.SubDepartments = new List<SelectListItem>();
            foreach (IGrouping<string, SubDepartmentMasterMetaData> t in v)
            {
                var optionGroup = new SelectListGroup() { Name = t.Key };
                foreach (var subdept in t)
                {
                    bool selected = selectedSubDepartments.Where(x => x.SUBDEPT_ID == subdept.SUBDEPT_ID).FirstOrDefault() != null ? true : false;
                    userMetaData.SubDepartments.Add(new SelectListItem() { Value = subdept.SUBDEPT_ID.ToString(), Text = subdept.SUBDEPT_NAME, Group = optionGroup, Selected = selected });
                }
            }

            return Json(userMetaData, JsonRequestBehavior.AllowGet);
        }
    }
}