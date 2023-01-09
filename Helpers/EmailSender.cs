using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Net.Mail;
using System.Net;
using System.Reflection.Emit;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Hosting;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace mss_project.Helpers
{
    public class EmailSender
    {
        private string email;
        private string password;
        private string smtp_server;
        private SmtpClient smtpClient;
        private static EmailSender instance = null;

        private EmailSender(string secret_store)
        {
            
            email = System.IO.File.ReadAllText(string.Format("{0}\\email.txt", secret_store));
            password = System.IO.File.ReadAllText(string.Format("{0}\\pass.txt", secret_store));
            smtp_server = System.IO.File.ReadAllText(string.Format("{0}\\smtp.txt", secret_store));
            Console.Write(email + " " + password + " " + smtp_server);
            smtpClient = new SmtpClient(smtp_server)
            {
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(email, password)
            };
            
        }

        public static EmailSender getInstance(string secret_store) 
        { 
            if (instance == null)
            {
                instance = new EmailSender(secret_store);
            }
            return instance;
        }

        public void sendEmail(string receiverEmail, string subject, string body)
        {
            Console.Write("SendEmail:\n" + receiverEmail + " " + subject + " " + body);
            using (var mess = new MailMessage(email, receiverEmail)
            {
                Subject = subject,
                Body = body
            })
            {
                smtpClient.Send(mess);
            }
        }
    }
}