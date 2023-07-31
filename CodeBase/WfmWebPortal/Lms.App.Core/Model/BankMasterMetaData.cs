using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wfm.App.Core.Model
{
    public class BankMasterMetaData
    {
        public System.Guid BANK_ID { get; set; }
        public string BANK_NAME { get; set; }
        public string COMPANY_NAME { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public string STATUS { get; set; }
        public string IFSC_CODE { get; set; }
        public string BRANCH_NAME { get; set; }
    }
}
