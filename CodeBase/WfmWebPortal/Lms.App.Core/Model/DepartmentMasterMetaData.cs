using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Wfm.App.Core.Model
{
    public class DepartmentMasterMetaData
    {
        public System.Guid DEPT_ID { get; set; }
        //[Remote("IsDepartmentNameAvailable", "Master", ErrorMessage = "Department Already Exist", AdditionalFields = "DEPT_ID")]
        public string DEPT_NAME { get; set; }
        public System.Guid COMPANY_ID { get; set; }
        public string COMPANY_NAME { get; set; }
        public string DEPT_HEAD_ID { get; set; }
        public string DEPT_HEAD_NAME { get; set; }
        public System.DateTime created_date { get; set; }
        public string Created_by { get; set; }
        public Nullable<System.DateTime> UPDATED_DATE { get; set; }
        public string UPDATED_BY { get; set; }
        public string status { get; set; }
        //public string BUILDING_IDS { get; set; }
        public System.Guid BUILDING_ID { get; set; }

        public IEnumerable<SelectListItem> DepartmentHeads { get; set; }
        public IEnumerable<SelectListItem> Buildings { get; set; }
    }
}
