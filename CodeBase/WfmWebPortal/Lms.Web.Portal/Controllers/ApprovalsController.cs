using Lms.Web.Portal.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wfm.App.BL;
using Wfm.App.Common;
using Wfm.App.Core;
using Wfm.App.Core.Model;

namespace Lms.Web.Portal.Controllers
{
    [SessionExpire]
    [WfmAuthorize]
    public class ApprovalsController : Controller
    {
        // GET: Approvals
        private IBaseBL baseBL;
        public ApprovalsController(IBaseBL baseBL)
        {
            this.baseBL = baseBL;
        }

        #region workforcetrainning


        public ActionResult workforcetrainning()
        {
            WorkforceApprovalMetaData metaData = new WorkforceApprovalMetaData();

            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            var deptList = this.baseBL.DepartmentBL.GetDepartmentsByUserId(loggedin_user.COMPANY_ID, loggedin_user.USER_ID);
            var onRollOrContracts = this.baseBL.IDCardGenerationBL.GetOnRollOrContracts(loggedin_user.COMPANY_ID);
            var defaultBind = deptList.Select(x => x.DEPT_ID).FirstOrDefault();
            metaData.DEPARTMENT_ID = defaultBind;
            metaData.Departments = new SelectList(deptList, "DEPT_ID", "DEPT_NAME");
            metaData.OnRollOrContracts = new SelectList(onRollOrContracts, "WF_EMP_TYPE", "EMP_TYPE");
            metaData.Status = "N";
            ViewBag.Buildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            return View(metaData);

        }



        public ActionResult ParticalWorkforceTrainning(Guid deptId, Guid? sub_dept_id, int? emptype_id, string status, Guid BUILDING_ID)
        {
            List<TRAINNING_WORKFORCE_MAPPING> list = new List<TRAINNING_WORKFORCE_MAPPING>();
            list = baseBL.WorkforceTrainningBL.GetWorkForceTrainningForApprovalByDepartmentId(deptId, sub_dept_id, emptype_id, status, BUILDING_ID);
            return View("_WorkforceTrainning", list);
        }
        [HttpPost]
        public ActionResult WorkForceTrainningApprovals(IEnumerable<Guid> twfmIds, FormCollection fm)
        {
            string remark = fm["Remark"];
            if (fm["Approve"] == "Approve")
            {
                baseBL.WorkforceTrainningBL.UpdateWorkForceTrainningByMappingId(twfmIds, "Y", remark);
            }
            else if (fm["Reject"] == "Reject")
            {
                baseBL.WorkforceTrainningBL.UpdateWorkForceTrainningByMappingId(twfmIds, "N", remark);
            }
            return RedirectToAction("workforcetrainning");
        }

        #endregion

        #region MRF
        public ActionResult mrfapprovals()
        {
            WorkforceApprovalMetaData metaData = new WorkforceApprovalMetaData();

            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            var deptList = this.baseBL.DepartmentBL.GetDepartmentsByUserId(loggedin_user.COMPANY_ID, loggedin_user.USER_ID);
            metaData.Departments = new SelectList(deptList, "DEPT_ID", "DEPT_NAME");
            ViewBag.Buildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            return View(metaData);

        }
        public ActionResult ParticalMRFApprovals(string deptId, Guid? sub_dept_id, string status, Guid BUILDING_ID)
        {
            Guid deptid = new Guid();
            if (!string.IsNullOrEmpty(deptId))
            {
                deptid = new Guid(deptId);
            }
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            List<MRFApprovalMetadata> datas = baseBL.ManPowerRequestBL.GetMRFApprovalByDepartmentId(deptid, sub_dept_id, status, BUILDING_ID);
            ViewBag.LoginUerrId = loggedin_user.USER_ID;
            return View("_MRFApprovals", datas);
        }
        [HttpPost]
        public ActionResult MRFApprovals(IEnumerable<Guid> twfmIds, FormCollection fm)
        {
            var mrfSearchULR = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~")) + "/ManPowerRequest/Edit?mrf_INETRNAL_ID=";
            string remark = fm["Remark"];
            if (fm["Approve"] == "Approve")
            {
                baseBL.ManPowerRequestBL.UpdatMRFApprovalsByMappingId(twfmIds, "Y", remark, mrfSearchULR);
            }
            else if (fm["Reject"] == "Reject")
            {
                baseBL.ManPowerRequestBL.UpdatMRFApprovalsByMappingId(twfmIds, "N", remark, mrfSearchULR);
            }
            return RedirectToAction("mrfapprovals");
        }
        #endregion

        #region Exit Approval
        public ActionResult exitapprovals()
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
            return View("_ExitApprovals", datas);
        }
        [HttpPost]
        public ActionResult ExitApprovals(IEnumerable<Guid> twfmIds, FormCollection fm)
        {
            var exitSearchULR = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~")) + "/ExitManagement/EmployeeDetail";
            string remark = fm["Remark"];
            if (fm["Approve"] == "Approve")
            {
                baseBL.AssetBL.UpdatExitApprovalsByMappingId(twfmIds, "Y", remark, exitSearchULR);
            }
            else if (fm["Reject"] == "Reject")
            {
                baseBL.AssetBL.UpdatExitApprovalsByMappingId(twfmIds, "N", remark, exitSearchULR);
            }
            return RedirectToAction("exitapprovals");
        }
        #endregion

        #region Transfer Approval  
        public ActionResult TransferApprovals()
        {
            WorkforceApprovalMetaData metaData = new WorkforceApprovalMetaData();
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            var deptList = this.baseBL.DepartmentBL.GetDepartmentsByUserId(loggedin_user.COMPANY_ID, loggedin_user.USER_ID);
            metaData.Departments = new SelectList(deptList, "DEPT_ID", "DEPT_NAME");
            metaData.Status = "Open";
            ViewBag.Buildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            return View(metaData);

        }

        public string TransferApprovalList(string deptId, string sub_dept_id, string status, string BUILDING_ID)
        {
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            string datas = baseBL.AssetBL.TransferApprovalList(deptId, sub_dept_id, status, BUILDING_ID,"","");
            return datas;
        }
        public string TransferRequestList(string deptId, string sub_dept_id, string status, string BUILDING_ID)
        {
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            string datas = baseBL.AssetBL.TransferApprovalList(deptId, sub_dept_id, status, BUILDING_ID, SessionHelper.Get<String>("Username"), SessionHelper.Get<String>("LoginType"));
            ViewBag.LoginUerrId = loggedin_user.USER_ID;
            return datas;
        }

        [HttpGet]
        public string ApprovedTransfer(string TransferID, string Remark)
        {
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            string ApprovedBy = loggedin_user.USER_NAME;
            return baseBL.AssetBL.ApprovedTransfer(TransferID, ApprovedBy, Remark);
        }

        #endregion
    }

}