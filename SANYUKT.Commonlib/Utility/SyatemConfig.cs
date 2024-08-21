using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SANYUKT.Commonlib.Utility
{
    public class SyatemConfig
    {
        public void SendEmail( string Subject, string Bodyyy, string Tomail)
        {
            try
            {
                //using (MailMessage mail = new MailMessage())
                //{
                //    mail.From = new MailAddress("alert@sanyuktpay.com");
                //    mail.To.Add("somebody@domain.com");
                //    mail.Subject = "Hello World";
                //    mail.Body = "<h1>Hello</h1>";
                //    mail.IsBodyHtml = true;


                //    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                //    {
                //        smtp.Credentials = new NetworkCredential("alert@sanyuktpay.com", "X9u%54m4=&VH3aX<");
                //        smtp.EnableSsl = true;
                //        smtp.Send(mail);
                //    }
                //}




                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("alert@sanyuktpay.com");
                mail.To.Add(Tomail);

                mail.Subject = Subject;

                mail.IsBodyHtml = true;

                mail.Body = Bodyyy;
                SmtpServer.Port = 587;

                SmtpServer.Credentials = new System.Net.NetworkCredential("alert@sanyuktpay.com", "X9u%54m4=&VH3aX<");
                SmtpServer.EnableSsl = true;
                SmtpServer.UseDefaultCredentials = false;

                SmtpServer.Send(mail);


            }
            catch (Exception ex)
            {
                SendEmail( "Error In Mail", ex.Message.ToString(), "ajaibit@gmail.com");
            }
        }
    }
}
