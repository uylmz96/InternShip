using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web;

namespace InternShip.MvcUI.App_Classes
{
    public static class Mail
    {
        public static void sendMailUseThread(string To, string Subject, string Body)
        {

            Thread mailThread = new Thread(() => sendMail(To, Subject, Body));
            mailThread.Start();

        }

        public static string sendMail(string To, string Subject, string Body)
        {
            try
            {
                var message = new MailMessage();
                message.From = new MailAddress("meubilgisayarmuhstaj@gmail.com");
                message.To.Add(new MailAddress(To));
                message.Subject = Subject;
                message.Body = Body;
                message.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                var credential = new NetworkCredential
                {
                    UserName = "meubilgisayarmuhstaj@gmail.com",
                    Password = "aQW75TETvyAm][{@"
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;//25 dene
                smtp.EnableSsl = true;
                smtp.Send(message);
                return "successMessage('" + To + " mail adresine yeni şifreniz gönderilmiştir.')";
            }
            catch (Exception ex)
            {
                return "errorMessage('Mail gönderme başarısız.');";
            }
        }

       
    }
}