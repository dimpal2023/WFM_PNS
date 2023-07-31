using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Wfm.App.Core.Model
{
    public class DashBoardMetaData
    {
        public IEnumerable<SelectListItem> Departments { get; set; }
        public IEnumerable<SelectListItem> SubDepartments { get; set; }
        public IEnumerable<SelectListItem> Buildings { get; set; }
        public Guid DEPARTMENT_ID { get; set; }
        public Guid SUBDEPARTMENT_ID { get; set; }
        public Guid BUILDING_ID { get; set; }
    }

    public class DashBoardJSONMetaData
    {
        public int TolalNewWorkforceCount30Days { get; set; }
        public int TolalWorkforceCount { get; set; }
        public int PendingHiring { get; set; }
        public int TotalPendingMRF { get; set; }
        public List<MonthAndTotal> MonthAndTotals { get; set; }
        public List<RetireMent_Employees> RetireMent_Employees { get; set; }
        public int OnTraining { get; set; }
        public int TotalFreezingStrength { get; set; }
        public int TotalTransfer { get; set; }
        public int TotalExit { get; set; }
        public string Role { get; set; }
    }

    public class MonthAndTotal
    {
        public string MonthName { get; set; }
        public decimal Processed { get; set; }
        public decimal Failed { get; set; }
    }

    public class RetireMent_Employees
    {
        public string Empcode { get; set; }
        public string EmpName { get; set; }
        public string Biometric_Code { get; set; }
        public string Department { get; set; }
        public string SubDepartment { get; set; }
        public string Joining_Date { get; set; }
        public string Retirement_Date { get; set; }
        public int Age { get; set; }
    }
}
