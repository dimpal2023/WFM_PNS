using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wfm.App.Core.Model
{
    public class ErrorLogsMetaData
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
