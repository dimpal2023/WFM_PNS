using System;
using System.Collections.Generic;
using Wfm.App.Core.Model;

namespace Wfm.App.Infrastructure.Interfaces
{
    public interface IToolTalkRepository
    {
        List<ToolTalkMasterMetaData> GetAllItems(string dept_id, string sub_dept_id, string BUILDING_ID);
        void Create(ToolTalkMasterMetaData toolTalk);
        ToolTalkMasterMetaData Find(Guid toolTalkId);
        void Update(ToolTalkMasterMetaData toolTalk);
        int Delete(Guid id);
        List<ToolTalkConfigurationMetaData> ConfiguredCheckLists();
        ToolTalkConfigurationMetaData FindConfiguration(Guid toolTalkId);
        void DeleteConfiguration(ToolTalkConfigurationMetaData configuredItem);
        void UpdateConfiguration(ToolTalkConfigurationMetaData configuredItem);
    }
}
