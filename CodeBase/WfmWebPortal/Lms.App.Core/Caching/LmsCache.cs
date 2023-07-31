using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;

namespace Wfm.App.Core.Caching
{
	public abstract class LmsCache : IDisposable
	{
		//Mutex to prevent multiple instances of the cache sync running simultaneously.
		private System.Threading.Mutex _syncCacheMutex;

		private const string FLUSH_ENTIRE_CACHE_DATE = "FLUSH_ENTIRE_CACHE_DATE";
		public LmsCache()
		{
		}
		public void AddItemToCache(string key, object value, TimeSpan slidingExpiration)
		{
			this.Add(key, value, ObjectCache.InfiniteAbsoluteExpiration, slidingExpiration, CacheItemPriority.Default, true);
		}
		public void AddItemToCache(string key, object value, DateTime absoluteExpiration)
		{
			this.Add(key, value, absoluteExpiration, System.Runtime.Caching.ObjectCache.NoSlidingExpiration, CacheItemPriority.Default, true);
		}
		private void Add(string key, object value, DateTimeOffset absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, Boolean localObject)
		{
			//'Sanity check the expirations provided.
			if (absoluteExpiration == DateTimeOffset.MaxValue)
				absoluteExpiration = System.Runtime.Caching.ObjectCache.InfiniteAbsoluteExpiration;
			else if (absoluteExpiration != System.Runtime.Caching.ObjectCache.InfiniteAbsoluteExpiration)
				slidingExpiration = System.Runtime.Caching.ObjectCache.NoSlidingExpiration;

			//This is to support a debug mode in which we essentially bypass cache.
			LmsCacheItemWrapper item = new LmsCacheItemWrapper(value, key, localObject);
			this.AddInternal(key, item, absoluteExpiration, slidingExpiration, priority);
		}
		public void Remove(string key)
		{
			this.RemoveInternal(key);
		}
		public void Remove(IEnumerable<string> keys)
		{
			foreach(string key in keys)
			{
				this.RemoveInternal(key);
			}
		}
		public Int32 FlushCache(string key)
		{
			Int32 removed = FlushCacheInternal(key);
			return removed;
		}
		public object Item(string key)
		{
			return Item(key, true);
		}
		public T Item<T>(string key)
		{
			object o = Item(key);
			if (o is T)
			{
				return (T)o;
			}

			return default(T);
		}
		protected void CacheObjectRemoved(string key, object value, string reason)
		{
			if(value != null)
			{
				if(value is LmsCacheItemWrapper)
				{
                    try
                    {
						((ILmsCacheItem)value).ItemRemoved();

					}
					catch (Exception ex)
                    {

                    }
				}
				if(value is IDisposable)
				{
					((IDisposable)value).Dispose();
				}
			}
			else if (value is IDisposable)
			{
				((IDisposable)value).Dispose();
			}
		}
		private object Item(string key, bool unwrapLmsCacheItem)
		{
			object ret = null;

			object o = this.ItemInternal(key);
			if(o != null)
			{
				if(unwrapLmsCacheItem && o is LmsCacheItemWrapper)
				{
					LmsCacheItemWrapper value = (LmsCacheItemWrapper)o;
					ret = value.Value;
				}
				else
				{
					ret = o;
				}
			}
			return ret;
		}

		protected abstract void AddInternal(string key, LmsCacheItemWrapper value, DateTimeOffset absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority);
		protected abstract object ItemInternal(String key);
		protected abstract void RemoveInternal(string key);
		protected abstract Int32 FlushCacheInternal(String keyStartsWith);
		protected abstract String GetItemKeyInternal(Object value);
		private bool disposedValue;
		public virtual void Dispose(bool disposing)
		{
			if(!this.disposedValue)
			{
				if(disposing)
				{
					this._syncCacheMutex.Dispose();
				}
			}
			this.disposedValue = true;
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
