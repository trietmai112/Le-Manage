using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Lib
{
    //use mailkit nuget: https://github.com/jstedfast/MailKit
    public class SendMailService
    {
        public void SendFromCompany(string nameTo, string emailTo, string subject, string body)
        {
            var message = new MimeMessage();
            
            message.From.Add(new MailboxAddress(ConfigurationManager.AppSettings["CompanyEmailFromName"], 
                                ConfigurationManager.AppSettings["CompanyEmailAddress"]));
            message.To.Add(new MailboxAddress(nameTo, emailTo));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {                
                Text = body                
            };

            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect(ConfigurationManager.AppSettings["CompanyEmailSmtpServer"],
                                    int.Parse(ConfigurationManager.AppSettings["CompanyEmailPort"]), false);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                //client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(ConfigurationManager.AppSettings["CompanyEmailAddress"], ConfigurationManager.AppSettings["CompanyEmailPassword"]);

                client.Send(message);
                client.Disconnect(true);
            }
        }

        public void Send(string nameFrom, string mailFrom, string passwordFrom, string nameTo, string emailTo, string subject, string body, string smtpServer, int port)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(nameFrom, mailFrom));
            message.To.Add(new MailboxAddress(nameTo, emailTo));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect(smtpServer,port, false);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                //client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(mailFrom, passwordFrom);

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
