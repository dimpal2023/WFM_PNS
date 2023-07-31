using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Wfm.App.Core.Model
{
    public class FilteSalarySlipMetaData
    {
        public IEnumerable<SelectListItem> Departments { get; set; }
        public IEnumerable<SelectListItem> SubDepartments { get; set; }
        public IEnumerable<SelectListItem> OnRollOrContracts { get; set; }
        [Required(ErrorMessage = "Required")]
        public Guid DEPARTMENT_ID { get; set; }
        public Guid? SUBDEPT_ID { get; set; }
        public short? WF_EMP_TYPE { get; set; }
        public string EMP_NAME { get; set; }
        public Nullable<Guid> WF_ID { get; set; }
        [Required(ErrorMessage = "Required")]
        public string MM_YYYY { get; set; }
    }
    public class DownloadSalarySlip
    {
        public string MM_YYYY { get; set; }
        public Guid DEPARTMENT_ID { get; set; }
        public IEnumerable<Guid> wfIds { get; set; }
    }

    public class WorkforceSalarySlip
    {
        public string EMP_CODE { get; set; }
        public string EMP_NAME { get; set; }
        public string DEPARTMENT { get; set; }
        public string UAN { get; set; }
        public string ESIC { get; set; }
        public decimal?W_BASIC_DA{ get; set; }
        public decimal?W_HR_ALL{ get; set; }
        public decimal?W_SPE_ALL{ get; set; }
        public decimal?W_ACTUAL_GROSS{ get; set; }
        public decimal?ACTUAL_PAID_DAY{ get; set; }
        public decimal?E_BASIC_DA{ get; set; }
        public decimal?E_HR_ALL{ get; set; }
        public decimal?E_SPE_ALL{ get; set; }
        public decimal?E_PRO_BONUS{ get; set; }
        public decimal?E_GROSS_SALARY{ get; set; }
        public decimal?WF_EPF{ get; set; }
        public decimal?WF_ESI{ get; set; }
        public decimal?WF_TDS{ get; set; }
        public decimal?WF_FINE{ get; set; }
        public decimal?WF_ADVANCE{ get; set; }
        public decimal?ACTUAL_WAGES_PAID{ get; set; }
        public decimal?CONTRI_EPF{ get; set; }
        public decimal?CONTRI_ESI{ get; set; }
        public decimal?ADMIN_CHARGE{ get; set; }
        public int?LEAVE_TAKEN{ get; set; }
        public int? LEAVE_BALANCE{ get; set; }
        public short? WF_EMP_TYPE { get; set; }
        public string COMPANY_NAME { get; set; }
        public string COMPANY_ADDRESS1 { get; set; }
        public string COMPANY_ADDRESS2 { get; set; }
        public string AGENCY_NAME { get; set; }
        public string AGENCY_ADDRESS1 { get; set; }
        public object AGENCY_ADDRESS2 { get; set; }
    }

    public class ExportSalaryMetaData
    {
        public string SalarySlip { get; set; }
    }
}
