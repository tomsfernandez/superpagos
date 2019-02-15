using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Web.Service.Email {
    public class MailtrapEmailSender : EmailSender {
        
        private SmtpClient Client { get; }
        
        public MailtrapEmailSender(string host, int port, string username, string password){
            Client = new SmtpClient(host, port){
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            };
        }
        
        public Task Send(string to, string @from, string subject, string htmlContent){
            var task = new Task(() => Client.Send(from, to, subject, htmlContent));
            task.Start();
            return task;
        }
    }
}