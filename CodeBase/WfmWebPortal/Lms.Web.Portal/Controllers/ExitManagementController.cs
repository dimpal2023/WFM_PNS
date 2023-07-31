using Lms.Web.Portal.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Wfm.App.BL;
using Wfm.App.Common;
using Wfm.App.Core;
using Wfm.App.Core.Model;

namespace Lms.Web.Portal.Controllers
{
    [SessionExpire]
    [WfmAuthorize]
    public class ExitManagementController : Controller
    {
        private IBaseBL baseBL;

        public ExitManagementController(IBaseBL baseBL)
        {
            this.baseBL = baseBL;
        }

        [HttpGet]
        public ActionResult ExitEmployeeRequest()
        {
            ExitManagementMetaData exitManagement = new ExitManagementMetaData();
            exitManagement.Departments = new SelectList(baseBL.DepartmentBL.GetAllDepartment(), "DEPT_ID", "DEPT_NAME");
            ViewBag.EmployeeTypes = new SelectList(baseBL.ManPowerRequestBL.GetEmpTypes(), "WF_EMP_TYPE", "EMP_TYPE");
            ViewBag.Buildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");

            if (exitManagement.DEPT_ID != null)
            {
                //exitManagement.Employees = new SelectList(baseBL.WorkforceBL.GetEmployeeListByDepartmentId(exitManagement.DEPT_ID).Select(x=> new { EMP_ID = x.EMP_ID,EMP_NAME = x.EMP_NAME + "(" + x.EMP_ID + ")" }).ToList(), "EMP_ID", "EMP_NAME");
            }

            return View(exitManagement);
        }
        [HttpGet]
        public ActionResult ExitRequestList()
        {
            WorkforceApprovalMetaData metaData = new WorkforceApprovalMetaData();

            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            var deptList = this.baseBL.DepartmentBL.GetDepartmentsByUserId(loggedin_user.COMPANY_ID, loggedin_user.USER_ID);
            metaData.Departments = new SelectList(deptList, "DEPT_ID", "DEPT_NAME");
            metaData.Status = "Open";
            ViewBag.Buildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            return View(metaData);
        }

        public ActionResult ParticalExitApprovals(string deptId, Guid? sub_dept_id, string status, Guid BUILDING_ID)
        {
            Guid deptid = new Guid();
            if (!string.IsNullOrEmpty(deptId))
            {
                deptid = new Guid(deptId);
            }
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            List<ExitApprovalMetaData> datas = baseBL.AssetBL.GetExitApprovals(deptid, sub_dept_id, status, BUILDING_ID);
            ViewBag.LoginUerrId = loggedin_user.USER_ID;
            return View("_ExitRequestList", datas);
        }

        [HttpGet]
        public ActionResult EmployeeTransfer()
        {

            ViewBag.Dept = new SelectList(baseBL.DepartmentBL.GetAllDepartment(), "DEPT_ID", "DEPT_NAME");
            ViewBag.EmployeeTypes = new SelectList(baseBL.ManPowerRequestBL.GetEmpTypes(), "WF_EMP_TYPE", "EMP_TYPE");
            ViewBag.SubDepartments = new SelectList(baseBL.SubDepartmentBL.GetSubDepartmentsByDeptId(Guid.Empty), "SUBDEPT_ID", "SUBDEPT_NAME", Guid.Empty);
            ViewBag.Buildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            ViewBag.TransferBuildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings_Transfer(), "BUILDING_ID", "BUILDING_NAME");
            ViewBag.EmpName = new List<SelectListItem>();
            ViewBag.EmpSal_TypeList = baseBL.MasterDataBL.GetEmpTypeList().Select(x =>
                                   new SelectListItem()
                                   {
                                       Text = x.EMP_TYPE,
                                       Value = x.EMP_TYPE_ID.ToString()
                                   });
            return View();
        }


        [HttpPost]
        public ActionResult ExitEmployeeRequest(ExitManagementMetaData exitManagement)
        {
            //if (ModelState.IsValid)
            //{
                AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
                exitManagement.CREATED_BY = loggedin_user.USER_NAME;
                exitManagement.MAIL_ID = loggedin_user.MAIL_ID;
                baseBL.AssetBL.SubmitEmployeeAssetsOnExit(exitManagement);
                TempData["getDeptId"] = exitManagement.DEPT_ID;
                TempData["getWFId"] = exitManagement.WF_ID;
                var redirectUrl = new UrlHelper(Request.RequestContext).Action("EmployeeDetail", "ExitManagement");
                return Json(new { Url = redirectUrl ,id="1"});
            //}
            exitManagement.Departments = new SelectList(baseBL.DepartmentBL.GetAllDepartment(), "DEPT_ID", "DEPT_NAME");
            if (exitManagement.DEPT_ID != null)
            {
                //exitManagement.Employees = new SelectList(baseBL.WorkforceBL.GetEmployeeListByDepartmentId(exitManagement.DEPT_ID).Select(x=> new { EMP_ID = x.EMP_ID,EMP_NAME = x.EMP_NAME + "(" + x.EMP_ID + ")" }).ToList(), "EMP_ID", "EMP_NAME");
            }

            return View("ExitEmployeeRequest", exitManagement);
        }

