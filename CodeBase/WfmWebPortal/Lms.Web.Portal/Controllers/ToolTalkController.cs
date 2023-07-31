using Lms.Web.Portal.Authorization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Wfm.App.BL;
using Wfm.App.Core;
using Wfm.App.Core.Model;

namespace Lms.Web.Portal.Controllers
{
    [SessionExpire]
    [WfmAuthorize]
    public class ToolTalkController : Controller
    {
        private IBaseBL baseBl;

        public ToolTalkController(IBaseBL baseBl)
        {
            this.baseBl = baseBl;
        }

        #region Create ToolTalk Master
        [HttpGet]
        public ActionResult Create()
        {
            ToolTalkMasterMetaData toolTalkMetaData = new ToolTalkMasterMetaData();

            ViewBag.Dept = new SelectList(baseBl.DepartmentBL.GetAllDepartment(), "DEPT_ID", "DEPT_NAME");
            ViewBag.SubDepartments = new SelectList(baseBl.SubDepartmentBL.GetSubDepartmentsByDeptId(toolTalkMetaData.DEPT_ID), "SUBDEPT_ID", "SUBDEPT_NAME", toolTalkMetaData.SUBDEPT_ID);
            ViewBag.Buildings = new SelectList(baseBl.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(ToolTalkMasterMetaData toolTalk)
        {
            if (ModelState.IsValid)
            {
                baseBl.ToolTalkBL.Create(toolTalk);
            }
            ViewBag.Dept = new SelectList(baseBl.DepartmentBL.GetAllDepartment(), "DEPT_ID", "DEPT_NAME");

            var redirectUrl = new UrlHelper(Request.RequestContext).Action("AllItems", "ToolTalk");
            return Json(new { Url = redirectUrl, id = "1" });
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.Dept = new SelectList(baseBl.DepartmentBL.GetAllDepartment(), "DEPT_ID", "DEPT_NAME");
            ViewBag.Buildings = new SelectList(baseBl.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            ToolTalkMasterMetaData tooltalk = baseBl.ToolTalkBL.Find(id.Value);
            if (tooltalk == null)
            {
                return HttpNotFound();
            }

            ViewBag.SubDepartments = new SelectList(baseBl.SubDepartmentBL.GetSubDepartmentsByDeptId(tooltalk.DEPT_ID), "SUBDEPT_ID", "SUBDEPT_NAME", tooltalk.SUBDEPT_ID);

            return View(tooltalk);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(ToolTalkMasterMetaData toolTalk)
        {
            if (ModelState.IsValid)
            {
                baseBl.ToolTalkBL.Update(toolTalk);
            }
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("AllItems", "ToolTalk");
            return Json(new { Url = redirectUrl, id = "1" });
        }

        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var result = baseBl.ToolTalkBL.Delete(id.Value);

            if (result == 0) return Json(result, JsonRequestBehavior.AllowGet);

            var redirectUrl = new UrlHelper(Request.RequestContext).Action("AllItems", "ToolTalk");
            return Json(new { Url = redirectUrl, id = "1" });
        }

        [Authorize]
        [HttpGet]
        public ActionResult AllItems()
        {
            //List<ToolTalkMasterMetaData> toolTalks = baseBl.ToolTalkBL.GetAllItems();

            //return View(toolTalks);
            ToolTalkMasterMetaData toolTalkMetaData = new ToolTalkMasterMetaData();

            ViewBag.Dept = new SelectList(baseBl.DepartmentBL.GetAllDepartment(), "DEPT_ID", "DEPT_NAME");
            ViewBag.SubDepartments = new SelectList(baseBl.SubDepartmentBL.GetSubDepartmentsByDeptId(toolTalkMetaData.DEPT_ID), "SUBDEPT_ID", "SUBDEPT_NAME", toolTalkMetaData.SUBDEPT_ID);
            ViewBag.Buildings = new SelectList(baseBl.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            return View();
        }
        public ActionResult GetData(string dept_id, string sub_dept_id, string BUILDING_ID)
        {
            List<ToolTalkMasterMetaData> toolTalks = baseBl.ToolTalkBL.GetAllItems(dept_id, sub_dept_id, BUILDING_ID);

            //return View(toolTalks);
            return PartialView("_AllItems", toolTalks);
        }
        #endregion

        #region Configure CheckList
        [Authorize]
        [HttpGet]
        public ActionResult Configure()
        {
            ViewBag.Dept = new SelectList(baseBl.DepartmentBL.GetAllDepartment(), "DEPT_ID", "DEPT_NAME");
            ViewBag.Shift = new SelectList(baseBl.ShiftBL.GetAllItems(), "SHIFT_ID", "SHIFT_NAME");

            ToolTalkConfigurationMetaData toolTalkConfigurationData = new ToolTalkConfigurationMetaData();
            toolTalkConfigurationData.TOOL_TALK_CHECK_LIST = new List<ToolTalkCheckList>();

            ViewBag.SubDepartments = new SelectList(baseBl.SubDepartmentBL.GetSubDepartmentsByDeptId(toolTalkConfigurationData.DEPT_ID), "SUBDEPT_ID", "SUBDEPT_NAME", toolTalkConfigurationData.SUBDEPT_ID);

            return View(toolTalkConfigurationData);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Configure(ToolTalkConfigurationMetaData toolTalkConfiguration)
        {
            int result = 0;

            if (ModelState.IsValid)
            {
                result = baseBl.ToolTalkBL.AddConfiguration(toolTalkConfiguration);
            }

            if (result == 0) return Json(result, JsonRequestBehavior.AllowGet);

            var redirectUrl = new UrlHelper(Request.RequestContext).Action("ConfiguredCheckLists", "ToolTalk");
            return Json(new { Url = redirectUrl });
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditConfiguration(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.Dept = new SelectList(baseBl.DepartmentBL.GetAllDepartment(), "DEPT_ID", "DEPT_NAME");
            ViewBag.Shift = new SelectList(baseBl.ShiftBL.GetAllItems(), "SHIFT_ID", "SHIFT_NAME");

            ToolTalkConfigurationMetaData configuredItem = baseBl.ToolTalkBL.FindConfiguration(id.Value);

            if (configuredItem == null)
            {
                return HttpNotFound();
            }

            ViewBag.SubDepartments = new SelectList(baseBl.SubDepartmentBL.GetSubDepartmentsByDeptId(configuredItem.DEPT_ID), "SUBDEPT_ID", "SUBDEPT_NAME", configuredItem.SUBDEPT_ID);

            return View(configuredItem);
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditConfiguration(ToolTalkConfigurationMetaData configuredItem)
        {
            if (ModelState.IsValid)
            {
                baseBl.ToolTalkBL.UpdateConfiguration(configuredItem);
            }
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("ConfiguredCheckLists", "ToolTalk");
            return Json(new { Url = redirectUrl });
        }

        [Authorize]
        [HttpPost]
        public ActionResult DeleteConfiguration(ToolTalkConfigurationMetaData configuredItem)
        {
            if (configuredItem == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            baseBl.ToolTalkBL.DeleteConfiguration(configuredItem);

            var redirectUrl = new UrlHelper(Request.RequestContext).Action("ConfiguredCheckLists", "ToolTalk");
            return Json(new { Url = redirectUrl });
        }

        [Authorize]
        [HttpGet]
        public ActionResult ConfiguredCheckLists()
        {
            List<ToolTalkConfigurationMetaData> configuredItems = baseBl.ToolTalkBL.ConfiguredCheckLists();

            return View(configuredItems);
        }
        #endregion

        #region Create Daily CheckList



        [Authorize]
        [HttpGet]
        public PartialViewResult GetCheckListBySubDeptId(string deptId, string subDeptId)
        {
            ToolTalkConfigurationMetaData toolTalkConfigurationData = new ToolTalkConfigurationMetaData();
            toolTalkConfigurationData.TOOL_TALK_CHECK_LIST = new List<ToolTalkCheckList>();

            if(!string.IsNullOrEmpty(deptId) && !string.IsNullOrEmpty(subDeptId))
            {
                Guid deptIdGuid = Guid.Parse(deptId);
                Guid SubDeptIdGuid = Guid.Parse(subDeptId);

                List<ToolTalkMasterMetaData> masterData = baseBl.ToolTalkBL.GetCheckListByDeptId(deptIdGuid, SubDeptIdGuid);

                foreach (var item in masterData)
                {
                    ToolTalkCheckList checkListItem = new ToolTalkCheckList();
                    checkListItem.ID = item.ID;
                    checkListItem.TOOL_TALK_ID = item.ID;
                    checkListItem.ITEM_NAME = item.ITEM_NAME;
                    checkListItem.CHECK = false;

                    toolTalkConfigurationData.TOOL_TALK_CHECK_LIST.Add(checkListItem);
                }
            }            

            return PartialView("_checklist", toolTalkConfigurationData);
        }

        [Authorize]
        [HttpGet]
        public PartialViewResult GetConfiguredCheckListBySubDeptId(string deptId, string subDeptId,string BUILDING_ID)
        {
            ToolTalkConfigurationMetaData toolTalkConfigurationData = new ToolTalkConfigurationMetaData();
            toolTalkConfigurationData.TOOL_TALK_CHECK_LIST = new List<ToolTalkCheckList>();

            if (!string.IsNullOrEmpty(deptId) && !string.IsNullOrEmpty(subDeptId))
            {                
                Guid deptIdGuid = Guid.Parse(deptId);
                Guid subDeptIdGuid = Guid.Parse(subDeptId);
                Guid BUILDING_IDGuid = Guid.Parse(BUILDING_ID);

                List<ToolTalkMasterMetaData> masterData = baseBl.ToolTalkBL.GetConfiguredCheckListBySubDeptId(deptIdGuid, subDeptIdGuid, BUILDING_IDGuid);

                if (masterData != null)
                {
                    foreach (var item in masterData)
                    {
                        ToolTalkCheckList checkListItem = new ToolTalkCheckList();
                        checkListItem.ID = item.ID;
                        checkListItem.TOOL_TALK_ID = item.ID;
                        checkListItem.ITEM_NAME = item.ITEM_NAME;
                        checkListItem.CHECK = false;

                        toolTalkConfigurationData.TOOL_TALK_CHECK_LIST.Add(checkListItem);
                    }
                }
            }

            return PartialView("_checklist", toolTalkConfigurationData);
        }

        [Authorize]
        [HttpGet]
        public ActionResult CreateDailyCheckList()
        {
            ViewBag.Dept = new SelectList(baseBl.DepartmentBL.GetAllDepartment(), "DEPT_ID", "DEPT_NAME");
            ViewBag.EmployeeTypes = new SelectList(baseBl.ManPowerRequestBL.GetEmpTypes(), "WF_EMP_TYPE", "EMP_TYPE");
            ViewBag.Buildings = new SelectList(baseBl.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            ViewBag.Shift = new SelectList(baseBl.ShiftBL.GetAllItems(), "ID", "SHIFT_NAME");
            ToolTalkDailyCheckListMetaData toolTalkDailyCheckList = new ToolTalkDailyCheckListMetaData();
            toolTalkDailyCheckList.TOOL_TALK_CHECK_LIST = new List<ToolTalkCheckList>();
            toolTalkDailyCheckList.DATE = DateTime.Now;

            ViewBag.SubDepartments = new SelectList(baseBl.SubDepartmentBL.GetSubDepartmentsByDeptId(toolTalkDailyCheckList.DEPT_ID), "SUBDEPT_ID", "SUBDEPT_NAME", toolTalkDailyCheckList.SUBDEPT_ID);

            return View(toolTalkDailyCheckList);
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateDailyCheckList(ToolTalkDailyCheckListMetaData toolTalkCheckList)
        {
            int result = 0;

            if (ModelState.IsValid)
            {
                result = baseBl.ToolTalkBL.CreateDailyCheckList(toolTalkCheckList);
            }
            if (result == 0 || result == 1) 
            return Json(result, JsonRequestBehavior.AllowGet);

            var redirectUrl = new UrlHelper(Request.RequestContext).Action("DailyCheckLists", "ToolTalk");
            return Json(new { Url = redirectUrl, id = "11" });
        }

        [Authorize]
        [HttpGet]
        public PartialViewResult GetCheckListData(Guid DEPT_ID, Guid SUB_DEPT_ID, Guid BUILDING_ID)
        {
            //Guid Dept = new Guid(DEPT_ID);
            //Guid SDept = new Guid(SUB_DEPT_ID);
            List<ToolTalkDailyCheckListMetaData> toolTalks = baseBl.ToolTalkBL.GetAllDailyCheckLists(DEPT_ID, SUB_DEPT_ID, BUILDING_ID);
            return PartialView("_DailyCheckLists", toolTalks);
        }

        [Authorize]
        [HttpGet]
        public ActionResult DailyCheckLists()
        {
            ToolTalkMasterMetaData toolTalkMetaData = new ToolTalkMasterMetaData();

            ViewBag.Dept = new SelectList(baseBl.DepartmentBL.GetAllDepartment(), "DEPT_ID", "DEPT_NAME");
            ViewBag.SubDepartments = new SelectList(baseBl.SubDepartmentBL.GetSubDepartmentsByDeptId(toolTalkMetaData.DEPT_ID), "SUBDEPT_ID", "SUBDEPT_NAME", toolTalkMetaData.SUBDEPT_ID);
            ViewBag.Buildings = new SelectList(baseBl.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult DeleteDailyCheckList(ToolTalkDailyCheckListMetaData dailyCheckListItem)
        {
            if (dailyCheckListItem == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            baseBl.ToolTalkBL.DeleteDailyCheckList(dailyCheckListItem);

            var redirectUrl = new UrlHelper(Request.RequestContext).Action("DailyCheckLists", "ToolTalk");
            return Json(new { Url = redirectUrl, id = "11" });
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditDailyCheckList(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.Dept = new SelectList(baseBl.DepartmentBL.GetAllDepartment(), "DEPT_ID", "DEPT_NAME");
            ViewBag.EmployeeTypes = new SelectList(baseBl.ManPowerRequestBL.GetEmpTypes(), "WF_EMP_TYPE", "EMP_TYPE");
            ViewBag.Buildings = new SelectList(baseBl.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            ViewBag.Shift = new SelectList(baseBl.ShiftBL.GetAllItems(), "ID", "SHIFT_NAME");
            ToolTalkDailyCheckListMetaData dailyCheckListItem = baseBl.ToolTalkBL.FindDailyCheckList(id.Value);

            if (dailyCheckListItem == null)
            {
                return HttpNotFound();
            }

            ViewBag.SubDepartments = new SelectList(baseBl.SubDepartmentBL.GetSubDepartmentsByDeptId(dailyCheckListItem.DEPT_ID), "SUBDEPT_ID", "SUBDEPT_NAME", dailyCheckListItem.SUBDEPT_ID);

            return View(dailyCheckListItem);
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditDailyCheckList(ToolTalkDailyCheckListMetaData dailyCheckListItem)
        {
            if (ModelState.IsValid)
            {
                baseBl.ToolTalkBL.UpdateDailyCheckList(dailyCheckListItem);
            }
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("DailyCheckLists", "ToolTalk");
            return Json(new { Url = redirectUrl, id = "11" });
        }
        #endregion

        public JsonResult GetSubDepartmentByDepartmentId(string departmentId)
        {
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            Guid guidDepartmentId = string.IsNullOrEmpty(departmentId) ? new Guid() : new Guid(departmentId);
            return Json(baseBl.SubDepartmentBL.GetSubDepartmentsByDeptId(loggedin_user.COMPANY_ID, loggedin_user.USER_ID, guidDepartmentId), JsonRequestBehavior.AllowGet);
        }
        //public JsonResult BindEmployeeList(string deptId, string subDeptId, string BUILDING_ID,int SHIFT_ID)
        //{
        //    AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
        //    Guid guidDepartmentId = string.IsNullOrEmpty(deptId) ? new Guid() : new Guid(deptId);
        //    Guid guidsubDeptId = string.IsNullOrEmpty(subDeptId) ? new Guid() : new Guid(subDeptId);
        //    Guid guidBUILDING_ID = string.IsNullOrEmpty(BUILDING_ID) ? new Guid() : new Guid(BUILDING_ID);
        //    return Json(baseBl.ToolTalkBL.GetEmployeeList(guidDepartmentId, guidsubDeptId, guidBUILDING_ID, SHIFT_ID), JsonRequestBehavior.AllowGet);
        //}
    }
}