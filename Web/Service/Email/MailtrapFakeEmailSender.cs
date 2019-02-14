using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Web.Service.Email
{
    public class MailtrapFakeEmailSender : EmailSender
    {
        private SmtpClient Client { get; }
        
        public MailtrapFakeEmailSender(IConfiguration config)
        {
            Client = new SmtpClient(config["MailtrapHost"], Int32.Parse(config["MailtrapPort"]))
            {
                Credentials = new NetworkCredential(config["MailtrapUsername"], config["MailtrapPassword"]),
                EnableSsl = true
            };
        }
        
        public Task Send(string to, string @from, string subject, string htmlContent)
        {
            var task = new Task(() => Client.Send(from, to, subject, htmlContent));
            task.Start();
            return task;
        }
    }
}