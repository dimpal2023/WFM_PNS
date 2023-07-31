using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wfm.App.Core.Model
{
    public class CompanyMasterMetaData
    {
        public System.Guid COMPANY_ID { get; set; }
        public string COMPANY_NAME { get; set; }
        public System.DateTime Created_date { get; set; }
        public string Created_by { get; set; }
        public string Status { get; set; }
    }
}
