using System;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using NLog.Extensions.Logging;
using NLog.Web;
using Swashbuckle.AspNetCore.Swagger;
using Web.Extensions;
using Web.Model;
using Web.Service;
using Web.Service.Email;
using Web.Service.Provider;

namespace Web {
    public class Startup {
        
        private IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("appsettings.private.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            env.ConfigureNLog("NLog.config");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            var connectionUrl = Environment.GetEnvironmentVariable("DefaultConnection") ??
                                Configuration.GetConnectionString("DefaultConnection");

            var securityKey = Configuration["AuthenticationKey"] ?? "The little brown fox jumps over the lazy dog";
            services.AddEntityFrameworkNpgsql().AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connectionUrl));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateLifetime = true,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        RequireSignedTokens = false,
                        ValidIssuer = "http://localhost:5000",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey))
                    };
                });
            IdentityModelEventSource.ShowPII = true; // así jwt muestra bien el error
            services.AddAutoMapper();
            services.AddCors();
            services.AddMvc().AddJsonOptions(options => {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });
            services.AddScoped<ProviderApiFactory>(provider => new RefitProviderApiFactory());
            var sendGridApiKey = Configuration["SendGridApiKey"];
            services.AddScoped<EmailSender>(provider => new SendGridEmailSender(sendGridApiKey));
            services.AddLogging();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info {Title = "Superpagos API", Version = "v1"}); });
            services.AddWebSocketCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseHsts();
            }
            
            app.UseWebSockets();
            app.AddWebSocketHandlerMiddleware("/test", app.ApplicationServices.GetService<EchoWebSocketHandler>());

            loggerFactory.AddConsole(Configuration.GetSection("Logging")); //log levels set in your configuration
            loggerFactory.AddDebug();
            loggerFactory.AddNLog();
            
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });
            app.UseCors();
            app.UseAuthentication();
            app.UseMvc();
        }
    }

    public class ConsoleLogger : ILogger<ConsoleLogger> {
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter) {
            Console.WriteLine(formatter.Invoke(state, exception));
        }

        public bool IsEnabled(LogLevel logLevel) {
            throw new NotImplementedException();
        }

        public IDisposable BeginScope<TState>(TState state) {
            throw new NotImplementedException();
        }
    }
}