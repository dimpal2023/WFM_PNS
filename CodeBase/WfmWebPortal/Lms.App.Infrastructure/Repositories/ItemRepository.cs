using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Wfm.App.Common;
using Wfm.App.Core;
using Wfm.App.Core.Model;

namespace Wfm.App.Infrastructure.Repositories
{
    public class ItemRepository
    {
        private ApplicationEntities _appEntity;
        DateTime dt;
        public ItemRepository()
        {
            dt = DateTime.Now;
            _appEntity = new ApplicationEntities();
        }

        public bool IsItemCodeAvailable(string iTEM_CODE_NAME, Guid iTEM_CODE_ID)
        {
            Guid companyid = SessionHelper.Get<Guid>("CompanyId");
            if (iTEM_CODE_ID != new Guid())
            {
                var itemCode = _appEntity.TAB_ITEM_CODE.Where(x => x.ITEM_CODE_NAME == iTEM_CODE_NAME && x.ITEM_CODE_ID != iTEM_CODE_ID && x.COMPANY_ID == companyid).FirstOrDefault();
                if (itemCode != null)
                {
                    return true;
                }
                return false;
            }
            else
            {
                var itemCode = _appEntity.TAB_ITEM_CODE.Where(x => x.ITEM_CODE_NAME == iTEM_CODE_NAME && x.COMPANY_ID == companyid).FirstOrDefault();
                if (itemCode != null)
                {
                    return true;
                }
            }
            return false;
        }

        public List<ItemMasterMetaData> GetItems(Guid? dept_id, Guid? sub_dept_id)
        {
            Guid companyid = SessionHelper.Get<Guid>("CompanyId");
            return (from itm in _appEntity.TAB_ITEM_OPERATION_MASTER
                    join dept in _appEntity.TAB_DEPARTMENT_MASTER on itm.DEPT_ID equals dept.DEPT_ID
                    where itm.DEPT_ID == (dept_id != null ? dept_id : itm.DEPT_ID)
                               && itm.COMPANY_ID == companyid
                    select new ItemMasterMetaData
                    {
                        ITEM_ID = itm.UNIQUE_OPERATION_ID,
                        ITEM_NAME = itm.ITEM_NAME,
                        ITEM_CODE_NAME = itm.ITEM_NAME,
                        DEPT_NAME = dept.DEPT_NAME,
                        //SUBDEPT_NAME = itm.SUBDEPT_NAME
                    }).ToList();
        }

        //public List<ItemOperationsMasterMetaData> GetItemOperations(Guid? dept_id, Guid? BUILDING_ID2)
        //{
        //    Guid companyid = SessionHelper.Get<Guid>("CompanyId");
        //    return (from itmOp in _appEntity.TAB_ITEM_OPERATION_MASTER
        //            join dept in _appEntity.TAB_DEPARTMENT_MASTER on itmOp.DEPT_ID equals dept.DEPT_ID
        //            join bm in _appEntity.TAB_BUILDING_MASTER on itmOp.BUILDING_ID equals bm.BUILDING_ID
        //            where itmOp.DEPT_ID == (dept_id != null ? dept_id : itmOp.DEPT_ID)
        //                       && itmOp.BUILDING_ID == BUILDING_ID2
        //                       && itmOp.COMPANY_ID == companyid
        //            select new ItemOperationsMasterMetaData
        //            {
        //                UNIQUE_OPERATION_ID = itmOp.UNIQUE_OPERATION_ID,
        //                RATE = itmOp.RATE,
        //                OPERATION = itmOp.OPERATION,
        //                ITEM_NAME = itmOp.ITEM_NAME,
        //                ITEM_CODE_NAME = itmOp.ITEM_CODE,
        //                DEPT_NAME = dept.DEPT_NAME,
        //                BUILDING_NAME=bm.BUILDING_NAME,
        //                BUILDING_ID=bm.BUILDING_ID
        //            }).ToList();
        //}
        public List<ItemOperationsMasterMetaData> GetItemOperations(Guid? dept_id, Guid? BUILDING_ID2)
        {
            Guid companyid = SessionHelper.Get<Guid>("CompanyId");
            return (from itmOp in _appEntity.TAB_ITEM_OPERATION_MASTER
                    join dept in _appEntity.TAB_DEPARTMENT_MASTER on itmOp.DEPT_ID equals dept.DEPT_ID
                    join bm in _appEntity.TAB_BUILDING_MASTER on itmOp.BUILDING_ID equals bm.BUILDING_ID
                    where itmOp.DEPT_ID == (dept_id != null ? dept_id : itmOp.DEPT_ID)
                               && itmOp.BUILDING_ID == (BUILDING_ID2 == null ? itmOp.BUILDING_ID : BUILDING_ID2)
                               && itmOp.COMPANY_ID == companyid
                    orderby itmOp.CREATED_DATE descending
                    select new ItemOperationsMasterMetaData
                    {
                        UNIQUE_OPERATION_ID = itmOp.UNIQUE_OPERATION_ID,
                        RATE = itmOp.RATE,
                        OPERATION = itmOp.OPERATION,
                        ITEM_NAME = itmOp.ITEM_NAME,
                        ITEM_CODE_NAME = itmOp.ITEM_CODE,
                        DEPT_NAME = dept.DEPT_NAME,
                        BUILDING_NAME = bm.BUILDING_NAME,
                        BUILDING_ID = bm.BUILDING_ID,
                        Status=itmOp.STATUS
                    }).ToList();
        }
        public ItemOperationsMasterMetaData GetItemOperationById(Guid id)
        {
            Guid companyid = SessionHelper.Get<Guid>("CompanyId");
            return (from itmOp in _appEntity.TAB_ITEM_OPERATION_MASTER
                    where itmOp.UNIQUE_OPERATION_ID == id
                         && itmOp.COMPANY_ID == companyid

                    select new ItemOperationsMasterMetaData
                    {
                        BUILDING_ID = (Guid)itmOp.BUILDING_ID,
                        DEPT_ID = (Guid)itmOp.DEPT_ID,
                        ITEM_CODE_NAME = itmOp.ITEM_CODE,
                        OPERATION = itmOp.OPERATION,
                        UNIQUE_OPERATION_ID = itmOp.UNIQUE_OPERATION_ID,
                        RATE = itmOp.RATE,
                        ITEM_NAME = itmOp.ITEM_NAME,
                        Status=itmOp.STATUS
                    }).FirstOrDefault();
        }

