using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Refit;
using Web.Dto;

namespace Web.Service {
    public interface ProviderApi {

        [Post("/")]
        Task<ObjectResult> AssociateAccount([Body] PaymentMethodConfirmation payload);
    }
}