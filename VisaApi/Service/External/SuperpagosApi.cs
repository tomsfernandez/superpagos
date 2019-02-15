using System.Threading.Tasks;
using Refit;
using VisaApi.Dto;

namespace VisaApi.Service.External {
    public interface ISuperpagosApi {

        [Post("")]
        Task PaymentEnded([Body] PaymentResponse response);
    }
}