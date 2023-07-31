using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Wfm.App.Core.Model
{
    public class WorkforceLeavesMetaData
    {
        public System.Guid ID { get; set; }
        public System.Guid WF_ID { get; set; }
        [Display(Name = "Employee ID")]
        public string EMP_ID { get; set; }
        public string HIDDENEMP_ID{ get; set; }
        public string EMP_NAME { get; set; }
        [Display(Name = "From Date")]
        public DateTime? FROM_DATE { get; set; }
        [Display(Name = "To Date")]
        public DateTime? TO_DATE { get; set; }
        [Display(Name = "To Date")]
        public string REMARKS { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
    }
}
