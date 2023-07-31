using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Wfm.App.EmailServices
{
    enum MessageType
    {
        Email = 1,
        SMS = 2        
    }

    public class EmailSender : ISender
    {
        public void Send(Message message)
        {            
            var senderEmail = new MailAddress(message.senderAddress, "Sumit");
            var receiverEmail = new MailAddress(message.recieverAddress, "Receiver");
            var password = "";
            var subject = "Gate Pass";
            var body = message;
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };
            using (var mess = new System.Net.Mail.MailMessage(senderEmail, receiverEmail)
            {
                Subject = subject,
                Body = message.content,
                IsBodyHtml = true               
            })
            {
                smtp.Send(mess);
            }
        }
    }
}
