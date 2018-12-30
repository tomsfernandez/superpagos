using Microsoft.Extensions.Configuration;

namespace Web.Tests {
    public class Startup {

        public static IConfiguration Configuration { get; } = BuildConfiguration();

        private static IConfiguration BuildConfiguration() {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.private.json", optional: true)
                .AddEnvironmentVariables();
            return builder.Build();    
        }
    }
}