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

        public bool SendEmail(string Reciever, string Subject, string Message)
        {
            if (Reciever == "" || Reciever == null)
                throw new Exception("Reciever mail cannot be empty!");
            if (Subject == "" || Subject == null)
                throw new Exception("Subject cannot be empty!");
            if (Message == "" || Message == null)
                throw new Exception("The message cannot be empty!");

            MailMessage mail = new MailMessage(MailFrom, Reciever, Subject, Message);
            var AlternameView = AlternateView.CreateAlternateViewFromString(Message, new ContentType("text/html"));
            mail.AlternateViews.Add(AlternameView);

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