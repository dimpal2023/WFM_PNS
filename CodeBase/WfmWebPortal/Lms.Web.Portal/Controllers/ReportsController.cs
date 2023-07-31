using Lms.Web.Portal.Authorization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Wfm.App.BL;
using Wfm.App.Core;
using Wfm.App.Core.Model;
using Lms.Web.Portal.DataAccess;
using System.Data;

namespace Lms.Web.Portal.Controllers
{
    [SessionExpire]
    [WfmAuthorize]
    public class ReportsController : Controller
    {
        private IBaseBL baseBl;


        public ReportsController(IBaseBL baseBl)
        {
            this.baseBl = baseBl;
        }

        [HttpGet]
        public ActionResult GetMonthlyAttendanceReport()
        {
            WorkforceAttendance workforceAttendance = new WorkforceAttendance();

            ViewBag.Dept = new SelectList(baseBl.DepartmentBL.GetAllDepartment(), "DEPT_ID", "DEPT_NAME");
            ViewBag.EmployeeTypes = new SelectList(baseBl.ManPowerRequestBL.GetEmpTypes(), "WF_EMP_TYPE", "EMP_TYPE");
            ViewBag.SubDepartments = new SelectList(baseBl.SubDepartmentBL.GetSubDepartmentsByDeptId(Guid.Empty), "SUBDEPT_ID", "SUBDEPT_NAME", Guid.Empty);
            ViewBag.Buildings = new SelectList(baseBl.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            ViewBag.EmpName = new List<SelectListItem>();
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
            ViewBag.EmpSal_TypeList = baseBl.MasterDataBL.GetEmpTypeList().Select(x =>
                                   new SelectListItem()
                                   {
                                       Text = x.EMP_TYPE,
                                       Value = x.EMP_TYPE_ID.ToString()
                                   });
            return View();
        }

        [HttpGet]
        public ActionResult MasterSalarySheet()
        {
            WorkforceAttendance workforceAttendance = new WorkforceAttendance();

            ViewBag.Dept = new SelectList(baseBl.DepartmentBL.GetAllDepartment(), "DEPT_ID", "DEPT_NAME");
            ViewBag.EmployeeTypes = new SelectList(baseBl.ManPowerRequestBL.GetEmpTypes(), "WF_EMP_TYPE", "EMP_TYPE");
            ViewBag.SubDepartments = new SelectList(baseBl.SubDepartmentBL.GetSubDepartmentsByDeptId(Guid.Empty), "SUBDEPT_ID", "SUBDEPT_NAME", Guid.Empty);
            ViewBag.Buildings = new SelectList(baseBl.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            ViewBag.EmpName = new List<SelectListItem>();
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
            ViewBag.EmpSal_TypeList = baseBl.MasterDataBL.GetEmpTypeList().Select(x =>
                                   new SelectListItem()
                                   {
                                       Text = x.EMP_TYPE,
                                       Value = x.EMP_TYPE_ID.ToString()
                                   });
            return View();
        }

        [HttpGet]
        public ActionResult DailyAttendanceReport()
        {
            WorkforceAttendance workforceAttendance = new WorkforceAttendance();

            ViewBag.Dept = new SelectList(baseBl.DepartmentBL.GetAllDepartment(), "DEPT_ID", "DEPT_NAME");
            ViewBag.EmployeeTypes = new SelectList(baseBl.ManPowerRequestBL.GetEmpTypes(), "WF_EMP_TYPE", "EMP_TYPE");
            ViewBag.SubDepartments = new SelectList(baseBl.SubDepartmentBL.GetSubDepartmentsByDeptId(Guid.Empty), "SUBDEPT_ID", "SUBDEPT_NAME", Guid.Empty);
            ViewBag.Buildings = new SelectList(baseBl.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            ViewBag.EmpName = new List<SelectListItem>();
            SelectList slItem = new SelectList("", "");

            ViewBag.Item = slItem;

            ViewBag.Shift = new SelectList(baseBl.ShiftBL.GetAllItems(), "ID", "SHIFT_NAME");

            ViewBag.EmpSal_TypeList = baseBl.MasterDataBL.GetEmpTypeList().Select(x =>
                                     new SelectListItem()
                                     {
                                         Text = x.EMP_TYPE,
                                         Value = x.EMP_TYPE_ID.ToString()
                                     });
            return View();
        }

        [HttpGet]
        public ActionResult YearAttendanceReport()
        {
            WorkforceAttendance workforceAttendance = new WorkforceAttendance();

            ViewBag.Dept = new SelectList(baseBl.DepartmentBL.GetAllDepartment(), "DEPT_ID", "DEPT_NAME");
            ViewBag.EmployeeTypes = new SelectList(baseBl.ManPowerRequestBL.GetEmpTypes(), "WF_EMP_TYPE", "EMP_TYPE");
            ViewBag.SubDepartments = new SelectList(baseBl.SubDepartmentBL.GetSubDepartmentsByDeptId(Guid.Empty), "SUBDEPT_ID", "SUBDEPT_NAME", Guid.Empty);
            ViewBag.Buildings = new SelectList(baseBl.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            ViewBag.EmpName = new List<SelectListItem>();
            SelectList slItem = new SelectList("", "");

            ViewBag.Item = slItem;


            ViewBag.Years = new SelectList(Enumerable.Range(DateTime.Today.Year - 1, 2).Select(x =>
                           new SelectListItem()
                           {
                               Text = x.ToString(),
                               Value = x.ToString()
                           }), "Value", "Text");

            ViewBag.EmpSal_TypeList = baseBl.MasterDataBL.GetEmpTypeList().Select(x =>
                                     new SelectListItem()
                                     {
                                         Text = x.EMP_TYPE,
                                         Value = x.EMP_TYPE_ID.ToString()
                                     });
            return View();
        }

        [HttpGet]
        public ActionResult MonthlyOTReport()
        {
            WorkforceAttendance workforceAttendance = new WorkforceAttendance();

            ViewBag.Dept = new SelectList(baseBl.DepartmentBL.GetAllDepartment(), "DEPT_ID", "DEPT_NAME");
            ViewBag.EmployeeTypes = new SelectList(baseBl.ManPowerRequestBL.GetEmpTypes(), "WF_EMP_TYPE", "EMP_TYPE");
            ViewBag.SubDepartments = new SelectList(baseBl.SubDepartmentBL.GetSubDepartmentsByDeptId(Guid.Empty), "SUBDEPT_ID", "SUBDEPT_NAME", Guid.Empty);
            ViewBag.Buildings = new SelectList(baseBl.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            ViewBag.EmpName = new List<SelectListItem>();
            SelectList slItem = new SelectList("", "");
            ViewBag.Item = slItem;
            ViewBag.EmpSal_TypeList = baseBl.MasterDataBL.GetEmpTypeList().Select(x =>
                                   new SelectListItem()
                                   {
                                       Text = x.EMP_TYPE,
                                       Value = x.EMP_TYPE_ID.ToString()
                                   });
            return View();
        }

        [HttpGet]
        public ActionResult APReport()
        {
            WorkforceAttendance workforceAttendance = new WorkforceAttendance();

            ViewBag.Dept = new SelectList(baseBl.DepartmentBL.GetAllDepartment(), "DEPT_ID", "DEPT_NAME");
            ViewBag.EmployeeTypes = new SelectList(baseBl.ManPowerRequestBL.GetEmpTypes(), "WF_EMP_TYPE", "EMP_TYPE");
            ViewBag.SubDepartments = new SelectList(baseBl.SubDepartmentBL.GetSubDepartmentsByDeptId(Guid.Empty), "SUBDEPT_ID", "SUBDEPT_NAME", Guid.Empty);
            ViewBag.Buildings = new SelectList(baseBl.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            ViewBag.EmpName = new List<SelectListItem>();
            SelectList slItem = new SelectList("", "");
            ViewBag.Item = slItem;
            ViewBag.EmpSal_TypeList = baseBl.MasterDataBL.GetEmpTypeList().Select(x =>
                                   new SelectListItem()
                                   {
                                       Text = x.EMP_TYPE,
                                       Value = x.EMP_TYPE_ID.ToString()
                                   });
            return View();
        }

        [HttpGet]
        public ActionResult PIBSalarySheet()
        {
            WorkforceAttendance workforceAttendance = new WorkforceAttendance();

            ViewBag.Dept = new SelectList(baseBl.DepartmentBL.GetAllDepartment(), "DEPT_ID", "DEPT_NAME");
            ViewBag.EmployeeTypes = new SelectList(baseBl.ManPowerRequestBL.GetEmpTypes(), "WF_EMP_TYPE", "EMP_TYPE");
            ViewBag.SubDepartments = new SelectList(baseBl.SubDepartmentBL.GetSubDepartmentsByDeptId(Guid.Empty), "SUBDEPT_ID", "SUBDEPT_NAME", Guid.Empty);
            ViewBag.Buildings = new SelectList(baseBl.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            ViewBag.EmpName = new List<SelectListItem>();
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
            ViewBag.EmpSal_TypeList = baseBl.MasterDataBL.GetEmpTypeList().Select(x =>
                                   new SelectListItem()
                                   {
                                       Text = x.EMP_TYPE,
                                       Value = x.EMP_TYPE_ID.ToString()
                                   });
            return View();
        }

        [HttpGet]
        public ActionResult LunchDinnerAllowanceReport()
        {
            WorkforceAttendance workforceAttendance = new WorkforceAttendance();

            ViewBag.Dept = new SelectList(baseBl.DepartmentBL.GetAllDepartment(), "DEPT_ID", "DEPT_NAME");
            ViewBag.EmployeeTypes = new SelectList(baseBl.ManPowerRequestBL.GetEmpTypes(), "WF_EMP_TYPE", "EMP_TYPE");
            ViewBag.SubDepartments = new SelectList(baseBl.SubDepartmentBL.GetSubDepartmentsByDeptId(Guid.Empty), "SUBDEPT_ID", "SUBDEPT_NAME", Guid.Empty);
            ViewBag.Buildings = new SelectList(baseBl.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            ViewBag.EmpName = new List<SelectListItem>();
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
            ViewBag.EmpSal_TypeList = baseBl.MasterDataBL.GetEmpTypeList().Select(x =>
                                   new SelectListItem()
                                   {
                                       Text = x.EMP_TYPE,
                                       Value = x.EMP_TYPE_ID.ToString()
                                   });
            return View();
        }

        [HttpGet]
        public ActionResult EmployeeExit_JoinReport()
        {
            WorkforceAttendance workforceAttendance = new WorkforceAttendance();

            ViewBag.Dept = new SelectList(baseBl.DepartmentBL.GetAllDepartment(), "DEPT_ID", "DEPT_NAME");
            ViewBag.EmployeeTypes = new SelectList(baseBl.ManPowerRequestBL.GetEmpTypes(), "WF_EMP_TYPE", "EMP_TYPE");
            ViewBag.SubDepartments = new SelectList(baseBl.SubDepartmentBL.GetSubDepartmentsByDeptId(Guid.Empty), "SUBDEPT_ID", "SUBDEPT_NAME", Guid.Empty);
            ViewBag.Buildings = new SelectList(baseBl.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            ViewBag.EmpName = new List<SelectListItem>();
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
            ViewBag.EmpSal_TypeList = baseBl.MasterDataBL.GetEmpTypeList().Select(x =>
                                   new SelectListItem()
                                   {
                                       Text = x.EMP_TYPE,
                                       Value = x.EMP_TYPE_ID.ToString()
                                   });
            return View();
        }

        [HttpGet]
        public ActionResult MissingPunchReport()
        {
            WorkforceAttendance workforceAttendance = new WorkforceAttendance();

            ViewBag.Dept = new SelectList(baseBl.DepartmentBL.GetAllDepartment(), "DEPT_ID", "DEPT_NAME");
            ViewBag.EmployeeTypes = new SelectList(baseBl.ManPowerRequestBL.GetEmpTypes(), "WF_EMP_TYPE", "EMP_TYPE");
            ViewBag.SubDepartments = new SelectList(baseBl.SubDepartmentBL.GetSubDepartmentsByDeptId(Guid.Empty), "SUBDEPT_ID", "SUBDEPT_NAME", Guid.Empty);
            ViewBag.Buildings = new SelectList(baseBl.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            ViewBag.EmpName = new List<SelectListItem>();
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
            ViewBag.EmpSal_TypeList = baseBl.MasterDataBL.GetEmpTypeList().Select(x =>
                                   new SelectListItem()
                                   {
                                       Text = x.EMP_TYPE,
                                       Value = x.EMP_TYPE_ID.ToString()
                                   });
            return View();
        }

        [HttpGet]
        public ActionResult PIB_BonusMonthlyReport()
        {
            WorkforceAttendance workforceAttendance = new WorkforceAttendance();

            ViewBag.Dept = new SelectList(baseBl.DepartmentBL.GetAllDepartment(), "DEPT_ID", "DEPT_NAME");
            ViewBag.EmployeeTypes = new SelectList(baseBl.ManPowerRequestBL.GetEmpTypes(), "WF_EMP_TYPE", "EMP_TYPE");
            ViewBag.SubDepartments = new SelectList(baseBl.SubDepartmentBL.GetSubDepartmentsByDeptId(Guid.Empty), "SUBDEPT_ID", "SUBDEPT_NAME", Guid.Empty);
            ViewBag.Buildings = new SelectList(baseBl.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            ViewBag.EmpName = new List<SelectListItem>();
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
            ViewBag.EmpSal_TypeList = baseBl.MasterDataBL.GetEmpTypeList().Select(x =>
                                   new SelectListItem()
                                   {
                                       Text = x.EMP_TYPE,
                                       Value = x.EMP_TYPE_ID.ToString()
                                   });
            return View();
        }

        [HttpGet]
        public ActionResult SalarySlip()
        {
            WorkforceAttendance workforceAttendance = new WorkforceAttendance();

            ViewBag.Dept = new SelectList(baseBl.DepartmentBL.GetAllDepartment(), "DEPT_ID", "DEPT_NAME");
            ViewBag.EmployeeTypes = new SelectList(baseBl.ManPowerRequestBL.GetEmpTypes(), "WF_EMP_TYPE", "EMP_TYPE");
            ViewBag.SubDepartments = new SelectList(baseBl.SubDepartmentBL.GetSubDepartmentsByDeptId(Guid.Empty), "SUBDEPT_ID", "SUBDEPT_NAME", Guid.Empty);
            ViewBag.Buildings = new SelectList(baseBl.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            ViewBag.EmpName = new List<SelectListItem>();
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
            ViewBag.EmpSal_TypeList = baseBl.MasterDataBL.GetEmpTypeList().Select(x =>
                                   new SelectListItem()
                                   {
                                       Text = x.EMP_TYPE,
                                       Value = x.EMP_TYPE_ID.ToString()
                                   });
            return View();
        }

        [HttpGet]
        public string GetReport(string BUILDING_ID, string DEPT_ID, string SUBDEPT_ID, int WF_EMP_TYPE, string EMP_NAME, int MONTH_ID, int YEAR_ID, int EMPLOYMENT_TYPE)
        {

            AttendanceDetails objCustomer = new AttendanceDetails();
            DLLReports objDB = new DLLReports(); //calling class DBdata
            string result = objDB.ExecuteProcedureReturnString("Proc_MonthlyAttendanceReport", BUILDING_ID, DEPT_ID, SUBDEPT_ID, WF_EMP_TYPE, EMP_NAME, MONTH_ID, YEAR_ID, EMPLOYMENT_TYPE);

            return result;

        }

        [HttpGet]
        public string GetLunchDinnerReport(string BUILDING_ID, string DEPT_ID, string SUBDEPT_ID, int WF_EMP_TYPE, string EMP_NAME, int MONTH_ID, int YEAR_ID, int EMPLOYMENT_TYPE)
        {

            AttendanceDetails objCustomer = new AttendanceDetails();
            DLLReports objDB = new DLLReports(); //calling class DBdata
            string result = objDB.ExecuteProcedureReturnString("Proc_LunchDinnerReport", BUILDING_ID, DEPT_ID, SUBDEPT_ID, WF_EMP_TYPE, EMP_NAME, MONTH_ID, YEAR_ID, EMPLOYMENT_TYPE);

            return result;

        }

        [HttpGet]
        public string GetExitJoiningReport(string BUILDING_ID, string DEPT_ID, string SUBDEPT_ID, int WF_EMP_TYPE, string EMP_NAME, int MONTH_ID, int YEAR_ID, int EMPLOYMENT_TYPE)
        {

            AttendanceDetails objCustomer = new AttendanceDetails();
            DLLReports objDB = new DLLReports(); //calling class DBdata
            string result = objDB.ExecuteProcedureReturnString("Proc_ExitJoiningReport", BUILDING_ID, DEPT_ID, SUBDEPT_ID, WF_EMP_TYPE, EMP_NAME, MONTH_ID, YEAR_ID, EMPLOYMENT_TYPE);

            return result;

        }

        [HttpGet]
        public string GetMissingPunchReport(string BUILDING_ID, string DEPT_ID, string SUBDEPT_ID, int WF_EMP_TYPE, string EMP_NAME, int MONTH_ID, int YEAR_ID, int EMPLOYMENT_TYPE)
        {

            AttendanceDetails objCustomer = new AttendanceDetails();
            DLLReports objDB = new DLLReports(); //calling class DBdata
            string result = objDB.ExecuteProcedureReturnString("Proc_MissingPunchReport", BUILDING_ID, DEPT_ID, SUBDEPT_ID, WF_EMP_TYPE, EMP_NAME, MONTH_ID, YEAR_ID, EMPLOYMENT_TYPE);

            return result;

        }

        [HttpGet]
        public string GetPIB_BonusMonthlyReport(string BUILDING_ID, string DEPT_ID, string SUBDEPT_ID, int WF_EMP_TYPE, string EMP_NAME, int MONTH_ID, int YEAR_ID, int EMPLOYMENT_TYPE)
        {

            AttendanceDetails objCustomer = new AttendanceDetails();
            DLLReports objDB = new DLLReports(); //calling class DBdata
            string result = objDB.ExecuteProcedureReturnString("Proc_PIB_BonusMonthlyReport", BUILDING_ID, DEPT_ID, SUBDEPT_ID, WF_EMP_TYPE, EMP_NAME, MONTH_ID, YEAR_ID, EMPLOYMENT_TYPE);

            return result;

        }

        [HttpGet]
        public string Get_MasterSalary(string BUILDING_ID, string DEPT_ID, string SUBDEPT_ID, int WF_EMP_TYPE, int MONTH_ID, int YEAR_ID, int EMPLOYMENT_TYPE)
        {

            AttendanceDetails objCustomer = new AttendanceDetails();
            DLLReports objDB = new DLLReports(); //calling class DBdata
            string result = objDB.Get_MasterSalary("Proc_MasterSalarySheet", BUILDING_ID, DEPT_ID, SUBDEPT_ID, WF_EMP_TYPE, MONTH_ID, YEAR_ID, EMPLOYMENT_TYPE);

            return result;

        }

        [HttpGet]
        public string Get_SalarySlip(string BUILDING_ID, string DEPT_ID, string SUBDEPT_ID, int WF_EMP_TYPE, int MONTH_ID, int YEAR_ID, int EMPLOYMENT_TYPE)
        {
            AttendanceDetails objCustomer = new AttendanceDetails();
            DLLReports objDB = new DLLReports(); //calling class DBdata
            string result = objDB.Get_MasterSalary("Proc_MasterSalarySheet", BUILDING_ID, DEPT_ID, SUBDEPT_ID, WF_EMP_TYPE, MONTH_ID, YEAR_ID, EMPLOYMENT_TYPE);
            return result;
        }

        [HttpPost]
        public ActionResult GetMonthlyAttendanceReport([Bind(Include = "WF_ID, DEPT_ID, MONTH_ID, YEAR_ID")] WorkforceAttendance workforceData)
        {
            List<WorkforceAttendance> wfAttData = new List<WorkforceAttendance>();

            wfAttData = baseBl.WorkforceBL.GetAttendanceBywfid(workforceData.WF_ID.Value, workforceData.DEPT_ID.Value, int.Parse(workforceData.MONTH_ID), int.Parse(workforceData.YEAR_ID));

            return PartialView("_WorkforceAttendanceData", wfAttData);
        }

        [HttpGet]
        public string BindEmployeeList(string deptId, string sub_dept_id, string BUILDING_ID, int SHIFT_ID)
        {
            DLLReports objDB = new DLLReports();
            //DataSet ds = new DataSet();
            string ds = objDB.BindEmployeeList("Proc_GetEmployeeDetails_ShiftWise", deptId, sub_dept_id, BUILDING_ID, SHIFT_ID);
            return ds;
        }


        #region Daily Attendance Report
        [HttpGet]
        public string GetDailyAttendanceReport(string BUILDING_ID, string DEPT_ID, string SUBDEPT_ID, string WF_EMP_TYPE, string EMP_NAME, string SHIFT_ID, string FROM_DATE, string EMPLOYMENT_TYPE)
        {
            DateTime Date = Convert.ToDateTime(FROM_DATE);
            AttendanceDetails objCustomer = new AttendanceDetails();
            DLLReports objDB = new DLLReports(); //calling class DBdata
            string result = objDB.GetDailyAttendanceReport("Proc_DailyAttendanceReport", BUILDING_ID, DEPT_ID, SUBDEPT_ID, WF_EMP_TYPE, EMP_NAME, SHIFT_ID, Date, EMPLOYMENT_TYPE);

            return result;

        }

        #endregion  Daily Attendance Report

        #region Year Attendance Report
        [HttpGet]
        public string GetYearAttendanceReport(string BUILDING_ID, string DEPT_ID, string SUBDEPT_ID, string WF_EMP_TYPE, string EMP_NAME, int YEAR_ID, string EMPLOYMENT_TYPE)
        {
            AttendanceDetails objCustomer = new AttendanceDetails();
            DLLReports objDB = new DLLReports(); //calling class DBdata
            string result = objDB.GetYearAttendanceReport("Proc_YearAttendanceReport", BUILDING_ID, DEPT_ID, SUBDEPT_ID, WF_EMP_TYPE, EMP_NAME, YEAR_ID, EMPLOYMENT_TYPE);

            return result;

        }

        #endregion  Year Attendance Report

        #region Monthly OT Report

        [HttpGet]
        public string GetMonthlyOTReport(string BUILDING_ID, string DEPT_ID, string SUBDEPT_ID, int WF_EMP_TYPE, string EMP_NAME, string FROM_DATE, string TO_DATE, int EMPLOYMENT_TYPE)
        {
            DateTime FDate = Convert.ToDateTime(FROM_DATE);
            DateTime TDate = Convert.ToDateTime(TO_DATE);
            AttendanceDetails objCustomer = new AttendanceDetails();
            DLLReports objDB = new DLLReports(); //calling class DBdata
            string result = objDB.GetReport("Proc_MonthlyOTReport", BUILDING_ID, DEPT_ID, SUBDEPT_ID, WF_EMP_TYPE, EMP_NAME, FDate, TDate, EMPLOYMENT_TYPE);

            return result;

        }
        #endregion Monthly OT Report

        #region AP Report

        [HttpGet]
        public string APReports(string BUILDING_ID, string DEPT_ID, string SUBDEPT_ID, int WF_EMP_TYPE, string EMP_NAME, string FROM_DATE, string TO_DATE, int EMPLOYMENT_TYPE)
        {
            DateTime FDate = Convert.ToDateTime(FROM_DATE);
            DateTime TDate = Convert.ToDateTime(TO_DATE);
            AttendanceDetails objCustomer = new AttendanceDetails();
            DLLReports objDB = new DLLReports(); //calling class DBdata
            string result = objDB.GetReport("Proc_APReport", BUILDING_ID, DEPT_ID, SUBDEPT_ID, WF_EMP_TYPE, EMP_NAME, FDate, TDate, EMPLOYMENT_TYPE);

            return result;

        }
        #endregion AP Report

        [HttpGet]
        public string Retirement_Data(string BUILDING_ID, string DEPT_ID, string SUBDEPT_ID)
        {
            AttendanceDetails objCustomer = new AttendanceDetails();
            DLLReports objDB = new DLLReports(); //calling class DBdata
            string result = objDB.Retirement_Data("Proc_RetirementReport", BUILDING_ID, DEPT_ID, SUBDEPT_ID);
            return result;
        }

        public JsonResult LoadWorkforceByWFType_N(Guid BUILDING_ID, string query, Guid deptId, Guid? sub_dept_id, int? emp_type_id, int? EMPLOYMENT_TYPE)
        {
            List<WorkforceMetaDataList> result = new List<WorkforceMetaDataList>();
            query = query.ToUpper();
            try
            {
                result = baseBl.WorkforceBL.BindWorkforceByWFType_New(BUILDING_ID, deptId, sub_dept_id, emp_type_id, EMPLOYMENT_TYPE).Where(x => x.EMP_NAME.ToUpper().Contains(query) || x.EMP_ID.ToUpper().Contains(query)).ToList();
                var list = (from q in result
                            select new { Name = string.Concat(q.EMP_NAME, " - ", q.EMP_ID), ID = q.WF_ID }).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult BindWorkforceByWFType(Guid BUILDING_ID, Guid deptId, int? emp_type_id, Guid? sub_dept_id)
        {
            List<WorkforceMetaDataList> result = new List<WorkforceMetaDataList>();
            try
            {
                result = baseBl.WorkforceBL.BindWorkforceByWFType(deptId, sub_dept_id, emp_type_id, BUILDING_ID).ToList();
                ViewBag.Employeess = new SelectList(result);
                var list = (from q in result
                            select new { Name = q.EMP_NAME, ID = q.WF_ID }).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }



        public JsonResult BindWorkforceByWFType_New(Guid BUILDING_ID, Guid deptId, Guid? sub_dept_id, int? emp_type_id, int? EMPLOYMENT_TYPE)
        {
            List<WorkforceMetaDataList> result = new List<WorkforceMetaDataList>();
            try
            {
                result = baseBl.WorkforceBL.BindWorkforceByWFType_New(BUILDING_ID, deptId, sub_dept_id, emp_type_id, EMPLOYMENT_TYPE).ToList();
                ViewBag.Employeess = new SelectList(result);
                var list = (from q in result
                            select new { Name = q.EMP_NAME, ID = q.WF_ID, EmpId = q.EMP_ID }).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}