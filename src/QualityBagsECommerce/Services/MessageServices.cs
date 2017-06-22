using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityBagsECommerce.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        //public Task SendEmailAsync(string email, string subject, string message)
        //{
        //    // Plug in your email service here to send an email.
        //    return Task.FromResult(0);
        //}

        public Task SendEmailAsync(string email, string subject, string message)
        {

            var mes = new MimeMessage();
            mes.From.Add(new MailboxAddress("Quality_Bag_Service", "lim92@myunitec.ac.nz"));
            mes.To.Add(new MailboxAddress("User", email));
            mes.Subject = subject;

            mes.Body = new TextPart("plain")
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;


                client.Connect("smtp.office365.com", 587, false);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("lim92@myunitec.ac.nz", "limeng0427");

                client.Send(mes);
                client.Disconnect(true);
            }

            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
