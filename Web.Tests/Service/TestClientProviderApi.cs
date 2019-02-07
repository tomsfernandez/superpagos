using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Web.Dto;
using Web.Model;
using Web.Service.Provider;

namespace Web.Tests.Service {
    public class TestClientProviderApi : ProviderApi{
        
        private HttpClient Client { get; }
        private string Url { get; }

        public TestClientProviderApi(HttpClient client, string url) {
            Client = client;
            Url = url;
        }

        public async Task AssociateAccount(PaymentMethodConfirmation payload) {
            var content = GetStringContent(payload);
            var result = await Client.PostAsync(Url, content);
            if (result.StatusCode.Equals(400)) throw new Exception("BAD_REQUEST");
            if (result.StatusCode.Equals(404)) throw new Exception("NOT_FOUND");
            throw new Exception("UNAUTHORIZED");
        }

        private StringContent GetStringContent<T>(T contentAsObject) {
            return new StringContent(JsonConvert.SerializeObject(contentAsObject), Encoding.UTF8,
                "application/json");
        }

        public Task<ObjectResult> Pay(StartPaymentMessage message) {
            throw new NotImplementedException();
        }

        public Task<ObjectResult> Rollback(RollbackMessage message) {
            throw new NotImplementedException();
        }
    }
}