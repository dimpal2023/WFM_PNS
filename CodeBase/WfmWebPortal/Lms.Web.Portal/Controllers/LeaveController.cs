using Lms.Web.Portal.Authorization;
using System.Collections.Generic;
using System.Web.Mvc;
using Wfm.App.BL;
using Wfm.App.Core.Model;

namespace Lms.Web.Portal.Controllers
{
    [SessionExpire]
    [WfmAuthorize]
    public class LeaveController : Controller
    {
        private IBaseBL baseBl;


        public LeaveController(IBaseBL baseBl)
        {
            this.baseBl = baseBl;
        }
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [Route("/Leave/Create")]
        public ActionResult Create([Bind(Exclude = "APPROVED, STATUS, NAME")] WorkforceLeavesMetaData workforceleave)
        {
            LeaveBL leaveBL = new LeaveBL();

            leaveBL.Create(workforceleave);

            var redirectUrl = new UrlHelper(Request.RequestContext).Action("All", "Leave");
            return Json(new { Url = redirectUrl });
        }

        [HttpPost]
        [Authorize]
        public JsonResult EmpSearch(WorkforceLeavesMetaData wfobj)
        {
            LeaveBL leaveBL = new LeaveBL();

            WorkforceLeavesMetaData objleave = leaveBL.Find(wfobj.EMP_ID);

            return Json(objleave, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult All()
        {
            LeaveBL leaveBL = new LeaveBL();

            List<WorkforceLeavesMetaData> leavelist = leaveBL.GetLeaveAllItems();

            return View(leavelist);
        }

        public ActionResult Types()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult Balance()
        {
            return View();
        }
    }
}