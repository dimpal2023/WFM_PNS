using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wfm.App.Core.Model
{
    public class AttendanceDetails
    {
        public int ID { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public string Employee { get; set; }
        public string BiometricCode { get; set; }
       
        public int Present { get; set; }
        public int Absent { get; set; }
        public int WeeklyOff { get; set; }
        public int Holiday { get; set; }
        public string OTHours { get; set; }
        public string GatePassHours { get; set; }
        public string GatePassDays { get; set; }
        public int EarlyByDay { get; set; }
        public int LateByDay { get; set; }
        public List<GetMonthlyAttendanceReport> AllReport { get; set; }
    }
    public class GetMonthlyAttendanceReport
    {
        public int Dates { get; set; }
        public string Status { get; set; }
        public string In { get; set; }
        public string Out { get; set; }
        public string Duration { get; set; }
        public string EarlyBy { get; set; }
        public string LateBy { get; set; }
        public string OT { get; set; }
        public string ShortDuration { get; set; }
        public string Shift { get; set; }
        public string Permission { get; set; }
    }
}
