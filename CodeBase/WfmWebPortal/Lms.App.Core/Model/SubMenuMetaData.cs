using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wfm.App.Core.Model
{
    public class SubMenuMetaData
    {
        public System.Guid ID { get; set; }
        public System.Guid MENU_ID { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string CONTROLLER_NAME { get; set; }
        public string ACTION_NAME { get; set; }
        public bool ACTIVE { get; set; }
    }
}
