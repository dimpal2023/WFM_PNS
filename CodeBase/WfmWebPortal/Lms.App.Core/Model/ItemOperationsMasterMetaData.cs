using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Wfm.App.Core.Model
{
    public class ItemOperationsMasterMetaData
    {

        public System.Guid UNIQUE_OPERATION_ID { get; set; }
        public System.Guid COMPANY_ID { get; set; }
        public System.Guid ITEM_ID { get; set; }
        public double? RATE { get; set; }
        public string OPERATION { get; set; }
        public string BUILDING_NAME { get; set; }
        public double? PERCENTAGE { get; set; }
        public DateTime RATE_APPLIED_DATE { get; set; }

        public IEnumerable<SelectListItem> Departments { get; set; }
        public IEnumerable<SelectListItem> Buildings { get; set; }
        public System.Guid BUILDING_ID { get; set; }
        public System.Guid SUBDEPT_ID { get; set; }
        public System.Guid DEPT_ID { get; set; }
        public System.Guid ITEM_CODE_ID { get; set; }
        public string ITEM_NAME { get; set; }
        public string ITEM_CODE_NAME { get; set; }
        public string DEPT_NAME { get; set; }
        public string SUBDEPT_NAME { get; set; }
        public string Status { get; set; }
    }
}
