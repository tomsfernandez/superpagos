using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Polly;
using Web.Dto;
using Web.Model;
using Web.Model.Domain;
using Web.Service.Provider;

namespace Web.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class WebHookController : ControllerBase{

        private AppDbContext Context { get; set; }
        private ProviderApiFactory ProviderApiFactory { get; set; }

        public WebHookController(AppDbContext context, ProviderApiFactory providerApiFactory) {
            Context = context;
            ProviderApiFactory = providerApiFactory;
        }

        [HttpPost("")]
        public async Task<IActionResult> Notify([FromBody] PaymentResponse response) {
            var movement = await Context.Movements.SingleOrDefaultAsync(x => x.OperationId.Equals(response.OperationId));
            if (movement == null) return BadRequest($"Operation with id: {response.OperationId} does not exist");
            var transaction = movement.Transaction;
            if (response.IsBadRequest() || response.IsError()) {
                transaction.Movements.ForEach(x => x.Rollback(ProviderApiFactory, Context).Wait());
            }
            else if (response.IsOk()) {
                movement.Success();
                Context.Movements.Update(movement);
            }
            await Context.SaveChangesAsync();
            return Ok();
        }
    }
}