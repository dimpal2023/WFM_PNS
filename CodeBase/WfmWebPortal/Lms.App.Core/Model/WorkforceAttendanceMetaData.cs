using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Wfm.App.Core.Model
{
    public class WorkforceAttendance
    {
        public Guid? WF_ID { get; set; }
        public Guid COMPANY_ID { get; set; }
        public string BIOMETRIC_CODE { get; set; }
        public DateTime? START_DATE { get; set; }
        public DateTime? END_DATE { get; set; }
        public DateTime? ATTENDANCE_DATE { get; set; }
        public TimeSpan? SHIFT_STARTTIME { get; set; }
        public TimeSpan? SHIFT_ENDTIME { get; set; }
        public decimal? DUTY_HOURS { get; set; }
        public decimal? OVERTIME_HOURS { get; set; }
        public string mdbfilename { get; set; }
        public string STATUS_CODE { get; set; }
        public string EMP_NAME { get; set; }
        public Guid? DEPT_ID { get; set; }
        public Guid? SUBDEPT_ID { get; set; }
        public string MONTH_ID { get; set; }
        public string YEAR_ID { get; set; }
        public string EMP_ID { get; set; }
        public short WF_EMP_TYPE { get; set; }
        public short EMPLOYMENT_TYPE { get; set; }
        public Guid BUILDING_ID { get; set; }
        public System.DateTime? FROM_DATE { get; set; }
        public System.DateTime? TO_DATE { get; set; }
        public System.Guid SHIFT_ID { get; set; }
        public int SHIFT_AUTOID { get; set; }
        public string SHIFT_NAME { get; set; }
    }
}
