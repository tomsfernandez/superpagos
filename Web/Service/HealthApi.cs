using System.Threading.Tasks;
using Refit;
using Web.Dto;

namespace Web.Service {
    public interface HealthApi {

        [Get("/api/health")]
        Task SuperpagosHealth();

        [Get("")]
        Task SendGridHealth();
    }
}