using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Net.Mail;

namespace EMailUtility
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
        (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            //bool flag;
            //if (SendMail() == true)
            //{
            //    flag = true;
            //}
            //else
            //{
            //    Error logger
            //}
            log.Info("==============================EmailUtility Started==============================");

            SendMail();

            log.Info("==============================EmailUtility Ended==============================");

            //GridView1.DataSource = ds;
            //GridView1.DataBind();
        }

        static void SendMail()
        {
            //bool flag = false;

            try
            {
                
                DataTable dt = GetData();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        try
                        {
                        //    string to = "dimpal.gupta@samparksolutions.com";
                        //    string from = "neerajkumarmca@gmail.com";
                        //    MailMessage message = new MailMessage(from, to);
                        //    message.Subject = "Using the new SMTP client.";
                        //    message.Body = @"Using this new feature, you can send an email message from an application very easily.";
                        //    SmtpClient client = new SmtpClient("smtp.office365.com",587);
                        //    // Credentials are necessary if the server requires the client
                        //    // to authenticate before it will send email on the client's behalf.
                        //    client.UseDefaultCredentials = true;
                        //    client.EnableSsl = true;
                        //    client.Send(message);
                        //}

                        
                        int mail_seq = int.Parse(dr["MAIL_SEQ"].ToString());
                        log.Info("Mail Sending For " + dr["USER_ID"].ToString() + " to " + dr["TO_MAIL"].ToString() + " Started");
                        MailMessage mail = new MailMessage();
                        //SmtpClient SmtpServer = new SmtpClient("webmail.emailsrvr.com", 25);

                        SmtpClient SmtpServer = new SmtpClient("smtp.office365.com");
                        SmtpServer.UseDefaultCredentials = true;
                        SmtpServer.EnableSsl = true;
                        mail.From = new MailAddress("itsupport@karam.in");
                        mail.To.Add(dr["TO_MAIL"].ToString());
                        if (dr["CC_MAIL"] != null)
                        {
                            //mail.CC.Add(dr["CC_MAIL"].ToString());
                        }
                        mail.Subject = "Test Mail";
                        mail.Body = dr["MAIL_CONTENT"].ToString();

                        SmtpServer.Port = 587;
                        //SmtpServer.Credentials = new System.Net.NetworkCredential("karamsupport@karam.in", "Passw0rd");                            
                        //SmtpServer.EnableSsl = true;


                        //Add this line to bypass the certificate validation
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                                System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                System.Security.Cryptography.X509Certificates.X509Chain chain,
                                System.Net.Security.SslPolicyErrors sslPolicyErrors)
                        {
                            return true;
                        };


                            SmtpServer.Send(mail);
                            log.Info("Sent Mail For " + dr["USER_ID"].ToString() + " " + dr["TO_MAIL"].ToString() + " SuccessFully");
                            //flag = true;

                            if (UpdateData(dr["USER_ID"].ToString()) == 1)
                            {
                                log.Info("Data For " + dr["USER_ID"].ToString() + " Updated SuccessFully");
                                // success logging
                            }
                            else
                            {
                                log.Error("Error updating Data For " + dr["USER_ID"].ToString());
                                // error logging
                            }

                    }
                    
                        catch (Exception ex)
                        {
                            // Exception logs here
                            log.Info("Error Sending Mail For " + dr["USER_ID"].ToString() + " " + dr["TO_MAIL"].ToString() + " : Exception - " + ex.Message);
                        }
                        
                    }
                }

            }
            catch (Exception ex)
            {
                // exception logging
                log.Error("Error Fetching Data From DB : Exception - " + ex.Message);                
            }
            //return flag;
        }

        static DataTable GetData()
        {
            DataTable dt;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr"].ToString());
            SqlDataAdapter da;
            //DataSet ds;

            string qry = "select * from TAB_ALL_MAIL where isnull(MAIL_FLAG,0) =0 and MAIL_SENT_DATE is null;";
            da = new SqlDataAdapter(qry, con);
            dt = new DataTable();
            da.Fill(dt);
            log.Info("Fetched Data From DB Sucessfully");
            return dt;
        }

        static int UpdateData(string userid)
        {
            int Updated = 0;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr"].ToString());
            try
            {
                SqlCommand cmd = new SqlCommand("Update TAB_ALL_MAIL set MAIL_SENT_DATE = GETDATE(), MAIL_FLAG=1 where USER_ID=" + userid + ";", con);

                con.Open();

                cmd.ExecuteNonQuery();
                
                Updated = 1;
            }
            catch (Exception ex)
            {
                log.Error("Error updating Data For " + userid + " : Exception " + ex.Message);                
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            return Updated;
        }
    }
}
