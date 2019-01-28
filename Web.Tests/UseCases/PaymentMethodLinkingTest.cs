using System;
using System.Linq;
using System.Net.Http;
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
using Web.Service.Provider;
using Web.Tests.Helpers;
using Web.Tests.Service;
using Xunit;
using ProviderFactory = Web.Tests.Helpers.ProviderFactory;

namespace Web.Tests.UseCases {
    public class PaymentMethodLinkingTest : IDisposable{
        
        private Provider VisaProvider { get; }
        private string VisaProviderToken { get; }
        private AppDbContext Context { get; }
        private IConfiguration Configuration { get; }
        private User Jaimito { get; }
        private ProviderApiFactory ApiFactory { get; }
        private PaymentMethodsController Controller { get; }

        public PaymentMethodLinkingTest() {
            Jaimito = UserFactory.GetJaimito();
            Configuration = Startup.Configuration;
            Context = TestHelper.MakeContext();
            VisaProviderToken = Configuration["FakeProviderToken"];
            VisaProvider = ProviderFactory.GetVisa();
            ApiFactory = new StubProviderApiFactory {
                OnAssociation = conf => {
                    if (conf.OperationTokenFromProvider.Equals(VisaProviderToken)) return new OkObjectResult("OK");
                    return new BadRequestObjectResult("BAD_REQUEST");
                }
            };
            Controller = new PaymentMethodsController(Context, ApiFactory);
        }

        [Fact]
        public async void test_01_visa_method_is_linked_succesfully() {
            Context.Providers.Add(VisaProvider);
            Context.Users.Add(Jaimito);
            Context.SaveChanges();
            var paymentMethodPayload = new PaymentMethodPayload {
                ProviderCode = VisaProvider.Code, 
                UserId = Jaimito.Id, OperationTokenFromProvider = VisaProviderToken
            };
            var linkingResult = await Controller.Post(paymentMethodPayload) as ObjectResult;
            linkingResult.Should().NotBeNull();
            linkingResult.StatusCode.Should().Be(200);
            Context.PaymentMethods.Count().Should().Be(1);
        }

        [Fact]
        public async void test_02_provider_token_is_bogus() {
            Context.Providers.Add(VisaProvider);
            Context.Users.Add(Jaimito);
            Context.SaveChanges();
            var paymentMethodPayload = new PaymentMethodPayload {
                ProviderCode = "A Bogus Code",
                UserId = Jaimito.Id, OperationTokenFromProvider = VisaProviderToken
            };
            var linkingResult = await Controller.Post(paymentMethodPayload) as ObjectResult;
            linkingResult.Should().NotBeNull();
            linkingResult.StatusCode.Should().Be(400);
        }

        public void Dispose() {
            Context.Database.EnsureDeleted();
        }
    }
}