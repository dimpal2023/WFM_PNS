using System;
using System.Collections.Generic;
using System.Linq;
using Wfm.App.Core;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Interfaces;

namespace Wfm.App.Infrastructure.Repositories
{
    public class WorkFlowMappingRepository : IWorkFlowMappingRepository
    {
        private ApplicationEntities _appEntity;

        public WorkFlowMappingRepository()
        {
            _appEntity = new ApplicationEntities();
        }

        public List<WorkflowMappingMasterMetaData> GetAllItems()
        {
            return null;
        }

        public WorkflowMappingMasterMetaData Find(string workflowId)
        {
            return null;
        }

        public void Create(List<WorkflowMappingMasterSaveMetaData> workflowMapping, string userName, Guid WORKFLOW_ID)
        {
            List<TAB_WORKFLOW_MAPPING_MASTER> tAB_WORKFLOW_MAPPING_MASTERs = new List<TAB_WORKFLOW_MAPPING_MASTER>();
            DateTime dt = DateTime.Now;
            foreach (var item in workflowMapping)
            {
                TAB_WORKFLOW_MAPPING_MASTER tAB_WORKFLOW_MAPPING_MASTER = new TAB_WORKFLOW_MAPPING_MASTER
                {
                    WORKFLOWMAPPING_ID = Guid.NewGuid(),
                    AUTO_APPROVE = item.AUTO_APPROVE,
                    AUTO_APPROVE_DAY = item.AUTO_APPROVE_DAY,
                    AUTO_REJECT = item.AUTO_REJECT,
                    AUTO_REJECT_DAY = item.AUTO_REJECT_DAY,
                    ROLE_ID = item.ROLE_ID,
                    USER_ID = item.EMP_ID,
                    status = "Y",
                    LEVEL_ID = item.LEVEL_ID,
                    WORKFLOW_ID = WORKFLOW_ID,
                    Created_by = userName,
                    created_date = dt
                };
                tAB_WORKFLOW_MAPPING_MASTERs.Add(tAB_WORKFLOW_MAPPING_MASTER);
            }

            List<TAB_WORKFLOW_MAPPING_MASTER> remove = _appEntity.TAB_WORKFLOW_MAPPING_MASTER.Where(x => x.WORKFLOW_ID == WORKFLOW_ID).ToList();
            _appEntity.TAB_WORKFLOW_MAPPING_MASTER.RemoveRange(remove);
            _appEntity.TAB_WORKFLOW_MAPPING_MASTER.AddRange(tAB_WORKFLOW_MAPPING_MASTERs);
            _appEntity.SaveChanges();

        }

        public void Update(WorkflowMappingMasterMetaData workflowMapping)
        {
            List<TAB_WORKFLOW_MAPPING_MASTER> workflowManppings = _appEntity.TAB_WORKFLOW_MAPPING_MASTER.Where(x => x.WORKFLOW_ID == workflowMapping.WORKFLOW_ID).ToList();
            _appEntity.SaveChanges();

        }

        public void Delete(Guid gatePassId)
        {
            _appEntity.SaveChanges();

        }

        public List<WorkflowMasterMetaData> GetWorkflowMasterMetaDatas()
        {
            List<WorkflowMasterMetaData> workflowMasterMetaDatas = _appEntity.TAB_WORKFLOW_MASTER.Select(x => new WorkflowMasterMetaData
            {
                WORKFLOW_ID = x.WORKFLOW_ID,
                WORKFLOW_NAME = x.WORKFLOW_NAME
            }).OrderBy(x=>x.WORKFLOW_NAME).ToList();
            return workflowMasterMetaDatas;
        }

