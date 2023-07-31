using Lms.Web.Portal.Authorization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
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
    public class WorkforceSalaryController : Controller
    {
        private IBaseBL baseBl;


        public WorkforceSalaryController(IBaseBL baseBl)
        {
            this.baseBl = baseBl;
        }


        [HttpPost]
        [Authorize]
        public JsonResult Search(WorkforceMetaData wfobj)
        {
            //WorkforceBL objwf = new WorkforceBL();
            //MasterDataBL mBL = new MasterDataBL();
            List<ManPowerRequestFormMetaDataList> mrfList = new List<ManPowerRequestFormMetaDataList>();
            ManPowerRequestFormMetaDataList objmrf = new ManPowerRequestFormMetaDataList();
            List<SelectListItem> empTypeList = new List<SelectListItem>();
            List<SelectListItem> designationList = new List<SelectListItem>();
            List<SelectListItem> skillList = new List<SelectListItem>();
            List<SelectListItem> WF_EMP_TYPELst = new List<SelectListItem>();

            mrfList = baseBl.WorkforceBL.GetActiveMRFList().Where(x => x.MRF_ID == wfobj.MRF_ID).ToList();

            if (mrfList != null)
            {
                objmrf = mrfList.FirstOrDefault();

                empTypeList = baseBl.MasterDataBL.GetEmpTypeList().Select(x =>
                                      new SelectListItem()
                                      {
                                          Text = x.EMP_TYPE.ToString(),
                                          Value = x.EMP_TYPE_ID.ToString()
                                      }).Where(f => f.Value == objmrf.WF_EMP_TYPE.ToString()).ToList();

                designationList = baseBl.MasterDataBL.GetDesignationList().Select(x =>
                          new SelectListItem()
                          {
                              Text = x.WF_DESIGNATION_NAME.ToString(),
                              Value = x.WF_DESIGNATION_ID.ToString()
                          }).Where(f => f.Value == objmrf.WF_DESIGNATION_ID.ToString()).ToList();

                skillList = baseBl.MasterDataBL.GetSkillList().Select(x =>
                          new SelectListItem()
                          {
                              Text = x.SKILL_NAME.ToString(),
                              Value = x.SKILL_ID.ToString()
                          }).Where(f => f.Value == objmrf.SKILL_ID.ToString()).ToList();

                WF_EMP_TYPELst = baseBl.MasterDataBL.GetWF_EMP_TYPE().Select(x =>
                          new SelectListItem()
                          {
                              Text = x.EMP_TYPE.ToString(),
                              Value = x.WF_EMP_TYPE.ToString()
                          }).Where(f => f.Value == objmrf.WF_EMP_TYPE.ToString()).ToList();
            }
            WorkforceMetaData wfMetaData = new WorkforceMetaData { EmpTypeList = empTypeList, SkillList = skillList, DesignationList = designationList, WF_EMP_TYPEList = WF_EMP_TYPELst };
            wfMetaData.MRF_INTERNAL_ID = objmrf.MRP_INETRNAL_ID;
            wfMetaData.WF_EMP_TYPE = objmrf.WF_EMP_TYPE;

            return Json(wfMetaData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public JsonResult EmpSalaryDetailSearch(WorkforceSalaryMetaData wfobj)
        {
            //WorkforceBL objwf = new WorkforceBL();

            WorkforceSalaryMetaData empsal = baseBl.WorkforceBL.Find(wfobj.EMP_ID);

            return Json(empsal, JsonRequestBehavior.AllowGet);
        }
        #region Special Allowance
        [HttpGet]
        public ActionResult SpecialAllowance()
        {
            SpecialAllowanceMetaData metaData = new SpecialAllowanceMetaData();
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            var deptList = this.baseBl.DepartmentBL.GetDepartmentsByUserId(loggedin_user.COMPANY_ID, loggedin_user.USER_ID);
            var onRollOrContracts = this.baseBl.IDCardGenerationBL.GetOnRollOrContracts(loggedin_user.COMPANY_ID);
            metaData.Departments = new SelectList(deptList, "DEPT_ID", "DEPT_NAME");
            metaData.OnRollOrContracts = new SelectList(onRollOrContracts, "WF_EMP_TYPE", "EMP_TYPE");
            ViewBag.Months = DateTimeFormatInfo.InvariantInfo.MonthNames
            .Select((monthName, index) => new SelectListItem
            {
                Value = (index + 1).ToString(),
                Text = monthName
            }).Where(x => x.Text != "");


            ViewBag.Years = new SelectList(Enumerable.Range(DateTime.Today.Year - 1, 2).Select(x =>
                           new SelectListItem()
                           {
                               Text = x.ToString(),
                               Value = x.ToString()
                           }), "Value", "Text");

            return View(metaData);
        }

        [HttpPost]
        public ActionResult AddSpecialAllowance(SpecialAllowanceMetaData specialAllowance)
        {
            if (this.baseBl.WorkforceBL.AddSpecialAllowance(specialAllowance))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(false, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult GetSpecialAllowanceByDept(Guid? deptId, Guid? sub_dept_id, int? emptype_id, int? year_id)
        //{
        //    List<SpecialAllowanceMetaData> specialAllowances = new List<SpecialAllowanceMetaData>();
        //    specialAllowances = this.baseBl.WorkforceBL.GetSpecialAllowanceByDept(deptId, sub_dept_id, emptype_id, year_id);
        //    return PartialView("_SpecialAllowance", specialAllowances);
        //}

        public ActionResult GetSpecialAllowanceById(Guid id)
        {
            SpecialAllowanceMetaData specialAllowances = new SpecialAllowanceMetaData();
            specialAllowances = this.baseBl.WorkforceBL.GetSpecialAllowanceById(id);
            return Json(specialAllowances, JsonRequestBehavior.AllowGet);

        }
        public JsonResult LoadWorkforceByWFType(Guid deptId, Guid? sub_dept_id, string query, int? emp_type_id)
        {
            List<WorkforceMetaDataList> result = new List<WorkforceMetaDataList>();
            query = query.ToUpper();
            try
            {
                result = baseBl.WorkforceBL.GetEmpAllItems(deptId, sub_dept_id, emp_type_id, null).Where(x => x.EMP_NAME.ToUpper().Contains(query)).ToList();
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
        #endregion

        #region Piec Wager 

        [Authorize]
        public ActionResult DailyWork()
        {
            WorkforceDailyWorkMetaData metaData = new WorkforceDailyWorkMetaData();
            Guid companyId = SessionHelper.Get<Guid>("CompanyId");
            var deptList = this.baseBl.DepartmentBL.GetDepartmentByCompanyId(companyId);
            var onRollOrContracts = this.baseBl.IDCardGenerationBL.GetOnRollOrContracts(companyId);
            metaData.Departments = new SelectList(deptList, "DEPT_ID", "DEPT_NAME");
            metaData.OnRollOrContracts = new SelectList(onRollOrContracts, "WF_EMP_TYPE", "EMP_TYPE");
            ViewBag.Buildings = new SelectList(baseBl.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            ViewBag.EmpName = new List<SelectListItem>();
            return View("WorkforceDailyWork", metaData);
        }

        [HttpGet]
        public ActionResult SearchDailyWork()
        {
            FilterDailyWork metaData = new FilterDailyWork();
            Guid companyId = SessionHelper.Get<Guid>("CompanyId");
            var deptList = this.baseBl.DepartmentBL.GetDepartmentByCompanyId(companyId);
            var onRollOrContracts = this.baseBl.IDCardGenerationBL.GetOnRollOrContracts(companyId);
            metaData.Departments = new SelectList(deptList, "DEPT_ID", "DEPT_NAME");
            metaData.OnRollOrContracts = new SelectList(onRollOrContracts, "WF_EMP_TYPE", "EMP_TYPE");
            ViewBag.Buildings = new SelectList(baseBl.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            ViewBag.EmpName = new List<SelectListItem>();
            return View(metaData);
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditDailyWork(Guid? id)
        {
            WorkforceDailyWorkMetaData metaData = new WorkforceDailyWorkMetaData();
            Guid companyId = SessionHelper.Get<Guid>("CompanyId");
            var deptList = this.baseBl.DepartmentBL.GetDepartmentByCompanyId(companyId);
            var onRollOrContracts = this.baseBl.IDCardGenerationBL.GetOnRollOrContracts(companyId);
            metaData = baseBl.WorkforceBL.EditDailyWork(id.Value);
            string buidling = "b31e2dc8-9a41-eb11-9471-8cdcd4d2c4bc";
            metaData.BUILDING_ID = new Guid(buidling);
            ViewBag.Departments = new SelectList(deptList, "DEPT_ID", "DEPT_NAME");
            ViewBag.SubDepartments = new SelectList(baseBl.SubDepartmentBL.GetSubDepartmentsByDeptId(metaData.DEPARTMENT_ID), "SUBDEPT_ID", "SUBDEPT_NAME", metaData.SUBDEPT_ID);
            ViewBag.OnRollOrContracts = new SelectList(onRollOrContracts, "WF_EMP_TYPE", "EMP_TYPE");
            ViewBag.Buildings = new SelectList(baseBl.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            ViewBag.EmpName = new SelectList(baseBl.WorkforceBL.GetPiecWagerWorkforceByWFTypes(metaData.DEPARTMENT_ID, metaData.SUBDEPT_ID, metaData.WF_EMP_TYPE), "WF_ID", "EMP_NAME");
            //ViewBag.ItemName = new SelectList(baseBl.WorkforceBL.GetItemByDeptId((Guid)(metaData.DEPARTMENT_ID)), "ITEM_ID", "ITEM_NAME");
            //ViewBag.OperationName = new SelectList(baseBl.WorkforceBL.GetItemOperationsByItemId((Guid)(metaData.ITEM_ID)), "UNIQUE_OPERATION_ID", "OPERATION");
            ViewBag.ID = id.Value;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(metaData);

        }

        [HttpPost]
        public ActionResult SearchDailyWork(FilterDailyWork filterDailyWork)
        {
            List<SerchDailyWorkMetaData> result = baseBl.WorkforceBL.GetDailyWorks(filterDailyWork);
            return PartialView("_SearchDailyWork", result);

        }

        //public ActionResult UpdateDailyWork(Guid ITEM_ID, Guid? UNIQUE_OPERATION_ID, string QTY, Guid? ID)
        //{
        //    var redirectUrl = new UrlHelper(Request.RequestContext).Action("SearchDailyWorkList", "Workforce");
        //    try
        //    {
        //        baseBl.WorkforceBL.UpdateDailyWork(UNIQUE_OPERATION_ID, QTY, ID);

        //        return Json(new { Url = redirectUrl, id = "1" });

        //    }
        //    catch
        //    {
        //        return Json(new { Url = redirectUrl, id = "0" });
        //    }

        //}

        public ActionResult AddWorkforceDailyWork(AddWorkForceItemMetaData metaData)
        {
            var result = baseBl.WorkforceBL.AddDailyWork(metaData);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PiecWagerWorkforceByWFType(Guid deptId, Guid? sub_dept_id, string query, int? emp_type_id)
        {
            List<WorkforceMetaDataList> result = new List<WorkforceMetaDataList>();
            query = query.ToUpper();
            try
            {
                result = baseBl.WorkforceBL.GetPiecWagerWorkforceByWFTypes(deptId, sub_dept_id, emp_type_id).Where(x => x.EMP_NAME.ToUpper().Contains(query)).ToList();
                var list = (from q in result
                            select new { Name = string.Concat(q.EMP_NAME, " - ", q.EMP_ID), ID = q.WF_ID }).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetOperationsById(Guid Id, Guid? WF_ID)
        {
            //WorkforceBL wfBL = new WorkforceBL();
            return Json(baseBl.WorkforceBL.GetOperationsById(Id, WF_ID), JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public JsonResult GetOperationsByitemId(string ItemId)
        //{
        //    //WorkforceBL wfBL = new WorkforceBL();
        //    Guid guiditemId = new Guid(ItemId);
        //    return Json(baseBl.WorkforceBL.GetItemOperationsByItemId(guiditemId), JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public JsonResult GetAttendanceBywfId(string wfid, string attdate)
        {
            //WorkforceBL wfBL = new WorkforceBL();
            Guid guidwfId = new Guid(wfid);
            DateTime attendance_date = Convert.ToDateTime(attdate);

            return Json(baseBl.WorkforceBL.GetWorkingHoursBywfId(guidwfId, attendance_date), JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Generate Salary slip
        public ActionResult GetworkforceSalary()
        {
            FilteSalarySlipMetaData metaData = new FilteSalarySlipMetaData();
            Guid companyId = SessionHelper.Get<Guid>("CompanyId");
            var deptList = this.baseBl.DepartmentBL.GetDepartmentByCompanyId(companyId);
            var onRollOrContracts = this.baseBl.IDCardGenerationBL.GetOnRollOrContracts(companyId);
            metaData.Departments = new SelectList(deptList, "DEPT_ID", "DEPT_NAME");
            metaData.OnRollOrContracts = new SelectList(onRollOrContracts, "WF_EMP_TYPE", "EMP_TYPE");
            return View("GetworkforceSalary", metaData);
        }
        public ActionResult GetworkforceSalarySlipByDept(Guid? deptId, Guid? sub_dept_id, int? emptype_id, Guid? wf_id)
        {
            List<WorkforceSalaryMetaData> employees = this.baseBl.WorkforceBL.GetworkforceSalary(deptId, sub_dept_id, emptype_id, wf_id);
            return PartialView("_SalarySlip", employees);
        }

        [HttpPost]
        public ActionResult ExportSalarySlip(DownloadSalarySlip download)
        {
            List<ExportSalaryMetaData> result = baseBl.WorkforceBL.GetWorkForceSalarySlip(download);
            return View(result);

        }
        [HttpGet]
        public ActionResult ExportSalarySlip()
        {
            return RedirectToAction("GetworkforceSalary");

        }
        [AllowAnonymous]
        public ActionResult DownloadSalarySlip()
        {

            return View();
        }


        #endregion
        [Authorize]
        public ActionResult Create()
        {
            Wfm.App.Common.Utility.LogMessagesNLog(Wfm.App.Core.Enums.LogLevels.INFO, "WFMUI", "WFM.App", "EmployeeController", "Create", "", "Edu Master loaded");
            Guid companyid = SessionHelper.Get<Guid>("CompanyId");
            //WorkforceBL wfBL = new WorkforceBL();
            //MasterDataBL mBL = new MasterDataBL();


            var mrfList = baseBl.WorkforceBL.GetApprovedAndNotHireMRF().Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.MRF_CODE.ToString(),
                                      Value = x.MRF_ID.ToString()
                                  });
            var wfemptypeList = baseBl.MasterDataBL.GetWF_EMP_TYPE().Select(x =>
                new SelectListItem()
                {
                    Text = x.EMP_TYPE,
                    Value = x.WF_EMP_TYPE.ToString()
                });
            var agencyList = baseBl.MasterDataBL.GetAgencyList().Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.AGENCY_NAME.ToString(),
                                      Value = x.AGENCY_ID.ToString()
                                  });

            var empStatusList = baseBl.MasterDataBL.GetEmpStatusList().Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.EMP_STATUS.ToString(),
                                      Value = x.EMP_STATUS_ID.ToString()
                                  });

            var companyList = baseBl.MasterDataBL.GetCompanyList().Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.COMPANY_NAME.ToString(),
                                      Value = x.COMPANY_ID.ToString()
                                  }).Where(f => f.Value == companyid.ToString());

            var educationList = baseBl.MasterDataBL.GetEducationList().Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.WF_COURSE_NAME.ToString(),
                                      Value = x.WF_EDUCATION_ID.ToString()
                                  });
            var skillList = baseBl.MasterDataBL.GetSkillList().Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.SKILL_NAME,
                                      Value = x.SKILL_ID.ToString()
                                  });


            var empTypeList = baseBl.MasterDataBL.GetEmpTypeList().Select(x =>
                                   new SelectListItem()
                                   {
                                       Text = x.EMP_TYPE,
                                       Value = x.EMP_TYPE_ID.ToString()
                                   });


            var deptList = baseBl.DepartmentBL.GetDepartmentByCompanyId(companyid).Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.DEPT_NAME.ToString(),
                                      Value = x.DEPT_ID.ToString()
                                  });

            var martialStatusList = baseBl.MasterDataBL.GetMartialStatusList().Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.MARITAL_NAME.ToString(),
                                      Value = x.MARITAL_ID.ToString()
                                  });
            var designationList = baseBl.MasterDataBL.GetDesignationList().Select(x =>
                                   new SelectListItem()
                                   {
                                       Text = x.WF_DESIGNATION_NAME,
                                       Value = x.WF_DESIGNATION_ID.ToString()
                                   });

            var stateList = baseBl.MasterDataBL.GetStateList().Select(x =>
                new SelectListItem()
                {
                    Text = x.STATE_NAME.ToString(),
                    Value = x.STATE_ID.ToString()
                });

            var cityList = baseBl.MasterDataBL.GetStateCityList().Select(x =>
                new SelectListItem()
                {
                    Text = x.CITY_NAME.ToString(),
                    Value = x.CITY_ID.ToString()
                });
            var BuildingList = baseBl.MasterDataBL.GetBuildingList().Select(x =>
                new SelectListItem()
                {
                    Text = x.BUILDING_NAME.ToString(),
                    Value = x.BUILDING_ID.ToString()
                });
            WorkforceMetaData wfMetaData = new WorkforceMetaData
            {
                MRFList = mrfList,
                AgencyList = agencyList,
                EmployeeStatusList = empStatusList,
                CompanyList = companyList,
                EducationList = educationList,
                EmpTypeList = empTypeList,
                DepartmentList = deptList,
                MartialStatusList = martialStatusList,
                SkillList = skillList,
                StateList = stateList,
                CityList = cityList,
                DesignationList = designationList,
                WF_EMP_TYPEList = wfemptypeList,
                Buildings = BuildingList
            };

            return View(wfMetaData);
        }
        public JsonResult LoadWorkforceAndWFId(Guid deptId, string query)
        {
            List<WorkforceMetaDataList> result = new List<WorkforceMetaDataList>();
            query = query.ToUpper();
            try
            {
                AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
                Guid companyId = loggedin_user.COMPANY_ID;
                result = baseBl.WorkforceBL.GetEmpAllItems(companyId, deptId, null, null).Where(x => x.EMP_NAME.ToUpper().Contains(query)).ToList();

                var list = (from q in result
                            select new { Name = q.EMP_NAME + " - " + q.EMP_ID, ID = q.WF_ID }).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [Authorize]
        public ActionResult Create([Bind(Exclude = "APPROVED, STATUS, NAME")] WorkforceMetaData workforce, HttpPostedFileBase PHOTO)
        {

            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (workforce.PHOTOfile != null)
            {

                System.IO.Stream fs = workforce.PHOTOfile.InputStream;
                System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                workforce.PHOTO = br.ReadBytes((Int32)fs.Length);
                //string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                ModelState.Remove("PHOTOfile");


            }
            if (workforce.SIGNATUREfile != null)
            {
                System.IO.Stream fs = workforce.SIGNATUREfile.InputStream;
                System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                workforce.EMP_SIGNATURE = br.ReadBytes((Int32)fs.Length);
                ModelState.Remove("SIGNATUREfile");
            }

            ModelState.Remove("AGENCY_ID");
            if (ModelState.IsValid)
            {
                baseBl.WorkforceBL.Create(workforce);
                ViewData["result"] = "success";
            }
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("AllItems", "Workforce");
            return Json(new { Url = redirectUrl, id = "1" });
        }

        [HttpGet]
        public JsonResult IsBiometricAvailable(string BIOMETRIC_CODE, Int64? AADHAR_NO)
        {
            return Json(!baseBl.WorkforceBL.IsBiometricAvailable(BIOMETRIC_CODE, AADHAR_NO), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetWorkforcByAadharNo(long aadharNo)
        {
            WorkforceMasterMetaData result = baseBl.WorkforceBL.GetWorkforcByAadharNo(aadharNo);
            if (result == null)
            {
                return Json("NotFound", JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetCities(string stateId)
        {
            System.Web.Mvc.JsonResult Jsoncitylist = new JsonResult();
            if (!string.IsNullOrWhiteSpace(stateId))
            {
                //MasterDataBL mBL = new MasterDataBL();
                var cityList = baseBl.MasterDataBL.GetCityList(stateId).Select(x =>
                      new SelectListItem()
                      {
                          Text = x.CITY_NAME.ToString(),
                          Value = x.CITY_NAME.ToString()
                      });
                Jsoncitylist = Json(cityList);
            }
            return Json(Jsoncitylist, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult Salary()
        {
            WorkforceSalaryMetaData data = new WorkforceSalaryMetaData();
            return View(data);
        }
        public ActionResult SalaryEdit(Guid wfid)
        {
            WorkforceSalaryMetaData data = this.baseBl.WorkforceBL.GetworkforceSalaryByWFId(wfid);
            data.ACTION = "UPDATE";
            return View("Salary", data);
        }

        [Authorize]
        [HttpGet]
        public ActionResult SalaryList()
        {
            List<DepartmentMasterMetaData> depts = baseBl.DepartmentBL.GetAllDepartment();
            depts.Insert(0, new DepartmentMasterMetaData { DEPT_ID = Guid.Parse("BD5E4EF7-DC4F-43C9-A223-8954B4CB6455"), DEPT_NAME = "Select All" });
            ViewBag.Dept = new SelectList(depts, "DEPT_ID", "DEPT_NAME");

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

            ViewBag.EmployeeTypes = new SelectList(baseBl.ManPowerRequestBL.GetEmpTypes(), "WF_EMP_TYPE", "EMP_TYPE");

            ViewBag.Years = new SelectList(Enumerable.Range(DateTime.Today.Year - 1, 2).Select(x =>
                           new SelectListItem()
                           {
                               Text = x.ToString(),
                               Value = x.ToString()
                           }), "Value", "Text");

            WorkforceSalaryData wsd = new WorkforceSalaryData();
            wsd.WorkforceSalaries = new List<WorkforceSalaryData>();

            return View(wsd);
        }

        [HttpPost]
        [Authorize]
        public ActionResult SalaryList([Bind(Include = "DEPT_ID, MONTH_ID, YEAR_ID, WF_EMP_TYPE")] WorkforceSalaryData workforceData)
        {
            List<WorkforceSalaryData> wfSalaryData = new List<WorkforceSalaryData>();

            wfSalaryData = baseBl.WorkforceBL.GetEmpSalary(workforceData.DEPT_ID.Value, workforceData.SUBDEPT_ID, int.Parse(workforceData.MONTH_ID), int.Parse(workforceData.YEAR_ID), workforceData.WF_EMP_TYPE);

            return PartialView("_WorkforceSalaryDetails", wfSalaryData);
        }

        public JsonResult LoadWorkforce(Guid deptId, string query)
        {
            List<WorkforceMetaDataList> result = new List<WorkforceMetaDataList>();
            query = query.ToUpper();
            try
            {
                if (deptId == Guid.Empty)
                    result = baseBl.WorkforceBL.GetEmpAllItems().Where(x => x.EMP_NAME.ToUpper().Contains(query)).ToList();
                else
                {
                    AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
                    Guid companyId = loggedin_user.COMPANY_ID;
                    result = baseBl.WorkforceBL.GetEmpAllItems(companyId, deptId, null, null).Where(x => x.DEPT_ID == deptId && x.EMP_NAME.ToUpper().Contains(query)).ToList();
                }

                var list = (from q in result
                            select new { Name = string.Concat(q.EMP_NAME, " - ", q.EMP_ID), ID = q.WF_ID }).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpPost]
        [Authorize]
        [Route("/Workforce/Salary")]
        public ActionResult Salary([Bind(Exclude = "APPROVED, STATUS, NAME")] WorkforceSalaryMetaData salary)
        {
            baseBl.WorkforceBL.Salary(salary);
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("GetworkforceSalary", "Workforce");
            return Json(new { Url = redirectUrl });

        }
        //[HttpGet]
        //public ActionResult GetworkforceSalary()
        //{
        //    WorkflowMasterVieweMetaData metaData = new WorkflowMasterVieweMetaData();
        //    AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
        //    var deptList = this.baseBl.DepartmentBL.GetDepartmentByCompanyId(loggedin_user.COMPANY_ID);
        //    var onRollOrContracts = this.baseBl.IDCardGenerationBL.GetOnRollOrContracts(loggedin_user.COMPANY_ID);
        //    var defaultBind = deptList.Select(x => x.DEPT_ID).FirstOrDefault();
        //    metaData.DEPARTMENT_ID = defaultBind;
        //    metaData.Departments = new SelectList(deptList, "DEPT_ID", "DEPT_NAME");
        //    metaData.OnRollOrContracts = new SelectList(onRollOrContracts, "WF_EMP_TYPE", "EMP_TYPE");
        //    return View(metaData);
        //}

        public ActionResult GetworkforceSalaryByDept(Guid? deptId, int? emptype_id)
        {
            List<WorkforceSalaryMetaData> employees = this.baseBl.WorkforceBL.GetworkforceSalary(deptId, emptype_id);
            return PartialView("_GetworkforceSalary", employees);
        }
        [Authorize]
        public ActionResult AllItems()
        {
            //WorkforceBL wfBL = new WorkforceBL();

            //List<WorkforceMetaDataList> emplist = wfBL.GetEmpAllItems();

            //return View(emplist);


            WorkflowMasterVieweMetaData metaData = new WorkflowMasterVieweMetaData();
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            var deptList = this.baseBl.DepartmentBL.GetDepartmentByCompanyId(loggedin_user.COMPANY_ID);
            var onRollOrContracts = this.baseBl.IDCardGenerationBL.GetOnRollOrContracts(loggedin_user.COMPANY_ID);
            var defaultBind = deptList.Select(x => x.DEPT_ID).FirstOrDefault();
            //var Buildings = new SelectList(baseBl.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            metaData.DEPARTMENT_ID = defaultBind;
            metaData.Departments = new SelectList(deptList, "DEPT_ID", "DEPT_NAME");
            metaData.OnRollOrContracts = new SelectList(onRollOrContracts, "WF_EMP_TYPE", "EMP_TYPE");
            metaData.Buildings = new SelectList(baseBl.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            return View(metaData);
        }
        public ActionResult GetEmployessByDeptId(Guid? deptId, Guid? sub_dept_id, int? emptype_id, Guid? BUILDING_ID)
        {
            WorkflowMasterVieweMetaData metaData = new WorkflowMasterVieweMetaData();
            List<WorkforceMetaDataList> employees = this.baseBl.WorkforceBL.GetEmpAllItems(deptId, sub_dept_id, emptype_id, BUILDING_ID);
            return PartialView("_Employees", employees);
        }
        //// GET: Employee/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        public ActionResult Edit(Guid? wfid)
        {
            //WorkforceBL wfBL = new WorkforceBL();
            //MasterDataBL mBL = new MasterDataBL();

            WorkforceMasterMetaData wm = new WorkforceMasterMetaData();
            wm = baseBl.WorkforceBL.FindWorkforceIWithFullDetailByWFId(Guid.Parse(wfid.ToString()));
            Guid companyid = SessionHelper.Get<Guid>("CompanyId");


            var mrfList = baseBl.WorkforceBL.GetActiveMRFList().Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.MRF_ID.ToString(),
                                      Value = x.MRF_ID.ToString()
                                  });

            var agencyList = baseBl.MasterDataBL.GetAgencyList().Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.AGENCY_NAME.ToString(),
                                      Value = x.AGENCY_ID.ToString()
                                  });

            var empStatusList = baseBl.MasterDataBL.GetEmpStatusList().Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.EMP_STATUS.ToString(),
                                      Value = x.EMP_STATUS_ID.ToString()
                                  });

            var companyList = baseBl.MasterDataBL.GetCompanyList().Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.COMPANY_NAME.ToString(),
                                      Value = x.COMPANY_ID.ToString()
                                  }).Where(f => f.Value == companyid.ToString());

            var educationList = baseBl.MasterDataBL.GetEducationList().Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.WF_COURSE_NAME.ToString(),
                                      Value = x.WF_EDUCATION_ID.ToString()
                                  });
            var empTypeList = baseBl.MasterDataBL.GetEmpTypeList().Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.EMP_TYPE,
                                      Value = x.EMP_TYPE_ID.ToString()
                                  });

            var deptList = baseBl.DepartmentBL.GetDepartmentByCompanyId(companyid).Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.DEPT_NAME.ToString(),
                                      Value = x.DEPT_ID.ToString()
                                  });

            var martialStatusList = baseBl.MasterDataBL.GetMartialStatusList().Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.MARITAL_NAME.ToString(),
                                      Value = x.MARITAL_ID.ToString()
                                  });

            var designationList = baseBl.MasterDataBL.GetDesignationList().Select(x =>
                                   new SelectListItem()
                                   {
                                       Text = x.WF_DESIGNATION_NAME,
                                       Value = x.WF_DESIGNATION_ID.ToString()
                                   });


            var skillList = baseBl.MasterDataBL.GetSkillList().Select(x =>
                                   new SelectListItem()
                                   {
                                       Text = x.SKILL_NAME,
                                       Value = x.SKILL_ID.ToString()
                                   });

            var stateList = baseBl.MasterDataBL.GetStateList().Select(x =>
                      new SelectListItem()
                      {
                          Text = x.STATE_NAME.ToString(),
                          Value = x.STATE_ID.ToString()
                      });

            var cityList = baseBl.MasterDataBL.GetStateCityList().Select(x =>
                new SelectListItem()
                {
                    Text = x.CITY_NAME.ToString(),
                    Value = x.CITY_ID.ToString()
                });

            var wfemptypeList = baseBl.MasterDataBL.GetWF_EMP_TYPE().Select(x =>
                new SelectListItem()
                {
                    Text = x.EMP_TYPE,
                    Value = x.WF_EMP_TYPE.ToString()
                });
            var BuildingList = baseBl.MasterDataBL.GetBuildingList().Select(x =>
               new SelectListItem()
               {
                   Text = x.BUILDING_NAME.ToString(),
                   Value = x.BUILDING_ID.ToString()
               });
            WorkforceMetaData wfMetaData = new WorkforceMetaData { MRFList = mrfList, AgencyList = agencyList, EmployeeStatusList = empStatusList, CompanyList = companyList, EducationList = educationList, EmpTypeList = empTypeList, DepartmentList = deptList, MartialStatusList = martialStatusList, SkillList = skillList, StateList = stateList, CityList = cityList, DesignationList = designationList, WF_EMP_TYPEList = wfemptypeList, Buildings = BuildingList };
            wfMetaData.WF_ID = wm.WF_ID;
            wfMetaData.WF_EMP_TYPE = wm.WF_EMP_TYPE;
            wfMetaData.EMP_ID = wm.EMP_ID;
            wfMetaData.EMP_NAME = wm.EMP_NAME;
            wfMetaData.FATHER_NAME = wm.FATHER_NAME;
            wfMetaData.EMP_STATUS_ID = Convert.ToInt16(wm.EMP_STATUS_ID);
            wfMetaData.COMPANY_ID = wm.COMPANY_ID;
            if (wm.AGENCY_ID != null)
            {
                wfMetaData.AGENCY_ID = Guid.Parse(wm.AGENCY_ID.ToString());
            }

            wfMetaData.DEPT_ID = Guid.Parse(wm.DEPT_ID.ToString());
            wfMetaData.SUBDEPT_ID = wm.SUBDEPT_ID;
            wfMetaData.GENDER = wm.GENDER;
            wfMetaData.BIOMETRIC_CODE = wm.BIOMETRIC_CODE;
            wfMetaData.DATE_OF_BIRTH = wm.DATE_OF_BIRTH;
            wfMetaData.NATIONALITY = wm.NATIONALITY;
            wfMetaData.WF_EDUCATION_ID = Convert.ToInt16(wm.WF_EDUCATION_ID);
            wfMetaData.MARITAL_ID = Convert.ToInt16(wm.MARITAL_ID);
            wfMetaData.DOJ = wm.DOJ;
            wfMetaData.DOJ_AS_PER_EPF = wm.DOJ_AS_PER_EPF;
            wfMetaData.WF_DESIGNATION_ID = Convert.ToInt16(wm.WF_DESIGNATION_ID);
            wfMetaData.SKILL_ID = Guid.Parse(wm.SKILL_ID.ToString());
            wfMetaData.EMP_TYPE_ID = Convert.ToInt16(wm.EMP_TYPE_ID);
            wfMetaData.PRESENT_ADDRESS = wm.PRESENT_ADDRESS;
            wfMetaData.PRESENT_ADDRESS_CITY = Guid.Parse(wm.PRESENT_ADDRESS_CITY.ToString());
            wfMetaData.PRESENT_ADDRESS_STATE = wm.PRESENT_ADDRESS_STATE;
            wfMetaData.PRESENT_ADDRESS_PIN = wm.PRESENT_ADDRESS_PIN;
            wfMetaData.PERMANENT_ADDRESS = wm.PERMANENT_ADDRESS;
            wfMetaData.PERMANENT_ADDRESS_CITY = Guid.Parse(wm.PERMANENT_ADDRESS_CITY.ToString());
            wfMetaData.PERMANENT_ADDRESS_STATE = Guid.Parse(wm.PERMANENT_ADDRESS_STATE.ToString());
            wfMetaData.PERMANENT_ADDRESS_PIN = wm.PERMANENT_ADDRESS_PIN;
            wfMetaData.MOBILE_NO = wm.MOBILE_NO;
            wfMetaData.ALTERNATE_NO = wm.ALTERNATE_NO;
            wfMetaData.EMAIL_ID = wm.EMAIL_ID;
            wfMetaData.REFERENCE_ID = wm.REFERENCE_ID;
            wfMetaData.AADHAR_NO = wm.AADHAR_NO;
            wfMetaData.BUILDING_ID = wm.BUILDING_ID;

            wfMetaData.IDENTIFICATION_MARK = wm.IDENTIFICATION_MARK;
            wfMetaData.PHOTOBase64 = wm.PHOTO == null ? "~/Content/IdCardImages/profile.png" : "data:image/png;base64," + Convert.ToBase64String(wm.PHOTO, 0, wm.PHOTO.Length);
            //======changed jitendra13 july
            //wfMetaData.EMP_SIGNATUREBase64 = wm.PHOTO == null? "" : "data:image/png;base64," + Convert.ToBase64String(wm.EMP_SIGNATURE, 0, wm.EMP_SIGNATURE.Length);
            wfMetaData.EMP_SIGNATUREBase64 = wm.EMP_SIGNATURE == null ? "" : "data:image/png;base64," + Convert.ToBase64String(wm.EMP_SIGNATURE, 0, wm.EMP_SIGNATURE.Length);
            //-----------------
            return View(wfMetaData);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(WorkforceMetaData workforce)
        {
            //if ((workforce.EMP_STATUS_ID == 1 || workforce.EMP_STATUS_ID == 3) && workforce.REFERENCE_ID == null)
            //{
            //    return View();
            //}
            try
            {

                if (workforce.PHOTOfile != null)
                {

                    System.IO.Stream fs = workforce.PHOTOfile.InputStream;
                    System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                    workforce.PHOTO = br.ReadBytes((Int32)fs.Length);
                    //string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);


                }
                if (workforce.SIGNATUREfile != null)
                {
                    System.IO.Stream fs = workforce.SIGNATUREfile.InputStream;
                    System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                    workforce.EMP_SIGNATURE = br.ReadBytes((Int32)fs.Length);
                }
                baseBl.WorkforceBL.Edit(workforce);
                var redirectUrl = new UrlHelper(Request.RequestContext).Action("AllItems", "Workforce");
                return Json(new { Url = redirectUrl, id = "1" });

            }
            catch
            {
                return View();
            }
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Employee/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }




        //[HttpPost]
        //[Authorize]
        //[Route("/Workforce/DailyWork")]
        //public ActionResult DailyWork([Bind(Exclude = "APPROVED, STATUS, NAME")]WorkforceDailyWorkData dailywork)
        //{
        //    var errors = ModelState.Values.SelectMany(v => v.Errors);
        //    baseBl.WorkforceBL.SaveDailyWork(dailywork);
        //    var redirectUrl = new UrlHelper(Request.RequestContext).Action("AllItems", "Workforce");
        //    return Json(new { Url = redirectUrl });
        //}

        [HttpPost]
        [Authorize]
        [Route("/Workforce/DailyWork")]
        public ActionResult DailyWork(List<WorkforceDailyWorkData> dailywork)
        {
            var items = dailywork.GroupBy(x => new
            {
                x.DEPT_ID,
                x.EMP_ID,
                x.WORK_DATE,
                x.Overtime
            })
            .Select(g => new
            {
                DEPT_ID = g.Key.DEPT_ID,
                EMP_ID = g.Key.EMP_ID,
                WORK_DATE = g.Key.WORK_DATE,
                OVETIME = g.Key.Overtime,
                DailyWorkItems = g.ToList()
            });

            foreach (var master in items)
            {
                WorkforceDailyWorkData masterDailyWorkData = new WorkforceDailyWorkData();
                masterDailyWorkData.DEPT_ID = master.DEPT_ID;
                masterDailyWorkData.EMP_ID = master.EMP_ID;
                masterDailyWorkData.WORK_DATE = master.WORK_DATE;

                baseBl.WorkforceBL.SaveDailyWorkMaster(masterDailyWorkData);

                foreach (var child in master.DailyWorkItems)
                {
                    WorkforceDailyWorkData childDailyWorkData = new WorkforceDailyWorkData();

                    childDailyWorkData.ITEM_ID = child.ITEM_ID;
                    childDailyWorkData.UNIQUE_OPERATION_ID = child.UNIQUE_OPERATION_ID;
                    childDailyWorkData.QTY = child.QTY;
                    childDailyWorkData.EMP_ID = child.EMP_ID;
                    childDailyWorkData.WORK_DATE = child.WORK_DATE;

                    baseBl.WorkforceBL.SaveDailyWork(childDailyWorkData);
                }
            }

            var redirectUrl = new UrlHelper(Request.RequestContext).Action("AllItems", "Workforce");
            return Json(new { Url = redirectUrl });
        }

        [HttpPost]
        [Authorize]
        public JsonResult DWEmpSearch(WorkforceSalaryMetaData wfobj)
        {
            //WorkforceBL objwf = new WorkforceBL();

            WorkforceDailyWorkData empDW = baseBl.WorkforceBL.DWFind(wfobj.EMP_ID);

            return Json(empDW, JsonRequestBehavior.AllowGet);
        }



        //[HttpGet]
        //public JsonResult GetItemsBydeptId(string deptId)
        //{
        //    //WorkforceBL wfBL = new WorkforceBL();
        //    Guid guiddeptId = new Guid(deptId);
        //    return Json(baseBl.WorkforceBL.GetItemByDeptId(guiddeptId), JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public JsonResult GetEmployeesBydeptId(string deptId)
        {
            //WorkforceBL wfBL = new WorkforceBL();
            Guid guiddeptId = new Guid(deptId);

            return Json(baseBl.WorkforceBL.GetEmployeeListByDepartmentId(guiddeptId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetEmployeeProfile(WorkforceMasterMetaData wfobj)
        {
            //WorkforceBL wfBL = new WorkforceBL();
            return Json(baseBl.WorkforceBL.FindWorkforceIWithFullDetailByWFId(wfobj.WF_ID), JsonRequestBehavior.AllowGet);
        }

        #region DailyWorkList
        [HttpGet]
        public ActionResult SearchDailyWorkList()
        {
            FilterDailyWork metaData = new FilterDailyWork();
            Guid companyId = SessionHelper.Get<Guid>("CompanyId");
            var deptList = this.baseBl.DepartmentBL.GetDepartmentByCompanyId(companyId);
            var onRollOrContracts = this.baseBl.IDCardGenerationBL.GetOnRollOrContracts(companyId);
            metaData.Departments = new SelectList(deptList, "DEPT_ID", "DEPT_NAME");
            metaData.OnRollOrContracts = new SelectList(onRollOrContracts, "WF_EMP_TYPE", "EMP_TYPE");
            ViewBag.Buildings = new SelectList(baseBl.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            ViewBag.EmpName = new List<SelectListItem>();
            return View(metaData);
        }
        [HttpPost]
        public ActionResult SearchDailyWorkList(FilterDailyWork filterDailyWork)
        {
            List<SerchDailyWorkMetaData> result = baseBl.WorkforceBL.SearchDailyWorkList(filterDailyWork);
            return PartialView("_SearchDailyWorkList", result);

        }
        [Authorize]
        public ActionResult Delete_DailyWork(Guid? id,string Date)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var result = baseBl.WorkforceBL.Delete_DailyWork(id.Value,Date);

            if (result == 0) return Json(result, JsonRequestBehavior.AllowGet);

            var redirectUrl = new UrlHelper(Request.RequestContext).Action("SearchDailyWorkList", "WorkManagement");
            return Json(new { Url = redirectUrl, id = "1" });
        }
        #endregion

        #region Salary Revision
        public ActionResult SalaryRevision()
        {
            ViewBag.wfemptypeList = baseBl.MasterDataBL.GetWF_EMP_TYPE().Select(x =>
                new SelectListItem()
                {
                    Text = x.EMP_TYPE,
                    Value = x.WF_EMP_TYPE.ToString()
                });
            ViewBag.skillList = baseBl.MasterDataBL.GetSkillList().Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.SKILL_NAME,
                                      Value = x.SKILL_ID.ToString(),
                                      
                                  });
            return View();
        }
        #endregion
    }
}
