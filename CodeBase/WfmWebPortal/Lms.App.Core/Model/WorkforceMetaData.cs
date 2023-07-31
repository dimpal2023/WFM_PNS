using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web;
using Wfm.App.Core.Attribute;

namespace Wfm.App.Core.Model
{
    public enum WF_EMP_TYPE:short
    {
        OnRoll=1,
        Contract
    }
    public class WorkforceMetaData : WorkforceCreateDDL
    {
        public System.Guid WF_ID { get; set; }

        [Display(Name = "Employee ID")]
        //[Required(ErrorMessage = "Employee ID Required")]
        [Remote("IS_EMPID_EXIST", "Workforce", ErrorMessage = "Employee ID Already Available", AdditionalFields = "AADHAR_NO")]
        public string EMP_ID { get; set; }
        //[Display(Name = "Employee ID")]
        //[Required(ErrorMessage = "Employee ID Required")]
       
        public short WF_EMP_TYPE { get; set; }

        public string WF_EMP_TEXT { get; set; }
        [Display(Name = "Company")]
        [Required(ErrorMessage = "Company Required")]
        public System.Guid COMPANY_ID { get; set; }

        [Display(Name = "Agency")]
        //[Required(ErrorMessage = "Required")]
        public System.Guid AGENCY_ID { get; set; }

        [Display(Name = "Bio Metric")]
        [Required(ErrorMessage = "Bio Metric Required")]
        [Remote("IsBiometricAvailable", "Workforce", ErrorMessage = "Biometric Already Available", AdditionalFields = "AADHAR_NO")]
        public string BIOMETRIC_CODE { get; set; }
        
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Full Name Required")]
        public string EMP_NAME { get; set; }

