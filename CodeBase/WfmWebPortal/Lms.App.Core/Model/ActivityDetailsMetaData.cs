using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wfm.App.Core.Model
{
    public class ActivityDetailsMetaData
    {
        public Nullable<System.Guid> USER_ID { get; set; }
        public string USER_OPERATION { get; set; }
        public string PAGE_NAME { get; set; }
        public string REMARK { get; set; }
        public System.DateTime created_date { get; set; }
    }
}
