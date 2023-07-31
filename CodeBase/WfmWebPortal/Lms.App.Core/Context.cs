using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Wfm.App.Core
{
	public sealed class Context
	{
		private const string CONTEXTITEMKEY = "LMS$Context$Current";
		private Caching.LmsCache _cache;
		private System.Web.HttpContext _httpConext;
		public System.Web.HttpContext HttpContext
		{
			get
			{
				if(_httpConext == null)
				{
					_httpConext = System.Web.HttpContext.Current;
				}
				return _httpConext;
			}
			set
			{
				_httpConext = value;
			}
		}
		public Caching.LmsCache Cache()
		{
			if (_cache == null)
			{
				_cache = new Caching.WebCacheDictionary();

			}

			return _cache;
		}
		public static Context Current
		{
			get
			{
				return Context.CurrentContext(true);
			}
		}
		public static Context CurrentContext(bool createIfNoneExists)
		{
			Context result = null;
			System.Web.HttpContext httpContext = System.Web.HttpContext.Current;
			if(result != null)
			{
				result = (Context)httpContext.Items[CONTEXTITEMKEY];
				if(result == null && createIfNoneExists)
				{
					result = new Context();
					httpContext.Items[CONTEXTITEMKEY] = result;
				}
			}
			else
			{
				result = (Context)CallContext.GetData(CONTEXTITEMKEY);
				if(result == null && createIfNoneExists)
				{
					result = new Context();
					CallContext.SetData(CONTEXTITEMKEY, result);
				}
			}
			return result;
		}
	}
}
