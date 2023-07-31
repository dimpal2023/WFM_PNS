using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.ComponentModel;


namespace Wfm.App.Common
{
    public static class SessionHelper
    {
        public static T Get<T>(string index)
        {
            //this try-catch is done to avoid the issue where the report session is timing out and breaking the entire session on a refresh of the report            


            if (HttpContext.Current.Session == null)
            {
                var i = HttpContext.Current.Session.Count - 1;

                while (i >= 0)
                {
                    try
                    {
                        var obj = HttpContext.Current.Session[i];
                        if (obj != null && obj.GetType().ToString() == "Microsoft.Reporting.WebForms.ReportHierarchy")
                            HttpContext.Current.Session.RemoveAt(i);
                    }
                    catch (Exception)
                    {
                        HttpContext.Current.Session.RemoveAt(i);
                    }

                    i--;
                }
                if (!HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.Equals("~/Account/Index"))
                {
                    HttpContext.Current.Response.Redirect("~/Account/Index");
                }
                throw new System.ComponentModel.DataAnnotations.ValidationException(string.Format("You session has expired or you are currently logged out.", index));
            }

            try
            {
                if (HttpContext.Current.Session.Keys.Count > 0 && !HttpContext.Current.Session.Keys.Equals(index))
                {

                    return (T)HttpContext.Current.Session[index];
                }
                else
                {
                    var i = HttpContext.Current.Session.Count - 1;

                    while (i >= 0)
                    {
                        try
                        {
                            var obj = HttpContext.Current.Session[i];
                            if (obj != null && obj.GetType().ToString() == "Microsoft.Reporting.WebForms.ReportHierarchy")
                                HttpContext.Current.Session.RemoveAt(i);
                        }
                        catch (Exception)
                        {
                            HttpContext.Current.Session.RemoveAt(i);
                        }

                        i--;
                    }
                    if (!HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.Equals("~/Home/Default"))
                    {
                        HttpContext.Current.Response.Redirect("~/Home/Default");
                    }
                    throw new System.ComponentModel.DataAnnotations.ValidationException(string.Format("You session does not contain {0} or has expired or you are currently logged out.", index));
                }
            }
            catch (Exception e)
            {
                var i = HttpContext.Current.Session.Count - 1;

                while (i >= 0)
                {
                    try
                    {
                        var obj = HttpContext.Current.Session[i];
                        if (obj != null && obj.GetType().ToString() == "Microsoft.Reporting.WebForms.ReportHierarchy")
                            HttpContext.Current.Session.RemoveAt(i);
                    }
                    catch (Exception)
                    {
                        HttpContext.Current.Session.RemoveAt(i);
                    }

                    i--;
                }
                if (!HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.Equals("~/Home/Default"))
                {
                    HttpContext.Current.Response.Redirect("~/Account/Logout");
                    //HttpContext.Current.Response.RedirectPermanent("~/Dashboard/Index");
                }
                return default(T);
            }
        }

        public static void Set<T>(string index, T value)
        {
            HttpContext.Current.Session[index] = (T)value;
        }
    }
}