        public List<ItemMasterMetaData> GetItemByItemCodeId(Guid item_code_id)
        {
            Guid companyid = SessionHelper.Get<Guid>("CompanyId");
            return (from itm in _appEntity.TAB_ITEM_OPERATION_MASTER
                    where itm.UNIQUE_OPERATION_ID == item_code_id
                               && itm.COMPANY_ID == companyid
                    select new ItemMasterMetaData
                    {
                        //ITEM_ID = itm.ITEM_ID,
                        ITEM_NAME = itm.ITEM_NAME,
                        //ITEM_CODE_NAME = itm.ITEM_CODE_NAME,
                    }).ToList();
        }

        public bool AddItemOperation(ItemOperationsMasterMetaData metaData)
        {
            try
            {
                DateTime dt = DateTime.Now;
                Guid companyid = SessionHelper.Get<Guid>("CompanyId");
                var created_by = SessionHelper.Get<String>("Username");
                if (metaData.UNIQUE_OPERATION_ID != new Guid())
                {
                    var itemOP = _appEntity.TAB_ITEM_OPERATION_MASTER.Find(metaData.UNIQUE_OPERATION_ID);
                    itemOP.COMPANY_ID = companyid;
                    itemOP.DEPT_ID = metaData.DEPT_ID;
                    itemOP.BUILDING_ID = metaData.BUILDING_ID;
                    itemOP.ITEM_CODE = metaData.ITEM_CODE_NAME;
                    itemOP.ITEM_NAME = metaData.ITEM_NAME;
                    itemOP.OPERATION = metaData.OPERATION;
                    itemOP.RATE = metaData.RATE;
                    itemOP.UPDATED_BY = created_by;
                    itemOP.UPDATED_DATE = dt;
                    itemOP.STATUS = metaData.Status;
                }
                else
                {
                    TAB_ITEM_OPERATION_MASTER itemOP = new TAB_ITEM_OPERATION_MASTER
                    {
                        UNIQUE_OPERATION_ID = Guid.NewGuid(),
                        BUILDING_ID = metaData.BUILDING_ID,
                        DEPT_ID = metaData.DEPT_ID,
                        ITEM_CODE = metaData.ITEM_CODE_NAME,
                        ITEM_NAME = metaData.ITEM_NAME,
                        OPERATION = metaData.OPERATION,
                        RATE = metaData.RATE,
                        COMPANY_ID = companyid,
                        STATUS = metaData.Status,
                        CREATED_BY = created_by,
                        CREATED_DATE = dt,
                    };
                    _appEntity.TAB_ITEM_OPERATION_MASTER.Add(itemOP);
                }
                _appEntity.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - ItemRepository.cs, Method - AddItemOperation", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
                return false;
            }
        }

