using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Wfm.App.Core.Model
{
    public class AssetAllocationMetaData
    {
        public System.Guid ASSET_ALLOCATION_ID { get; set; }
        public System.Guid COMPANY_ID { get; set; }
        public System.Guid DEPT_ID { get; set; }
        public string EMP_ID { get; set; }
        public System.Guid ASSET_ID { get; set; }
        public int QUANTITY { get; set; }
        public string PURPOSE { get; set; }
        public string IS_ACTIVE { get; set; }
        public System.DateTime ASSET_ASSIGN_DATE { get; set; }
        public string ASSET_ASSIGN_BY { get; set; }
        public Nullable<System.DateTime> ASSET_HANDOVER_DATE { get; set; }
        public Nullable<System.DateTime> ASSET_CLOSE_DATE { get; set; }
        public string ASSET_CLOSE_BY { get; set; }
    }

    public class AssetAllocationMetaDataForm
    {
       [Required(ErrorMessage = "Required Department")]
        public System.Guid DEPT_ID { get; set; }
        public System.Guid BUILDING_ID { get; set; }
        public System.Guid SUBDEPT_ID { get; set; }
        [Required(ErrorMessage = "Required Employee")]
        public Guid WF_ID { get; set; }
        [Required(ErrorMessage = "Required Employee")]
        public string EMP_NAME { get; set; }
        [Required(ErrorMessage = "Required Employee Type")]
        public short WF_EMP_TYPE { get; set; }

        public IEnumerable<SelectListItem> Departments { get; set; }
        public IEnumerable<SelectListItem> Buildings { get; set; }
        public IEnumerable<SelectListItem> Employees { get; set; }
        public List<_AddAsset> ListMetaDatas { get; set; }
        public IEnumerable<SelectListItem> Assets { get; set; }
        public string ASSIGN_BY { get; set; }
        public Guid COMPANY_ID { get; set; }
    }

    public class _AddAsset
    {
       [Required(ErrorMessage = "Required Asset")]
        public System.Guid ASSET_ID { get; set; }
       [Required(ErrorMessage = "Required Type")]
        public string ASSET_TYPE { get; set; }
       [Required(ErrorMessage = "Required Purpose")]
        public string PURPOSE { get; set; }
       [Required(ErrorMessage = "Required Quantity")]
        public int? QUANTITY { get; set; }

    }
}
