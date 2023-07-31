using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Wfm.App.Core.Model
{
    public class ItemMasterMetaData
    {
        public System.Guid ITEM_ID    { get; set; }
        public System.Guid ITEM_CODE_ID { get; set; }
        public System.Guid SUBDEPT_ID { get; set; }
        public System.Guid DEPT_ID    { get; set; }
        public string ITEM_NAME { get; set; }
        public string ITEM_CODE_NAME { get; set; }
        public string DEPT_NAME { get; set; }
        public string SUBDEPT_NAME { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }

        //public List<ItemMasterMetaDataList> listMetaDatas { get; set; }
    }

    //public class ItemMasterMetaDataList
    //{
    //    public System.Guid ITEM_ID { get; set; }
    //    public System.Guid ITEM_CODE_ID { get; set; }
    //    public System.Guid SUBDEPT_ID { get; set; }
    //    public System.Guid DEPT_ID { get; set; }
    //    public string ITEM_NAME { get; set; }
    //    public string ITEM_CODE_NAME { get; set; }
    //    public string DEPT_NAME { get; set; }
    //    public string SUBDEPT_NAME { get; set; }
    //}
}
