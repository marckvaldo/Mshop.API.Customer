using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MShop.Infra.Keycloak.Config;
using MShop.Infra.Keycloak.Interfaces;
using MShop.Infra.Keycloak.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Infra.Keycloak
{
    public static class ServiceResgistrationExtensions
    {
        public static IServiceCollection AddKeycloakServices(this IServiceCollection services)
        {
            
            

            services.AddSingleton<KeycloakSettings>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                return new KeycloakSettings
                {
                    AuthServerUrl = configuration["Keycloak:AuthServerUrl"],
                    Realm = configuration["Keycloak:Realm"],
                    ClientId = configuration["Keycloak:ClientId"],
                    ClientSecret = configuration["Keycloak:ClientSecret"]
                };
            });


            services.AddHttpClient<IKeycloakUserService, KeycloakUserService>(client =>
            {
                client.BaseAddress = new Uri("https://keycloak.example.com/auth/admin/realms/your-realm/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddScoped<IKeycloakUserService, KeycloakUserService>();            
            return services;
        }
    }
}
