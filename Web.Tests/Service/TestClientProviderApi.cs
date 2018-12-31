using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Web.Dto;
using Web.Service.Provider;

namespace Web.Tests.Service {
    public class TestClientProviderApi : ProviderApi{
        
        private HttpClient Client { get; }
        private string Url { get; }

        public TestClientProviderApi(HttpClient client, string url) {
            Client = client;
            Url = url;
        }

        public async Task<ObjectResult> AssociateAccount(PaymentMethodConfirmation payload) {
            var content = GetStringContent(payload);
            var result = await Client.PostAsync(Url, content);
            if (result.StatusCode.Equals(200)) return new OkObjectResult("OK");
            if (result.StatusCode.Equals(400)) return new BadRequestObjectResult("BAD_REQUEST");
            if (result.StatusCode.Equals(404)) return new NotFoundObjectResult("NOT_FOUND");
            return new UnauthorizedObjectResult("UNAUTHORIZED");
        }

        private StringContent GetStringContent<T>(T contentAsObject) {
            return new StringContent(JsonConvert.SerializeObject(contentAsObject), Encoding.UTF8,
                "application/json");
        }
    }
}