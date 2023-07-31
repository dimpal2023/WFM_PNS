using System;
using System.Collections.Generic;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Interfaces;

namespace Wfm.App.BL
{
    public class ManPowerRequestBL
    {
        public IBaseRepository baseRepository;

        public ManPowerRequestBL(IBaseRepository baseRepo)
        {
            baseRepository = baseRepo;
        }
        public List<BuildingMasterMetaData> GetBuildings()
        {
            return baseRepository.ManPowerRequestRepo.GetBuildings();
        }
        public List<BuildingMasterMetaData> GetBuildings_Transfer()
        {
            return baseRepository.ManPowerRequestRepo.GetBuildings_Transfer();
        }
        public List<SkillMasterMetaData> GetSkills()
        {
            return baseRepository.ManPowerRequestRepo.GetSkills();
        }
        public List<WFDesignationMasterMetaData> GetDesignations()
        {
            return baseRepository.ManPowerRequestRepo.GetDesignations();
        }

        public List<WFDesignationMasterMetaData> GetDesignationBySkill(Guid SKILL_ID)
        {
            return baseRepository.ManPowerRequestRepo.GetDesignationBySkill(SKILL_ID);
        }

        public List<RECMasterMetaData> GetMPRHiring()
        {
            return baseRepository.ManPowerRequestRepo.GetMPRHiring();
        }

        public List<DepartmentMasterMetaData> GetFloorByBuildingId(Guid buildingId)
        {
            return baseRepository.ManPowerRequestRepo.GetFloorByBuildingId(buildingId);
        }
         public List<DepartmentMasterMetaData> GetAllFloorByBuildingId(Guid buildingId)
        {
            return baseRepository.ManPowerRequestRepo.GetAllFloorByBuildingId(buildingId);
        }

        public List<WorkforceTypeMetaData> GetEmpTypes()
        {
            return baseRepository.ManPowerRequestRepo.GetEmpTypes();
        }

        public bool Create(ManPowerRequestFormMetaData data)
        {
            return baseRepository.ManPowerRequestRepo.Create(data);
        }
        public List<ManPowerRequestFormMetaDataList> GetMRFAllItems(Guid companyId)
        {
            return baseRepository.ManPowerRequestRepo.GetMRFAllItems(companyId);
        }
        public List<ManPowerRequestFormMetaDataList> GetMRFAllItems1(Guid companyId, string dept_id, string sub_dept_id, string BUILDING_ID)
        {
            return baseRepository.ManPowerRequestRepo.GetMRFAllItems1(companyId, dept_id, sub_dept_id, BUILDING_ID);
        }
       
        public ManPowerRequestFormMetaData GetMRFByMRF_INETRNAL_ID(Guid mrf_INETRNAL_ID)
        {
            return baseRepository.ManPowerRequestRepo.GetMRFByMRF_INETRNAL_ID(mrf_INETRNAL_ID);
        }

        public ManPowerRequestFormMetaData GetMRFDetailsByMRF_INETRNAL_ID(Guid mrf_INETRNAL_ID)
        {
            return baseRepository.ManPowerRequestRepo.GetMRFDetailsByMRF_INETRNAL_ID(mrf_INETRNAL_ID);
        }

        public bool Edit(ManPowerRequestFormMetaData model)
        {
            return baseRepository.ManPowerRequestRepo.Edit(model);
        }

        public List<MRFApprovalMetadata> GetMRFApprovalByDepartmentId(Guid deptId,Guid? sub_dept_id, string status, Guid BUILDING_ID)
        {
            return baseRepository.ManPowerRequestRepo.GetMRFApprovalByDepartmentId(deptId, sub_dept_id,status, BUILDING_ID);
        }
        public List<MRFApprovalMetadata> GetMRFApprovalByMRFId(Guid cOMPANY_ID, Guid mrf_id)
        {
            return baseRepository.ManPowerRequestRepo.GetMRFApprovalByMRFId(cOMPANY_ID, mrf_id);
        }

        public void UpdatMRFApprovalsByMappingId(IEnumerable<Guid> twfmIds, string status, string remark, string mrfSearchULR)
        {
            baseRepository.ManPowerRequestRepo.UpdatMRFApprovalsByMappingId(twfmIds, status, remark, mrfSearchULR);
        }
    }
}