using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Wfm.App.Core.Model
{
    public class WorkflowMappingMasterMetaData
    {
        [Required(ErrorMessage = "Required")]
        public System.Guid WORKFLOW_ID { get; set; }
        [Required(ErrorMessage = "Required")]
        public Nullable<int> LEVEL_ID { get; set; }

        public System.DateTime created_date { get; set; }
        public string Created_by { get; set; }
        public Nullable<System.DateTime> UPDATED_DATE { get; set; }
        public string UPDATED_BY { get; set; }
        public string status { get; set; }
        public IEnumerable<SelectListItem> WorkFlowMaster { get; set; }
        public IEnumerable<SelectListItem> LevelOfApproval { get; set; }
        public List<WorkflowMappingMasterSaveMetaData> ListMetaDatas { get; set; }
        public IEnumerable<RoleMasterMetaData> Roles { get; set; }
        public IEnumerable<ListItem> IsAutoApprovalOrRejects { get; set; }
        public IEnumerable<ListItem> ApprovalOrRejectDays { get; set; }
    }

    public class WorkflowMappingMasterSaveMetaData
    {
        public Nullable<int> LEVEL_ID { get; set; }
        public string LEVEL_NAME { get; set; }
        [Required(ErrorMessage = "Required")]
        public Nullable<System.Guid> ROLE_ID { get; set; }
        [Required(ErrorMessage = "Required")]
        public Guid EMP_ID { get; set; }
        [Required(ErrorMessage = "Required")]
        public string AUTO_APPROVE { get; set; }
        public Nullable<int> AUTO_APPROVE_DAY { get; set; }
        [Required(ErrorMessage = "Required")]
        public string AUTO_REJECT { get; set; }
        public Nullable<int> AUTO_REJECT_DAY { get; set; }
    }

    public class WorkflowMappingMasterVieweMetaData
    {
        public System.Guid WORKFLOW_ID { get; set; }
        public string WORKFLOW_NAME { get; set; }

        public List<WorkflowMappingMasterVieweListMetaData> listMetaDatas { get; set; }
    }

    public class WorkflowMappingMasterVieweListMetaData
    {
        public Nullable<System.Guid> DEPT_ID { get; set; }
        public string ROLE_NAME { get; set; }
        public Nullable<int> LEVEL_ID { get; set; }
        public string LEVEL_NAME { get; set; }
        public Nullable<System.Guid> EMP_ID { get; set; }
        public string APPROVAL_NAME { get; set; }
        public string AUTO_APPROVE { get; set; }
        public Nullable<int> AUTO_APPROVE_DAY { get; set; }
        public string AUTO_REJECT { get; set; }
        public Nullable<int> AUTO_REJECT_DAY { get; set; }

        public string Status { get; set; }

    }
    public class WorkflowMasterVieweMetaData
    {
        public IEnumerable<SelectListItem> Departments { get; set; }
        public IEnumerable<SelectListItem> SubDepartments { get; set; }
        public IEnumerable<SelectListItem> OnRollOrContracts { get; set; }
        public IEnumerable<SelectListItem> Buildings { get; set; }
        public Guid DEPARTMENT_ID { get; set; }
        public Guid SUBDEPT_ID { get; set; }
        public Guid BUILDING_ID { get; set; }
        public short WF_EMP_TYPE { get; set; }
    }

    public class PartialWorkflowMasterVieweMetaData
    {
        public System.Guid WF_ID { get; set; }
        public bool IsWF_ID { get; set; }
        public string COMPANY_NAME { get; set; }
        public string BIOMETRIC_CODE { get; set; }
        public string EMP_NAME { get; set; }
        public string EMP_ID { get; set; }
        public string DEPT_NAME { get; set; }
        public string DESIGNATION_NAME { get; set; }
        public string MOBILE_NO { get; set; }
    }
    public class GenerateCardWorkflowMasterVieweMetaData : PartialWorkflowMasterVieweMetaData
    {
        public string ADDRESS1 { get; set; }
        public string ADDRESS2 { get; set; }
        public string AGENCY_ADDRESS1 { get; set; }
        public string AGENCY_ADDRESS2 { get; set; }
        public string AGENCY_NAME { get; set; }
        public string EMP_LOC_ADDR { get; set; }
        public string EMP_PERM_ADDR { get; set; }
        public string EMP_EMG_MOB { get; set; }
        public DateTime? DOJ { get; set; }
        public byte[] PHOTO { get; set; }
        public short WF_EMP_TYPE { get; set; }
    }
    public class GenerateCardViewModel
    {
        public string EmployeeCard { get; set; }
    }
}
