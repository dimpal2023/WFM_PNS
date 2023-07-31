using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Repositories;

namespace Lms.Web.Portal
{
    public class SessionExpireAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Controller controller = filterContext.Controller as Controller;

            HttpContext httpContext = HttpContext.Current;
            var rd = httpContext.Request.RequestContext.RouteData;
            string currentAction = rd.GetRequiredString("action");
            string currentController = rd.GetRequiredString("controller");

            if (currentController != "Account" && currentAction != "Index")
            {
                if (HttpContext.Current.Session["USER"] == null || HttpContext.Current.Session["MENU"] == null)
                {
                    filterContext.Result = new RedirectResult("~/Account/Index?ReturnUrl=" + currentController + "/" + currentAction);
                    return;
                }
            }

            base.OnActionExecuting(filterContext);
        }        
    }
}