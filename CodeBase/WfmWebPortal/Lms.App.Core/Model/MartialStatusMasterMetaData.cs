using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wfm.App.Core.Model
{
    public class MartialStatusMasterMetaData
    {
        public short MARITAL_ID { get; set; }
        public string MARITAL_NAME { get; set; }
        public System.Guid COMPANY_ID { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string STATUS { get; set; }
    }
}
