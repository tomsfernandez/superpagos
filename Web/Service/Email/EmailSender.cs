using System.Threading.Tasks;

namespace Web.Service.Email {
    public interface EmailSender {

        Task Send(string to, string from, string subject, string htmlContent);
    }
}