        public bool IsItemOperationAvailable(string operation, Guid operation_id,string ItemCode,string ItemName,Guid? BUILDING_ID,Guid? DEPT_ID)
        {
            Guid companyid = SessionHelper.Get<Guid>("CompanyId");
            if (operation_id != new Guid())
            {
                var itemOperation = _appEntity.TAB_ITEM_OPERATION_MASTER.Where(x => x.OPERATION == operation
                && x.UNIQUE_OPERATION_ID != operation_id
                && x.ITEM_CODE == ItemCode
                && x.ITEM_NAME == ItemName
                && x.BUILDING_ID == BUILDING_ID
                && x.DEPT_ID == DEPT_ID
                && x.COMPANY_ID == companyid).FirstOrDefault();
                if (itemOperation != null)
                {
                    return true;
                }

                if (itemOperation != null)
                {
                    return true;
                }

                return false;
            }
            else
            {
                var itemOperation = _appEntity.TAB_ITEM_OPERATION_MASTER.Where(x => x.OPERATION == operation
                && x.COMPANY_ID == companyid
                && x.DEPT_ID == DEPT_ID
                && x.ITEM_CODE == ItemCode
                && x.ITEM_NAME == ItemName
                && x.BUILDING_ID == BUILDING_ID).FirstOrDefault();
                if (itemOperation != null)
                {
                    return true;
                }
            }
            return false;
        }

        public List<ItemCodeMasterMetaData> GetItemCodeByDeptAndSubDeptId(Guid? dept_id, Guid? sub_dept_id)
        {
            Guid companyid = SessionHelper.Get<Guid>("CompanyId");
            return (from iCode in _appEntity.TAB_ITEM_CODE
                    where iCode.SUBDEPT_ID == (sub_dept_id != null ? sub_dept_id : iCode.SUBDEPT_ID)
                               && iCode.DEPT_ID == (dept_id != null ? dept_id : iCode.DEPT_ID)
                               && iCode.COMPANY_ID == companyid
                    select new ItemCodeMasterMetaData
                    {
                        ITEM_CODE_ID = iCode.ITEM_CODE_ID,
                        ITEM_CODE_NAME = iCode.ITEM_CODE_NAME,
                    }).OrderBy(x => x.ITEM_CODE_NAME).ToList();
        }

        public ItemMasterMetaData GetItemById(Guid id)
        {
            return (from itm in _appEntity.TAB_ITEM_OPERATION_MASTER
                    where itm.UNIQUE_OPERATION_ID == id
                    select new ItemMasterMetaData
                    {
                        ITEM_ID = itm.UNIQUE_OPERATION_ID,
                        //ITEM_CODE_ID = itm.ITEM_CODE_ID,
                        //ITEM_NAME = itm.ITEM_NAME,
                        //DEPT_ID = itm.DEPT_ID,
                        //SUBDEPT_ID = itm.SUBDEPT_ID,
                    }).FirstOrDefault();
        }

        public bool AddItemCode(ItemCodeMasterMetaData metaData)
        {
            try
            {
                DateTime dt = DateTime.Now;
                Guid companyid = SessionHelper.Get<Guid>("CompanyId");
                var created_by = SessionHelper.Get<String>("Username");
                if (metaData.ITEM_CODE_ID != new Guid())
                {
                    var itemCode = _appEntity.TAB_ITEM_CODE.Find(metaData.ITEM_CODE_ID);
                    itemCode.COMPANY_ID = companyid;
                    itemCode.DEPT_ID = metaData.DEPT_ID;
                    itemCode.ITEM_CODE_NAME = metaData.ITEM_CODE_NAME;
                    itemCode.SUBDEPT_ID = metaData.SUBDEPT_ID;
                    itemCode.UPDATED_BY = created_by;
                    itemCode.UPDATED_DATE = dt;
                }
                else
                {
                    TAB_ITEM_CODE itemCode = new TAB_ITEM_CODE
                    {
                        ITEM_CODE_ID = Guid.NewGuid(),
                        COMPANY_ID = companyid,
                        DEPT_ID = metaData.DEPT_ID,
                        ITEM_CODE_NAME = metaData.ITEM_CODE_NAME,
                        SUBDEPT_ID = metaData.SUBDEPT_ID,
                        CREATED_BY = created_by,
                        CREATED_DATE = dt
                    };
                    _appEntity.TAB_ITEM_CODE.Add(itemCode);
                }
                _appEntity.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - ItemRepository.cs, Method - AddItemCode", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
                return false;
            }
        }

