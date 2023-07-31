using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wfm.App.Core.Caching
{
	public class LmsCacheItemWrapper
	{
		private object _value;

		public object Value {
			get {
				return Value;
			}
		}

		private string _key;
		public string Key
		{
			get
			{
				return _key;
			}
		}

		private DateTime _dateAdded;
		public DateTime DateAdded {
			get
			{
				return _dateAdded;
			}
		}

		private bool _localObject;
		public bool LocalObject {
			get
			{
				return _localObject;
			}
		}

		public LmsCacheItemWrapper(object value, string key, bool localObject)
		{
			this._key = key;
			this._value = value;
			DateTime ld = DateTime.UtcNow;
			this._dateAdded = new DateTime(ld.Year, ld.Month, ld.Day,ld.Hour, ld.Minute, ld.Second,0, DateTimeKind.Utc);
			this._localObject = localObject;
		}
	}
}
