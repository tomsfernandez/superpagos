using Refit;

namespace Web.Service {
    public class RefitProviderApiFactory : ProviderApiFactory{
        public ProviderApi Create(string endpoint) {
            return RestService.For<ProviderApi>(endpoint);
        }
    }
}