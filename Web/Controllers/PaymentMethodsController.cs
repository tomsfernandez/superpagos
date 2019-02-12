using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Kendo.DynamicLinq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Dto;
using Web.Extensions;
using Web.Model;
using Web.Model.Domain;
using Web.Model.JwtClaim;
using Web.Model.KeyBuilder;
using Web.Service.Provider;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace Web.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentMethodsController : AuthenticatedController {

        private AppDbContext Context { get; }
        private ProviderApiFactory ProviderApiFactory { get; }
        
        public PaymentMethodsController(AppDbContext context, ProviderApiFactory factory, 
            ClaimExtractorFactory extractorFactory) : base(extractorFactory) {
            Context = context;
            ProviderApiFactory = factory;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id) {
            var userId = GetIdFromToken();
            var methods = await Context.PaymentMethods
                .Where(x => x.Id == id && x.User.Id == userId)
                .ToListAsync();
            return Ok(methods);
        }

        [HttpGet("")]
        public async Task<IActionResult> Get() {
            var userId = GetIdFromToken();
            var methods = await Context.PaymentMethods
                .Include(x => x.Provider)
                .Where(x => x.User.Id == userId)
                .Select(x => new {
                    x.Id,
                    x.Provider.Name,
                    x.Provider.Company,
                    x.CreationTimestamp
                }).ToListAsync();
            return Ok(methods);
        }

        // todo: Add circuit breaker
        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody] PaymentMethodPayload payload) {
            var errors = ValidateAssociationPayload(payload);
            if (errors.Count > 0) return BadRequest(errors);
            var provider = Context.Providers.Single(x => x.Code.Equals(payload.ProviderCode));
            var endpoint = provider.LinkEndpoint;
            var confirmationPayload = CreateConfirmationPayload(payload);
            var api = ProviderApiFactory.Create(endpoint);
            await api.AssociateAccount(confirmationPayload);
            var userId = GetIdFromToken();
            var user = Context.Users.Find(userId);
            var paymentMethod = new PaymentMethod {
                CreationTimestamp = DateTime.Now,
                Provider = provider,
                User = user,
                Token = confirmationPayload.AssociationToken
            };
            Context.PaymentMethods.Add(paymentMethod);
            Context.SaveChanges();
            return Ok("OK");
        }

        private PaymentMethodConfirmation CreateConfirmationPayload(PaymentMethodPayload payload) {
            var tokenStore = new JwtTokenStore();
            var keyBuilder = new SimpleKeyBuilder(Guid.NewGuid().ToString());
            var permanentTokenLife = DateTime.Now.AddHours(1);
            var token = tokenStore.GiveToken(permanentTokenLife, keyBuilder, new List<Claim>());
            return new PaymentMethodConfirmation {
                AssociationToken = token,
                OperationTokenFromProvider = payload.OperationTokenFromProvider
            };
        }

        private List<string> ValidateAssociationPayload(PaymentMethodPayload payload) {
            var errors = new List<string>();
            var userId = GetIdFromToken();
            if(!Context.Users.Any(x => x.Id.Equals(userId))) 
                errors.Add($"User with id {userId} doesnt exist");
            if(!Context.Providers.Any(x => x.Code.Equals(payload.ProviderCode)))
                errors.Add($"Provider with code {payload.ProviderCode} doesnt exist");
            if(string.IsNullOrEmpty(payload.OperationTokenFromProvider))
                errors.Add("The Operation Token is null or empty");
            return errors;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id) {
            var paymentMethod = await Context.PaymentMethods.FindAsync(id);
            if (paymentMethod == null) return NotFound();
            var userId = GetIdFromToken();
            if (paymentMethod.User.Id != userId) return Unauthorized();
            if (Context.PaymentButtons.Any(x => x.Method.Id == id))
                return BadRequest("Debe borrar sus botónes de pago antes de poder borrar su medio de pago");
            Context.PaymentMethods.Remove(paymentMethod);
            Context.SaveChanges();
            return Ok("OK");
        }
    }
}