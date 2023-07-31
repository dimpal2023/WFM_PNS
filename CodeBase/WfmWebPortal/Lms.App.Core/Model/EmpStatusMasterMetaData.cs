using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wfm.App.Core.Model
{
    public class EmpStatusMasterMetaData
    {
        public short EMP_STATUS_ID { get; set; }
        public string EMP_STATUS { get; set; }
        public System.Guid COMPANY_ID { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string STATUS { get; set; }
    }
}
