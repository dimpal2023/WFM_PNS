using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lms.Web.Portal.Controllers
{
    public class ErrorController : Controller
    {
        [HandleError]
        public ActionResult Error404()
        {
            Response.StatusCode = 404;
            return View();
        }

        [HandleError]
        public ActionResult Error500()
        {
            Response.StatusCode = 500;
            return View();
        }

        [HandleError]
        public ActionResult Error401()
        {
            Response.StatusCode = 403;
            return View();
        }
    }
}