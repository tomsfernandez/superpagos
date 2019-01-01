using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Dto;

namespace Web.Controllers {
    
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase{

        private AppDbContext Context { get; }

        public PaymentController(AppDbContext context) {
            Context = context;
        }
        
        [HttpPost("")]
        public IActionResult MakePayment([FromBody] PaymentDto dto) {
            var errors = dto.Validate(Context);
            var messages = MakeMessages(dto);
            return Ok();
        }

        private List<StartPaymentMessage> MakeMessages(PaymentDto dto) {
            return new List<StartPaymentMessage>();
        }
    }
}