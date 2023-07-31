using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wfm.App.Core.Model
{
    public class StateMetaData
    {
        public System.Guid STATE_ID { get; set; }
        public string STATE_NAME { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public char STATUS { get; set; }
    }

    public class StateCityMetaData
    {
        public System.Guid STATE_ID { get; set; }
        public System.Guid CITY_ID { get; set; }
        public string CITY_NAME { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string STATUS { get; set; }
    }
}
