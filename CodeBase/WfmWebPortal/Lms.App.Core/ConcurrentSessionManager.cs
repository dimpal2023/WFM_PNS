using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace Wfm.App.Core
{
	public class ConcurrentSessionManager
	{
		private string _userId;
		private string _sessionId;
		private string _userAgent;
		private List<ActiveSession> _activeSessionList = new List<ActiveSession>();

		public string UserId {
			get {
				return _userId;
			}
			set {
				_userId = value;
			}
		}
		public string UserKey
		{
			get
			{
				return "ConcurrentSessionListFor_" + UserId;
			}
		}
		public string SessionId
		{
			get {
				if(HttpContext.Current != null && HttpContext.Current.Request.Cookies["ASP.NET_SessionId"] != null)
				{
					_sessionId = HttpContext.Current.Request.Cookies["ASP.NET_SessionId"].Value.ToString();
				}
				return _sessionId;
			}
		}

		public ConcurrentSessionManager(string userId)
		{
			_userId = userId;
		}
		public void AddLoggedInUserDetailInCache()
		{
			//if user sessoin not exist then create new session.
			if (!IsCurrentUserSessionExists())
			{
				ActiveSession activeSession = new ActiveSession()
				{
					UserID = UserId,
					SessionId = SessionId,
					UserAgent = _userAgent,
					CookieExpirationDate = GetCookieExpirationDate()
				};
				if (_activeSessionList == null) _activeSessionList = new List<ActiveSession>();
				_activeSessionList.Add(activeSession);
			}
			else
			{
				ActiveSession currentUserSession = _activeSessionList.Find(m => m.UserID == UserId && m.SessionId == SessionId && m.UserAgent == _userAgent);
				currentUserSession.CookieExpirationDate = GetCookieExpirationDate();
			}
			UpdateSessionListInCache();
		}
		public void RemoveUserDetailsFromCache()
		{
			if(this.IsCurrentUserSessionExists())
			{
				ActiveSession currentUserSession = _activeSessionList.Find(m => m.UserID == UserId && m.SessionId == SessionId && m.UserAgent == _userAgent);
				_activeSessionList.Remove(currentUserSession);
				UpdateSessionListInCache();
			}
		}
		public int GetCurrentUserActiveSessions()
		{
			if(IsCurrentUserSessionExists())
			{
				return (from activeSession in _activeSessionList
						where activeSession.CookieExpirationDate > DateTime.Now
						select activeSession).Distinct().Count();
			}
			return -1;
		}
		private static DateTime GetCookieExpirationDate()
		{
			DateTime cookieExpirationDate = DateTime.Now;
			if(PortalSettings.HttpContext != null && PortalSettings.HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName] != null)
			{
				FormsAuthenticationTicket encTicket = FormsAuthentication.Decrypt(PortalSettings.HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName].Value);
				cookieExpirationDate = encTicket.Expiration;
			}
			return cookieExpirationDate;
		}
		private bool IsCurrentUserSessionExists()
		{
			bool result = false;
			List<ActiveSession> activeSessionList = (List<ActiveSession>)PortalSettings.Current().Cache().Item(UserKey);
			if(activeSessionList != null && activeSessionList.Count>0)
			{
				ActiveSession currentUserSession = activeSessionList.Find(m => m.UserID == UserId && m.SessionId == SessionId && m.UserAgent == _userAgent);
				result = currentUserSession != null;
			}

			return result;
		}
		private void UpdateSessionListInCache()
		{
			lock(PortalSettings.Current().Cache())
			{
				PortalSettings.Current().Cache().Add(UserKey, _activeSessionList, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(24, 0, 0));
			}
		}
	}
}
