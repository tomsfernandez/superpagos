using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Web.Dto;
using Web.Model;
using Web.Model.JwtClaim;
using Web.Service.Provider;

namespace Web.Controllers {
    
    [Route("api/[controller]")]
    [ApiController,Authorize]
    public class PaymentController : AuthenticatedController{

        private AppDbContext Context { get; }
        private IConfiguration Config { get; }
        private int AmountOfRetries { get; }
        private ProviderApiFactory ProviderApiFactory { get; }
        private MovementSaga Saga { get; }
        private TransactionBuilder TransactionBuilder { get; }

        public PaymentController(AppDbContext context, IConfiguration configuration, 
            ProviderApiFactory providerApiFactory,
            ClaimExtractorFactory extractorFactory) : base(extractorFactory) {
            Context = context;
            Config = configuration;
            AmountOfRetries = int.Parse(Config["PaymentRetryAmount"]);
            ProviderApiFactory = providerApiFactory;
            Saga = new MovementSaga {Factory = providerApiFactory, Context = context};
            TransactionBuilder = new TransactionBuilder();
        }

        [HttpPost("")]
        public IActionResult MakePayment([FromBody] PaymentDto dto) {
            var errors = dto.Validate(Context);
            if (!AuthenticatedUserIsOwnerOfPaymentMethod(dto.PaymentMethodId))
                errors.Add("The logged user is not the same as the owner of the account");
            if (errors.Any()) return BadRequest(errors);
            var transaction = TransactionBuilder.Build(Context, dto);
            Context.Transactions.Add(transaction);
            Context.SaveChangesAsync();
            var isSuccessful = Saga.Start(transaction.Movements);
            if (!isSuccessful) {
                return BadRequest();
            }
            return Ok();
        }

        private bool AuthenticatedUserIsOwnerOfPaymentMethod(long paymentMethodId) {
            var userId = GetIdFromToken();
            if (Context.PaymentMethods.Find(paymentMethodId) == null) return false;
            return Context.PaymentMethods.Single(x => x.Id == paymentMethodId).User.Id != userId;
        }
    }
}