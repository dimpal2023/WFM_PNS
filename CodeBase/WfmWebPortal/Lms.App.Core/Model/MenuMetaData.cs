namespace Wfm.App.Core.Model
{
    public class MenuMetaData
    {        
        public System.Guid ID { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public bool ACTIVE { get; set; }
        public string ICONCLASS { get; set; }

        public string CONTROLLER_NAME { get; set; }
    }
}
