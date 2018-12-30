using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Web.Service.Email {
    public class SendGridEmailSender : EmailSender{

        private SendGridClient SendGridClient { get; }

        public SendGridEmailSender(string apiToken) {
            SendGridClient = new SendGridClient(apiToken);
        }

        public async Task Send(string to, string from, string subject, string htmlContent) {
            var fromAddress = new EmailAddress(from);
            var toAddress = new EmailAddress(to);
            var msg = MailHelper.CreateSingleEmail(fromAddress, toAddress, subject, "", htmlContent);
            var response = await SendGridClient.SendEmailAsync(msg);
        }
    }
}