using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Wfm.App.Core.Model
{
    public class UserMetaData
    {

        public System.Guid USER_ID { get; set; }
        [Remote("IsUserIdAvailable", "User", ErrorMessage = "User Already Available")]
        [Display(Name ="User Id"), Required]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
        public string USER_LOGIN_ID { get; set; }
        public string USER_NAME { get; set; }
        public Nullable<System.Guid> DEPT_ID { get; set; }
        [Display(Name = "Department"), Required]
        public string[] DEPT_IDs { get; set; }
        public Nullable<System.Guid> SUBDEPT_ID { get; set; }
        [Display(Name = "Sub Department"), Required]
        public string[] SUBDEPT_IDs { get; set; }
        public System.Guid COMPANY_ID { get; set; }
        [Remote("IsUserEmailAvailable", "User", ErrorMessage = "Email Already Available")]

        [Display(Name = "Email-Id"), Required, EmailAddress] 
        public string MAIL_ID { get; set; }
        [Display(Name = "Mobile"), Required]
        [Remote("IsUserMobileAvailable", "User", ErrorMessage = "Mobile Already Available")]
        [RegularExpression(@"^[0-9]\S*$", ErrorMessage = "Only numbers without any white space allowed")]
        [MaxLength(10)]
        public string MOBILE_NO { get; set; }


        [Remote("IsUserEmailAvailableAtEdit", "User", ErrorMessage = "Email Already Available", AdditionalFields = "USER_ID")]

        [Display(Name = "Email-Id"), Required, EmailAddress]
        public string EditMAIL_ID { get; set; }
        [Remote("IsUserMobileAvailableAtEdit", "User", ErrorMessage = "Mobile Already Available", AdditionalFields = "USER_ID")]
        [Display(Name = "Mobile"), Required]
        public string EditMOBILE_NO { get; set; }
        public string CURRENT_PASSWORD { get; set; }
        public Nullable<System.DateTime> LAST_LOGIN_DATE { get; set; }
        public string LAST_LOGIN_IP { get; set; }
        public int WRONG_ATTEMP_COUNT { get; set; }
        public Nullable<System.DateTime> PASSWORD_CHANGE_DATE { get; set; }
        public Nullable<int> PASSWORD_EXPIRY_DAY { get; set; }
        public string PASSWORD1 { get; set; }
        public string PASSWORD2 { get; set; }
        public string PASSWORD3 { get; set; }
        public System.DateTime created_date { get; set; }
        public string Created_by { get; set; }
        public Nullable<System.DateTime> UPDATED_DATE { get; set; }
        public string UPDATED_BY { get; set; }
        public string status { get; set; }

        public IEnumerable<SelectListItem> Departments { get; set; }
        public List<SelectListItem> SubDepartments { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
        [Display(Name = "Roles"), Required]
        public string[] USER_ROLES { get; set; }
        public Guid? ROLE_ID { get; set; }
        public Guid? BUILDING_ID { get; set; }
    }
}
