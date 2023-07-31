using Lms.Web.Portal.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Wfm.App.BL;
using Wfm.App.Core;
using Wfm.App.Core.Model;

namespace Lms.Web.Portal.Controllers
{
    [SessionExpire]
    [WfmAuthorize]
    public class WorkforceTrainningController : Controller
    {
        private IBaseBL baseBL;

        public WorkforceTrainningController(IBaseBL baseBL)
        {
            this.baseBL = baseBL;
        }

        public ActionResult TrainningMaster()
        {
            TrainningByDepartments metaData = new TrainningByDepartments();

            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            var deptList = this.baseBL.DepartmentBL.GetDepartmentByCompanyId(loggedin_user.COMPANY_ID);
            var defaultBind = deptList.Select(x => x.DEPT_ID).FirstOrDefault();
            metaData.DEPARTMENT_ID = defaultBind;
            metaData.Departments = new SelectList(deptList, "DEPT_ID", "DEPT_NAME");
            metaData.Buildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            return View(metaData);
        }

        public ActionResult GetTrainningMasterByDeptId(Guid deptId, Guid? sub_dept_id)
        {
            List<TrainningMasterMetaData> trainnings = this.baseBL.WorkforceTrainningBL.GetTrainningMaster(deptId, sub_dept_id);
            return PartialView("_TrainningList", trainnings);
        }
        public ActionResult GetTrainningMasterByDeptId_N()
        {
            List<TrainningMasterMetaData> trainnings = this.baseBL.WorkforceTrainningBL.GetTrainningMaster_N();
            return PartialView("_TrainningList", trainnings);
        }
        public ActionResult AddTrainning()
        {
            AddTrainningMetaData addTrainning = new AddTrainningMetaData();
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            var deptList = this.baseBL.DepartmentBL.GetDepartmentByCompanyId(loggedin_user.COMPANY_ID);
            addTrainning.Departments = new SelectList(deptList, "DEPT_ID", "DEPT_NAME");
            addTrainning.Buildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            return View("AddTrainningName", addTrainning);
        }

        [HttpPost]
        public ActionResult AddTrainningName(AddTrainningMetaData addTrainning)
        {
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            Guid companyId = loggedin_user.COMPANY_ID;
            addTrainning.CMP_ID = companyId;
            addTrainning.CREATED_BY = loggedin_user.USER_LOGIN_ID;
            if (baseBL.WorkforceTrainningBL.AddTrainningMaster(addTrainning))
            {
                ViewData["result"] = "success";
                addTrainning.Departments = new SelectList(baseBL.DepartmentBL.GetDepartmentByCompanyId(loggedin_user.COMPANY_ID), "DEPT_ID", "DEPT_NAME");
                addTrainning.Buildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
                return View("AddTrainningName", addTrainning);
            }
            addTrainning.Departments = new SelectList(baseBL.DepartmentBL.GetDepartmentByCompanyId(loggedin_user.COMPANY_ID), "DEPT_ID", "DEPT_NAME");
            addTrainning.Buildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            return View("AddTrainningName", addTrainning);
        }

        [HttpGet]
        public ActionResult EditTrainning(Guid id)
        {
            AddTrainningMetaData addTrainning = new AddTrainningMetaData();
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            addTrainning = baseBL.WorkforceTrainningBL.GetTrainningMasterByTrainningId(loggedin_user.COMPANY_ID, id);
            addTrainning.Departments = new SelectList(baseBL.DepartmentBL.GetDepartmentByCompanyId(loggedin_user.COMPANY_ID), "DEPT_ID", "DEPT_NAME");
            ViewBag.SubDepartments = new SelectList(baseBL.SubDepartmentBL.GetSubDepartmentsByDeptId(addTrainning.DEPT_ID), "SUBDEPT_ID", "SUBDEPT_NAME", addTrainning.SUBDEPT_ID);
            addTrainning.Buildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            return View(addTrainning);
        }

        [HttpPost]
        public ActionResult EditTrainning(AddTrainningMetaData trainning)
        {
            AddTrainningMetaData addTrainning = new AddTrainningMetaData();
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            Guid companyId = loggedin_user.COMPANY_ID;
            trainning.UPDATED_BY = loggedin_user.USER_LOGIN_ID;
            if (baseBL.WorkforceTrainningBL.UpdateTrainningMaster(trainning))
            {
                ViewData["result"] = "success";
                addTrainning.Departments = new SelectList(baseBL.DepartmentBL.GetDepartmentByCompanyId(loggedin_user.COMPANY_ID), "DEPT_ID", "DEPT_NAME");
                ViewBag.SubDepartments = new SelectList(baseBL.SubDepartmentBL.GetSubDepartmentsByDeptId(addTrainning.DEPT_ID), "SUBDEPT_ID", "SUBDEPT_NAME", addTrainning.SUBDEPT_ID);
                addTrainning.Buildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
                return View("EditTrainning", addTrainning);
            }
            trainning.Departments = new SelectList(this.baseBL.DepartmentBL.GetDepartmentByCompanyId(loggedin_user.COMPANY_ID), "DEPT_ID", "DEPT_NAME");
            addTrainning.Buildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            return View(addTrainning);

        }

