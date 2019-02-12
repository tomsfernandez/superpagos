using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Web.Dto;
using Web.Model;

namespace Web.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class FakeProviderController : ControllerBase{

        private IConfiguration Configuration { get; }
        private string ProviderToken { get; }
        private string FailedPaymentToken { get; }

        public FakeProviderController(IConfiguration configuration) {
            Configuration = configuration;
            ProviderToken = Configuration["FakeProviderToken"];
            FailedPaymentToken = Configuration["FailedPaymentMethodToken"];
        }

        [HttpPost("link")]
        public IActionResult LinkWithSuperpagos([FromBody] PaymentMethodConfirmation dto) {
            if (ProviderToken.Equals(dto.OperationTokenFromProvider)) return Ok();
            return Forbid();
        }

        [HttpPost("pay")]
        public IActionResult Payment([FromBody] StartPaymentMessage dto) {
            if (FailedPaymentToken.Equals(dto.Token)) return Ok();
            return BadRequest();
        }

        [HttpPost("rollback")]
        public IActionResult Rollback([FromBody] RollbackMessage dto) {
            return Ok();
        }
    }
}