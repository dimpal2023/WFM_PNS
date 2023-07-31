using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Wfm.App.Core.Model
{
    public class GatePassMetaData
    {
        public System.Guid ID { get; set; }        
        public WorkforceMasterMetaData WORKFORCE { get; set; }

        public Guid DEPT_ID { get; set; }
        public string DEPT_NAME { get; set; }
        public Guid SUBDEPT_ID { get; set; }
        public string SUBDEPT_NAME { get; set; }
        public short WF_EMP_TYPE { get; set; }

        public string[] EMP_NAME { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Start Date")]        
        public System.DateTime? START_DATE { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("End Date")]
        public System.DateTime? END_DATE { get; set; }

        [DisplayName("Out Time")]
        public System.TimeSpan OUT_TIME { get; set; }

        [DisplayName("In Time")]
        public System.TimeSpan IN_TIME { get; set; }

        [DisplayName("Actual Out Time")]
        public System.TimeSpan ACTUAL_OUTTIME { get; set; }

        [DisplayName("Actual In Time")]
        public System.TimeSpan ACTUAL_INTIME { get; set; }
        public string PURPOSE { get; set; }    
        public string REMARK { get; set; }      
        public Nullable<bool> STATUS { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> UPDATED_DATE { get; set; }
        public string UPDATED_BY { get; set; }

        [DisplayName("MOBILE NO")]
        //[Required(ErrorMessage = "Required")]
        public string MOBILE_NO { get; set; }
        public string WORKFORCE_IDS { get; set; }
        public Guid BUILDING_ID { get; set; }
    }

    public class GatePassAllItemsMetaData
    {
        public Guid DEPT_ID { get; set; }
        public string DEPT_NAME { get; set; }
        public int STATUS_ID { get; set; }
        public string STATUS_NAME { get; set; }
        public Guid SUBDEPT_ID { get; set; }
        public Guid BUILDING_ID { get; set; }
        public string SUBDEPT_NAME { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? FROM_DATE { get; set; }
        public DateTime? TO_DATE { get; set; }
        public List<GatePassMetaData> ALLITEMS { get; set; }
        public List<GatePassMetaData> TODAY { get; set; }
        public List<GatePassMetaData> YESTERDAY { get; set; }
    }
}
