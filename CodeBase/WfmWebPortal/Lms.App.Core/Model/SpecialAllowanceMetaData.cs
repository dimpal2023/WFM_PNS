using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Wfm.App.Core.Model
{
    public class SpecialAllowanceMetaData
    {
        public Guid SpecialAllowanceId { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }
        public IEnumerable<SelectListItem> OnRollOrContracts { get; set; }
        [Required(ErrorMessage = "Required Departmnet")]
        public Guid DEPARTMENT_ID { get; set; }
        [Required(ErrorMessage = "Required Sub Departmnet")]
        public Guid? SUBDEPT_ID { get; set; }
        [Required(ErrorMessage = "Required Workforce Type")]
        public short WF_EMP_TYPE { get; set; }
        [Required(ErrorMessage = "Required Workforce")]
        public string WorkforceName { get; set; }
        public string WorkforceCode { get; set; }
        public Guid WF_ID { get; set; }

        [Required(ErrorMessage = "Required Amount")]
        public decimal? Amount { get; set; }

        [Required(ErrorMessage = "Required Year")]
        public short YEAR_ID { get; set; }
        [Required(ErrorMessage = "Required Month")]
        public short MONTH_ID { get; set; }
        public Guid BUILDING_ID { get; set; }
        public int ALLOWANCE_TYPE { get; set; }
        public string Remarks { get; set; }
    }
}
