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




                MailMessage msg = new MailMessage();

                msg.From = new MailAddress("alert@sanyuktpay.com");
                msg.To.Add("ajaibit@gmail.com");
                msg.Subject = "test";
                msg.Body = "Test Content";
                //msg.Priority = MailPriority.High;


                using (SmtpClient client = new SmtpClient())
                {
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("alert@sanyuktpay.com", "Finpay@12345@");
                    client.Host = "smtp.gmail.com";
                    client.Port = 587;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;

                    client.Send(msg);
                }




            }
            catch (Exception ex)
            {
                SendEmail( "Error In Mail", ex.Message.ToString(), "ajaibit@gmail.com");
            }
        }
    }
}
