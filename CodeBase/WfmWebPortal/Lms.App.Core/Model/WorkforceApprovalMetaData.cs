using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Wfm.App.Core.Model
{
   public class WorkforceApprovalMetaData: WorkflowMasterVieweMetaData
    {
        public IEnumerable<SelectListItem> Trainning_Status { get; set; }
        public string Status { get; set; }

    }
}
