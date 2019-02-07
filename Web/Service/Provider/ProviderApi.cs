using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Refit;
using Web.Dto;
using Web.Model;

namespace Web.Service.Provider {
    public interface ProviderApi {

        [Post("")]
        Task AssociateAccount([Body] PaymentMethodConfirmation payload);

        [Post("")]
        Task<ObjectResult> Pay([Body] StartPaymentMessage message);

        [Post("")]
        Task<ObjectResult> Rollback([Body] RollbackMessage message);
    }
}