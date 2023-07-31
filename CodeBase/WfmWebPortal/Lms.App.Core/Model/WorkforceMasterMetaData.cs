using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Wfm.App.Core.Model;
using System.Collections.Generic;

namespace Wfm.App.Core.Model
{
    public class WorkforceMasterMetaData
    {
        public System.Guid WF_ID { get; set; }
        [Required]
        [DisplayName("Employee ID")]
        public string EMP_ID { get; set; }

        [Required]
        [DisplayName("Employee Type")]
        public short WF_EMP_TYPE { get; set; }
        public System.Guid COMPANY_ID { get; set; }
        public Nullable<System.Guid> AGENCY_ID { get; set; }
        public string BIOMETRIC_CODE { get; set; }
        public string EMP_NAME { get; set; }
        public string FATHER_NAME { get; set; }
        public string GENDER { get; set; }
        public Nullable<System.Guid> DEPT_ID { get; set; }
        public Nullable<System.Guid> SUBDEPT_ID { get; set; }
        public System.Guid BUILDING_ID { get; set; }
        public Nullable<System.DateTime> DATE_OF_BIRTH { get; set; }
        public string NATIONALITY { get; set; }
        public Nullable<short> WF_EDUCATION_ID { get; set; }
        public Nullable<short> MARITAL_ID { get; set; }
        public Nullable<System.DateTime> DOJ { get; set; }
        public Nullable<System.DateTime> DOJ_AS_PER_EPF { get; set; }
        public Nullable<short> WF_DESIGNATION_ID { get; set; }
        public string WF_DESIGNATION_NAME { get; set; }
        public Nullable<System.Guid> SKILL_ID { get; set; }
        public string SKILL_NAME { get; set; }
        public Nullable<short> EMP_TYPE_ID { get; set; }
        public string MOBILE_NO { get; set; }
        public string ALTERNATE_NO { get; set; }
        public string EMAIL_ID { get; set; }
        public string PRESENT_ADDRESS { get; set; }
        public Nullable<System.Guid> PRESENT_ADDRESS_CITY { get; set; }
        public Nullable<System.Guid> PRESENT_ADDRESS_STATE { get; set; }
        public Nullable<int> PRESENT_ADDRESS_PIN { get; set; }
        public string PERMANENT_ADDRESS { get; set; }
        public Nullable<System.Guid> PERMANENT_ADDRESS_CITY { get; set; }
        public Nullable<System.Guid> PERMANENT_ADDRESS_STATE { get; set; }
        public Nullable<int> PERMANENT_ADDRESS_PIN { get; set; }
        public Nullable<long> AADHAR_NO { get; set; }
        public Nullable<short> EMP_STATUS_ID { get; set; }
        public string IDENTIFICATION_MARK { get; set; }
        public Nullable<System.DateTime> EXIT_DATE { get; set; }
        public string EXIT_REASON { get; set; }
        public string EXIT_DATE_SHORT { get; set; }
        public Nullable<System.DateTime> RETIREMMENT_DATE { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> UPDATED_DATE { get; set; }
        public string UPDATED_BY { get; set; }
        public string STATUS { get; set; }
        public System.Guid MRF_INTERNAL_ID { get; set; }
        public string MRF_ID { get; set; }
        public string REFERENCE_NAME { get; set; }
        public string REFERENCE_ID { get; set; }
        public byte[] PHOTO { get; set; }
        public byte[] EMP_SIGNATURE { get; set; }
    }

    public class WorkforceMasterMetaData1
    {
        public string USER_NAME { get; set; }
        public System.Guid DEPT_ID { get; set; }
        public System.Guid USER_ID { get; set; }
        public string EMP_ID { get; set; }
        public string EMP_NAME { get; set; }
    }

    //public partial class WorkforceMetaDataq
    //{
    //    public System.Guid WF_ID { get; set; }
    //    public string EMP_ID { get; set; }
    //    public string EMP_NAME { get; set; }
    //    public string MOBILE_NO { get; set; }
    //    public string FATHER_NAME { get; set; }
    //    public string EMAIL_ID { get; set; }
    //}
}
