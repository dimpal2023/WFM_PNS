using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Wfm.App.Core.Model
{
    public class AssetMappingMasterMetaData
    {
        public System.Guid ASSET_ID { get; set; }
        public System.Guid COMPANY_ID { get; set; }
        public string COMPANY_NAME { get; set; }
        public System.Guid? BUILDING_ID { get; set; }
        public System.Guid? DEPARTMENT_ID { get; set; }
        public System.Guid? SUBDEPT_ID { get; set; }
        public string ASSET_NAME { get; set; }
        public Nullable<int> ASSET_LIFE { get; set; }
        public string REFUNDABLE { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public string IS_ACTIVE { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }
        public IEnumerable<SelectListItem> Buildings { get; set; }

    }
}