        public bool AddItem(ItemMasterMetaData metaData)
        {
            try
            {
                DateTime dt = DateTime.Now;
                Guid companyid = SessionHelper.Get<Guid>("CompanyId");
                var created_by = SessionHelper.Get<String>("Username");
                if (metaData.ITEM_ID != new Guid())
                {
                    var item = _appEntity.TAB_ITEM_OPERATION_MASTER.Find(metaData.ITEM_ID);
                    item.COMPANY_ID = companyid;
                    item.DEPT_ID = metaData.DEPT_ID;
                    //item.ITEM_CODE_ID = metaData.ITEM_CODE_ID;
                    item.ITEM_NAME = metaData.ITEM_NAME;
                    //item.SUBDEPT_ID = metaData.SUBDEPT_ID;
                    item.UPDATED_BY = created_by;
                    item.UPDATED_DATE = dt;
                }
                //else
                //{
                //    TAB_ITEM_MASTER item = new TAB_ITEM_MASTER
                //    {
                //        ITEM_ID = Guid.NewGuid(),
                //        COMPANY_ID = companyid,
                //        DEPT_ID = metaData.DEPT_ID,
                //        ITEM_CODE_ID = metaData.ITEM_CODE_ID,
                //        SUBDEPT_ID = metaData.SUBDEPT_ID,
                //        ITEM_NAME = metaData.ITEM_NAME,
                //        STATUS="Y",
                //        CREATED_BY = created_by,
                //        CREATED_DATE = dt
                //    };
                //    _appEntity.TAB_ITEM_MASTER.Add(item);
                //}
                _appEntity.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - ItemRepository.cs, Method - AddItem", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
                return false;
            }
        }

        public bool IsItemAvailable(string item_name, Guid item_id)
        {
            Guid companyid = SessionHelper.Get<Guid>("CompanyId");
            if (item_id != new Guid())
            {
                var item = _appEntity.TAB_ITEM_OPERATION_MASTER.Where(x => x.ITEM_NAME == item_name && x.ITEM_ID != item_id && x.COMPANY_ID == companyid).FirstOrDefault();
                if (item != null)
                {
                    return true;
                }
                return false;
            }
            else
            {
                var item = _appEntity.TAB_ITEM_OPERATION_MASTER.Where(x => x.ITEM_NAME == item_name && x.COMPANY_ID == companyid).FirstOrDefault();
                if (item != null)
                {
                    return true;
                }
            }
            return false;
        }

        public List<ItemCodeMasterMetaData> GetItemCodes(Guid? dept_id, Guid? sub_dept_id)
        {
            Guid companyid = SessionHelper.Get<Guid>("CompanyId");
            return (from iCode in _appEntity.TAB_ITEM_CODE
                    join dept in _appEntity.TAB_DEPARTMENT_MASTER on iCode.DEPT_ID equals dept.DEPT_ID
                    join sd in _appEntity.TAB_SUBDEPARTMENT_MASTER on iCode.SUBDEPT_ID equals sd.SUBDEPT_ID
                    where iCode.SUBDEPT_ID == (sub_dept_id != null ? sub_dept_id : iCode.SUBDEPT_ID)
                               && iCode.DEPT_ID == (dept_id != null ? dept_id : iCode.DEPT_ID)
                               && iCode.COMPANY_ID == companyid
                    select new ItemCodeMasterMetaData
                    {
                        ITEM_CODE_ID = iCode.ITEM_CODE_ID,
                        ITEM_CODE_NAME = iCode.ITEM_CODE_NAME,
                        DEPT_NAME = dept.DEPT_NAME,
                        SUBDEPT_NAME = sd.SUBDEPT_NAME

                    }).ToList();
        }

        public ItemCodeMasterMetaData GetItemCodeById(Guid id)
        {
            return (from iCode in _appEntity.TAB_ITEM_CODE
                    where iCode.ITEM_CODE_ID == id
                    select new ItemCodeMasterMetaData
                    {
                        ITEM_CODE_ID = iCode.ITEM_CODE_ID,
                        ITEM_CODE_NAME = iCode.ITEM_CODE_NAME,
                        DEPT_ID = iCode.DEPT_ID,
                        SUBDEPT_ID = iCode.SUBDEPT_ID,

                    }).FirstOrDefault();
        }
       
    }
}
