using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wfm.App.Core.Model
{
    public class EmplTypeMasterMetaData
    {
        public short EMP_TYPE_ID { get; set; }
        public string EMP_TYPE { get; set; }
        public System.Guid COMPANY_ID { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string STATUS { get; set; }
    }
}
