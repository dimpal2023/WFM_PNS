using Lms.Web.Portal.Authorization;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Wfm.App.BL;
using Wfm.App.Core;
using Wfm.App.Core.Model;

namespace Lms.Web.Portal.Controllers
{
    [SessionExpire]
    [WfmAuthorize]
    public class WorkFlowMappingController : Controller
    {
        private IBaseBL baseBL;
        public WorkFlowMappingController(IBaseBL baseBL)
        {
            this.baseBL = baseBL;
        }
        
        public ActionResult AllItems()
        {
            List<WorkflowMappingMasterVieweMetaData> datas = baseBL.WorkflowMappingBL.GetWorkflowMappingMaster();
            return View(datas);
        }

        #region Add/Edit Workflow mapping
        [HttpGet]
        [Route("/WorkFlowMapping/Create")]
        public ActionResult Create()
        {
            WorkflowMappingMasterMetaData workflowMappingMasterMetaData = new WorkflowMappingMasterMetaData();
            workflowMappingMasterMetaData.WorkFlowMaster = new SelectList(baseBL.WorkflowMappingBL.GetWorkflowMasterMetaDatas(), "WORKFLOW_ID", "WORKFLOW_NAME");
            workflowMappingMasterMetaData.LevelOfApproval = new SelectList(baseBL.LevelBL.GetAllLevelMaster(), "LEVEL_ID", "LEVEL_NAME");
            return View(workflowMappingMasterMetaData);
        }

        [HttpPost]
        [Route("/WorkFlowMapping/Create")]
        public ActionResult Create(WorkflowMappingMasterMetaData datas)
        {
            if (!ModelState.IsValid)
            {
                datas.WorkFlowMaster = new SelectList(baseBL.WorkflowMappingBL.GetWorkflowMasterMetaDatas(), "WORKFLOW_ID", "WORKFLOW_NAME");
                datas.LevelOfApproval = new SelectList(baseBL.LevelBL.GetAllLevelMaster(), "LEVEL_ID", "LEVEL_NAME");

                return View(datas);
            }
            var userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            baseBL.WorkflowMappingBL.Create(datas.ListMetaDatas, userName, datas.WORKFLOW_ID);
            return RedirectToAction("AllItems");
        }

        [HttpGet]
        [Route("/WorkFlowMapping/Edit")]
        public ActionResult Edit(string workflowId, int levelId)
        {
            WorkflowMappingMasterMetaData datas = new WorkflowMappingMasterMetaData();
            datas.WorkFlowMaster = new SelectList(baseBL.WorkflowMappingBL.GetWorkflowMasterMetaDatas(), "WORKFLOW_ID", "WORKFLOW_NAME");
            datas.LevelOfApproval = new SelectList(baseBL.LevelBL.GetAllLevelMaster(), "LEVEL_ID", "LEVEL_NAME");
            datas.LEVEL_ID = levelId;
            datas.WORKFLOW_ID = new Guid(workflowId);
            return View(datas);
        }

        #endregion

        #region  workflow mapping

        #endregion

        public ActionResult EmployeeDDL(Guid role_Id, int index, string empId)
        {
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            ViewBag.RowIndex = index;            
            ViewBag.EmployeeByRole = new SelectList(baseBL.UserBL.GetUserByRoleId(loggedin_user.COMPANY_ID,role_Id), "USER_ID", "USER_NAME", empId);
            return PartialView("_EmployeeDDL");
        }
        [NonAction]
        public void BindDropDown(int levelId, Guid workFlowId)
        {
            ViewBag.WorkFlowMaster = new SelectList(baseBL.WorkflowMappingBL.GetWorkflowMasterMetaDatas(), "WORKFLOW_ID", "WORKFLOW_NAME", workFlowId.ToString());
            ViewBag.LevelOfApproval = new SelectList(baseBL.LevelBL.GetAllLevelMaster(), "LEVEL_ID", "LEVEL_NAME", levelId);
        }

        [HttpGet]
        public JsonResult GetEmployeeByDepartmentId(string roleId)
        {
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            return Json(baseBL.UserBL.GetUserByRoleId(loggedin_user.COMPANY_ID,new Guid(roleId)), JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult BindLevelOfApproavl(string workFlowId, int levelId)
        {
            WorkflowMappingMasterMetaData workflowMappingMasterMetaData = new WorkflowMappingMasterMetaData();
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;

            //List<WorkflowMappingMasterMetaData> workflowMappingMasterSaveedMetaData = workFlowMappingBL.GetWorkflowMappingMasterByWorkflowIdAndLevelId(new Guid(workFlowId),levelId);
            workflowMappingMasterMetaData.ListMetaDatas = baseBL.WorkflowMappingBL.GetWorkflowMappingMasterByWorkflowIdAndLevelId(new Guid(workFlowId), levelId);
            workflowMappingMasterMetaData.Roles = this.baseBL.DepartmentBL.GetRoleByCompanyId(loggedin_user.COMPANY_ID);
            workflowMappingMasterMetaData.ApprovalOrRejectDays = ApprovalOrRejectDays();
            workflowMappingMasterMetaData.IsAutoApprovalOrRejects = IsAutoApprovalOrReject();

            return PartialView("_EditLevelOfApproval", workflowMappingMasterMetaData);
        }

        [NonAction]
        public List<ListItem> IsAutoApprovalOrReject()
        {
            var yesNo = new List<ListItem>();
            yesNo.Add(new ListItem { Text = "Yes", Value = "Y" });
            yesNo.Add(new ListItem { Text = "No", Value = "N" });
            return yesNo;
        }

        [NonAction]
        public List<ListItem> ApprovalOrRejectDays()
        {
            var days = new List<ListItem>();
            for (int i = 1; i < 20; i++)
            {
                string val = i.ToString();
                days.Add(new ListItem { Text = val, Value = val });
            }
            return days;
        }
    }
}