using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Wfm.App.Core;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Repositories;

namespace Lms.Web.Portal.Authorization
{
    public class WfmAuthorizeAttribute : AuthorizeAttribute
    {        
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            var controller = httpContext.Request.RequestContext.RouteData.GetRequiredString("controller");
            var action = httpContext.Request.RequestContext.RouteData.GetRequiredString("action");

            if (httpContext.User.Identity.IsAuthenticated)
            {
                return IsUserHaveAccess(controller, action);
            }

            return base.AuthorizeCore(httpContext);
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (filterContext.Result != null)
            {
                if (filterContext.Result.GetType() == typeof(HttpUnauthorizedResult))
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "Error401" }));
                }
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {            
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(filterContext);
                return;

            }            
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { controller = "Error", action = "Error401" })
                );
            }

        }

        //Controlling user access if user does not have access to mapped menus or controller/action
        private bool IsUserHaveAccess(string controller, string action)
        {            
            if (HttpContext.Current.Session["MENU"] != null)
            {
                Dictionary<MenuMetaData, List<SubMenuMetaData>> userMenu = (Dictionary<MenuMetaData, List<SubMenuMetaData>>)HttpContext.Current.Session["MENU"];

                if (userMenu != null)
                {
                    var menuObj = userMenu.Where(x => string.Equals(x.Key.CONTROLLER_NAME, controller, StringComparison.OrdinalIgnoreCase) || x.Key.CONTROLLER_NAME.IndexOf(controller, StringComparison.OrdinalIgnoreCase) > 0);
                    if (menuObj != null && menuObj.Count() > 0)
                    {
                        var submenuObj = menuObj.Where(x => (x.Value.Where(y => string.Equals(y.ACTION_NAME, action, StringComparison.OrdinalIgnoreCase) && (string.Equals(y.CONTROLLER_NAME, controller, StringComparison.OrdinalIgnoreCase) || x.Key.CONTROLLER_NAME.IndexOf(controller, StringComparison.OrdinalIgnoreCase) > 0)).FirstOrDefault()) != null);

                        var existMapping = GetActionMethodMapping(controller, action); //GetActionMethods(controller).Where(x => x.ToUpper() == action.ToUpper()).FirstOrDefault();

                        if (submenuObj.Count() == 0 && existMapping)
                        {                            
                            return false;
                        }
                    }
                    else
                    {
                        using (MenuRepository menuRepo = new MenuRepository())
                        {
                            if (menuRepo.GetAllMenu().Where(x => string.Equals(x.CONTROLLER_NAME, controller, StringComparison.OrdinalIgnoreCase) || x.CONTROLLER_NAME.IndexOf(controller, StringComparison.OrdinalIgnoreCase) > 0).ToList().Count() > 0)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        private bool GetActionMethodMapping(string controller, string action)
        {
            ApplicationEntities db = new ApplicationEntities();

            string controllerLower = controller.ToLower();
            string actionLower = action.ToLower();

            return db.TAB_SUBMENU.Where(x => (x.CONTROLLER_NAME.ToLower() == controllerLower || x.CONTROLLER_NAME.ToLower().Contains(controllerLower)) && x.ACTION_NAME.ToLower() == actionLower).FirstOrDefault() != null;
        }
    }
}