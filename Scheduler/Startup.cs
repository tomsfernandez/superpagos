using System;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Scheduler {
    public class Startup {

        public IConfiguration Configuration { get; }
        
        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("appsettings.private.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.Configure<CookiePolicyOptions>(options => {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc()
//                .AddApplicationPart(typeof(FakeSuperpagosController).Assembly)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);            
            var connectionString = Configuration.GetConnectionString("HangfireConnection");
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbCreatorContext>().UseNpgsql(connectionString);
            using(var context = new DbCreatorContext(dbContextOptionsBuilder.Options))
            services.AddHangfire(config => {
                config.UsePostgreSqlStorage(connectionString);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHangfireDashboard("");
            app.UseHangfireServer();
            
//            var api = RestService.For<ISuperpagosApi>(Configuration["FakeSuperpagosEndpoint"]);
            BackgroundJob.Enqueue( () => Console.WriteLine("HOLASASDOIAHODFINSOI"));

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}