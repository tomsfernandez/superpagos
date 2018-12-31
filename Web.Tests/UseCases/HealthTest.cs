using System.Net.Http;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace Web.Tests.UseCases {
    public class HealthTest {

        private TestServer Server { get; }
        private HttpClient Client { get; }

        public HealthTest() {
            Server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            Client = Server.CreateClient();
        }

        [Fact]
        public async void test_01_health_test_is_setup_correctly() {
            var result = await Client.GetAsync("api/health");
            result.IsSuccessStatusCode.Should().BeTrue();
        }
    }
}