        [HttpGet]
        public ActionResult AddTrainningWorkforce()
        {
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            TrainningWorkforceMetaData trainningWorkforce = new TrainningWorkforceMetaData();
            ViewBag.RowCount = 0;
            trainningWorkforce.Departments = new SelectList(this.baseBL.DepartmentBL.GetDepartmentByCompanyId(loggedin_user.COMPANY_ID), "DEPT_ID", "DEPT_NAME");
            ViewBag.EmployeeTypes = new SelectList(baseBL.ManPowerRequestBL.GetEmpTypes(), "WF_EMP_TYPE", "EMP_TYPE");
            ViewBag.EmpName = new List<SelectListItem>();
            ViewBag.Buildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            List<AddWorkforceMappingTrainning> trainnings = new List<AddWorkforceMappingTrainning>
            {
                new AddWorkforceMappingTrainning
                {
                    TrainningMasterByDepart= new SelectList(baseBL.WorkforceTrainningBL.GetTrainningMasterByDepartId(new Guid(), loggedin_user.COMPANY_ID), "TRAINNING_ID", "TRAINNING_NAME")
                }
            };
            trainningWorkforce.ListMetaDatas = trainnings;
            return View(trainningWorkforce);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddTrainningWorkforce(TrainningWorkforceMetaData model, HttpPostedFileBase PHOTO)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (model.PHOTOfile != null)
            {
                System.IO.Stream fs = model.PHOTOfile.InputStream;
                System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                model.PHOTO = br.ReadBytes((Int32)fs.Length);
                ModelState.Remove("PHOTOfile");
            }
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            ModelState.Remove("WF_EMP_TYPE");
           
            model.CMP_ID = loggedin_user.COMPANY_ID;
            model.CREATED_BY = loggedin_user.USER_LOGIN_ID;
           
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("WorkforceTraining", "TrainningWorkforce");
            if (baseBL.WorkforceTrainningBL.AddWorkforceTrainning(model))
                ViewData["result"] = "success";

            return AddTrainningWorkforce();
            
        }

        public ActionResult AddWorkforceTrainningByDeptId(string row, Guid deptId, Guid sub_Id)
        {
            ViewBag.RowNumber = Convert.ToInt32(row);
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            AddWorkforceTrainningMetaData addTrainning = new AddWorkforceTrainningMetaData();
            addTrainning.Departments = new SelectList(this.baseBL.DepartmentBL.GetDepartmentByCompanyId(loggedin_user.COMPANY_ID), "DEPT_ID", "DEPT_NAME", deptId);
            addTrainning.TrainningMasterByDepart = new SelectList(baseBL.WorkforceTrainningBL.GetTrainningMasterByDepartId1(deptId, sub_Id, loggedin_user.COMPANY_ID), "TRAINNING_ID", "TRAINNING_NAME");

            return PartialView("_AddTrainning", addTrainning);
        }

