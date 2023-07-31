using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wfm.App.Core.Model;

namespace Wfm.App.EmailServices
{
    public abstract class Message
    {
        public string content;
        public string recieverAddress;
        public string senderAddress;
        public string subject;
        public string linkToQRCode;
        public string phoneNo;

        public abstract string CreateMessage(GatePassMetaData gatepass);    
    }
}
