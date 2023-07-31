using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Wfm.App.Core.Model
{
    public class WorkforceSalaryData
    {
        public string EMP_ID { get; set; }
        public string EMP_NAME { get; set; }
        public string BIO_CODE { get; set; }

        [Display(Name = "Deprtment ID")]
        public System.Guid? DEPT_ID { get; set; }
        public Guid? SUBDEPT_ID { get; set; }

        [Display(Name = "Deprtment")]
        public string DEPT_NAME { get; set; }

        [Display(Name = "Deprtment")]
        public string DEPT { get; set; }

        public string DESIGNATION { get; set; }

        [Display(Name = "Employee ID")]
        public System.Guid WF_ID { get; set; }

        public int? SALARY_MONTH { get; set; }
        public DateTime? STARTDATE { get; set; }
        public DateTime? ENDDATE { get; set; }
        public decimal? PAID_DAYS { get; set; }
        public decimal? OVERTIME_DAYS { get; set; }
        public int? TOTAL_LEAVE_TAKEN { get; set; }
        public int? TOTAL_LEAVE_AVAILABLE { get; set; }
        public int? LEAVE_ADJUSTED { get; set; }
        public int? TOTAL_LEAVE_BALANCE { get; set; }
        public decimal? BASIC_DA { get; set; }
        public decimal? HRA { get; set; }
        public decimal? SPECIAL_ALLOWANCES { get; set; }
        public decimal? PRODUCTION_INCENTIVE_BONUS { get; set; }
        public decimal? PF { get; set; }
        public decimal? ESI { get; set; }
        public decimal? TDS { get; set; }
        public decimal? SHOP_FLOOR_FINE { get; set; }
        public decimal? OTHER_DEDUCTION { get; set; }
        public decimal? ADVANCE { get; set; }
        public decimal? OVERTIME_WAGES { get; set; }
        public decimal? WORKINGDAY_WAGES { get; set; }
        public decimal? TOTAL_WAGES { get; set; }
        public decimal? TOTAL_WAGES_AFTER_DEDUCTION { get; set; }
        public decimal? EMPLOYER_EPF { get; set; }
        public decimal? EMPLOYER_ESI { get; set; }
        public decimal? ADMIN_CHARGES { get; set; }
        public decimal? EDLI_CHARGES { get; set; }
        public DateTime? CREATED_ON { get; set; }
        public string MODE_OF_PAYMENT { get; set; }
        public string PAID_STATUS { get; set; }


        public string MONTH_ID { get; set; }
        public string YEAR_ID { get; set; }
        public short WF_EMP_TYPE { get; set; }

        public List<WorkforceSalaryData> WorkforceSalaries { get; set; }
    }
    public class SalaryRevision
    {
        public short WF_EMP_TYPE { get; set; }
        public string PERCENTAGE { get; set; }
        public string BASIC_SALARY { get; set; }
        public DateTime? WEF { get; set; }
        public System.Guid SKILL_ID { get; set; }
    }
}
