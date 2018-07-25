using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mail;

namespace NasAPI.Inferstructures
{
    public class SendEmail
    {


        public  string sendEmail(string ReciverMail,String Message,String subject)
        {
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();

            msg.From = new MailAddress("YourMail@gmail.com");
            msg.To.Add(ReciverMail);
            msg.Subject = subject + DateTime.Now.ToString();
            msg.Body = Message;
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            string Email = ConfigurationManager.AppSettings["CSEmail"];
            string Password = ConfigurationManager.AppSettings["CSPassword"];

            client.Credentials = new NetworkCredential("YourMail@gmail.com", "YourPassword");
            client.Timeout = 20000;
            try
            {
                client.Send(msg);
                return "Mail has been successfully sent!";
            }
            catch (Exception ex)
            {
                return "Fail Has error" + ex.Message;
            }
            finally
            {
                msg.Dispose();
            }
        }

    }
}