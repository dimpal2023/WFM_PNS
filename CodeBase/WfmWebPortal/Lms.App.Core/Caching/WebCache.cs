using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Wfm.App.Core.Caching
{
	public class WebCache : LmsCache
	{
		private string _cacheKeyPrefix;

		public string CacheKeyPrefix
		{
			get
			{
				return _cacheKeyPrefix;
			}
		}

		public WebCache()
		{
			this._cacheKeyPrefix = "$$PNICACHE$$";
		}
		protected override void AddInternal(string key, LmsCacheItemWrapper value, DateTimeOffset absoluteExpiration, TimeSpan slidingExpiration, System.Runtime.Caching.CacheItemPriority priority)
		{
			string fullCacheKey = GetFullCacheKey(key);
			CacheItemRemovedCallback removedCallback = new CacheItemRemovedCallback(this.WebCacheObjectRemoved);

			HttpRuntime.Cache.Insert(fullCacheKey, value, null, absoluteExpiration.DateTime, slidingExpiration, GetWebPriority(priority), removedCallback);
		}
		protected override int FlushCacheInternal(string keyStartsWith)
		{
			bool flushAll = (keyStartsWith.Length == 0);
			List<string> fullCacheKeys = new List<string>();

			foreach(DictionaryEntry entry in this.GetAllItems())
			{
				if(entry.Key != null && entry.Key is string && entry.Value is LmsCacheItemWrapper)
				{
					if(flushAll)
					{
						fullCacheKeys.Add(entry.Key.ToString());
					}
					else
					{
						if(((LmsCacheItemWrapper)entry.Value).Key.ToString().StartsWith(keyStartsWith, StringComparison.Ordinal))
						{
							fullCacheKeys.Add(entry.Key.ToString());
						}
					}
				}
			}

			foreach(String fullCacheKey in fullCacheKeys)
			{
				HttpRuntime.Cache.Remove(fullCacheKey);
			}

			return fullCacheKeys.Count;
		}
		protected override string GetItemKeyInternal(object value)
		{
			string key = null;

			foreach(DictionaryEntry entry in HttpRuntime.Cache)
			{
				if(entry.Value is LmsCacheItemWrapper)
				{
					if(((LmsCacheItemWrapper)entry.Value).Value == value)
					{
						key = entry.Key.ToString();
						break;
					}
				}
				else
				{
					if(entry.Value == value)
					{
						key = entry.Key.ToString();
					}
				}
			}

			return this.GetRegularCacheKey(key);
		}
		protected override object ItemInternal(string key)
		{
			string fullCacheKey = GetFullCacheKey(key);
			return HttpRuntime.Cache.Get(fullCacheKey);
		}
		protected override void RemoveInternal(string key)
		{
			string fullCacheKey = GetFullCacheKey(key);
			HttpRuntime.Cache.Remove(fullCacheKey);
		}
		protected string GetFullCacheKey(string key)
		{
			return string.Concat(this.CacheKeyPrefix, key);
		}
		public virtual void WebCacheObjectRemoved(string k, object v, CacheItemRemovedReason r)
		{
			this.CacheObjectRemoved(k, v, r.ToString());
		}
		private static System.Web.Caching.CacheItemPriority GetWebPriority(System.Runtime.Caching.CacheItemPriority priority)
		{
			System.Web.Caching.CacheItemPriority p = System.Web.Caching.CacheItemPriority.Default;
			switch (priority)
			{
				case System.Runtime.Caching.CacheItemPriority.Default:
					p = System.Web.Caching.CacheItemPriority.Default;
					break;
				case System.Runtime.Caching.CacheItemPriority.NotRemovable:
					p = System.Web.Caching.CacheItemPriority.NotRemovable;
					break;
			}

			return p;
		}
		private List<DictionaryEntry> GetAllItems()
		{
			IEnumerable<DictionaryEntry> cacheItems;
			cacheItems = HttpRuntime.Cache.Cast<DictionaryEntry>();

			return cacheItems.ToList();
		}
		protected string GetRegularCacheKey(string key)
		{
			if(!string.IsNullOrEmpty(key) && key.StartsWith(this.CacheKeyPrefix, StringComparison.OrdinalIgnoreCase))
			{
				return key.Substring(this.CacheKeyPrefix.Length);
			}
			return key;
		}
		public int FlushCache()
		{
			return this.FlushCache(string.Empty);
		}

		private bool disposedValue;
		public override void Dispose(bool disposing)
		{
			if(!disposedValue)
			{
				if(disposing)
				{
					this.FlushCache();
				}
			}
			this.disposedValue = true;
		}
	}
}
