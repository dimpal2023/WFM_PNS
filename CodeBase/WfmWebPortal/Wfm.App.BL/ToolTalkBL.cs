using System;
using System.Collections.Generic;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Interfaces;

namespace Wfm.App.BL
{
    public class ToolTalkBL
    {
        private IBaseRepository baseRepository;

        public ToolTalkBL(IBaseRepository baseRepo)
        {
            baseRepository = baseRepo;
        }
            
        public List<ToolTalkMasterMetaData> GetAllItems(string dept_id, string sub_dept_id, string BUILDING_ID)
        {
            return baseRepository.ToolTalkRepo.GetAllItems(dept_id, sub_dept_id, BUILDING_ID);
        }

        public void Create(ToolTalkMasterMetaData toolTalk)
        {
            baseRepository.ToolTalkRepo.Create(toolTalk);
        }

        public ToolTalkMasterMetaData Find(Guid tooltalkid)
        {
            return baseRepository.ToolTalkRepo.Find(tooltalkid);
        }

        public void Update(ToolTalkMasterMetaData toolTalk)
        {
            baseRepository.ToolTalkRepo.Update(toolTalk);
        }

        public int Delete(Guid id)
        {
            return baseRepository.ToolTalkRepo.Delete(id);
        }

        public List<ToolTalkMasterMetaData> GetCheckListByDeptId(Guid deptId, Guid subDeptId)
        {
            return baseRepository.ToolTalkRepo.GetCheckListByDeptId(deptId, subDeptId);
        }

        public int AddConfiguration(ToolTalkConfigurationMetaData toolTalkConfigure)
        {
            return baseRepository.ToolTalkRepo.AddConfiguration(toolTalkConfigure);
        }

        public List<ToolTalkConfigurationMetaData> ConfiguredCheckLists()
        {
            return baseRepository.ToolTalkRepo.ConfiguredCheckLists();
        }

        public ToolTalkConfigurationMetaData FindConfiguration(Guid toolTalkId)
        {
            return baseRepository.ToolTalkRepo.FindConfiguration(toolTalkId);
        }

        public void UpdateConfiguration(ToolTalkConfigurationMetaData configuredItem)
        {
            baseRepository.ToolTalkRepo.UpdateConfiguration(configuredItem);
        }

        public void DeleteConfiguration(ToolTalkConfigurationMetaData configuredItem)
        {
            baseRepository.ToolTalkRepo.DeleteConfiguration(configuredItem);
        }

        public int CreateDailyCheckList(ToolTalkDailyCheckListMetaData toolTalkCheckList)
        {
            return baseRepository.ToolTalkRepo.CreateDailyCheckList(toolTalkCheckList);
        }

        public List<ToolTalkDailyCheckListMetaData> GetAllDailyCheckLists(Guid deptId, Guid subDeptId, Guid BUILDING_ID)
        {
            return baseRepository.ToolTalkRepo.GetAllDailyCheckLists(deptId, subDeptId, BUILDING_ID);
        }

        public void DeleteDailyCheckList(ToolTalkDailyCheckListMetaData dailyCheckListItem)
        {
            baseRepository.ToolTalkRepo.DeleteDailyCheckList(dailyCheckListItem);
        }

        public void UpdateDailyCheckList(ToolTalkDailyCheckListMetaData dailyCheckListItem)
        {
            baseRepository.ToolTalkRepo.UpdateDailyCheckList(dailyCheckListItem);
        }

        public ToolTalkDailyCheckListMetaData FindDailyCheckList(Guid dailyCheckListId)
        {
            return baseRepository.ToolTalkRepo.FindDailyCheckList(dailyCheckListId);
        }

        public List<ToolTalkMasterMetaData> GetConfiguredCheckListBySubDeptId(Guid deptId, Guid subDeptId,Guid BUILDING_ID)
        {
            return baseRepository.ToolTalkRepo.GetConfiguredCheckListBySubDeptId(deptId, subDeptId, BUILDING_ID);
        }
        
    }
}
