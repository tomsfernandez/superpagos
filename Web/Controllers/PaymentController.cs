using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Polly;
using Refit;
using Web.Dto;
using Web.Model;
using Web.Model.Domain;
using Web.Service.Provider;

namespace Web.Controllers {
    
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase{

        private AppDbContext Context { get; }
        private IConfiguration Config { get; }
        private int AmountOfRetries { get; }
        private ProviderApiFactory ProviderApiFactory { get; }
        private MovementSaga Saga { get; }
        private TransactionBuilder TransactionBuilder { get; }

        public PaymentController(AppDbContext context, IConfiguration configuration, ProviderApiFactory providerApiFactory) {
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
            if (errors.Any()) return BadRequest(errors);
            var transaction = TransactionBuilder.Build(Context, dto);
            Context.Transactions.Add(transaction);
            Context.SaveChangesAsync();
            var isSuccesfull = Saga.Start(transaction.Movements);
            if (!isSuccesfull) {
                return BadRequest();
            }
            return Ok();
        }
    }
}