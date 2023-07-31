using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wfm.App.Core.Model
{
    public class ShiftMasterMetaData
    {
        public System.Guid SHIFT_ID { get; set; }

        [Required]
        [DisplayName("Name")]
        public string SHIFT_NAME { get; set; }
        
        public List<CompanyMasterMetaData> COMPANIES { get; set; }        

        public Guid COMPANY_ID { get; set; }
        public string COMPANY_NAME { get; set; }

        [Required]
        [DisplayName("Start Time")]
        public Nullable<System.TimeSpan> SHIFT_START_TIME { get; set; }

        [Required]
        [DisplayName("End Time")]
        public Nullable<System.TimeSpan> SHIFT_END_TIME { get; set; }
        public System.DateTime created_date { get; set; }
        public string Created_by { get; set; }
        public Nullable<System.DateTime> UPDATED_DATE { get; set; }
        public string UPDATED_BY { get; set; }
        public string status { get; set; }
        public int ID { get; set; }

    }
}
