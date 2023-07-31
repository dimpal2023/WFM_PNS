using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Wfm.App.Core.Attribute;
using System.Web;

namespace Wfm.App.Core.Model
{
    public partial class TrainningMasterMetaData
    {
        public System.Guid TRAINNING_ID { get; set; }
        public System.Guid CMP_ID { get; set; }
        public string CMP_NAME { get; set; }
        public string DEPT_NAME { get; set; }
        public string SUB_NAME { get; set; }
        public string TRAINNING_NAME { get; set; }
        public Nullable<System.Guid> DEPT_ID { get; set; }
        public Nullable<System.Guid> SUBDEPT_ID { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }
        public Nullable<System.DateTime> UPDATED_DATE { get; set; }
        public System.DateTime? EXPECTED_DATE { get; set; }
        [DataType(DataType.Date)]
        public string UPDATED_BY { get; set; }
        public string STATUS { get; set; }
    }

    public class TrainningByDepartments
    {
        public IEnumerable<SelectListItem> Departments { get; set; }
        public IEnumerable<SelectListItem> SubDepartments { get; set; }
        public IEnumerable<SelectListItem> Buildings { get; set; }
        public Guid DEPARTMENT_ID { get; set; }
        public Guid SUBDEPARTMENT_ID { get; set; }
        public Guid BUILDING_ID { get; set; }
    }

    public class AddTrainningMetaData
    {
        public IEnumerable<SelectListItem> Buildings { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }

        [Display(Name = "Unit"), Required]
        public Guid BUILDING_ID { get; set; }

        [Display(Name = "Department"), Required]
        public Nullable<System.Guid> DEPT_ID { get; set; }


        [Display(Name = "Sub Department"), Required]
        public Nullable<System.Guid> SUBDEPT_ID { get; set; }
        public System.Guid TRAINNING_ID { get; set; }
        public System.Guid CMP_ID { get; set; }
        public string CMP_NAME { get; set; }
        public string DEPT_NAME { get; set; }
        public string SUBDEPT_NAME { get; set; }
        public string TRAINNING_NAME { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }

        //[Display(Name = "Expected Date"), Required]
        //public Nullable<System.DateTime> EXPECTED_DATE { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Expected Date"), Required]
        //public System.DateTime EXPECTED_DATE { get; set; }
        public Nullable<System.DateTime> EXPECTED_DATE { get; set; }

        public Nullable<System.DateTime> UPDATED_DATE { get; set; }
        public string UPDATED_BY { get; set; }
        public string STATUS { get; set; }
    }

    public class TrainningWorkforceMetaData
    {
        public byte[] PHOTO { get; set; }
        public string PHOTOBase64 { get; set; }
        [FileSize(80000000)]
        [Required(ErrorMessage = "Select Group Photo file.")]
        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.gif|.jpeg|.PNG|.JPG)$", ErrorMessage = "Only Image files allowed.")]
        public HttpPostedFileBase PHOTOfile { get; set; }
       

