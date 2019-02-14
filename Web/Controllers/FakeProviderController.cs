using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Refit;
using Web.Dto;
using Web.Model;
using Web.Service.Provider;

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
            var task = GetSuccessResponseTask(dto);
            task.Start();
            return Ok();
        }

        [HttpPost("rollback")]
        public IActionResult Rollback([FromBody] RollbackMessage dto) {
            return Ok();
        }

        public Task GetSuccessResponseTask(StartPaymentMessage dto) {
            var paymentResponse = new PaymentResponse {
                Code = 200,
                Message = "",
                OperationId = dto.OperationId,
                Timestamp = DateTime.Now
            };
            return new Task(() => {
                Thread.Sleep(2000);
                var api = RestService.For<SuperpagosApi>(dto.ResponseEndpoint);
                api.PaymentEnded(paymentResponse);
            });
        }
    }
}