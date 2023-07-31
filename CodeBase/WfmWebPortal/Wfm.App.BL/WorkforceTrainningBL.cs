using System;
using System.Collections.Generic;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Interfaces;
namespace Wfm.App.BL
{
    public class WorkforceTrainningBL
    {
        public IBaseRepository baseRepository;
        public WorkforceTrainningBL(IBaseRepository baseRepo)
        {
            baseRepository = baseRepo;
        }

        public bool AddTrainningMaster(AddTrainningMetaData trainning)
        {
            return baseRepository.WorkforceTrainningRepo.AddTrainningMaster(trainning);
        }

        public bool UpdateTrainningMaster(AddTrainningMetaData trainning)
        {
            return baseRepository.WorkforceTrainningRepo.UpdateTrainningMaster(trainning);
        }

        public void UpdateWorkForceTrainningByMappingId(IEnumerable<Guid> twfmIds, string status, string remark)
        {
            baseRepository.WorkforceTrainningRepo.UpdateWorkForceTrainningByMappingId(twfmIds, status, remark);
        }

        public List<TrainningMasterMetaData> GetTrainningMaster(Guid dept_id, Guid? sub_dept_id)
        {
            return baseRepository.WorkforceTrainningRepo.GetTrainningMaster(dept_id, sub_dept_id);
        }
        public List<TrainningMasterMetaData> GetTrainningMaster_N()
        {
            return baseRepository.WorkforceTrainningRepo.GetTrainningMaster_N();
        }
        public AddTrainningMetaData GetTrainningMasterByTrainningId(Guid cmp_id, Guid trainning_id)
        {
            return baseRepository.WorkforceTrainningRepo.GetTrainningMasterByTrainningId(cmp_id, trainning_id);
        }

        public List<TrainningMasterMetaData> GetTrainningMasterByDepartId(Guid deptId, Guid sub_deptId)
        {
            return baseRepository.WorkforceTrainningRepo.GetTrainningMasterByDepartId(deptId, sub_deptId);
        }
        public List<TrainningMasterMetaData> GetTrainningMasterByDepartId1(Guid deptId, Guid sub_deptId, Guid Company)
        {
            return baseRepository.WorkforceTrainningRepo.GetTrainningMasterByDepartId1(deptId, sub_deptId, Company);
        }


        public bool AddWorkforceTrainning(TrainningWorkforceMetaData model)
        {
            return baseRepository.WorkforceTrainningRepo.AddWorkforceTrainning(model);
        }

        public List<GetTRAINNING_WORKFORCE> GetEmployeeTrainningStatus(Guid wf_id, Guid cmp_id)
        {
            return baseRepository.WorkforceTrainningRepo.GetEmployeeTrainningStatus(wf_id, cmp_id);
        }

         public string SaveAttendTranning_Emp(Guid wf_id, Guid cmp_id,string EmpList)
        {
            return baseRepository.WorkforceTrainningRepo.SaveAttendTranning_Emp(wf_id, cmp_id, EmpList);
        }


        public List<GetTRAINNING_WORKFORCE> GetTrainningWorkforceList(string DEPT_ID, string SUB_DEPT_ID, DateTime FROM_DATE, DateTime TO_DATE, Guid cmp_id,string BUILDING_ID)
        {
            return baseRepository.WorkforceTrainningRepo.GetTrainningWorkforceList(DEPT_ID, SUB_DEPT_ID, FROM_DATE, TO_DATE, cmp_id, BUILDING_ID);
        }

        public TRAINNING_WORKFORCE_MAPPING UpdateEmployeeTrainningStatus(Guid wftm_id)
        {
            return baseRepository.WorkforceTrainningRepo.UpdateEmployeeTrainningStatus(wftm_id);
        }

        public List<AddWorkforceMappingTrainning> UpdateEmployeeTrainningList(Guid wftm_id)
        {
            return baseRepository.WorkforceTrainningRepo.UpdateEmployeeTrainningList(wftm_id);
        }

        public bool UpdateEmployeeTrainningStatus(TRAINNING_WORKFORCE_MAPPING wftm)
        {
            return baseRepository.WorkforceTrainningRepo.UpdateEmployeeTrainningStatus(wftm);
        }

        public List<TRAINNING_WORKFORCE_MAPPING> GetWorkForceTrainningForApprovalByDepartmentId(Guid dept_id, Guid? sub_dept_id, int? workforce_type_id, string trainning_status, Guid BUILDING_ID)
        {
            return baseRepository.WorkforceTrainningRepo.GetWorkForceTrainningForApprovalByDepartmentId(dept_id,sub_dept_id, workforce_type_id, trainning_status, BUILDING_ID);
        }
    }
}
