using System;
using System.Collections.Generic;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Interfaces;

namespace Wfm.App.BL
{
    public class AssetBL 
    {
        private IBaseRepository baseRepository;

        public AssetBL(IBaseRepository baseRepo)
        {
            baseRepository = baseRepo;
        }

        public void Create(AssetMappingMasterMetaData asset)
        {
            baseRepository.AssetRepo.Create(asset);
        }

        public List<AssetMappingMasterMetaData> GetAllAsset()
        {
            return baseRepository.AssetRepo.GetAllAsset();
        }

        public List<AssetMasterMetaData> GetAssetByDepartmentId(Guid department_id, Guid sub_dept_id)
        {
            return baseRepository.AssetRepo.GetAssetByDepartmentId(department_id, sub_dept_id);
        }


        public AssetMappingMasterMetaData Find(Guid asset_id)
        {
            return baseRepository.AssetRepo.Find(asset_id);
        }

        public void Update(AssetMappingMasterMetaData asset)
        {
            baseRepository.AssetRepo.Update(asset);
        }

        public void Delete(Guid asset_id)
        {
            baseRepository.AssetRepo.Delete(asset_id);
        }

        public List<AssetMasterMetaData> GetAssetByDeptId(Guid dept_id, Guid sub_dept_id)
        {
            return baseRepository.AssetRepo.GetAssetByDepartmentId(dept_id,sub_dept_id);
        }

        public void AddAssetAllocation(AssetAllocationMetaDataForm assetAllocation)
        {
            baseRepository.AssetRepo.AddAssetAllocation(assetAllocation);
        }

        public void SubmitEmployeeAssetsOnExit(ExitManagementMetaData exitManagement)
        {
            baseRepository.AssetRepo.SubmitEmployeeAssetsOnExit(exitManagement);
        }

        public string Employee_Transfer(Guid BUILDING_ID, Guid DEPT_ID, Guid SUBDEPT_ID, int EMPLOYMENT_TYPE, Guid? WF_ID)
        {
           return baseRepository.AssetRepo.Employee_Transfer(BUILDING_ID, DEPT_ID, SUBDEPT_ID, EMPLOYMENT_TYPE,WF_ID);
        }

        public List<AssetMappingMetaData> GetAssignedAssetByEmpId(Guid emp_id)
        {
            return baseRepository.AssetRepo.GetAssignedAssetByEmpId(emp_id);
        }

        public ExitManagementMetaData GetEmployeesExitApprovalDetails(Guid emp_id)
        {
            return baseRepository.AssetRepo.GetEmployeesExitApprovalDetails(emp_id);
        }

        public List<ExitApprovalMetaData> GetExitApprovals(Guid deptId, Guid? sub_dept_id , string status, Guid BUILDING_ID)
        {
            return baseRepository.AssetRepo.GetExitApprovals(deptId, sub_dept_id, status, BUILDING_ID);
        }


        public void UpdatExitApprovalsByMappingId(IEnumerable<Guid> twfmIds, string status, string remark, string exitSearchULR)
        {
            baseRepository.AssetRepo.UpdatExitApprovalsByMappingId(twfmIds, status, remark, exitSearchULR);
        }

        public string TransferApprovalList(string deptId, string sub_dept_id, string status, string BUILDING_ID,string CurrentUser,string UserType)
        {
            return baseRepository.AssetRepo.TransferApprovalList(deptId, sub_dept_id, status, BUILDING_ID, CurrentUser,UserType);
        }
        public string ApprovedTransfer(string TransferID, string ApprovedBy,string Remark)
        {
           return baseRepository.AssetRepo.ApprovedTransfer(TransferID, ApprovedBy, Remark);
        }
        public string GetAllAssetAllocation_Details(string BUILDING_ID, string DEPT_ID, string SUBDEPT_ID, string WF_EMP_TYPE, string EMP_NAME)
        {
            return baseRepository.AssetRepo.GetAllAssetAllocation_Details(BUILDING_ID, DEPT_ID, SUBDEPT_ID, WF_EMP_TYPE, EMP_NAME);
        }
    }
}