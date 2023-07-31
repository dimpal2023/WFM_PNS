using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wfm.App.Core
{
	public class ActiveSession
	{
		public string UserID { get; set; }
		public string SessionId { get; set; }
		public string UserAgent { get; set; }

		/*
		 * <summary>
		 * Date time at which form authentication ticket expires for the current user.
		 * </summary>
		 * <returns>DateTime</returns>
		 */
		public DateTime CookieExpirationDate { get; set; }
	}
}
