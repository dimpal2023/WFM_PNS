//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Wfm.App.Core
{
    using System;
    using System.Collections.Generic;
    
    public partial class TAB_WORKFORCE_SALARY_MASTER_OLD
    {
        public System.Guid WF_ID { get; set; }
        public System.Guid COMPANY_ID { get; set; }
        public string UAN_NO { get; set; }
        public string PAN_CARD { get; set; }
        public string EPF_NO { get; set; }
        public string ESIC_NO { get; set; }
        public Nullable<System.Guid> BANK_ID { get; set; }
        public string BANK_IFSC { get; set; }
        public string BANK_BRANCH { get; set; }
        public string BANK_ACCOUNT_NO { get; set; }
        public Nullable<int> BASIC_DA { get; set; }
        public Nullable<int> HRA { get; set; }
        public Nullable<int> SPECIAL_ALLOWANCES { get; set; }
        public Nullable<int> GROSS { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> UPDATED_DATE { get; set; }
        public string UPDATED_BY { get; set; }
        public string STATUS { get; set; }
    }
}