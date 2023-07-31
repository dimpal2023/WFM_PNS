using Lms.Web.Portal.Authorization;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Wfm.App.BL;
using Wfm.App.Core;
using Wfm.App.Core.Model;

namespace Lms.Web.Portal.Controllers
{
    [SessionExpire]
    [WfmAuthorize]
    public class DashboardController : Controller
    {
        private IBaseBL baseBL;

        public DashboardController(IBaseBL baseBL)
        {
            this.baseBL = baseBL;
        }
        [Authorize]
        public ActionResult Index()
        {
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;

            DashBoardMetaData dashBoard = new DashBoardMetaData();
            var deptList = this.baseBL.DepartmentBL.GetDepartmentByCompanyId(loggedin_user.COMPANY_ID);            
            var defaultBind = deptList.Select(x => x.DEPT_ID).FirstOrDefault();
            dashBoard.DEPARTMENT_ID = defaultBind;
            dashBoard.Departments = new SelectList(deptList, "DEPT_ID", "DEPT_NAME");

            var subDeptList = this.baseBL.SubDepartmentBL.GetSubDepartmentsByDeptId(loggedin_user.COMPANY_ID, loggedin_user.USER_ID, Guid.Parse(defaultBind.ToString()));
            var defaultSubDept = subDeptList.Select(x => x.SUBDEPT_ID).FirstOrDefault();
            dashBoard.SUBDEPARTMENT_ID = defaultSubDept;
            dashBoard.SubDepartments = new SelectList(subDeptList, "SUBDEPT_ID", "SUBDEPT_NAME");

            dashBoard.Buildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            return View(dashBoard);
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetDashboardData(string jsonInput = "")
        {
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            DashBoardJSONMetaData json = new DashBoardJSONMetaData();
            if (!string.IsNullOrEmpty(jsonInput))
            {
                string []splittedDeptSubDept = Regex.Split(jsonInput, ",");
                if (splittedDeptSubDept != null && splittedDeptSubDept.Length == 3)
                {
                    Guid deptId = Guid.Parse(splittedDeptSubDept[0]);
                    Guid subDeptId = Guid.Parse(splittedDeptSubDept[1]);
                    Guid BUILDING_ID = Guid.Parse(splittedDeptSubDept[2]);
                    json = this.baseBL.DashBoardBL.GetDashboardDataJSON(deptId, subDeptId, loggedin_user.COMPANY_ID, BUILDING_ID);
                }
            }
            JsonResult result = new JsonResult();
            result = this.Json(JsonConvert.SerializeObject(json), JsonRequestBehavior.AllowGet);
            return result;
        }

        [HttpGet]
        public JsonResult GetSubDepartmentByDepartmentId(string departmentId)
        {
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            Guid guidDepartmentId = string.IsNullOrEmpty(departmentId) ? new Guid() : new Guid(departmentId);
            return Json(baseBL.SubDepartmentBL.GetSubDepartmentsByDeptId(loggedin_user.COMPANY_ID, loggedin_user.USER_ID, guidDepartmentId), JsonRequestBehavior.AllowGet);
        }

         [HttpGet]
        public JsonResult GetCityByState(string State_ID)
        {
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            Guid guidDepartmentId = string.IsNullOrEmpty(State_ID) ? new Guid() : new Guid(State_ID);
            return Json(baseBL.SubDepartmentBL.GetCityByState(guidDepartmentId), JsonRequestBehavior.AllowGet);
        }

    }
}