        [HttpGet]
        public JsonResult GetTrainningBydeptId(Guid deptId, Guid sub_deptId)
        {
            var result = baseBL.WorkforceTrainningBL.GetTrainningMasterByDepartId(deptId, sub_deptId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TrainningWorkforce()
        {
            TrainningWorkforceMetaData trainning = new TrainningWorkforceMetaData();
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            trainning.Departments = new SelectList(baseBL.DepartmentBL.GetDepartmentByCompanyId(loggedin_user.COMPANY_ID), "DEPT_ID", "DEPT_NAME");
            ViewBag.EmployeeTypes = new SelectList(baseBL.ManPowerRequestBL.GetEmpTypes(), "WF_EMP_TYPE", "EMP_TYPE");
            ViewBag.Buildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            return View(trainning);
        }

        public ActionResult GetTrainningWorkforceStatus(Guid wf_id)
        {
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            List<GetTRAINNING_WORKFORCE> list = new List<GetTRAINNING_WORKFORCE>();
            list = baseBL.WorkforceTrainningBL.GetEmployeeTrainningStatus(wf_id, loggedin_user.COMPANY_ID);

            return View("_GetTrainningWorkforceStatus", list);
        }

         public string SaveAttendTranning_Emp(Guid wf_id,string EmpList)
        {
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
           string result= baseBL.WorkforceTrainningBL.SaveAttendTranning_Emp(wf_id, loggedin_user.COMPANY_ID, EmpList);

            return result;
        }


        public ActionResult GetTrainningWorkforceList(string DEPT_ID, string SUB_DEPT_ID, string FROM_DATE, string TO_DATE,string BUILDING_ID)
        {
            DateTime FDate = Convert.ToDateTime(FROM_DATE.ToString());
            DateTime ToDate = Convert.ToDateTime(TO_DATE.ToString());
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            List<GetTRAINNING_WORKFORCE> list = new List<GetTRAINNING_WORKFORCE>();
            list = baseBL.WorkforceTrainningBL.GetTrainningWorkforceList(DEPT_ID, SUB_DEPT_ID, FDate, ToDate, loggedin_user.COMPANY_ID, BUILDING_ID);

            return View("_GetTrainningWorkforceStatus", list);
        }

        [HttpGet]
        public ActionResult UpdateTrainningWorkforceStatus(Guid wftm_id)
        {
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            TRAINNING_WORKFORCE_MAPPING updatemodel = new TRAINNING_WORKFORCE_MAPPING();
            List<AddWorkforceMappingTrainning> data = baseBL.WorkforceTrainningBL.UpdateEmployeeTrainningList(wftm_id);

            updatemodel = baseBL.WorkforceTrainningBL.UpdateEmployeeTrainningStatus(wftm_id);
            updatemodel.PHOTOBase64= updatemodel.PHOTO == null ? "~/Content/IdCardImages/profile.png" : "data:image/png;base64," + Convert.ToBase64String(updatemodel.PHOTO, 0, updatemodel.PHOTO.Length);
            updatemodel.ListMetaDatas = data;
            updatemodel.Departments = new SelectList(baseBL.DepartmentBL.GetDepartmentByCompanyId(loggedin_user.COMPANY_ID), "DEPT_ID", "DEPT_NAME");
            updatemodel.TrainningMasterByDepart = new SelectList(this.baseBL.WorkforceTrainningBL.GetTrainningMaster(updatemodel.DEPT_ID.Value, updatemodel.SUBDEPT_ID), "TRAINNING_ID", "TRAINNING_NAME", updatemodel.TRAINNING_ID);
            ViewBag.TranningList = new SelectList(this.baseBL.WorkforceTrainningBL.GetTrainningMaster(updatemodel.DEPT_ID.Value, updatemodel.SUBDEPT_ID), "TRAINNING_ID", "TRAINNING_NAME");
            ViewBag.EmployeeTypes = new SelectList(baseBL.ManPowerRequestBL.GetEmpTypes(), "WF_EMP_TYPE", "EMP_TYPE");

            ViewBag.Employees = baseBL.WorkforceBL.BindWorkforceByWFType(updatemodel.DEPT_ID.Value, updatemodel.SUBDEPT_ID, updatemodel.WF_EMP_TYPE, updatemodel.BUILDING_ID).ToList();

            ViewBag.RowNumber = updatemodel.ListMetaDatas.Count;
            ViewBag.Buildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            ViewBag.EmpName = updatemodel.WORKFORCE_NAME;
            ViewBag.ISComplete = updatemodel.ISTRAINNINGCOMPLETED;
            ViewData["result"] = "success";
            return View(updatemodel);
        }

        [HttpPost]
        public ActionResult UpdateTrainningWorkforceStatus(TRAINNING_WORKFORCE_MAPPING mapping)
        {
            if (mapping.PHOTOfile != null)
            {

                System.IO.Stream fs = mapping.PHOTOfile.InputStream;
                System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                mapping.PHOTO = br.ReadBytes((Int32)fs.Length);
                //string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);


            }
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            TRAINNING_WORKFORCE_MAPPING updatemodel = new TRAINNING_WORKFORCE_MAPPING();
            updatemodel.CreatedBy = loggedin_user.USER_LOGIN_ID;
            if (baseBL.WorkforceTrainningBL.UpdateEmployeeTrainningStatus(mapping))
            {
                ViewBag.Msg = "Workforce Training details updated.";
                //return RedirectToAction("TrainningWorkforce");
            }
            
            return UpdateTrainningWorkforceStatus(mapping.TRAINNING_WORKFORCE_ID);
        }

    }
}