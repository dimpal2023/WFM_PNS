using System;
using System.Collections.Generic;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Interfaces;
using Wfm.App.Infrastructure.Repositories;

namespace Wfm.App.BL
{
    public class WorkFlowMappingBL
    {
        private IBaseRepository baseRepository;

        public WorkFlowMappingBL(IBaseRepository baseRepo)
        {
            baseRepository = baseRepo;
        }

        public List<Core.Model.WorkflowMappingMasterMetaData> GetAllItems()
        {
            return baseRepository.WorkflowMappingRepo.GetAllItems();
        }

        public void Create(List<WorkflowMappingMasterSaveMetaData> workflowMapping, string userName, Guid WORKFLOW_ID)
        {

            baseRepository.WorkflowMappingRepo.Create(workflowMapping, userName, WORKFLOW_ID);
        }

        public WorkflowMappingMasterMetaData Find(string wfId)
        {
            return baseRepository.WorkflowMappingRepo.Find(wfId);
        }

        public List<WorkflowMappingMasterVieweMetaData> GetWorkflowMappingMaster()
        {
            return baseRepository.WorkflowMappingRepo.GetWorkflowMappingMaster();
        }

        public void Update(WorkflowMappingMasterMetaData workflowMapping)
        {
            baseRepository.WorkflowMappingRepo.Update(workflowMapping);
        }

        public void Delete(Guid gpId)
        {
            baseRepository.WorkflowMappingRepo.Delete(gpId);
        }

        public List<WorkflowMasterMetaData> GetWorkflowMasterMetaDatas()
        {
            return baseRepository.WorkflowMappingRepo.GetWorkflowMasterMetaDatas();
        }        

        public List<WorkflowMappingMasterSaveMetaData> GetWorkflowMappingMasterByWorkflowIdAndLevelId(Guid guid, int levelId)
        {
            return baseRepository.WorkflowMappingRepo.GetWorkflowMappingMasterByWorkflowIdAndLevelId(guid, levelId);
        }
    }
}
