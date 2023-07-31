using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wfm.App.BL;
using Wfm.App.Common;
using Wfm.App.Core.Model;

namespace Lms.Web.Portal.Controllers
{
    public class MasterController : Controller
    {
        private IBaseBL baseBL;
        public MasterController(IBaseBL baseBL)
        {
            this.baseBL = baseBL;

        }
        // GET: Master
        #region Departments
        public ActionResult Departments()
        {
            DepartmentMasterMetaData metaData = new DepartmentMasterMetaData();
            metaData.DepartmentHeads = new SelectList(baseBL.DepartmentBL.GetDepartmentHeads(), "DEPT_HEAD_ID", "DEPT_HEAD_NAME");
            metaData.Buildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            return View(metaData);
        }
        [HttpPost]
        public ActionResult AddDepartment(DepartmentMasterMetaData metaData)
        {
            if (ModelState.IsValid)
            {
                    
                    if(baseBL.DepartmentBL.IsDepartmentNameAvailable(metaData.DEPT_NAME, metaData.DEPT_ID, metaData.BUILDING_ID))
                    {
                        return Json(new { Status="Dublicate"}, JsonRequestBehavior.AllowGet);
                    }
                    if (this.baseBL.DepartmentBL.AddDepartment(metaData))
                        {
                    return Json(new { Status = "Ok" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDepartments(string dept_head)
        {
            List<DepartmentMasterMetaData> departments = baseBL.DepartmentBL.GetDepartmentsByHeadId(dept_head);
            return PartialView("_Departments", departments);
        }
        public ActionResult GetDepartmentById(Guid id)
        {
            DepartmentMasterMetaData department = baseBL.DepartmentBL.GetDepartmentById(id);

            return Json(department, JsonRequestBehavior.AllowGet);
        }

    
        #endregion



        #region SubDepartments
        public ActionResult SubDepartments()
        {
            Guid companyid = SessionHelper.Get<Guid>("CompanyId");
            SubDepartmentMasterMetaData metaData = new SubDepartmentMasterMetaData();
            metaData.Departments = new SelectList(baseBL.DepartmentBL.GetDepartmentByCompanyId(companyid), "DEPT_ID", "DEPT_NAME");
            metaData.Buildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            return View(metaData);
        }
        [HttpPost]
        public ActionResult AddSubDepartment(SubDepartmentMasterMetaData metaData)
        {

            if (ModelState.IsValid)
            {

                if (baseBL.SubDepartmentBL.IsSubDepartmentNameAvailable(metaData.SUBDEPT_NAME,metaData.DEPT_ID, metaData.SUBDEPT_ID, metaData.BUILDING_ID))
                {
                    return Json(new { Status = "Dublicate" }, JsonRequestBehavior.AllowGet);
                }
                if (this.baseBL.SubDepartmentBL.AddSubDepartment(metaData))
                {
                    return Json(new { Status = "Ok" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetSubDepartments(Guid? dept_id)
        {
            List<SubDepartmentMasterMetaData> SubDepartments = baseBL.SubDepartmentBL.GetSubDepartmentsByDeptId(dept_id);
            return PartialView("_SubDepartments", SubDepartments);
        }

        public ActionResult GetSubDepartments_N(Guid? dept_id, Guid? BUILDING_ID2)
        {
            List<SubDepartmentMasterMetaData> SubDepartments = baseBL.SubDepartmentBL.GetSubDepartmentsByDeptId_N(dept_id, BUILDING_ID2);
            return PartialView("_SubDepartments", SubDepartments);
        }
        public ActionResult GetSubDepartmentById(Guid id)
        {
            SubDepartmentMasterMetaData SubDepartment = baseBL.SubDepartmentBL.GetSubDepartmentById(id);

            return Json(SubDepartment, JsonRequestBehavior.AllowGet);
        }


        #endregion



        #region ItemCodes
        public ActionResult ItemCodes()
        {
            ItemCodeMasterMetaData metaData = new ItemCodeMasterMetaData();
            Guid companyid = SessionHelper.Get<Guid>("CompanyId");
            metaData.Departments = new SelectList(baseBL.DepartmentBL.GetDepartmentByCompanyId(companyid), "DEPT_ID", "DEPT_NAME"); 
            return View(metaData);
        }
        [HttpPost]
        public ActionResult AddItemCode(ItemCodeMasterMetaData metaData)
        {
            if (ModelState.IsValid)
            {

                if (baseBL.ItemBL.IsItemCodeAvailable(metaData.ITEM_CODE_NAME, metaData.ITEM_CODE_ID))
                {
                    return Json(new { Status = "Dublicate" }, JsonRequestBehavior.AllowGet);
                }
                if (this.baseBL.ItemBL.AddItemCode(metaData))
                {
                    return Json(new { Status = "Ok" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetItemCodes(Guid? dept_id, Guid? sub_dept_id)
        {
            List<ItemCodeMasterMetaData> ItemCodes = baseBL.ItemBL.GetItemCodes(dept_id, sub_dept_id);
            return PartialView("_ItemCodes", ItemCodes);
        }
        public ActionResult GetItemCodeById(Guid id)
        {
            ItemCodeMasterMetaData ItemCode = baseBL.ItemBL.GetItemCodeById(id);

            return Json(ItemCode, JsonRequestBehavior.AllowGet);
        }


        #endregion




        #region Items
        public ActionResult Items()
        {
            ItemMasterMetaData metaData = new ItemMasterMetaData();
            Guid companyid = SessionHelper.Get<Guid>("CompanyId");
            metaData.Departments = new SelectList(baseBL.DepartmentBL.GetDepartmentByCompanyId(companyid), "DEPT_ID", "DEPT_NAME");
            return View(metaData);
        }
        [HttpPost]
        public ActionResult AddItem(ItemMasterMetaData metaData)
        {
            if (ModelState.IsValid)
            {

                if (baseBL.ItemBL.IsItemAvailable(metaData.ITEM_NAME, metaData.ITEM_ID))
                {
                    return Json(new { Status = "Dublicate" }, JsonRequestBehavior.AllowGet);
                }
                if (this.baseBL.ItemBL.AddItem(metaData))
                {
                    return Json(new { Status = "Ok" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetItems(Guid? dept_id, Guid? sub_dept_id)
        {
            List<ItemMasterMetaData> Items = baseBL.ItemBL.GetItems(dept_id, sub_dept_id);
            return PartialView("_Items", Items);
        }
        public ActionResult GetItemCodeByDeptAndSubDeptId(Guid? dept_id, Guid? sub_dept_id)
        {
            List<ItemCodeMasterMetaData> Items = baseBL.ItemBL.GetItemCodeByDeptAndSubDeptId(dept_id, sub_dept_id);
            return Json(Items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemById(Guid id)
        {
            ItemMasterMetaData Item = baseBL.ItemBL.GetItemById(id);

            return Json(Item, JsonRequestBehavior.AllowGet);
        }


        #endregion


        #region ItemOperations
        public ActionResult ItemOperations()
        {
            ItemOperationsMasterMetaData metaData = new ItemOperationsMasterMetaData();
            Guid companyid = SessionHelper.Get<Guid>("CompanyId");
            metaData.Departments = new SelectList(baseBL.DepartmentBL.GetDepartmentByCompanyId(companyid), "DEPT_ID", "DEPT_NAME");
            metaData.Buildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            return View(metaData);
        }
        [HttpPost]
        //public ActionResult AddItemOperation(ItemOperationsMasterMetaData metaData)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        if (baseBL.ItemBL.IsItemOperationAvailable(metaData.OPERATION, metaData.UNIQUE_OPERATION_ID,metaData.ITEM_CODE_NAME,metaData.ITEM_NAME,metaData.BUILDING_ID,metaData.DEPT_ID))
        //        {
        //            return Json(new { Status = "Dublicate" }, JsonRequestBehavior.AllowGet);
        //        }
        //        if (this.baseBL.ItemBL.AddItemOperation(metaData))
        //        {
        //            return Json(new { Status = "Ok" }, JsonRequestBehavior.AllowGet);
        //        }
        //        return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //        return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);

        //}

        public ActionResult AddItemOperation(ItemOperationsMasterMetaData metaData)
        {
            if (ModelState.IsValid)
            {
                if (metaData.UNIQUE_OPERATION_ID.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    if (baseBL.ItemBL.IsItemOperationAvailable(metaData.OPERATION, metaData.UNIQUE_OPERATION_ID, metaData.ITEM_CODE_NAME, metaData.ITEM_NAME, metaData.BUILDING_ID, metaData.DEPT_ID))
                    {
                        return Json(new { Status = "Dublicate" }, JsonRequestBehavior.AllowGet);
                    }
                }
                if (this.baseBL.ItemBL.AddItemOperation(metaData))
                {
                    if (metaData.UNIQUE_OPERATION_ID.ToString() == "00000000-0000-0000-0000-000000000000")
                    {
                        return Json(new { Status = "Save" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Status = "Update" }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetItemOperations(Guid? dept_id, Guid? BUILDING_ID2)
        {
            List<ItemOperationsMasterMetaData> ItemOperations = baseBL.ItemBL.GetItemOperations(dept_id, BUILDING_ID2);
            return PartialView("_ItemOperations", ItemOperations);
        }
        public ActionResult GetItemByItemCodeId(Guid item_code_id)
        {
            List<ItemMasterMetaData> ItemOperations = baseBL.ItemBL.GetItemByItemCodeId(item_code_id);
            return Json(ItemOperations, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemOperationById(Guid id)
        {
            ItemOperationsMasterMetaData Item = baseBL.ItemBL.GetItemOperationById(id);

            return Json(Item, JsonRequestBehavior.AllowGet);
        }


        #endregion


    }
}