using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web.Controllers;
using Web.Model.Domain;
using Web.Service.Provider;

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

        public void ConfigureServices(IServiceCollection services) {
            services.AddEntityFrameworkNpgsql().AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase());    
            services.AddMvc()
                .AddApplicationPart(typeof(HealthController).Assembly)
                .AddApplicationPart(typeof(FakeProviderController).Assembly)
                .AddJsonOptions(options => {
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                });
            services.AddAuthorization(options => {
                options.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, Role.ADMIN.ToString()));
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            app.UseMvc();
        }
    }
}