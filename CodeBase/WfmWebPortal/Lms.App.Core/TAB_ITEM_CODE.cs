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
    
    public partial class TAB_ITEM_CODE
    {
        public System.Guid ITEM_CODE_ID { get; set; }
        public string ITEM_CODE_NAME { get; set; }
        public System.Guid COMPANY_ID { get; set; }
        public System.Guid DEPT_ID { get; set; }
        public System.Guid SUBDEPT_ID { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> UPDATED_DATE { get; set; }
        public string UPDATED_BY { get; set; }
    
        public virtual TAB_COMPANY_MASTER TAB_COMPANY_MASTER { get; set; }
        public virtual TAB_DEPARTMENT_MASTER TAB_DEPARTMENT_MASTER { get; set; }
        public virtual TAB_SUBDEPARTMENT_MASTER TAB_SUBDEPARTMENT_MASTER { get; set; }
    }
}