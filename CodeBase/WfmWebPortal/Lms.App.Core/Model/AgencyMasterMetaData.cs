using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wfm.App.Core.Model
{
    public class AgencyMasterMetaData
    {
        public System.Guid AGENCY_ID { get; set; }
        public string AGENCY_NAME { get; set; }
        public System.Guid COMPANY_ID { get; set; }
        public string AGENCY_ADDRESS { get; set; }
        public string AUTTHENTICATED_PERSON_NAME { get; set; }
        public string AUTHENTICATED_PERSON_NUM { get; set; }
        public System.DateTime created_date { get; set; }
        public string Created_by { get; set; }
        public string status { get; set; }
    }

}
