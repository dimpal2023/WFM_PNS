using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wfm.App.Core.Model
{
    public class ToolTalkMasterMetaData
    {
        public System.Guid ID { get; set; }
        public System.Guid DEPT_ID { get; set; }
        public string ITEM_NAME { get; set; }
        public string DEPT_NAME { get; set; }
        public System.Guid SUBDEPT_ID { get; set; }
        public string SUBDEPT_NAME { get; set; }
        public Guid BUILDING_ID { get; set; }
    }

    public class ToolTalkCheckListMetaData
    {
        public System.Guid DEPT_ID { get; set; }
        public System.Guid SHIFT_ID { get; set; }
        public Guid BUILDING_ID { get; set; }
        public List<System.Guid> ID { get; set; }
        public System.Guid SUBDEPT_ID { get; set; }
    }
}
