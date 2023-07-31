using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Interfaces;

namespace Wfm.App.BL
{
    public class ItemBL
    {
        private IBaseRepository baseRepository;
        public ItemBL(IBaseRepository baseRepo)
        {
            baseRepository = baseRepo;
        }

        public bool IsItemCodeAvailable(string iTEM_CODE_NAME, Guid iTEM_CODE_ID)
        {
            return baseRepository.ItemRepo.IsItemCodeAvailable(iTEM_CODE_NAME, iTEM_CODE_ID);
        }

        public bool AddItemCode(ItemCodeMasterMetaData metaData)
        {
            return baseRepository.ItemRepo.AddItemCode(metaData);

        }

        public List<ItemCodeMasterMetaData> GetItemCodes(Guid? dept_id, Guid? sub_dept_id)
        {
            return baseRepository.ItemRepo.GetItemCodes(dept_id, sub_dept_id);

        }

        public ItemCodeMasterMetaData GetItemCodeById(Guid id)
        {
            return baseRepository.ItemRepo.GetItemCodeById(id);

        }

        public List<ItemMasterMetaData> GetItems(Guid? dept_id, Guid? sub_dept_id)
        {
            return baseRepository.ItemRepo.GetItems(dept_id, sub_dept_id);
        }

        public List<ItemCodeMasterMetaData> GetItemCodeByDeptAndSubDeptId(Guid? dept_id, Guid? sub_dept_id)
        {
            return baseRepository.ItemRepo.GetItemCodeByDeptAndSubDeptId(dept_id, sub_dept_id);
        }

        public ItemMasterMetaData GetItemById(Guid id)
        {
            return baseRepository.ItemRepo.GetItemById(id);
        }

        public bool IsItemAvailable(string item_name, Guid item_id)
        {
            return baseRepository.ItemRepo.IsItemAvailable(item_name, item_id);
        }

        public bool AddItem(ItemMasterMetaData metaData)
        {
            return baseRepository.ItemRepo.AddItem(metaData);
        }

        public bool IsItemOperationAvailable(string operation, Guid operation_id, string ItemCode, string ItemName, Guid BUILDING_ID, Guid DEPT_ID)
        {
            return baseRepository.ItemRepo.IsItemOperationAvailable(operation, operation_id, ItemCode, ItemName, BUILDING_ID, DEPT_ID);

        }

        public bool AddItemOperation(ItemOperationsMasterMetaData metaData)
        {
            return baseRepository.ItemRepo.AddItemOperation(metaData);
        }

        public List<ItemOperationsMasterMetaData> GetItemOperations(Guid? dept_id, Guid? BUILDING_ID2)
        {
            return baseRepository.ItemRepo.GetItemOperations(dept_id, BUILDING_ID2);
        }

        public List<ItemMasterMetaData> GetItemByItemCodeId(Guid item_code_id)
        {
            return baseRepository.ItemRepo.GetItemByItemCodeId(item_code_id);
        }

        public ItemOperationsMasterMetaData GetItemOperationById(Guid id)
        {
            return baseRepository.ItemRepo.GetItemOperationById(id);
        }
    }
}
