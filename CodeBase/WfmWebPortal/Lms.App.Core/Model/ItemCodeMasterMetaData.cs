using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Wfm.App.Core.Model
{
    public class ItemCodeMasterMetaData
    {
        public System.Guid ITEM_CODE_ID { get; set; }
        public string ITEM_CODE_NAME { get; set; }
        public System.Guid COMPANY_ID { get; set; }
        public System.Guid SUBDEPT_ID { get; set; }
        public string SUBDEPT_NAME { get; set; }
        public string DEPT_NAME { get; set; }
        public System.Guid DEPT_ID { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> UPDATED_DATE { get; set; }
        public string UPDATED_BY { get; set; }
    }
}
