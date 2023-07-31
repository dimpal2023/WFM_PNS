using Lms.Web.Portal.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Wfm.App.BL;
using Wfm.App.Core;
using Wfm.App.Core.Model;

namespace Lms.Web.Portal.Controllers
{
    [SessionExpire]
    [WfmAuthorize]
    public class IDCardGenerationController : Controller
    {
        // GET: IDCardGeneration

        private IBaseBL baseBL;


        public IDCardGenerationController(IBaseBL baseBL)
        {
            this.baseBL = baseBL;
        }
        public ActionResult GetEmployess(Guid? deptId)
        {
            WorkflowMasterVieweMetaData metaData = new WorkflowMasterVieweMetaData();
            AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;
            var deptList = this.baseBL.DepartmentBL.GetDepartmentByCompanyId(loggedin_user.COMPANY_ID);
            var onRollOrContracts = this.baseBL.IDCardGenerationBL.GetOnRollOrContracts(loggedin_user.COMPANY_ID);
            var defaultBind = deptList.Select(x => x.DEPT_ID).FirstOrDefault();
            metaData.DEPARTMENT_ID = defaultBind;
            metaData.Departments = new SelectList(deptList, "DEPT_ID", "DEPT_NAME");
            metaData.OnRollOrContracts = new SelectList(onRollOrContracts, "WF_EMP_TYPE", "EMP_TYPE");
            ViewBag.Buildings = new SelectList(baseBL.ManPowerRequestBL.GetBuildings(), "BUILDING_ID", "BUILDING_NAME");
            return View(metaData);
        }

        public ActionResult GetEmployessByDeptId(Guid? deptId, Guid? sub_dept_id, int? emptype_id,Guid? BUILDING_ID)
        {
            WorkflowMasterVieweMetaData metaData = new WorkflowMasterVieweMetaData();
            List<PartialWorkflowMasterVieweMetaData> employees = this.baseBL.IDCardGenerationBL.GetAllEmployeesByCopanyIdAndDeptId(deptId,sub_dept_id, emptype_id, BUILDING_ID);
            return PartialView("_Employess", employees);
        }

        [HttpPost]
        public ActionResult GenerateCards(IEnumerable<Guid> wfIds)
        {
            List<GenerateCardViewModel> generateCards = new List<GenerateCardViewModel>();
            if (wfIds != null && wfIds.Count() > 0)
            {
                generateCards = this.baseBL.IDCardGenerationBL.GenerateCards(wfIds);

            }
            return View(generateCards);
        }

        [HttpGet]
        public ActionResult GenerateCards()
        {
            IEnumerable<Guid> wfIds = TempData["EmployessWFID"] as IEnumerable<Guid>;
            List<GenerateCardViewModel> generateCards = TempData["GenerateCardHTML"] as List<GenerateCardViewModel>;
            //if (wfIds != null && wfIds.Count() > 0)
            //{
            //    //TempData["EmployessWFID"] = wfIds;
            //    //TempData["GenerateCardHTML"] = generateCards;
            //    //TempData.Keep("EmployessWFID");
            //    //TempData.Keep("GenerateCardHTML");
            //    //GenerateCards();
            //    //generateCards = this.baseBL.IDCardGenerationBL.GenerateCards(wfIds);
            //    StringBuilder builder = new StringBuilder();
            //    foreach (var html in generateCards)
            //    {
            //        builder.Append(html.EmployeeCard);
            //    }
            //    string htmlCard = builder.ToString();

            //    var globalSettings = new GlobalSettings
            //    {
            //        ColorMode = ColorMode.Color,
            //        Orientation = Orientation.Portrait,
            //        Margins = new MarginSettings { Top = 10 },
            //        PaperSize = PaperKind.A4,
            //    };
            //    var objectSettings = new ObjectSettings()
            //    {
            //        PagesCount = true,
            //        HtmlContent = htmlCard,
            //        HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 },
            //        FooterSettings = { FontName = "Arial", FontSize = 9, Center = "Report Header" }
            //    };

            //    var pdf = new HtmlToPdfDocument()
            //    {
            //        GlobalSettings = globalSettings,
            //        Objects = { objectSettings }


            //    };
            //    var _converter = new BasicConverter(new PdfTools());

            //    var file = _converter.Convert(pdf);
            //    return File(file, "applicatio/pdf", "Invoice.pdf");
            //}
            return View(generateCards);
        }
    }
}