using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wfm.App.Core.Model
{
    public class MRFApprovalMetaData
    {
        public System.Guid MRF_APPROVER_ID { get; set; }
        public System.Guid MRP_INETRNAL_ID { get; set; }
        public Nullable<System.Guid> APPROVE_BY { get; set; }
        public string APPROVAL_NAME { get; set; }
        public Nullable<System.DateTime> APPROVE_DATE { get; set; }
        public string APPROVER_REMARK { get; set; }
        public string APPROVER_STATUS { get; set; }
    }
}
