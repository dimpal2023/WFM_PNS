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
            
            log.Info("==============================EmailUtility Started==============================");

            SendMail();

            log.Info("==============================EmailUtility Ended==============================");

  
        }

        static void SendMail()
        {
            
            try
            {

                DataTable dt = GetData();
                if (dt.Rows.Count > 0)
                {
                    Console.WriteLine(System.DateTime.Now + " Total Record Found " + dt.Rows.Count);
                    log.Info(" Total Record Found " + dt.Rows.Count);
                    foreach (DataRow dr in dt.Rows)
                    {
                        try
                        {
                            int mail_seq = int.Parse(dr["MAIL_SEQ"].ToString());
                            log.Info("Mail Sending For , Mail Seq no " + dr["MAIL_SEQ"].ToString() + " , " + dr["USER_ID"].ToString() + " to " + dr["TO_MAIL"].ToString() + " Started");
                            Console.WriteLine(System.DateTime.Now + "Mail Sending For , Mail Seq no " + dr["MAIL_SEQ"].ToString() + " , " + dr["USER_ID"].ToString() + " to " + dr["TO_MAIL"].ToString() + " Started");
                            MailMessage mail = new MailMessage();
                            SmtpClient SmtpServer = new SmtpClient("smtp.office365.com", 587);
                            mail.IsBodyHtml = true;
                            SmtpServer.EnableSsl = true;
                            SmtpServer.UseDefaultCredentials = false;

                            SmtpServer.Credentials = new System.Net.NetworkCredential("karamsupport@karam.in", "India@123");

                            mail.From = new MailAddress("karamsupport@karam.in");
                            mail.To.Add(dr["TO_MAIL"].ToString());
                            if (dr["CC_MAIL"]!= null) 
                            {
                                //mail.CC.Add(dr["CC_MAIL"].ToString());
                            }
                            mail.Subject = "WFM Alert | " + dr["MAIL_REMARK"].ToString();
                            mail.Body = dr["MAIL_CONTENT"].ToString();

                          
                                SmtpServer.Send(mail);
                                log.Info("Mail Sent For , Mail Seq no " + dr["MAIL_SEQ"].ToString() + " , " + dr["USER_ID"].ToString() + " to " + dr["TO_MAIL"].ToString() );
                                Console.WriteLine(System.DateTime.Now + "Mail Sent For , Mail Seq no " + dr["MAIL_SEQ"].ToString() + " , " + dr["USER_ID"].ToString() + " to " + dr["TO_MAIL"].ToString() );

                            if (UpdateData(dr["MAIL_SEQ"].ToString()) == 1)
                                {
                                log.Info("Data Updated Successfully , Mail Seq no " + dr["MAIL_SEQ"].ToString() + " , " + dr["USER_ID"].ToString() + " to " + dr["TO_MAIL"].ToString() + " Started");
                                Console.WriteLine(System.DateTime.Now + "Data Updated Successfully , Mail Seq no " + dr["MAIL_SEQ"].ToString() + " , " + dr["USER_ID"].ToString() + " to " + dr["TO_MAIL"].ToString() );

                            }
                            else
                                {
                                log.Error("Data Error while updating in DB , Mail Seq no " + dr["MAIL_SEQ"].ToString() + " , " + dr["USER_ID"].ToString() + " to " + dr["TO_MAIL"].ToString() );
                                Console.WriteLine(System.DateTime.Now + "Data Error while updating in DB , Mail Seq no " + dr["MAIL_SEQ"].ToString() + " , " + dr["USER_ID"].ToString() + " to " + dr["TO_MAIL"].ToString() );

                            }

                        }
                        catch (Exception ex)
                        {
                            log.Error("Error Sending Mail , Mail Seq no " + dr["MAIL_SEQ"].ToString() + " , " + dr["USER_ID"].ToString() + " to " + dr["TO_MAIL"].ToString() + " : Exception - " + ex.Message);
                            Console.WriteLine(System.DateTime.Now + "Error Sending Mail , Mail Seq no " + dr["MAIL_SEQ"].ToString() + " , " + dr["USER_ID"].ToString() + " to " + dr["TO_MAIL"].ToString() + " : Exception - " + ex.Message);

                          
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                // exception logging
                log.Error("Error Fetching Data From DB : Exception - " + ex.Message);
                Console.WriteLine(System.DateTime.Now + "Error Fetching Data From DB : Exception - " + ex.Message);

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
            log.Info("Mail sending Data fetched From DB Sucessfully");
            Console.WriteLine(System.DateTime.Now + " Mail sending Data fetched From DB Sucessfully");
            return dt;
        }

        static int UpdateData(string mail_seq)
        {
            int Updated = 0;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr"].ToString());
            try
            {
                SqlCommand cmd = new SqlCommand("Update TAB_ALL_MAIL set MAIL_SENT_DATE = GETDATE(), MAIL_FLAG=1 where mail_seq='" + mail_seq + "';", con);

                con.Open();

                cmd.ExecuteNonQuery();

                Updated = 1;
            }
            catch (Exception ex)
            {
                log.Error("Error updating Data For " + mail_seq + " : Exception " + ex.Message);
                Console.WriteLine(System.DateTime.Now + " Error updating Data For " + mail_seq + " : Exception " + ex.Message);
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
