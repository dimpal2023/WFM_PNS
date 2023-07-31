using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Wfm.App.Core.Model;
using System.Collections.Generic;

namespace Wfm.App.Core.Model
{
    public class WorkforcePhotoMasterMetaData
    {
        public System.Guid WF_ID { get; set; }
        public byte[] PHOTO { get; set; }
        public byte[] EMP_SIGNATURE { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> UPDATED_DATE { get; set; }
        public string UPDATED_BY { get; set; }
    }
}
