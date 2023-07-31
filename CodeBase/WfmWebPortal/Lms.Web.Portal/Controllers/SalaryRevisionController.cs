using Lms.Web.Portal.Authorization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Wfm.App.BL;
using Wfm.App.Core;
using Wfm.App.Core.Model;
using Lms.Web.Portal.DataAccess;
using System.Data;

namespace Lms.Web.Portal.Controllers
{
    public class SalaryRevisionController : Controller
    {
        private IBaseBL baseBl;


        public SalaryRevisionController(IBaseBL baseBl)
        {
            this.baseBl = baseBl;
        }
        [HttpGet]
        public string GetBasicSalary(string SKILL_ID)
        {
            DLLReports objDB = new DLLReports();
            DataSet ds = new DataSet();
             ds = objDB.GetBasicSalary("Proc_SalaryRevision","",SKILL_ID, "", "", "","1");

            return ds.GetXml();
        }
        public string SubmitSalaryRevision(string WF_EMP_TYPE, string SKILL_ID, string BASIC_SALARY, string PERCENTAGE, string WEF)
        {
            DLLReports objDB = new DLLReports();
            DataSet ds = new DataSet();
            ds = objDB.GetBasicSalary("Proc_SalaryRevision", WF_EMP_TYPE, SKILL_ID, BASIC_SALARY, PERCENTAGE, WEF,"2");

            return ds.GetXml();
        }
    }

}