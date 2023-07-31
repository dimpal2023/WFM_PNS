using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Wfm.App.Core.Model
{
    public class WorkforceDailyWorkData
    {
        [Display(Name = "Employee ID")]
        public System.Guid WF_ID { get; set; }
        public string EMP_ID { get; set; }
        public string SEARCHEDEMP_ID { get; set; }
        public string EMP_NAME { get; set; }
        [Display(Name = "Company")]
        public System.Guid COMPANY_ID { get; set; }

        [Display(Name = "Deprtment ID")]
        public System.Guid? DEPT_ID { get; set; }

        [Display(Name = "Deprtment")]
        public string DEPT_NAME { get; set; }

        public Guid? SUBDEPT_ID { get; set; }
        [Display(Name = "Work Date")]
        public DateTime? WORK_DATE { get; set; }

        [Display(Name = "Operation Id")]
        public System.Guid UNIQUE_OPERATION_ID { get; set; }

        [Display(Name = "Operation")]
        public string OPERATION_NAME { get; set; }

        [Display(Name = "ITEM")]
        public string ITEM { get; set; }

        [Display(Name = "ITEM ID")]
        public Guid ITEM_ID { get; set; }

        [Display(Name = "Quantity")]
        public int? QTY { get; set; }

        [Display(Name = "Rate")]
        public float? RATE { get; set; }

        public DateTime CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }

        public DateTime UPDATED_DATE { get; set; }
        public string UPDATED_BY { get; set; }
        public int? EMP_TYPE_ID { get; set; }
        public IEnumerable<SelectListItem> ItemList { get; set; }
        public Nullable<bool> Overtime { get; set; }
    }

    public class WorkforceDailyWorkMetaData
    {
        public IEnumerable<SelectListItem> Departments { get; set; }
        public IEnumerable<SelectListItem> OnRollOrContracts { get; set; }
        [Required(ErrorMessage = "Required")]

        public Guid BUILDING_ID { get; set; }
        public Guid DEPARTMENT_ID { get; set; }
        public System.Guid? SUBDEPT_ID { get; set; }
        public short WF_EMP_TYPE { get; set; }

        [Display(Name = "Employee ID")]
        
        [Required(ErrorMessage = "Required")]
        public Nullable<Guid> WF_ID { get; set; }
        public string EMP_NAME { get; set; }
        public string ITEM_NAME { get; set; }
        [Display(Name = "Working Date")]
        [Required(ErrorMessage = "Required")]
        public DateTime? WORK_DATE { get; set; }
        [Required(ErrorMessage = "Required")]
        public Nullable<Guid> ITEM_ID { get; set; }
        [Required(ErrorMessage = "Required")]
        public Nullable<Guid> UNIQUE_OPERATION_ID { get; set; }
        [Display(Name = "Quantity/ Hours")]
        [Required(ErrorMessage = "Required")]
        public decimal QTY { get; set; }
        public double? Price { get; set; }
        public bool OverTime { get; set; }
        public Guid DW_ID { get; set; }
        public string MachineNo { get; set; }
        public string AvgPercentage { get; set; }
        public string WASTE { get; set; }
        public string REJECTION_ON_LOOM { get; set; }
        public string REJECTION_ON_FINISHING { get; set;}
}

    public class AddWorkForceItem
    {
        public Nullable<Guid> ITEM_ID { get; set; }
        public string ITEM_CODE { get; set; }
        public Nullable<Guid> UNIQUE_OPERATION_ID { get; set; }
        public int QTY { get; set; }
        public Nullable<Guid> WF_ID { get; set; }
        public double? RATE { get; set; }
    }
    public class AddWorkForceItemMetaData
    {
        public Nullable<Guid> WF_ID { get; set; }
        public string WORK_DATE { get; set; }
        public bool IsOverTime { get; set; }
        public string OverTime { get; set; }
        public int OT_Min { get; set; }
        public List<OPERATIONS> OPERATIONs { get; set; }
    }
    public class OPERATIONS
    {
        public Nullable<Guid> UNIQUE_OPERATION_ID { get; set; }
        public decimal QTY { get; set; }
        public string MachineNo { get; set; }
        public decimal AvgPercentage { get; set; }
        public decimal WASTE { get; set; }
        public decimal REJECTION_ON_LOOM { get; set; }
        public decimal REJECTION_ON_FINISHING { get; set; }
    }

    public class SerchDailyWorkMetaData
    {
        public double Total { get; set; }
        public DateTime? WorkingDate { get; set; }
        public string WorkforceName { get; set; }
        public Nullable<Guid> WF_ID { get; set; }
        public List<PartialDailyWorkMetaData> partialDailyWorkMetaDatas { get; set; }
    }
    public class PartialDailyWorkMetaData
    {
        [Display(Name = "Item")]
        public string ITEM { get; set; }
        Nullable<Guid> ITEM_ID { get; set; }
        public string ITEM_CODE { get; set; }
        public Nullable<Guid> UNIQUE_OPERATION_ID { get; set; }
        [Display(Name = "Operation")]
        public string OPERATION_NAME { get; set; }

        [Display(Name = "Quantity")]
        public decimal QTY { get; set; }
        public decimal? TotalPrice { get; set; }
        public double? RATE { get; set; }
        public Guid DW_ID { get; set; }
    }

    public class FilterDailyWork
    {
        public IEnumerable<SelectListItem> Departments { get; set; }
        public IEnumerable<SelectListItem> OnRollOrContracts { get; set; }
        [Required(ErrorMessage = "Required")]
        public Guid DEPARTMENT_ID { get; set; }
        public short? WF_EMP_TYPE { get; set; }
        public System.Guid? SUBDEPT_ID { get; set; }

        [Display(Name = "Employee ID")]
        public Guid BUILDING_ID { get; set; }
        public Nullable<Guid> WF_ID { get; set; }
        public string EMP_NAME { get; set; }
        [Display(Name = "Start Date")]
        [Required(ErrorMessage = "Required")]
        public string StartDate { get; set; }
        [Display(Name = "End Date")]
        [Required(ErrorMessage = "Required")]
        public string EndDate { get; set; }
        public Guid DW_ID { get; set; }
    }
    public class BiometricAndAttendance
    {
        public Guid? WF_ID { get; set; }
        public string BIOMETRIC_CODE { get; set; }
        public string START_DATE { get; set; }
        public string END_DATE { get; set; }
        public string ATTENDANCE_DATE { get; set; }
        public TimeSpan? SHIFT_STARTTIME { get; set; }
        public TimeSpan? SHIFT_ENDTIME { get; set; }
        public string DUTY_HOURS { get; set; }
        public string OVERTIME_HOURS { get; set; }
        public string mdbfilename { get; set; }
        public string STATUS_CODE { get; set; }
        public string EMP_NAME { get; set; }
        public Guid? DEPT_ID { get; set; }
        public string MONTH_ID { get; set; }
        public string YEAR_ID { get; set; }
        public string EMP_ID { get; set; }
        public int OT_Min { get; set; }
    }
}
