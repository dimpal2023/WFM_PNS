using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Wfm.App.Core.Model
{
    public class WorkforceSalaryMetaData
    {
        [Display(Name = "Employee ID")]
        public System.Guid WF_ID { get; set; }
        public string EMP_ID { get; set; }
        public string EMP_NAME { get; set; }
        [Display(Name = "Company")]
        public System.Guid COMPANY_ID { get; set; }

        [Display(Name = "UAN NO.")]
        public string UAN_NO { get; set; }

        [Display(Name = "PAN NO.")]
        public string PAN_CARD { get; set; }

        [Display(Name = "EPF NO.")]
        public string EPF_NO { get; set; }

        [Display(Name = "ESIC NO.")]
        public string ESIC_NO { get; set; }

        [Display(Name = "Bank ID")]
        public System.Guid? BANK_ID { get; set; }

        [Display(Name = "Bank IFSC Code")]
        public string BANK_IFSC { get; set; }

        [Display(Name = "Bank Branch")]
        public string BANK_BRANCH { get; set; }

        [Display(Name = "Bank Account No.")]
        public string BANK_ACCOUNT_NO { get; set; }

        [Display(Name = "Basic Salary")]
        public int? BASIC_DA { get; set; }

        [Display(Name = "HRA")]
        public int? HRA { get; set; }

        [Display(Name = "Special Allowance")]
        public int? SPECIAL_ALLOWANCES { get; set; }

        [Display(Name = "Gross Salary")]
        public int? GROSS { get; set; }

        public DateTime CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }

        public string ACTION { get; set; }
        public string SELECTEDBANKNAME { get; set; }

        public System.Guid SELECTEDBANKID { get; set; }
    }
}
