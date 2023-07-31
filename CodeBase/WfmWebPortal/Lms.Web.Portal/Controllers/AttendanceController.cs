using Lms.Web.Portal.Authorization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Wfm.App.BL;
using Wfm.App.Core;
using Wfm.App.Core.Model;

namespace Lms.Web.Portal.Controllers
{
    [SessionExpire]
    [WfmAuthorize]
    public class AttendanceController : Controller
    {
        private IBaseBL baseBl;


        public AttendanceController(IBaseBL baseBl)
        {
            this.baseBl = baseBl;
        }
        // GET: Attendance
        public ActionResult Employee()
        {
            return View();
        }
        
        public ActionResult Today()
        {
            return View();
        }

        [HttpGet]        
        public ActionResult AttendanceList()
        {
            WorkforceAttendance workforceAttendance = new WorkforceAttendance();

            ViewBag.Dept = new SelectList(baseBl.DepartmentBL.GetAllDepartment(), "DEPT_ID", "DEPT_NAME");
            ViewBag.EmployeeTypes = new SelectList(baseBl.ManPowerRequestBL.GetEmpTypes(), "WF_EMP_TYPE", "EMP_TYPE");
            ViewBag.SubDepartments = new SelectList(baseBl.SubDepartmentBL.GetSubDepartmentsByDeptId(Guid.Empty), "SUBDEPT_ID", "SUBDEPT_NAME", Guid.Empty);

            SelectList slItem = new SelectList("", "");

            ViewBag.Item = slItem;

            ViewBag.Months = System.Globalization.DateTimeFormatInfo
               .InvariantInfo
               .MonthNames
               .Select((monthName, index) => new SelectListItem
               {
                   Value = (index + 1).ToString(),
                   Text = monthName
               });

            ViewBag.Years = new SelectList(Enumerable.Range(DateTime.Today.Year - 1, 2).Select(x =>
                           new SelectListItem()
                           {
                               Text = x.ToString(),
                               Value = x.ToString()
                           }), "Value", "Text");

            return View();
        }

        [HttpPost]        
        public ActionResult AttendanceList([Bind(Include = "WF_ID, DEPT_ID, MONTH_ID, YEAR_ID")] WorkforceAttendance workforceData)
        {
            List<WorkforceAttendance> wfAttData = new List<WorkforceAttendance>();

            wfAttData = baseBl.WorkforceBL.GetAttendanceBywfid(workforceData.WF_ID.Value, workforceData.DEPT_ID.Value, int.Parse(workforceData.MONTH_ID), int.Parse(workforceData.YEAR_ID));

            return PartialView("_WorkforceAttendanceData", wfAttData);
        }

        [HttpGet]        
        public ActionResult FaultyAttendanceList()
        {
            ViewBag.Dept = new SelectList(baseBl.DepartmentBL.GetAllDepartment(), "DEPT_ID", "DEPT_NAME");

            SelectList slItem = new SelectList("", "");

            ViewBag.Item = slItem;

            ViewBag.Months = DateTimeFormatInfo
               .InvariantInfo
               .MonthNames
               .Select((monthName, index) => new SelectListItem
               {
                   Value = (index + 1).ToString(),
                   Text = monthName
               });

            ViewBag.Years = new SelectList(Enumerable.Range(DateTime.Today.Year - 1, 2).Select(x =>
                           new SelectListItem()
                           {
                               Text = x.ToString(),
                               Value = x.ToString()
                           }), "Value", "Text");

            return View();
        }

        [HttpPost]        
        public ActionResult FaultyAttendanceList([Bind(Include = "DEPT_ID, MONTH_ID, YEAR_ID")] WorkforceSalaryData workforceData)
        {
            List<WorkforceFaultyData> wffaultyAttData = new List<WorkforceFaultyData>();

            wffaultyAttData = baseBl.WorkforceBL.GetFaultyData(workforceData.DEPT_ID.Value, int.Parse(workforceData.MONTH_ID), int.Parse(workforceData.YEAR_ID));

            return PartialView("_WorkforceFaultyAttendanceData", wffaultyAttData);
        }

        public JsonResult GetSubDepartmentByDepartmentId(string departmentId)
        {
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            Guid guidDepartmentId = string.IsNullOrEmpty(departmentId) ? new Guid() : new Guid(departmentId);
            return Json(baseBl.SubDepartmentBL.GetSubDepartmentsByDeptId(loggedin_user.COMPANY_ID, loggedin_user.USER_ID, guidDepartmentId), JsonRequestBehavior.AllowGet);
        } 
        public JsonResult GetAllSubDepartmentByDepartmentId(string departmentId)
        {
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            Guid guidDepartmentId = string.IsNullOrEmpty(departmentId) ? new Guid() : new Guid(departmentId);
            return Json(baseBl.SubDepartmentBL.GetAllSubDepartmentByDepartmentId(loggedin_user.COMPANY_ID, loggedin_user.USER_ID, guidDepartmentId), JsonRequestBehavior.AllowGet);
        }
    }
}