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
    public class GatePassController : Controller
    {
        private IBaseBL baseBl;

        public GatePassController(IBaseBL baseBl)
        {
            this.baseBl = baseBl;
        }

        public ActionResult Create()
        {
            GatePassMetaData gatePassMetaData = new GatePassMetaData();
            List<DepartmentMasterMetaData> depts = baseBl.DepartmentBL.GetAllDepartment();

            ViewBag.Dept = new SelectList(depts, "DEPT_ID", "DEPT_NAME");
            ViewBag.EmployeeTypes = new SelectList(baseBl.ManPowerRequestBL.GetEmpTypes(), "WF_EMP_TYPE", "EMP_TYPE");
            ViewBag.SubDepartments = new SelectList(baseBl.SubDepartmentBL.GetSubDepartmentsByDeptId(gatePassMetaData.DEPT_ID), "SUBDEPT_ID", "SUBDEPT_NAME", gatePassMetaData.SUBDEPT_ID);
            ViewBag.EmpName = new List<SelectListItem>();
            ViewBag.Buildings = new SelectList(baseBl.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            return View();
        }

        [HttpPost]
        //[Route("/GatePass/Create")]
        public ActionResult Create([Bind(Exclude = "APPROVED, STATUS, NAME")] GatePassMetaData gatepass)
        {
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("AllItems", "GatePass");
            string res = baseBl.GatePassBL.Create(gatepass);
            if (res == "true")
            {
                ViewData["result"] = "success";
                return Json(new { Url = redirectUrl, result = res });
            }
            else
            {
                return Json(new { result = res });
            }
                
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            Guid companyId = loggedin_user.COMPANY_ID;
            ViewBag.Dept = new SelectList(baseBl.DepartmentBL.GetDepartmentByCompanyId(companyId), "DEPT_ID", "DEPT_NAME");
            ViewBag.EmployeeTypes = new SelectList(baseBl.ManPowerRequestBL.GetEmpTypes(), "WF_EMP_TYPE", "EMP_TYPE");
           
            GatePassMetaData gatepass = baseBl.GatePassBL.FindGatePass(id.Value);
            ViewBag.EmpNames = gatepass.WORKFORCE_IDS;
            ViewBag.SubDepartments = new SelectList(baseBl.SubDepartmentBL.GetSubDepartmentsByDeptId(gatepass.DEPT_ID), "SUBDEPT_ID", "SUBDEPT_NAME", gatepass.SUBDEPT_ID);
            ViewBag.Buildings = new SelectList(baseBl.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            ViewBag.Employees = new SelectList(baseBl.WorkforceBL.BindWorkforceByWFType(gatepass.DEPT_ID, gatepass.SUBDEPT_ID, gatepass.WF_EMP_TYPE, gatepass.BUILDING_ID), "EMP_NAME", "EMP_NAME");
            if (gatepass == null)
            {
                return HttpNotFound();
            }
            return View(gatepass);
        }

        [HttpPost]
        public ActionResult Edit(GatePassMetaData gatepass)
        {
           string res= baseBl.GatePassBL.Update(gatepass);
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("AllItems", "GatePass");
            if (res == "true")
            {
                return Json(new { Url = redirectUrl, result = res });
            }
            else
            {
                return Json(new { result = res });
            }
        }

        [HttpPost]
        public JsonResult FindWorkforce(GatePassMetaData gatepass)
        {
            GatePassMetaData workforce = baseBl.GatePassBL.FindWorkforce(gatepass.WORKFORCE.WF_ID);
            workforce.ID = gatepass.ID;

            return Json(workforce, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            baseBl.GatePassBL.Delete(id.Value);
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("AllItems", "GatePass");
            return Json(new { Url = redirectUrl });
        }

        public ActionResult AllItems()
        {
            GatePassMetaData gatePassMetaData = new GatePassMetaData();
            List<DepartmentMasterMetaData> depts = baseBl.DepartmentBL.GetAllDepartment();
            ViewBag.Dept = new SelectList(depts, "DEPT_ID", "DEPT_NAME");
            ViewBag.EmployeeTypes = new SelectList(baseBl.ManPowerRequestBL.GetEmpTypes(), "WF_EMP_TYPE", "EMP_TYPE");
            ViewBag.SubDepartments = new SelectList(baseBl.SubDepartmentBL.GetSubDepartmentsByDeptId(gatePassMetaData.DEPT_ID), "SUBDEPT_ID", "SUBDEPT_NAME", gatePassMetaData.SUBDEPT_ID);
            ViewBag.EmpName = new List<SelectListItem>();
            ViewBag.Buildings = new SelectList(baseBl.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            return View();

        }
        public ActionResult AllItems1(string dept_id, string sub_dept_id, string fromdate, string todate, string STATUS_ID, string BUILDING_ID)
        {
            DateTime F_Date = Convert.ToDateTime(fromdate);
            DateTime To_Date= Convert.ToDateTime(todate);
            string rolename = string.Empty;

            if (Session["ROLE"] != null)
            {
                rolename = Session["ROLE"].ToString();
            }
            GatePassAllItemsMetaData gatePasses = baseBl.GatePassBL.GetAllItems(rolename, dept_id, sub_dept_id, F_Date, To_Date, STATUS_ID, BUILDING_ID);
            //return View(gatePasses);
            //List<ItemMasterMetaData> Items = baseBL.ItemBL.GetItems(dept_id, sub_dept_id);
            return PartialView("_AllItems", gatePasses);
        }

        [HttpPost]
        public ActionResult Out(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           string result= baseBl.GatePassBL.Out(id.Value);
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("AllItems", "GatePass");
            return Json(new { Url = redirectUrl,result= result });
        }

        [HttpPost]
        public ActionResult In(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            baseBl.GatePassBL.In(id.Value);
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("AllItems", "GatePass");
            return Json(new { Url = redirectUrl });
        }

        [HttpGet]
        public JsonResult GetSubDepartmentByDepartmentId(string departmentId)
        {
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            Guid guidDepartmentId = string.IsNullOrEmpty(departmentId) ? new Guid() : new Guid(departmentId);
            return Json(baseBl.SubDepartmentBL.GetSubDepartmentsByDeptId(loggedin_user.COMPANY_ID, loggedin_user.USER_ID, guidDepartmentId), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
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
    }
}