namespace Web.Service.Provider {
    public interface ProviderApiFactory {

        ProviderApi Create(string endpoint);
    }
}