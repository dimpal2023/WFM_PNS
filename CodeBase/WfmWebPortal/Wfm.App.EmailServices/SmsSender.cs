using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Wfm.App.EmailServices
{
    public class SmsSender : ISender
    {
        public void Send(Message message)
        {
            string username = "neerajkumarmca";
            string sendername = "naiven";
            string smstype = "TRANS";
            string apikey = "d533577e-d520-49ed-b650-a3281c16cd6e";
            string userAuthenticationURI = string.Format("http://sms.bulksmsind.in/sendSMS?username={0}&message={1}&sendername={2}&smstype={3}&numbers={4}&apikey={5}", username, message.content, sendername, smstype, message.phoneNo, apikey);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(userAuthenticationURI);
            request.Method = "GET";
            request.ContentType = "application/json";
            WebResponse response = request.GetResponse();            
        }
    }
}
