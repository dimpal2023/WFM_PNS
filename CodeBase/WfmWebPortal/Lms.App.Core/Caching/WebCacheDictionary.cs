using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Wfm.App.Core.Caching
{
	public class WebCacheDictionary : WebCache
	{
		public WebCacheDictionary()
		{
		}

		protected override void AddInternal(string key, LmsCacheItemWrapper value, DateTimeOffset absoluteExpiration, TimeSpan slidingExpiration, System.Runtime.Caching.CacheItemPriority priority)
		{
			if( !value.LocalObject)
			{
				lock(this.Items)
				{
					this.Items[key] = value.DateAdded;
				}
			}
			base.AddInternal(key, value, absoluteExpiration, slidingExpiration, priority);
		}

		public override void WebCacheObjectRemoved(string k, object v, CacheItemRemovedReason r)
		{
			lock(this.Items)
			{
				this.Items.Remove(k);
			}
			base.WebCacheObjectRemoved(k, v, r);
		}

		private Dictionary<string, DateTime> _items;
		private Dictionary<string, DateTime> Items
		{
			get
			{
				if(_items == null)
				{
					lock(this)
					{
						if(_items == null)
						{
							_items = new Dictionary<string, DateTime>();
						}
					}
				}
				return _items;
			}
		}
	}
}
