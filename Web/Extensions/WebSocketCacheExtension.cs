using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Web.Service;

namespace Web.Extensions {
    public static class WebSocketCacheExtension {

        public static IApplicationBuilder AddWebSocketHandlerMiddleware(this IApplicationBuilder builder, 
            PathString path, WebSocketHandler handler) {
            return builder.Map(path, app => app.UseMiddleware<WebSocketManagerMiddleware>(handler));
        }

        public static IServiceCollection AddWebSocketCache(this IServiceCollection services) {
            services.AddTransient<WebSocketConnectionManager>();
            Assembly.GetEntryAssembly().ExportedTypes
                .Where(x => x.GetTypeInfo().BaseType == typeof(WebSocketHandler))
                .ToList()
                .ForEach(x => services.AddSingleton(x));
            return services;
        }
    }
}