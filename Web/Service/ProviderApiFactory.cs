namespace Web.Service {
    public interface ProviderApiFactory {

        ProviderApi Create(string endpoint);
    }
}