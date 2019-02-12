using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite.Internal.UrlActions;
using Microsoft.Extensions.Configuration;
using Web.Dto;

namespace Web.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class FakeProviderController : ControllerBase{

        private IConfiguration Configuration { get; }
        private string ProviderToken { get; }

        public FakeProviderController(IConfiguration configuration) {
            Configuration = configuration;
            ProviderToken = Configuration["FakeProviderToken"];
        }

        [HttpPost("link")]
        public IActionResult LinkWithSuperpagos(PaymentMethodConfirmation dto) {
            if (ProviderToken.Equals(dto.OperationTokenFromProvider)) return Ok();
            return Forbid();
        }
    }
}