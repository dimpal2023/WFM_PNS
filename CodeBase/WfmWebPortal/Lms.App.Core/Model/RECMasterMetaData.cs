using System;

namespace Wfm.App.Core.Model
{
    public class RECMasterMetaData
    {
        public short REC_TYPE { get; set; }
        public string REC_NAME { get; set; }
        public System.Guid COMPANY_ID { get; set; }
        public Nullable<System.Guid> WORKFLOW_ID { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string STATUS { get; set; }
    }
}
