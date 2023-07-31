using Lms.Web.Portal.Authorization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Wfm.App.BL;
using Wfm.App.Core;
using Wfm.App.Core.Model;

namespace Lms.Web.Portal.Controllers
{
    [SessionExpire]
    [WfmAuthorize]
    public class ShiftController : Controller
    {
        private IBaseBL baseBl;
        
        public ShiftController(IBaseBL baseBl)
        {
            this.baseBl = baseBl;
        }

        [Authorize]
        public ActionResult Create()
        {
            ShiftMasterMetaData shift = new ShiftMasterMetaData {
                COMPANIES = baseBl.ShiftBL.GetCompanies()
            };
            return View(shift);
        }        

        [HttpPost]
        [Authorize]
        [Route("/Shift/Create")]
        public ActionResult Create([Bind(Include = "COMPANY_ID, SHIFT_NAME, SHIFT_START_TIME, SHIFT_END_TIME")]ShiftMasterMetaData shift)
        {
            if (ModelState.IsValid)
            {
                if(Session["USER"] != null)
                {
                    AccountValidateUser_Result loggedin_user = Session["USER"] as AccountValidateUser_Result;

                    shift.Created_by = loggedin_user.USER_NAME;
                    shift.UPDATED_BY = loggedin_user.USER_NAME;
                }

                baseBl.ShiftBL.Create(shift);
            }
            else
            {
                return View(shift);
            }
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("AllItems", "Shift");
            return Json(new { Url = redirectUrl });
        }

        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShiftMasterMetaData shift = baseBl.ShiftBL.Find(id.Value);
            if (shift == null)
            {
                return HttpNotFound();
            }
            return View(shift);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit([Bind(Exclude = "COMPANIES")] ShiftMasterMetaData shift)
        {
            if (ModelState.IsValid)
            {                
                baseBl.ShiftBL.Update(shift);
            }
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("AllItems", "Shift");
            return Json(new { Url = redirectUrl });
        }

        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            baseBl.ShiftBL.Delete(id.Value);
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("AllItems", "Shift");
            return Json(new { Url = redirectUrl });
        }

        [Authorize]
        public ActionResult AllItems()
        {
            List<ShiftMasterMetaData> shifts = baseBl.ShiftBL.GetAllItems();
            return View(shifts);
        }
    }
}