using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Refit;
using Web.Dto;
using Web.Model.Domain;
using Web.Service;
using Web.Service.Provider;
using static System.Net.HttpStatusCode;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace Web.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodsController : ControllerBase {

        private AppDbContext Context { get; }
        private ProviderApiFactory ProviderApiFactory { get; }

        public PaymentMethodsController(AppDbContext context, ProviderApiFactory factory) {
            Context = context;
            ProviderApiFactory = factory;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id) {
            var methods = await Context.PaymentMethods
                .Where(x => x.User.Id.Equals(id))
                .ToListAsync();
            return Ok(methods);
        }

        // todo: Add circuit breaker
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PaymentMethodPayload payload) {
            var errors = ValidateAssociationPayload(payload);
            if (errors.Count > 0) return BadRequest(errors);
            var provider = Context.Providers.Single(x => x.Code.Equals(payload.ProviderCode));
            var endpoint = provider.EndPoint;
            var confirmationPayload = CreateConfirmationPayload(payload);
            var api = ProviderApiFactory.Create(endpoint);
            var result = await api.AssociateAccount(confirmationPayload);
            if (!result.StatusCode.Equals(Status200OK)) return result;
            var user = Context.Users.Find(payload.UserId);
            var paymentMethod = new PaymentMethod {
                CreationTimestamp = DateTime.Now,
                Provider = provider,
                User = user,
                Token = confirmationPayload.AssociationToken
            };
            Context.PaymentMethods.Add(paymentMethod);
            Context.SaveChanges();
            return result;
        }

        // todo: Add token confection
        private PaymentMethodConfirmation CreateConfirmationPayload(PaymentMethodPayload payload) {
            var token = "aToken";
            return new PaymentMethodConfirmation {
                AssociationToken = token,
                OperationTokenFromProvider = payload.OperationTokenFromProvider
            };
        }

        private List<string> ValidateAssociationPayload(PaymentMethodPayload payload) {
            var errors = new List<string>();
            if(!Context.Users.Any(x => x.Id.Equals(payload.UserId))) 
                errors.Add($"User with id {payload.UserId} doesnt exist");
            if(!Context.Providers.Any(x => x.Code.Equals(payload.ProviderCode)))
                errors.Add($"Provider with code {payload.ProviderCode} doesnt exist");
            if(string.IsNullOrEmpty(payload.OperationTokenFromProvider))
                errors.Add("The Operation Token is null or empty");
            return errors;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            var user = await Context.Users.FindAsync(id);
            if (user == null) return NotFound();
            Context.Users.Remove(user);
            Context.SaveChanges();
            return Ok();
        }
    }
}