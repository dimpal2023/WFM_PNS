using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wfm.App.Core.Model
{
    public class AssetMasterMetaData
    {
        public System.Guid ASSET_ID { get; set; }
        public System.Guid COMPANY_ID { get; set; }
        public System.Guid DEPARTMENT_ID { get; set; }
        public string ASSET_NAME { get; set; }
        public Nullable<int> ASSET_LIFE { get; set; }
        public string REFUNDABLE { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public string IS_ACTIVE { get; set; }
    }
}
