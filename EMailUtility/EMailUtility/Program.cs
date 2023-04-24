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
                            int mail_seq = int.Parse(dr["MAIL_SEQ"].ToString());
                            log.Info("Mail Sending For " + dr["USER_ID"].ToString() + " to " + dr["TO_MAIL"].ToString() + " Started");
                            MailMessage mail = new MailMessage();
                            SmtpClient SmtpServer = new SmtpClient("smtp.office365.com", 587);

                            mail.From = new MailAddress("karamsupport@karam.in");
                            mail.To.Add(dr["TO_MAIL"].ToString());
                            mail.IsBodyHtml = true;
                            if (dr["CC_MAIL"].ToString() != null && dr["CC_MAIL"].ToString() != "")
                            {
                                mail.CC.Add(dr["CC_MAIL"].ToString());
                            }
                            mail.Subject = dr["MAIL_REMARK"].ToString();
                            mail.Body = dr["MAIL_CONTENT"].ToString();
                            SmtpServer.Credentials = new System.Net.NetworkCredential("karamsupport@karam.in", "Angel@123");                            
                            SmtpServer.EnableSsl = true;
                            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                            //Add this line to bypass the certificate validation
                            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                                    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                                    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                            {
                                return true;
                            };

                            //if (mail_seq == 16 || mail_seq == 17 || mail_seq == 18)
                            //{
                                SmtpServer.Send(mail);
                                log.Info("Sent Mail For " + dr["USER_ID"].ToString() + " " + dr["TO_MAIL"].ToString() + " SuccessFully");
                                //flag = true;

                                if (UpdateData(dr["MAIL_SEQ"].ToString()) == 1)
                                {
                                    log.Info("Data For " + dr["USER_ID"].ToString() + " Updated SuccessFully");
                                    // success logging
                                }
                                else
                                {
                                    log.Error("Error updating Data For " + dr["USER_ID"].ToString());
                                    // error logging
                                }
                            //}
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

            string qry = "select * from TAB_ALL_MAIL where MAIL_FLAG is null and MAIL_SENT_DATE is null;";
            da = new SqlDataAdapter(qry, con);
            dt = new DataTable();
            da.Fill(dt);
            log.Info("Fetched Data From DB Sucessfully");
            return dt;
        }

        static int UpdateData(string ID)
        {
            int Updated = 0;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr"].ToString());
            try
            {
                SqlCommand cmd = new SqlCommand("Update TAB_ALL_MAIL set MAIL_SENT_DATE = GETDATE(), MAIL_FLAG=1 where MAIL_SEQ=" + ID + ";", con);

                con.Open();

                cmd.ExecuteNonQuery();
                
                Updated = 1;
            }
            catch (Exception ex)
            {
                log.Error("Error updating Data For " + ID + " : Exception " + ex.Message);                
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