        [Required(ErrorMessage = "Required Department")]
        public System.Guid DEPT_ID { get; set; }
        public System.Guid? SUBDEPT_ID { get; set; }
        [Required(ErrorMessage = "Required Employee")]
        public Guid WF_ID { get; set; }
        [Required(ErrorMessage = "Required Employee")]
        public string EMP_NAME { get; set; }
        public string[] EMP_NAMES { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<Guid> CMP_ID { get; set; }
        public Guid BUILDING_ID { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }
        public List<AddWorkforceMappingTrainning> ListMetaDatas { get; set; }
        public short WF_EMP_TYPE { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime? FROM_DATE { get; set; }
        public System.DateTime? TO_DATE { get; set; }
        public string ISCOMPLETED { get; set; }

        
    } 

    public class AddWorkforceMappingTrainning
    {
        [Required(ErrorMessage = "Required Trainnning")]
        public System.Guid TRAINNING_ID { get; set; }
        [Required(ErrorMessage = "Required Start Date")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? START_DATE { get; set; }
        [Required(ErrorMessage = "Required End Date")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? END_DATE { get; set; }
        [Required(ErrorMessage = "Required Department")]
        public string Venue { get; set; }
        public string Time { get; set; }
        public string TRAINNER_NAME { get; set; }
        public string ISCOMPLETED { get; set; }
        public Nullable<Guid> DEPT_ID { get; set; }
        public IEnumerable<SelectListItem> TrainningMasterByDepart { get; set; }
        public string PRESENTED_EMP { get; set; }
        public System.Guid TRAINNING_MAPPING_ID { get; set; }

       
    }

    public class AddWorkforceTrainningMetaData
    {
        public List<AddWorkforceMappingTrainning> ListMetaDatas { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }
        public IEnumerable<SelectListItem> TrainningMasterByDepart { get; set; }
        public byte[] PHOTO { get; set; }
        public string PHOTOBase64 { get; set; }
        [FileSize(80000000)]
        [Required(ErrorMessage = "Select Group Photo file.")]
        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.gif|.jpeg|.PNG|.JPG)$", ErrorMessage = "Only Image files allowed.")]
        public HttpPostedFileBase PHOTOfile { get; set; }
    }

    public class WorkforceTrainningStatus
    {
        public System.Guid TRAINNING_WORKFORCE_ID { get; set; }
        public string CMP_NAME { get; set; }
        public string DEPT_NAME { get; set; }
        public string TRAINNING_NAME { get; set; }
        public Nullable<System.Guid> DEPT_ID { get; set; }
        public string STATUS { get; set; }
    }

    public class GetTRAINNING_WORKFORCE
    {
        public System.Guid TRAINNING_WORKFORCE_ID { get; set; }
        public string WORKFORCE_NAME { get; set; }
        public string ISCOMPLETED { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> UPDATED_DATE { get; set; }
        public string UPDATED_BY { get; set; }
        public string STATUS { get; set; }
        public string DEPT_NAME { get; set; }
        public string SUBDEPT_NAME { get; set; }
        public string VENUE { get; set; }

        public List<TRAINNING_WORKFORCE_MAPPING> TrainningMapping { get; set; }
    }
    public class TRAINNING_WORKFORCE_MAPPING
    {
        public System.Guid TRAINNING_MAPPING_ID { get; set; }
        public System.Guid TRAINNING_WORKFORCE_ID { get; set; }
        public System.Guid TRAINNING_ID { get; set; }
        public Nullable<Guid> DEPT_ID { get; set; }
        public Nullable<Guid> SUBDEPT_ID { get; set; }
        public Guid BUILDING_ID { get; set; }
        public short WF_EMP_TYPE { get; set; }
        public string EMP_NAME { get; set; }
        public string[] EMP_NAMES { get; set; }
        public string DEPT_NAME { get; set; }
        public string SUBDEPT_NAME { get; set; }
        public string WORKFORCE_NAME { get; set; }
        public string WORKFORCE_CODE { get; set; }
        public string TRAINNING_CODE { get; set; }
        
        public System.DateTime TRAINNING_START_DATE { get; set; }
        public System.DateTime TRAINNING_END_DATE { get; set; }
        public Nullable<System.DateTime> ACTUAL_START_DATE { get; set; }
        public Nullable<System.DateTime> ACTUAL_END_DATE { get; set; }
        public string ISTRAINNINGCOMPLETED { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }
        public IEnumerable<SelectListItem> TrainningMasterByDepart { get; set; }
        public List<AddWorkforceMappingTrainning> ListMetaDatas { get; set; }
        public string CreatedBy { get; set; }
        public string TRAINNING_NAME { get; set; }
        public string TRAINNING_TIME{ get; set; }
        public string TRAINNING_VENUE{ get; set; }
        public byte[] PHOTO { get; set; }
        public string PHOTOBase64 { get; set; }
        [FileSize(80000000)]
        [Required(ErrorMessage = "Select Group Photo file.")]
        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.gif|.jpeg|.PNG|.JPG)$", ErrorMessage = "Only Image files allowed.")]
        public HttpPostedFileBase PHOTOfile { get; set; }
    }

   
}
