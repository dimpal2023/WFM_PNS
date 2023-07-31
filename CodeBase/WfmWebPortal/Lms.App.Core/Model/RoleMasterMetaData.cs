using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wfm.App.Core.Model
{
    public class RoleMasterMetaData
    {
        public System.Guid ROLE_ID { get; set; }
        public string ROLE_NAME { get; set; }
        public System.Guid COMPANY_ID { get; set; }
        public System.DateTime created_date { get; set; }
        public string Created_by { get; set; }
        public Nullable<System.DateTime> UPDATED_DATE { get; set; }
        public string UPDATED_BY { get; set; }
        public string IS_EDITABLE { get; set; }
        public string status { get; set; }
    }
}