        [Display(Name = "Father Name")]
        [Required(ErrorMessage = "Father Name Required")]
        public string FATHER_NAME { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Gender Required")]
        public string GENDER { get; set; }

        [Display(Name = "Department")]
        [Required(ErrorMessage = "Department Required")]
        public System.Guid DEPT_ID { get; set; }

        [Display(Name = "Sub Department")]
        [Required(ErrorMessage = "Sub Department Required")]
        public System.Guid? SUBDEPT_ID { get; set; }

        [Display(Name = "Unit")]
        [Required(ErrorMessage = "Unit Required")]
        public System.Guid BUILDING_ID { get; set; }



        [Display(Name = "Date of Birth"),Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime? DATE_OF_BIRTH { get; set; }

        [Display(Name = "Natonality")]
        [Required(ErrorMessage = "Nationality Required")]
        public string NATIONALITY { get; set; }

        [Display(Name = "Education")]
        [Required(ErrorMessage = "Education Required")]
        public short WF_EDUCATION_ID { get; set; }

        [Display(Name = "Martial Status")]
        [Required(ErrorMessage = "Martial Status Required")]
        public short MARITAL_ID { get; set; }

        
        [Display(Name = "Date of Joining"),Required]
        public System.DateTime? DOJ { get; set; }

        
        [Display(Name = "EPF Date of Joining"),Required]
        public System.DateTime? DOJ_AS_PER_EPF { get; set; }

        [Display(Name = "Designation")]
        [Required(ErrorMessage = "Designation Required")]
        public short WF_DESIGNATION_ID { get; set; }

        [Display(Name = "Skill")]
        [Required(ErrorMessage = "Skill Required")]
        public System.Guid SKILL_ID { get; set; }
        [Display(Name = "Employee Type")]
        [Required(ErrorMessage = "Employee Type Required")]
        public short EMP_TYPE_ID { get; set; }

        //[Display(Name = "Training/ Probation Period")]
        //[Required(ErrorMessage = "Training/ Probation Period Required")]
       
        //public short TimePeriod { get; set; }

        [Display(Name = "Mobile No.")]
        [Required(ErrorMessage = "Mobile No. Required")]
        public string MOBILE_NO { get; set; }
        [Display(Name = "Alt. Mobile No.")]
        //[Required(ErrorMessage = "Required")]
        public string ALTERNATE_NO { get; set; }
        [Display(Name = "Email")]
        //[Required(ErrorMessage = "Required")]
        public string EMAIL_ID { get; set; }
        [Display(Name = "Present Address")]
        [Required(ErrorMessage = "Required")]
        public string PRESENT_ADDRESS { get; set; }

        [Display(Name = "Present State")]
        [Required(ErrorMessage = "Present State Required")]
        public System.Guid? PRESENT_ADDRESS_STATE { get; set; }

        [Display(Name = "Present City")]
        [Required(ErrorMessage = "Present City Required")]
        public System.Guid PRESENT_ADDRESS_CITY { get; set; }

        [Display(Name = "Present Pin"),Required]
        //[Required(ErrorMessage = "Required")]
        public int? PRESENT_ADDRESS_PIN { get; set; }

        [Display(Name = "Permanent Address")]
        [Required(ErrorMessage = "Required")]
        public string PERMANENT_ADDRESS { get; set; }

        [Display(Name = "Present State")]
        [Required(ErrorMessage = "Required")]
        public System.Guid PERMANENT_ADDRESS_STATE { get; set; }

        [Display(Name = "Present City")]
        [Required(ErrorMessage = "Required")]
        public System.Guid PERMANENT_ADDRESS_CITY { get; set; }
        [Display(Name = "Present Pin")]
        [Required(ErrorMessage = "Required")]
        public int? PERMANENT_ADDRESS_PIN { get; set; }
        [Display(Name = "Aadhar No.")]
        [Range(100000000000, 999999999999, ErrorMessage = "Aadhar No must be length of 12")]

        [Required(ErrorMessage = "Required Aadhar")]
        public Int64? AADHAR_NO { get; set; }
        [Display(Name = "Employee Status")]
        [Required(ErrorMessage = "Required")]
        public short EMP_STATUS_ID { get; set; }
        [Display(Name = "Identification Mark")]
        public string IDENTIFICATION_MARK { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        [Required(ErrorMessage = "Required MRF")]
        public System.Guid MRF_INTERNAL_ID { get; set; }
        public string REFERENCE_NAME { get; set; }
        [Display(Name = "Training/ Probation Period")]
        public string REFERENCE_ID { get; set; }
        public string MRF_ID { get; set; }
        public byte[] PHOTO { get; set; }
        public string PHOTOBase64 { get; set; }
        public byte[] EMP_SIGNATURE { get; set; }
        public string EMP_SIGNATUREBase64 { get; set; }

        [FileSize(80000000)]
        [Required(ErrorMessage = "Select Employee Photo file.")]
        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.gif|.jpeg|.PNG|.JPG)$", ErrorMessage = "Only Image files allowed.")]
        public HttpPostedFileBase PHOTOfile { get; set; }
        [Required(ErrorMessage = "Select Employee Signature file.")]
        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.gif|.jpeg|.PNG|.JPG)$", ErrorMessage = "Only Image files allowed.")]
        [FileSize(80000000)]
        public HttpPostedFileBase SIGNATUREfile { get; set; }
    }
    public class WorkforcePhotoMetaData
    {
        public System.Guid WF_ID { get; set; }
        public byte[] PHOTO { get; set; }
        public byte[] EMP_SIGNATURE { get; set; }
    }

