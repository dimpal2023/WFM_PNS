using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Wfm.App.Core
{
	public class PortalSettings
	{
		private const string CALLCONTEXT_KEY = "PORTALSETTINGS_KEY";
		public const string BCMS_CACHE_REQUEST_SETTINGS = "PortalSettings";

		private static PNICache pniCache = new PNICache();
		public PortalSettings()
		{
			
		}
		public PNICache Cache()
		{
			return pniCache;
		}
		public static PortalSettings Current()
		{
			if(HttpContext.Current == null)
			{
				PortalSettings res = (PortalSettings)System.Runtime.Remoting.Messaging.CallContext.GetData(CALLCONTEXT_KEY);
				if(res == null)
				{
					res = new PortalSettings();
					System.Runtime.Remoting.Messaging.CallContext.SetData(CALLCONTEXT_KEY, res);
				}
				return res;
			}
			else
			{
				if(HttpContext.Current.Items[BCMS_CACHE_REQUEST_SETTINGS] == null)
				{
					HttpContext.Current.Items.Add(BCMS_CACHE_REQUEST_SETTINGS, new PortalSettings());
					return (PortalSettings)HttpContext.Current.Items[BCMS_CACHE_REQUEST_SETTINGS];
				}
			}
			return new PortalSettings();
		}
		public static HttpContext HttpContext
		{
			get
			{
				return Context.Current.HttpContext;
			}

		}
	}
}
