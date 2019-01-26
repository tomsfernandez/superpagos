using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Polly;
using Refit;
using Web.Dto;
using Web.Model;
using Web.Model.Domain;
using Web.Model.Sagas;
using Web.Service.Provider;
using static Web.Model.Domain.Operation;
using Transaction = Web.Model.Domain.Transaction;

namespace Web.Controllers {
    
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase{

        private AppDbContext Context { get; }
        private IConfiguration Config { get; }
        private int AmountOfRetries { get; }
        private ProviderApiFactory ProviderApiFactory { get; }
        private SagaFactory SagaFactory { get; }

        public PaymentController(AppDbContext context, IConfiguration configuration, ProviderApiFactory providerApiFactory) {
            Context = context;
            Config = configuration;
            AmountOfRetries = int.Parse(Config["PaymentRetryAmount"]);
            ProviderApiFactory = providerApiFactory;
            SagaFactory = new PaymentSagaFactory();
        }

        [HttpPost("")]
        public IActionResult MakePayment([FromBody] PaymentDto dto) {
            var errors = dto.Validate(Context);
            if (errors.Any()) return BadRequest(errors);
            var saga = SagaFactory.Build();
            var transaction = InsertTransaction(Context, dto);
            var sagaErrors = saga.Start();
            if (sagaErrors.Any()) return BadRequest(sagaErrors);
            return Ok();
        }

        private Transaction InsertTransaction(AppDbContext context, PaymentDto dto) {
            var transaction = new Model.Sagas.Transaction("description", );
            context.Transactions.Add(transaction);
            context.SaveChanges();
            return transaction;
        }
    }
}