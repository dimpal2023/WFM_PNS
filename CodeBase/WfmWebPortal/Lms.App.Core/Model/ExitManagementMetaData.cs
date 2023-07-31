using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Wfm.App.Core.Model
{
    public class ExitManagementMetaData
    {
        [Required(ErrorMessage = "Required Unit")]
        public System.Guid BUILDING_ID { get; set; }
        [Required(ErrorMessage = "Required Department")]
        public System.Guid DEPT_ID { get; set; }
        [Required(ErrorMessage = "Required Sub Department")]
        public Guid? SUBDEPT_ID { get; set; }
        [Required(ErrorMessage = "Required Employee")]
        public Guid WF_ID { get; set; }
        public string EMP_NAME { get; set; }        
        public WorkforceMetaData WorkforceMetaData { get; set; }

        [Required(ErrorMessage = "Required")]
        public string IS_NOTICE_SERVE { get; set; }
        [Required(ErrorMessage = "Required")]
        public byte NOTICE_DAYS { get; set; }
        [Required(ErrorMessage = "Required")]
        public string REASON_OF_LEAVING { get; set; }
        [Required(ErrorMessage = "Required")]
        public System.DateTime? RESIGNATION_DATE { get; set; }
        public System.DateTime? EXIT_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public List<AssetMappingMetaData> AssetMappingMetaDatas { get; set; }
        public List<WorkforceExitApprover> WorkforceExitApprovers { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }
        public IEnumerable<SelectListItem> Employees { get; set; }
        public string MAIL_ID { get; set; }
        public string Con_MSG { get; set; }
        
        
    }

    public class TransferManagementMetaData
    {
        [Required(ErrorMessage = "Required")]
        public Guid BUILDING_ID { get; set; }
        public Guid New_BUILDING_ID { get; set; }
        [Required(ErrorMessage = "Required")]
        public System.Guid DEPT_ID { get; set; }
        public System.Guid New_DEPT_ID { get; set; }
        [Required(ErrorMessage = "Required")]
        public Guid? SUBDEPT_ID { get; set; }
        public Guid? New_SUBDEPT_ID { get; set; }
        public Guid WF_ID { get; set; }
        public string EMP_NAME { get; set; }
        public short WF_EMP_TYPE { get; set; }
        [Required(ErrorMessage = "Required")]
        public short EMPLOYMENT_TYPE { get; set; }
        public short New_EMPLOYMENT_TYPE { get; set; }
        public string CREATED_BY { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
       
    }

    public class AssetMappingMetaData
    {
        public System.Guid ASSET_ID { get; set; }
        public System.Guid ASSET_ALLOCATION_ID { get; set; }
        public string ASSET_NAME { get; set; }
        public Nullable<int> ASSET_LIFE { get; set; }
        public string REFUNDABLE { get; set; }
        public bool IS_REFUNDABLE { get; set; }
        public string IS_REFOUND { get; set; }
        public string IS_ACTIVE { get; set; }
        public int QUANTITY { get; set; }
        public System.DateTime? ALLOCATION_DATE { get; set; }
    }

    public class WorkforceExitApprover
    {
        public string EMP_ID { get; set; }
        public Nullable<System.Guid> APPROVE_BY { get; set; }
        public string APPROVER_NAME { get; set; }
        public Nullable<System.DateTime> APPROVE_DATE { get; set; }
        public string APPROVER_STATUS { get; set; }
    }
    public class ExitApprovalMetaData: WorkforceExitApprover
    {
        public Guid EXIT_APPROVER_ID { get; set; }
        public string DEPT_NAME { get; set; }
        public Guid WF_ID { get; set; }
        public string WORKFORCE_NAME { get; set; }
        public WorkforceMetaData WorkforceMetaData { get; set; }

        [Required(ErrorMessage = "Required")]
        public string IS_NOTICE_SERVE { get; set; }
        public byte NOTICE_DAYS { get; set; }
        public string REASON_OF_LEAVING { get; set; }
        public System.DateTime? RESIGNATION_DATE { get; set; }
        public System.DateTime? EXIT_DATE { get; set; }
        public string REMARK { get; set; }
        public Guid USER_ID { get; set; }
    }
}
