using System.Net.Http;
using Web.Service.Provider;

namespace Web.Tests.Service {
    public class HttpClientProviderApiFactory : ProviderApiFactory {

        public HttpClient Client { get; }

        public HttpClientProviderApiFactory(HttpClient client) {
            Client = client;
        }

        public ProviderApi Create(string endpoint) {
            return new TestClientProviderApi(Client, endpoint);
        }
    }
}