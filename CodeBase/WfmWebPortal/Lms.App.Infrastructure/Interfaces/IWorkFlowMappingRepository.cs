using System;
using System.Collections.Generic;
using Wfm.App.Core.Model;

namespace Wfm.App.Infrastructure.Interfaces
{
    public interface IWorkFlowMappingRepository
    {
        List<WorkflowMappingMasterMetaData> GetAllItems();
        WorkflowMappingMasterMetaData Find(string workflowId);
        void Create(List<WorkflowMappingMasterSaveMetaData> workflowMapping, string userName, Guid WORKFLOW_ID);
        void Update(WorkflowMappingMasterMetaData workflowMapping);
        void Delete(Guid gatePassId);
        List<WorkflowMasterMetaData> GetWorkflowMasterMetaDatas();
        List<WorkflowMappingMasterVieweMetaData> GetWorkflowMappingMaster();
        List<WorkflowMappingMasterSaveMetaData> GetWorkflowMappingMasterByWorkflowIdAndLevelId(Guid workflowId, int levelId);
        List<WorkflowMappingMasterSaveMetaData> GetWorkflowMappingMasterByWorkflowId(Guid workflowId);
    }
}
