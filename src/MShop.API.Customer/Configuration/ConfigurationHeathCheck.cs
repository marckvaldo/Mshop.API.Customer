using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MShop.API.Customer.HealChecks;

namespace MShop.API.Customer.Configuration
{
    public static class ConfigurationHeathCheck
    {
        public static IServiceCollection AddConfigurationHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddCheck<HealthDataBase>("DataBase");

            return services;
        }

        public static WebApplication AddMapHealthCheck(this WebApplication app)
        {
            app.MapHealthChecks("/_metrics", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse

            });
            return app;
        }
    }
}
