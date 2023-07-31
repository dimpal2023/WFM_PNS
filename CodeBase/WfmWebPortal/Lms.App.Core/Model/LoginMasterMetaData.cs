using System;
using System.ComponentModel.DataAnnotations;

namespace Wfm.App.Core.Model
{
    public class LoginMasterMetaData
    {      
        [Required]  
        [Display(Name = "User name")]
        public string USER_LOGIN_ID { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string CURRENT_PASSWORD { get; set; }

        public string MAIL_ID { get; set; }

        public Nullable<bool> REMEMBER_ME { get; set; } = false;
    }

    public class ResetPasswordLoginMasterMetaData
    {
        [Required]
        [Display(Name = "Current password")]
        public string CURRENT_PASSWORD { get; set; }

        [Required]
        [Display(Name = "New password")]
        public string New_PASSWORD { get; set; }

        [Required, Compare("New_PASSWORD")]
        [Display(Name = "Confirm password")]
        public string Confirm_PASSWORD { get; set; }
        public Guid USER_ID { get; set; }
    }
}