    public class WorkforceCreateDDL
    {
        public IEnumerable<SelectListItem> EmployeeTypeList { get; set; }
        public IEnumerable<SelectListItem> CompanyList { get; set; }
        public IEnumerable<SelectListItem> AgencyList { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> NationalityList { get; set; }
        public IEnumerable<SelectListItem> EducationList { get; set; }
        public IEnumerable<SelectListItem> MartialStatusList { get; set; }
        public IEnumerable<SelectListItem> DesignationList { get; set; }
        public IEnumerable<SelectListItem> SkillList { get; set; }
        public IEnumerable<SelectListItem> EmpTypeList { get; set; }
        public IEnumerable<SelectListItem> StateList { get; set; }
        public IEnumerable<SelectListItem> CityList { get; set; }
        public IEnumerable<SelectListItem> EmployeeStatusList { get; set; }
        public IEnumerable<SelectListItem> Buildings { get; set; }
        public IEnumerable<SelectListItem> MRFList { get; set; }
        public IEnumerable<SelectListItem> WF_EMP_TYPEList { get; set; }
    }

    
    public class WorkforceMetaDataList
    {
        public System.Guid WF_ID { get; set; }
        public string EMP_ID { get; set; }
        public string WFEMPTYPE { get; set; }
        public string EMP_Status { get; set; }
        public string UNIT_NAME { get; set; }
        public string DEPT_NAME { get; set; }
        public string SUBDEPT_NAME { get; set; }
        public string SKILL_NAME { get; set; }
        public string Education { get; set; }
        public string ETYPE { get; set; }
        public short WF_EMP_TYPE { get; set; }
        public System.Guid COMPANY_ID { get; set; }
        public System.Guid AGENCY_ID { get; set; }
        public string BIOMETRIC_CODE { get; set; }
        public string EMP_NAME { get; set; }
        public string FATHER_NAME { get; set; }
        public string GENDER { get; set; }
        public System.Guid DEPT_ID { get; set; }
        public string DEPT { get; set; }
        public System.Guid SUBDEPT_ID { get; set; }
        public string SUBDEPT { get; set; }
        public System.DateTime? DATE_OF_BIRTH { get; set; }
        public string NATIONALITY { get; set; }
        public short WF_EDUCATION_ID { get; set; }
        public short MARITAL_ID { get; set; }
        public System.DateTime? DOJ { get; set; }
        public System.DateTime? DOJ_AS_PER_EPF { get; set; }
        public short WF_DESIGNATION_ID { get; set; }
        public string WF_DESIGNATION { get; set; }
        public System.Guid SKILL_ID { get; set; }
        public short EMP_TYPE_ID { get; set; }
        public short TimePeriod { get; set; }
        public string MOBILE_NO { get; set; }
        public string ALTERNATE_NO { get; set; }
        public string EMAIL_ID { get; set; }
        public string PRESENT_ADDRESS { get; set; }
        public System.Guid PRESENT_ADDRESS_STATE { get; set; }
        public System.Guid PRESENT_ADDRESS_CITY { get; set; }
        public int? PRESENT_ADDRESS_PIN { get; set; }
        public string PERMANENT_ADDRESS { get; set; }
        public System.Guid PERMANENT_ADDRESS_STATE { get; set; }
        public System.Guid PERMANENT_ADDRESS_CITY { get; set; }
        public int? PERMANENT_ADDRESS_PIN { get; set; }
        public Int64? AADHAR_NO { get; set; }
        public short EMP_STATUS_ID { get; set; }
        public string IDENTIFICATION_MARK { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public System.Guid? MRF_INTERNAL_ID { get; set; }
        public string REFERENCE_NAME { get; set; }
        public string REFERENCE_ID { get; set; }
        public string MRF_ID { get; set; }
        public byte[] PHOTO { get; set; }
        public byte[] EMP_SIGNATURE { get; set; }

        public string HRA { get; set; }
        public string BASIC_DA { get; set; }
        public string SPECIAL_ALLOWANCE { get; set; }
        public string GROSS { get; set; }
        public string STATUS { get; set; }
        public string UAN { get; set; }
        public string PAN { get; set; }
        public string EPF { get; set; }
        public string ESIC { get; set; }
        public string ACCNO { get; set; }
        public string IFSC { get; set; }
        public string BranchName { get; set; }

    }
}
