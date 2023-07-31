using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Wfm.App.Core.Model
{
    public class WorkforceFaultyData
    {
        public string EMP_ID { get; set; }
        public string EMP_NAME { get; set; }
        public string BIO_CODE { get; set; }
        public System.Guid? DEPT_ID { get; set; }
        public string DEPT_NAME { get; set; }
        public System.Guid WF_ID { get; set; }
        public DateTime? ATTENDANCE_DATE { get; set; }
        public string PUNCH_RECORD { get; set; }
        public string REMARKS { get; set; }
        public string MONTH_ID { get; set; }
        public string YEAR_ID { get; set; }
    }
}
