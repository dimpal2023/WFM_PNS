using Lms.Web.Portal.Authorization;
using System;
using System.Collections.Generic;
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
    public class AssetManagementController : Controller
    {
        private IBaseBL baseBL;

        public AssetManagementController(IBaseBL baseBL)
        {
            this.baseBL = baseBL;
        }

        #region Asset add edit and delete
        
        public ActionResult AllAssets()
        {
            List<AssetMappingMasterMetaData> assets = this.baseBL.AssetBL.GetAllAsset();
            return View(assets);
        }

        [HttpGet]        
        public ActionResult Create()
        {
            AssetMappingMasterMetaData assetMappingMaster = new AssetMappingMasterMetaData();
            assetMappingMaster.Departments= new SelectList(baseBL.DepartmentBL.GetAllDepartment(), "DEPT_ID", "DEPT_NAME");
            assetMappingMaster.Buildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            return View(assetMappingMaster);
        }

        [HttpPost]        
        [Route("/GatePass/Create")]
        public ActionResult Create(AssetMappingMasterMetaData asset)
        {
            if (ModelState.IsValid)
            {
                if (Session["USER"] != null)
                {
                    AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;

                    asset.CREATED_BY = loggedin_user.USER_NAME;
                    asset.COMPANY_ID = loggedin_user.COMPANY_ID;
                }
               baseBL.AssetBL.Create(asset);
            }
            else
            {
                return View(asset);
            }
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("AllAssets", "AssetManagement");
            return Json(new { Url = redirectUrl });
        }

        [HttpGet]        
        public ActionResult Edit(Guid? id)
        {
            Guid companyid = SessionHelper.Get<Guid>("CompanyId");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssetMappingMasterMetaData asset =this.baseBL.AssetBL.Find(id.Value);
            asset.Departments = new SelectList(baseBL.DepartmentBL.GetDepartmentByCompanyId(companyid), "DEPT_ID", "DEPT_NAME");
            asset.Buildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");

            if (asset == null)
            {
                return HttpNotFound();
            }
            return View(asset);
        }

        [HttpPost]        
        public ActionResult Edit([Bind(Exclude = "Departments")] AssetMappingMasterMetaData asset)
        {
            if (ModelState.IsValid)
            {
                baseBL.AssetBL.Update(asset);
            }
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("AllAssets", "AssetManagement");
            return Json(new { Url = redirectUrl });
        }
        
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            baseBL.AssetBL.Delete(id.Value);
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("AllAssets", "AssetManagement");
            return Json(new { Url = redirectUrl });
        }
        #endregion

        #region Asset allocation 
        [HttpGet]        
        public ActionResult AssetAllocation()
        {
            AssetAllocationMetaDataForm assetAllocation = new AssetAllocationMetaDataForm();
            assetAllocation.Departments = new SelectList(baseBL.DepartmentBL.GetAllDepartment(), "DEPT_ID", "DEPT_NAME");
            ViewBag.EmployeeTypes = new SelectList(baseBL.ManPowerRequestBL.GetEmpTypes(), "WF_EMP_TYPE", "EMP_TYPE");
            assetAllocation.Buildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            return View(assetAllocation);
        }

        [HttpPost]        
        public ActionResult AssetAllocation(AssetAllocationMetaDataForm assetAllocation)
        {
            assetAllocation.Departments = new SelectList(baseBL.DepartmentBL.GetAllDepartment(), "DEPT_ID", "DEPT_NAME");
            if (ModelState.IsValid)
            {
                AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;

                assetAllocation.ASSIGN_BY = loggedin_user.USER_NAME;
                assetAllocation.COMPANY_ID = loggedin_user.COMPANY_ID;
                baseBL.AssetBL.AddAssetAllocation(assetAllocation);
                TempData["getDeptId"] = assetAllocation.DEPT_ID;
                TempData["getWFId"] = assetAllocation.WF_ID;
                TempData.Keep();
                return RedirectToAction("EmployeeDetail", "ExitManagement");
                //var redirectUrl = new UrlHelper(Request.RequestContext).Action("EmployeeDetail", "ExitManagement");
                //return Json(new { Url = redirectUrl });
            }
            assetAllocation.Employees = new SelectList(baseBL.WorkforceBL.GetEmployeeListByDepartmentId(assetAllocation.DEPT_ID), "EMP_ID", "EMP_NAME");
            return View(assetAllocation);
        }
        
        public ActionResult GetEmployeeById(string empid)
        {
            WorkforceMetaData workforce = new WorkforceMetaData();
            workforce = baseBL.WorkforceBL.FindWorkforceByWFId(new Guid(empid));
            return PartialView("_EmployeeDetails", workforce);
        }
        [HttpGet]
        public ActionResult GetAssetByDeptId(Guid deptId,Guid sub_dept_id,string row)
        {
            ViewBag.RowNumber = Convert.ToInt32(row);
            AssetAllocationMetaDataForm addAsset = new AssetAllocationMetaDataForm();
            addAsset.Assets = new SelectList(baseBL.AssetBL.GetAssetByDeptId(deptId,sub_dept_id), "ASSET_ID", "ASSET_NAME");

            return PartialView("_AddAsset", addAsset);
        }
        #endregion
    }
}