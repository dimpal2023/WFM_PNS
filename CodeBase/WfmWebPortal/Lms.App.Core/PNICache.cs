using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Wfm.App.Core
{
	public class PNICache
	{
		public enum CacheMethod
		{
			InMemory = 0,
			SQLServer = 1
		}
		public PNICache()
		{

		}
		private Caching.LmsCache _cache;
		public Caching.LmsCache AppCache
		{
			get
			{
				if (_cache == null)
				{
					_cache = new Caching.WebCacheDictionary();
				}
				return _cache;
			}
		}
		private object _itemsLocker = new object();
		private Dictionary<string, object> _items;
		public object ItemGet(string key)
		{
			if (_items.ContainsKey(key))
			{
				return _items[key];
			}
			else
				return null;
		}
		public void ItemSet(string key, object value)
		{
			if (!_items.ContainsKey(key))
			{
				lock(_itemsLocker)
				{
					if (!_items.ContainsKey(key))
					{
						_items.Add(key, value);
						return;
					}
				}
			}
			_items[key] = value;
		}
		public void Add(string sKey, object value, bool flagCacheItemAsUpdated = false)
		{
			if (flagCacheItemAsUpdated)
				this.AppCache.Remove(sKey);
			this.AppCache.AddItemToCache(sKey, value, DateTime.Now.AddMinutes(5));
		}
		public void Add(string sKey, object value, DateTime absoluteExpiration, TimeSpan slidingExpiration, bool bIsLocalObjectOnly = false, bool flagCacheItemAsUpdated = false)
		{

			if (flagCacheItemAsUpdated)
				this.AppCache.Remove(sKey);
			
			if(absoluteExpiration == System.Web.Caching.Cache.NoAbsoluteExpiration)
			{
				this.AppCache.AddItemToCache(sKey, value, slidingExpiration);
			}
			else
			{
				this.AppCache.AddItemToCache(sKey, value, absoluteExpiration);
			}
		}
		public object Item(string sKey)
		{
			return this.AppCache.Item(sKey);
		}
		public T Item<T>(string sKey)
		{
			return (T)this.AppCache.Item(sKey);
		}
		public T TryItem<T>(string sKey)
		{
			return (T)this.AppCache.Item(sKey);
		}
		public void Remove(IEnumerable<string> keys)
		{
			this.AppCache.Remove(keys);
		}
		public int FlushCache(string sKeyStartsWith = "")
		{
			return this.AppCache.FlushCache(sKeyStartsWith);
		}
	}
}
