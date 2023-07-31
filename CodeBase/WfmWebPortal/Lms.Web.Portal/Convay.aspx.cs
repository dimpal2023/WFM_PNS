using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

namespace Lms.Web.Portal
{
    public partial class Convay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod(EnableSession = true)]
        public static string ChangeFormat(string Value, string Password)
        {
            string Result = "";
            if (Value == "1")
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(Password);
                Result= System.Convert.ToBase64String(plainTextBytes);
            }
            else
            {
                var base64EncodedBytes = System.Convert.FromBase64String(Password);
                Result = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }

            return Result;

        }
    }
}