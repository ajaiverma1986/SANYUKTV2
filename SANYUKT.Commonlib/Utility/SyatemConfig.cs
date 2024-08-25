using Org.BouncyCastle.Crypto.Tls;
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
                

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com",587);

                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false ;

                MailAddress from = new MailAddress("alert@sanyuktpay.com","Alert");
                MailAddress to = new MailAddress("ajaibit@gmail.com");

                MailMessage msg = new MailMessage(from, to);

                msg.Subject = "hello";
                msg.Body = "how are u";

                msg.IsBodyHtml = false;

                smtpClient.Send(msg);

                //MailMessage msg = new MailMessage();

                //msg.From = new MailAddress("alert@sanyuktpay.com");
                //msg.To.Add("ajaibit@gmail.com");
                //msg.Subject = "test";
                //msg.Body = "Test Content";
                ////msg.Priority = MailPriority.High;


                //using (SmtpClient client = new SmtpClient())
                //{
                //    client.EnableSsl = true;
                //    client.UseDefaultCredentials = false;
                //    client.Credentials = new NetworkCredential("alert@sanyuktpay.com", "Finpay@12345@");
                //    client.Host = "smtp.gmail.com";
                //    client.Port = 587;
                //    client.DeliveryMethod = SmtpDeliveryMethod.Network;

                //    client.Send(msg);
                //}




            }
            catch (Exception ex)
            {
                SendEmail( "Error In Mail", ex.Message.ToString(), "ajaibit@gmail.com");
            }
        }
    }
}
