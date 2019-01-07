using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.IO;
using System.Net;
using System.Net.Mime;

namespace Logic
{
    public class EmailHandler
    {
       
        string MailSmtpHost = "smtp.gmail.com";
        int MailSmtpPort= 587;
        string MailSmtpUsername = "BrightLearnSystem@gmail.com";
        string MailSmtpPassword = "BrightLearn1!";
        string MailFrom = "BrightLearnSystem@gmail.com";

        public bool SendEmail(string to, string subject, string body)
        {
            if (to == "" || to == null)
                throw new Exception("Reciever mail cannot be empty!");
            if (subject == "" || subject == null)
                throw new Exception("Subject cannot be empty!");
            if (body == "" || body == null)
                throw new Exception("The message cannot be empty!");

            MailMessage mail = new MailMessage(MailFrom, to, subject, body);
            var alternameView = AlternateView.CreateAlternateViewFromString(body, new ContentType("text/html"));
            mail.AlternateViews.Add(alternameView);

            var smtpClient = new SmtpClient(MailSmtpHost, MailSmtpPort);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(MailSmtpUsername, MailSmtpPassword);
            try
            {
                smtpClient.Send(mail);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return true;
        }
    }
}