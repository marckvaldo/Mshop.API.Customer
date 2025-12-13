using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MShop.Infra.Keycloak.CircuitBreaker;
using MShop.Infra.Keycloak.Config;
using MShop.Infra.Keycloak.Handlers;
using MShop.Infra.Keycloak.Interfaces;
using MShop.Infra.Keycloak.Services;
using Polly;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Infra.Keycloak
{
    public static class ServiceResgistrationExtensions
    {
        public static IServiceCollection AddKeycloakServices(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddSingleton<ICircuitBreaker, CircuitBreaker.CircuitBreaker>();   

            services.AddCacheAndDistributedLock(configuration).GetAwaiter().GetResult();

            services.AddSingleton<KeycloakSettings>(sp =>
            {
                //var configuration = sp.GetRequiredService<IConfiguration>();
                return new KeycloakSettings
                {
                    AuthServerUrl = configuration["Keycloak:AuthServerUrl"],
                    Realm = configuration["Keycloak:Realm"],
                    ClientId = configuration["Keycloak:ClientId"],
                    ClientSecret = configuration["Keycloak:ClientSecret"],
                    GroupName = configuration["Keycloak:GroupName"]
                };
            });

            services.AddHttpClient("keycloak-token-client", (provider, client) =>
            {
                var settings = provider.GetRequiredService<KeycloakSettings>();
                client.BaseAddress = new Uri(settings.AuthServerUrl);
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            });

            services.AddScoped<IIdentityTokenProviderService, KeyCloakTokenService>();

            services.AddTransient<KeycloakHandlerDelegate>();
            services.AddHttpClient<IIdentityProviderService, KeycloakService>((provider, client) =>
            {
                var settings = provider.GetRequiredService<KeycloakSettings>();

                client.BaseAddress = new Uri(settings.AuthServerUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
                );
                client.Timeout = TimeSpan.FromSeconds(300);
            })
            .AddHttpMessageHandler<KeycloakHandlerDelegate>();
            
            return services;
        }

        public static async Task<IServiceCollection> AddCacheAndDistributedLock(this IServiceCollection services, IConfiguration configuration)
        {
            var redisOptions = new ConfigurationOptions
            {
                EndPoints = { configuration["Redis:Endpoint"] },
                User = configuration["Redis:User"],
                Password = configuration["Redis:Password"],
                AbortOnConnectFail = false,
                Ssl = false,
                ConnectRetry = 5,
                ConnectTimeout = 5000,
                AsyncTimeout = 5000
            };

            services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = redisOptions;
                options.InstanceName = "mshop_";
            });

            var redis = ConnectionMultiplexer.Connect(redisOptions);
            services.AddSingleton<IConnectionMultiplexer>(redis);
            services.AddSingleton<IDistributedLockFactory>(sp =>
            {
                return RedLockFactory.Create(new List<RedLockMultiplexer> 
                { 
                    //redis 
                    new RedLockMultiplexer(redis)
                });
            });



            return services;
        }
    }
}
