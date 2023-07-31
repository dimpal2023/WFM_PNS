using Lms.Web.Portal.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Wfm.App.BL;
using Wfm.App.Core;
using Wfm.App.Core.Model;

namespace Lms.Web.Portal.Controllers
{
    [SessionExpire]
    [WfmAuthorize]
    public class ManPowerRequestController : Controller
    {
        private IBaseBL baseBL;

        public ManPowerRequestController(IBaseBL baseBL)
        {
            this.baseBL = baseBL;

        }

        public ActionResult AllItems()
        {
            //AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            //Guid companyId = loggedin_user.COMPANY_ID;
            //List<ManPowerRequestFormMetaDataList> datas = baseBL.ManPowerRequestBL.GetMRFAllItems(companyId);
            //return View(datas);
            GatePassMetaData gatePassMetaData = new GatePassMetaData();
            List<DepartmentMasterMetaData> depts = baseBL.DepartmentBL.GetAllDepartment();
            ViewBag.Dept = new SelectList(depts, "DEPT_ID", "DEPT_NAME");
            ViewBag.SubDepartments = new SelectList(baseBL.SubDepartmentBL.GetSubDepartmentsByDeptId(gatePassMetaData.DEPT_ID), "SUBDEPT_ID", "SUBDEPT_NAME", gatePassMetaData.SUBDEPT_ID);
            ViewBag.Buildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            return View();
        }
        public ActionResult AllItems1(string dept_id, string sub_dept_id, string BUILDING_ID)
        {
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            Guid companyId = loggedin_user.COMPANY_ID;
            List<ManPowerRequestFormMetaDataList> datas = baseBL.ManPowerRequestBL.GetMRFAllItems1(companyId, dept_id, sub_dept_id, BUILDING_ID);
            return PartialView("_AllItems", datas);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ManPowerRequestFormMetaData manPowerRequiremnetMetaData = new ManPowerRequestFormMetaData();
            manPowerRequiremnetMetaData.REC_TYPE = 1;
            return View(BindBuildingSkillDesignation(manPowerRequiremnetMetaData));
        }
        [HttpGet]
        public ActionResult GetMFRHiringType(Guid mrf_Id)
        {
            var result = baseBL.ManPowerRequestBL.GetMRFDetailsByMRF_INETRNAL_ID(mrf_Id);
            return Json(new { result.WF_EMP_TYPE, result.WF_DESIGNATION_ID, result.SKILL_ID, result.DEPT_ID,result.SUBDEPT_ID,result.BUILDING_ID }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(ManPowerRequestFormMetaData model)
        {
            if (!ModelState.IsValid)
            {
                return View(BindBuildingSkillDesignation(model));
            }
            if (baseBL.ManPowerRequestBL.Create(model))
                ViewData["result"] = "success";
                //return RedirectToAction("AllItems");

            return View(BindBuildingSkillDesignation(model));
        }

        [HttpGet]
        public ActionResult Edit(string mrf_INETRNAL_ID)
        {
            ManPowerRequestFormMetaData manPowerRequiremnetMetaData = baseBL.ManPowerRequestBL.GetMRFByMRF_INETRNAL_ID(new Guid(mrf_INETRNAL_ID));
            if (manPowerRequiremnetMetaData.MRF_STATUS != "Open")
            {
                AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
                List<MRFApprovalMetadata> datas = baseBL.ManPowerRequestBL.GetMRFApprovalByMRFId(loggedin_user.COMPANY_ID, new Guid(mrf_INETRNAL_ID));

                return View("_ViewApproval", datas);
            }
            return View(BindBuildingSkillDesignation(manPowerRequiremnetMetaData));
        }

        [HttpPost]
        public ActionResult Edit(ManPowerRequestFormMetaData model)
        {
            if (!ModelState.IsValid)
            {
                return View(BindBuildingSkillDesignation(model));
            }
            if (baseBL.ManPowerRequestBL.Edit(model))
                ViewData["result"] = "success";
                //return RedirectToAction("AllItems");
            @ViewBag.Alert = "You can not exceed the freezing strength.";
            return View(BindBuildingSkillDesignation(model));
        }
        [NonAction]
        public ManPowerRequestFormMetaData BindBuildingSkillDesignation(ManPowerRequestFormMetaData manPowerRequiremnetMetaData)
        {
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            manPowerRequiremnetMetaData.Skills = new SelectList(baseBL.ManPowerRequestBL.GetSkills(), "SKILL_ID", "SKILL_NAME");
            manPowerRequiremnetMetaData.Designations = new SelectList(baseBL.ManPowerRequestBL.GetDesignationBySkill(manPowerRequiremnetMetaData.SKILL_ID), "WF_DESIGNATION_ID", "WF_DESIGNATION_NAME");
            manPowerRequiremnetMetaData.Buildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            manPowerRequiremnetMetaData.MPRHirings = baseBL.ManPowerRequestBL.GetMPRHiring();
            //manPowerRequiremnetMetaData.COMPANY_ID = manPowerRequiremnetMetaData.MPRHirings.FirstOrDefault().COMPANY_ID;
            manPowerRequiremnetMetaData.COMPANY_ID = loggedin_user.COMPANY_ID;
            manPowerRequiremnetMetaData.WORKFLOW_ID = manPowerRequiremnetMetaData.MPRHirings.FirstOrDefault().WORKFLOW_ID.Value;
            manPowerRequiremnetMetaData.EmpTypes = new SelectList(baseBL.ManPowerRequestBL.GetEmpTypes(), "WF_EMP_TYPE", "EMP_TYPE");
            ViewBag.Floor = new SelectList(baseBL.ManPowerRequestBL.GetFloorByBuildingId(manPowerRequiremnetMetaData.BUILDING_ID), "DEPT_ID", "DEPT_NAME", manPowerRequiremnetMetaData.DEPT_ID);
            ViewBag.SubDepartments = new SelectList(baseBL.SubDepartmentBL.GetSubDepartmentsByDeptId(loggedin_user.COMPANY_ID, loggedin_user.USER_ID, manPowerRequiremnetMetaData.DEPT_ID), "SUBDEPT_ID", "SUBDEPT_NAME", manPowerRequiremnetMetaData.SUBDEPT_ID);
            var list = ManPowerRequestController.GetReplaceTypeDropDown("2"); //select second option by default;
            ViewData["ReplaceType"] = list;
            return manPowerRequiremnetMetaData;
        }


        public static IEnumerable<SelectListItem> GetReplaceTypeDropDown(object selectedValue)
        {
            return new List<SelectListItem>
        {
            new SelectListItem{ Text="Exit", Value = "Exit", Selected = "1" == selectedValue.ToString()},
            new SelectListItem{ Text="Transfer", Value = "Transfer", Selected = "2" == selectedValue.ToString()},
        };
        }
        [HttpGet]
        public JsonResult GetFloorByBuildingId(string buildingId)
        {
            Guid guidBuildingId = string.IsNullOrEmpty(buildingId) ? new Guid() : new Guid(buildingId);
            return Json(baseBL.ManPowerRequestBL.GetFloorByBuildingId(guidBuildingId), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllFloorByBuildingId(string buildingId)
        {
            Guid guidBuildingId = string.IsNullOrEmpty(buildingId) ? new Guid() : new Guid(buildingId);
            return Json(baseBL.ManPowerRequestBL.GetAllFloorByBuildingId(guidBuildingId), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSubDepartmentByDepartmentId(string departmentId)
        {
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            Guid guidDepartmentId = string.IsNullOrEmpty(departmentId) ? new Guid() : new Guid(departmentId);
            return Json(baseBL.SubDepartmentBL.GetSubDepartmentsByDeptId(loggedin_user.COMPANY_ID, loggedin_user.USER_ID, guidDepartmentId), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDesignationBySkill(string SkillId)
        {
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            Guid guidDepartmentId = string.IsNullOrEmpty(SkillId) ? new Guid() : new Guid(SkillId);
            return Json(baseBL.ManPowerRequestBL.GetDesignationBySkill(guidDepartmentId), JsonRequestBehavior.AllowGet);
        }
    }
}