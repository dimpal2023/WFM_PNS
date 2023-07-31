﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wfm.App.Core.Model
{
    public class WorkforceEduMasterMetaData
    {
        public short WF_EDUCATION_ID { get; set; }
        public string WF_COURSE_NAME { get; set; }
        public System.Guid COMPANY_ID { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string STATUS { get; set; }
    }
}
