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
    
    public partial class TAB_ERROR_LOGS
    {
        public int ErrorID { get; set; }
        public string User_ID { get; set; }
        public string Db_User { get; set; }
        public Nullable<int> ErrorNumber { get; set; }
        public Nullable<int> ErrorState { get; set; }
        public Nullable<int> ErrorSeverity { get; set; }
        public Nullable<int> ErrorLine { get; set; }
        public string ErrorProcedure { get; set; }
        public string ErrorMessage { get; set; }
        public Nullable<System.DateTime> ErrorDateTime { get; set; }
    }
}
