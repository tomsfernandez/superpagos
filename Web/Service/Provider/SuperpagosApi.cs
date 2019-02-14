using System.Threading.Tasks;
using Refit;
using Web.Dto;

namespace Web.Service.Provider {
    public interface SuperpagosApi {

        [Post("")]
        Task PaymentEnded([Body] PaymentResponse response);
    }
}