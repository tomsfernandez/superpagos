using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Web.Controllers;
using Web.Dto;
using Web.Model.Domain;
using Web.Model.JwtClaim;
using Web.Service.Provider;
using Web.Tests.Helpers;
using Web.Tests.Service;
using Xunit;
using ProviderFactory = Web.Tests.Helpers.ProviderFactory;

namespace Web.Tests.UseCases {
    public class PaymentMethodLinkingTest : IDisposable, IClassFixture<DatabaseFixture> {
        
        private Provider VisaProvider { get; }
        private string VisaProviderToken { get; }
        private AppDbContext Context { get; }
        private IConfiguration Configuration { get; }
        private User Jaimito { get; }
        private ProviderApiFactory ApiFactory { get; }
        private PaymentMethodsController Controller { get; }

        public PaymentMethodLinkingTest(DatabaseFixture fixture) {
            Jaimito = UserFactory.GetJaimito();
            Configuration = Startup.Configuration;
            Context = fixture.DatabaseContext;
            VisaProviderToken = Configuration["FakeProviderToken"];
            VisaProvider = ProviderFactory.GetVisa();
            ApiFactory = new StubProviderApiFactory {
                OnAssociation = conf => {
                    if (conf.OperationTokenFromProvider.Equals(VisaProviderToken)) return new OkObjectResult("OK");
                    return new BadRequestObjectResult("BAD_REQUEST");
                }
            };
            Setup();
            Controller = new PaymentMethodsController(Context, ApiFactory, GetClaimsExtractorFactoryFor(Jaimito));
        }

        [Fact]
        public async void test_01_visa_method_is_linked_succesfully() {
            var paymentMethodPayload = new PaymentMethodPayload {
                ProviderCode = VisaProvider.Code, 
                OperationTokenFromProvider = VisaProviderToken
            };
            var linkingResult = await Controller.Post(paymentMethodPayload) as ObjectResult;
            linkingResult.Should().NotBeNull();
            linkingResult.StatusCode.Should().Be(200);
            Context.PaymentMethods.Count().Should().Be(1);
        }

        [Fact]
        public async void test_02_provider_token_is_bogus() {
            var paymentMethodPayload = new PaymentMethodPayload {
                ProviderCode = "A Bogus Code",
                OperationTokenFromProvider = VisaProviderToken
            };
            var linkingResult = await Controller.Post(paymentMethodPayload) as ObjectResult;
            linkingResult.Should().NotBeNull();
            linkingResult.StatusCode.Should().Be(400);
        }

        private ClaimExtractorFactory GetClaimsExtractorFactoryFor(User user) {
            var idClaim = new Claim(ClaimTypes.Name, user.Id.ToString());
            return new SameClaimExtractorFactory(new List<Claim>{idClaim});
        }

        private void Setup() {
            Context.Providers.Add(VisaProvider);
            Context.Users.Add(Jaimito);
            Context.SaveChanges();
        }

        public void Dispose() {
            Context.PaymentMethods.DeleteFromQuery();
            Context.Users.DeleteFromQuery();
            Context.Providers.DeleteFromQuery();
        }
    }
}