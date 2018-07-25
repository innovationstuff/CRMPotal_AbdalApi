using System.Web.Mail;
using System;
using System.Net;
using System.Net.Mail;
using System.Configuration;
public class MailSender
{
    public static string Mail
    {
        get
        {
            return ConfigurationManager.AppSettings["CSEmail"].ToString();
        }
    }
    public static string MailPass
    {
        get
        {
            return ConfigurationManager.AppSettings["CSPassword"].ToString();
        }
    }
    public static string ExchangeDomain
    {
        get
        {
            return ConfigurationManager.AppSettings["ExchangeDomain"].ToString();
        }
    }
    public static string SMTPServer
    {
        get
        {
            return ConfigurationManager.AppSettings["SMTPServer"].ToString();
        }
    }

    public static bool SendEmail(
        string pTo,
        string ccEmails,
        string pSubject,
        string pBody,
        System.Web.Mail.MailFormat pFormat,
        string pAttachmentPath)
    {
        if (pTo.Contains("mawarid") || ccEmails.Contains("mawarid"))
        {
            return false;
        }

        //string   Mail = System.Configuration.ConfigurationManager.AppSettings["CSEmail"];
        //string MailPass = System.Configuration.ConfigurationManager.AppSettings["CSPassword"];
        string exchangeDomain = System.Configuration.ConfigurationManager.AppSettings["ExchangeDomain"];
        // pTo = "crm@naas.com.sa";
        //ccEmails = "crm@naas.com.sa";
        try
        {
            System.Web.Mail.MailMessage myMail = new System.Web.Mail.MailMessage();

            NetworkCredential credential = new NetworkCredential(Mail, MailPass, SMTPServer);

            myMail.Fields.Add
                ("http://schemas.microsoft.com/cdo/configuration/smtpserver",
                              SMTPServer);
            myMail.Fields.Add
                ("http://schemas.microsoft.com/cdo/configuration/smtpserverport",
                              "465");
            myMail.Fields.Add
                ("http://schemas.microsoft.com/cdo/configuration/sendusing",
                              "2");
            //sendusing: cdoSendUsingPort, value 2, for sending the message using 
            //the network.

            //smtpauthenticate: Specifies the mechanism used when authenticating 
            //to an SMTP 
            //service over the network. Possible values are:
            //- cdoAnonymous, value 0. Do not authenticate.
            //- cdoBasic, value 1. Use basic clear-text authentication. 
            //When using this option you have to provide the user name and password 
            //through the sendusername and sendpassword fields.
            //- cdoNTLM, value 2. The current process security context is used to 
            // authenticate with the service.
            myMail.Fields.Add
            ("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
            //Use 0 for anonymous
            myMail.Fields.Add
            ("http://schemas.microsoft.com/cdo/configuration/sendusername",
                Mail);
            myMail.Fields.Add
            ("http://schemas.microsoft.com/cdo/configuration/sendpassword",
                 MailPass);
            myMail.Fields.Add
            ("http://schemas.microsoft.com/cdo/configuration/smtpusessl",
                 "true");

            myMail.From = Mail;
            pTo = pTo.Replace(",", ";");
            pTo = pTo.Replace(",", ";");
            myMail.To = pTo;
            myMail.Cc = ccEmails;
            myMail.Subject = pSubject;
            myMail.BodyFormat = pFormat;
            myMail.Body = pBody;
            if (pAttachmentPath.Trim() != "")
            {
                MailAttachment MyAttachment =
                        new MailAttachment(pAttachmentPath);
                myMail.Attachments.Add(MyAttachment);
                myMail.Priority = System.Web.Mail.MailPriority.High;
            }

            System.Web.Mail.SmtpMail.SmtpServer = SMTPServer+":465";
            System.Web.Mail.SmtpMail.Send(myMail);
            return true;
        }
        catch (Exception ex)
        {

            throw;
        }
    }
    public static bool SendEmail02(
         string pTo,
         string ccEmails,
         string pSubject,
         string pBody,
         bool isBodyHTML,
         string pAttachmentPath)
    {
        if (pTo.Contains("mawarid") || ccEmails.Contains("mawarid"))
        {
            return false;
        }

        try
        {
            SmtpClient smtpClient = new SmtpClient();
            NetworkCredential basicCredential = new NetworkCredential(Mail, MailPass);
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.IsBodyHtml = isBodyHTML;
            MailAddress fromAddress = new MailAddress(Mail,"CRM Email");

            // setup up the host, increase the timeout to 5 minutes
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Host = SMTPServer;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = basicCredential;
            smtpClient.Timeout = 100000;
  //          smtpClient.Port = 465;
//            smtpClient.EnableSsl = true;
            message.From = fromAddress;
            message.Subject = pSubject + " - " + DateTime.Now.Date.ToString().Split(' ')[0];
            message.IsBodyHtml = true;
            message.Body = pBody.Replace("\r\n", "<br>");
            string[] to = pTo.Split(';',',');
            foreach (string mailto in to)
            {
                string mailto02 = mailto.Trim(';').Trim(',');
                if (!string.IsNullOrEmpty(mailto02))
                    message.To.Add(mailto02);
            }
            string[] ccs = ccEmails.Split(';', ',');
            for (int i = 0; i < ccs.Length; i++)
            {
                if (!string.IsNullOrEmpty(ccs[i]))
                message.CC.Add(ccs[i]);
            }
            //pTo = pTo.Replace(",", ";");
            //pTo = pTo.Replace(",", ";");
            if (!string.IsNullOrEmpty(pAttachmentPath))
            {
                message.Attachments.Add(new Attachment(pAttachmentPath));
            }
            smtpClient.Send(message);

            
        }
        catch (Exception ex)
        {

            throw;
        }
        return true;
    }
}