        public List<WorkflowMappingMasterVieweMetaData> GetWorkflowMappingMaster()
        {
            List<WorkflowMappingMasterVieweMetaData> result = (from wfm in _appEntity.TAB_WORKFLOW_MAPPING_MASTER
                                                               join wf in _appEntity.TAB_WORKFLOW_MASTER on wfm.WORKFLOW_ID equals wf.WORKFLOW_ID
                                                               join dt in _appEntity.TAB_ROLE_MASTER on wfm.ROLE_ID equals dt.ROLE_ID
                                                               join emp in _appEntity.TAB_LOGIN_MASTER on wfm.USER_ID equals emp.USER_ID
                                                               join lb in _appEntity.TAB_LEVEL_MASTER on wfm.LEVEL_ID equals lb.LEVEL_ID
                                                               select new
                                                               {
                                                                   wf.WORKFLOW_ID,
                                                                   wf.WORKFLOW_NAME,
                                                                   dt.ROLE_NAME,
                                                                   emp.USER_NAME,
                                                                   lb.LEVEL_NAME,
                                                                   wfm.AUTO_APPROVE,
                                                                   wfm.AUTO_APPROVE_DAY,
                                                                   wfm.AUTO_REJECT,
                                                                   wfm.AUTO_REJECT_DAY,
                                                                   wfm.status,
                                                                   wfm.LEVEL_ID

                                                               }).GroupBy(g => g.WORKFLOW_NAME)
                          .Select(x =>
                          new WorkflowMappingMasterVieweMetaData
                          {
                              WORKFLOW_NAME = x.Key,
                              WORKFLOW_ID = x.FirstOrDefault().WORKFLOW_ID,
                              listMetaDatas = x.Select(y => new WorkflowMappingMasterVieweListMetaData
                              {
                                  APPROVAL_NAME = y.USER_NAME,
                                  ROLE_NAME = y.ROLE_NAME,
                                  LEVEL_NAME = y.LEVEL_NAME,
                                  AUTO_APPROVE = y.AUTO_APPROVE,
                                  AUTO_APPROVE_DAY = y.AUTO_APPROVE_DAY,
                                  AUTO_REJECT = y.AUTO_REJECT,
                                  AUTO_REJECT_DAY = y.AUTO_REJECT_DAY,
                                  Status = y.status,
                                  LEVEL_ID = y.LEVEL_ID
                              }).OrderBy(y => y.LEVEL_ID).ToList()
                          }).ToList();
            return result;
        }
        public List<WorkflowMappingMasterSaveMetaData> GetWorkflowMappingMasterByWorkflowIdAndLevelId(Guid workflowId, int levelId)
        {
            List<LevelMasterMetaData> LevelMasterMetaDatas = _appEntity.TAB_LEVEL_MASTER
                                .Where(x => x.LEVEL_ID <= levelId && x.status == "Y")
                                .Select(x => new LevelMasterMetaData
                                {
                                    LEVEL_ID = x.LEVEL_ID,
                                    LEVEL_NAME = x.LEVEL_NAME
                                }).ToList();

            List<WorkflowMappingMasterSaveMetaData> workflowMappingMasterSaveedMetaData = GetWorkflowMappingMasterByWorkflowId(workflowId);
            List<WorkflowMappingMasterSaveMetaData> results = (from lb in LevelMasterMetaDatas
                                                               join wfm in workflowMappingMasterSaveedMetaData on lb.LEVEL_ID equals wfm.LEVEL_ID into lb_wfm
                                                               from levels in lb_wfm.DefaultIfEmpty()
                                                               select new WorkflowMappingMasterSaveMetaData
                                                               {
                                                                   LEVEL_NAME = lb.LEVEL_NAME,
                                                                   AUTO_APPROVE = levels == null ? "" : levels.AUTO_APPROVE,
                                                                   AUTO_APPROVE_DAY = levels == null ? null : levels.AUTO_APPROVE_DAY,
                                                                   AUTO_REJECT = levels == null ? null : levels.AUTO_REJECT,
                                                                   AUTO_REJECT_DAY = levels == null ? null : levels.AUTO_REJECT_DAY,
                                                                   ROLE_ID = levels == null ? new Guid() : levels.ROLE_ID,
                                                                   EMP_ID = levels == null ? new Guid() : levels.EMP_ID,
                                                                   LEVEL_ID = lb.LEVEL_ID
                                                               }).ToList();
            return results;
        }

        public List<WorkflowMappingMasterSaveMetaData> GetWorkflowMappingMasterByWorkflowId(Guid workflowId)
        {

            List<WorkflowMappingMasterSaveMetaData> result = (from wfm in _appEntity.TAB_WORKFLOW_MAPPING_MASTER

                                                              join level in _appEntity.TAB_LEVEL_MASTER on wfm.LEVEL_ID equals level.LEVEL_ID
                                                              where wfm.WORKFLOW_ID == workflowId
                                                              select new WorkflowMappingMasterSaveMetaData
                                                              {
                                                                  LEVEL_NAME = level.LEVEL_NAME,
                                                                  AUTO_APPROVE = wfm.AUTO_APPROVE,
                                                                  AUTO_APPROVE_DAY = wfm.AUTO_APPROVE_DAY,
                                                                  AUTO_REJECT = wfm.AUTO_REJECT,
                                                                  AUTO_REJECT_DAY = wfm.AUTO_REJECT_DAY,
                                                                  ROLE_ID = wfm.ROLE_ID,
                                                                  EMP_ID = wfm.USER_ID,
                                                                  LEVEL_ID = wfm.LEVEL_ID
                                                              }).ToList();
            return result;
        }
    }
}
