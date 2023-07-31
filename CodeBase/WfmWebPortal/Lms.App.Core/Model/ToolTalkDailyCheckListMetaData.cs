using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wfm.App.Core.Model
{
    public partial class ToolTalkDailyCheckListMetaData
    {
        public System.Guid ID { get; set; }
        public System.Guid TOOL_TALK_ID { get; set; }
        public string TOOL_TALK_NAME { get; set; }
        public System.Guid WF_ID { get; set; }
        public string EMP_NAME { get; set; }     
        public short WF_EMP_TYPE { get; set; }
        public System.Guid DEPT_ID { get; set; }
        public System.Guid BUILDING_ID { get; set; }
        public string DEPT_NAME { get; set; }
        public System.Guid SUBDEPT_ID { get; set; }
        public string SUBDEPT_NAME { get; set; }
        public System.DateTime DATE { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> UPDATED_DATE { get; set; }
        public string UPDATED_BY { get; set; }
        public List<ToolTalkCheckList> TOOL_TALK_CHECK_LIST { get; set; }
        public System.Guid SHIFT_ID { get; set; }
        public int SHIFT_AUTOID { get; set; }
        public string SHIFT_NAME { get; set; }
        public string DELIVERED_BY { get; set; }
    }
}
