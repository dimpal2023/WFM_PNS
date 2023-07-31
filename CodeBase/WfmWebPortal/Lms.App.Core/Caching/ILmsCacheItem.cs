using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wfm.App.Core.Caching
{
	public interface ILmsCacheItem
	{
		void ItemRemoved();
	}
}
