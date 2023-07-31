using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Wfm.App.Core.Model
{
    public class ManPowerRequestFormMetaData : ManPowerRequiremnetDDL
    {
        public System.Guid MRP_INETRNAL_ID { get; set; }
        public string MRF_ID { get; set; }

        [Required(ErrorMessage = "Required")]
        public short REC_TYPE { get; set; }
        public System.Guid COMPANY_ID { get; set; }

        [Required(ErrorMessage = "Required")]
        public System.Guid WORKFLOW_ID { get; set; }
        [Required(ErrorMessage = "Required")]
        public System.Guid BUILDING_ID { get; set; }
        //[Required(ErrorMessage = "Required")]
        //public System.Guid FLOOR_ID { get; set; }
        [Required(ErrorMessage = "Required")]
        public System.Guid DEPT_ID { get; set; }
        [Required(ErrorMessage = "Required")]
        public System.Guid SUBDEPT_ID { get; set; }

        [Required(ErrorMessage = "Required")]
        public System.Guid SKILL_ID { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(1, int.MaxValue)]
        public short WF_DESIGNATION_ID { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Quantity")]
        public short? QUANTITY { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(1, int.MaxValue)]
        public short WF_EMP_TYPE { get; set; }
        [Required(ErrorMessage = "Required")]
        [MaxLength(500, ErrorMessage = "Max lenght 500 character!")]
        public string REMARK { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }
        public string MRF_STATUS { get; set; }
        public Nullable<System.DateTime> FINAL_ACTION_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public string ReplaceType { get; set; }
        public int MRF_CODE  { get; set; }
    }

    public class ManPowerRequiremnetDDL
    {
        public IEnumerable<SelectListItem> Skills { get; set; }
        public IEnumerable<SelectListItem> Designations { get; set; }
        public IEnumerable<SelectListItem> Buildings { get; set; }
        public IEnumerable<SelectListItem> EmpTypes { get; set; }
        public IEnumerable<RECMasterMetaData> MPRHirings { get; set; }

    }

    public class ManPowerRequestFormMetaDataList
    {
        public System.Guid MRP_INETRNAL_ID { get; set; }
        public string BUILDING_NAME { get; set; }
        public string SKILL_NAME { get; set; }
        public string WF_DESIGNATION_NAME { get; set; }
        public string REC_NAME { get; set; }
        public int MRF_CODE { get; set; }
        public string FLOOR_NAME { get; set; }
        public string SUBDEPT_NAME { get; set; }
        public string EMP_TYPE { get; set; }
        public short? QUANTITY { get; set; }
        public int HIRING_QUANTITY { get; set; }
        public string MRF_STATUS { get; set; }
        public string MRF_ID{ get; set; }
        public Guid COMPANY_ID { get; set; }
        public Guid BUILDING_ID { get; set; }
        //public Guid FLOOR_ID { get; set; }
        public Guid DEPT_ID { get; set; }
        public Guid SUBDEPT_ID { get; set; }
        public Guid SKILL_ID { get; set; }
        public short WF_DESIGNATION_ID { get; set; }
        public short EMP_TYPE_ID { get; set; }
        public short WF_EMP_TYPE { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
       
    }
    public class MRFApprovalMetadata : ManPowerRequestFormMetaDataList
    {
        public Guid MRF_APPROVER_ID { get; set; }
        public Guid USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public DateTime? APPROVE_DATE { get; set; }
        public string APPROVER_STATUS { get; set; }
        public DateTime? MRF_Date { get; set; }
    }
    public class MRFDLL
    {
        public Guid MRF_ID { get; set; }
        public string MRF_CODE { get; set; }

    }
}