        [HttpGet]
        public ActionResult GetEmployeeAssetById(string emp_id)
        {
            ExitManagementMetaData exitManagement = new ExitManagementMetaData();
            exitManagement = baseBL.AssetBL.GetEmployeesExitApprovalDetails(new Guid(emp_id));
            exitManagement.WorkforceMetaData = baseBL.WorkforceBL.FindWorkforceByWFId(new Guid(emp_id));
            if (exitManagement.IS_NOTICE_SERVE != null)
            {
                exitManagement.IS_NOTICE_SERVE = exitManagement.IS_NOTICE_SERVE == "Yes" ? "Y" : "N";
            }
            return PartialView("_EmployeeDetails", exitManagement);
        }

        [HttpGet]
        public ActionResult EmployeeDetail()
        {
            ExitManagementMetaData exitManagement = new ExitManagementMetaData();
            string getDeptId = string.Empty;
            Guid getWFId = new Guid();
            if (TempData["getDeptId"] != null && new Guid(TempData["getWFId"].ToString()) != getWFId)
            {
                getDeptId = TempData["getDeptId"].ToString();
                getWFId = new Guid(TempData["getWFId"].ToString());

            }
            exitManagement.Departments = new SelectList(baseBL.DepartmentBL.GetAllDepartment(), "DEPT_ID", "DEPT_NAME", getDeptId);
            ViewBag.EmployeeTypes = new SelectList(baseBL.ManPowerRequestBL.GetEmpTypes(), "WF_EMP_TYPE", "EMP_TYPE");
            ViewBag.Buildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            if (!String.IsNullOrEmpty(getDeptId))
            {
                var employee = baseBL.WorkforceBL.FindByWFId(getWFId);
                exitManagement.DEPT_ID = new Guid(getDeptId);
                exitManagement.EMP_NAME = employee.EMP_NAME + " - " + employee.EMP_ID;
                exitManagement.WF_ID = getWFId;
            }

            return View(exitManagement);
        }

        [HttpGet]
        public ActionResult EmployeesExitApprovalDetails(string emp_id)
        {
            ExitManagementMetaData exitManagement = new ExitManagementMetaData();
            exitManagement = baseBL.AssetBL.GetEmployeesExitApprovalDetails(new Guid(emp_id));
            exitManagement.WorkforceMetaData = baseBL.WorkforceBL.FindWorkforceByWFId(new Guid(emp_id));
            return PartialView("_EmployeesExitApprovalDetails", exitManagement);
        }

        [HttpGet]
        public JsonResult GetEmployeesBydeptIdAutoComplete(string term, string deptId)
        {
            Guid guiddeptId = new Guid(deptId);
            var result = baseBL.WorkforceBL.GetEmployeesBydeptIdAutoComplete(guiddeptId, term);

            return Json(result);
        }

        public JsonResult BindEmployeeData(Guid BUILDING_ID, int? emp_type_id)
        {
            List<WorkforceMetaDataList> result = new List<WorkforceMetaDataList>();
            try
            {
                result = baseBL.WorkforceBL.BindEmployeeData(BUILDING_ID, emp_type_id).ToList();
                ViewBag.Employeess = new SelectList(result);
                var list = (from q in result
                            select new { Name = q.EMP_NAME, ID = q.WF_ID, Dept = q.DEPT, SubDept = q.REFERENCE_NAME, EmploymentType = q.IDENTIFICATION_MARK,EmpID=q.EMP_ID }).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public string Employee_Transfer(Guid BUILDING_ID, Guid DEPT_ID, Guid SUBDEPT_ID ,int EMPLOYMENT_TYPE, Guid? WF_ID)
        {
             
              return  baseBL.AssetBL.Employee_Transfer(BUILDING_ID, DEPT_ID, SUBDEPT_ID,EMPLOYMENT_TYPE, WF_ID);
               
        }
        public ActionResult TransferRequestList()
        {
            WorkforceApprovalMetaData metaData = new WorkforceApprovalMetaData();
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            var deptList = this.baseBL.DepartmentBL.GetDepartmentsByUserId(loggedin_user.COMPANY_ID, loggedin_user.USER_ID);
            metaData.Departments = new SelectList(deptList, "DEPT_ID", "DEPT_NAME");
            metaData.Status = "Open";
            ViewBag.Buildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            return View(metaData);

        }

        public string TransferRequestLists(string deptId, string sub_dept_id, string status, string BUILDING_ID)
        {
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            string datas = baseBL.AssetBL.TransferApprovalList(deptId, sub_dept_id, status, BUILDING_ID, SessionHelper.Get<String>("Username"), SessionHelper.Get<String>("LoginUserId"));
            ViewBag.LoginUerrId = loggedin_user.USER_ID;
            return datas;
        }

        [HttpGet]
        public string GetAllAssetAllocation_Details(string BUILDING_ID,string DEPT_ID, string SUBDEPT_ID, string WF_EMP_TYPE, string EMP_NAME)
        {
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            string datas = baseBL.AssetBL.GetAllAssetAllocation_Details(BUILDING_ID, DEPT_ID, SUBDEPT_ID, WF_EMP_TYPE, EMP_NAME);
            ViewBag.LoginUerrId = loggedin_user.USER_ID;
            return datas;
        }

    }
}