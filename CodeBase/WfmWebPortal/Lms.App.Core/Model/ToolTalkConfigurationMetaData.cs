using System.Collections.Generic;

namespace Wfm.App.Core.Model
{
    public partial class ToolTalkConfigurationMetaData
    {
        public System.Guid ID { get; set; }
        public System.Guid DEPT_ID { get; set; }
        public string DEPT_NAME { get; set; }
        public System.Guid SUBDEPT_ID { get; set; }
        public string SUBDEPT_NAME { get; set; }
        public System.Guid SHIFT_ID { get; set; }
        public string SHIFT_NAME { get; set; }
        public List<ToolTalkCheckList> TOOL_TALK_CHECK_LIST { get; set; }       
    }

    public class ToolTalkCheckList
    {
        public System.Guid ID { get; set; }
        public System.Guid TOOL_TALK_ID { get; set; }
        public System.Guid CONFIG_DAILY_ID { get; set; }
        public string ITEM_NAME { get; set; }
        public bool? CHECK { get; set; }
    }
}
