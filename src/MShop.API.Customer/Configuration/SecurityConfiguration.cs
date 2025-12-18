using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;
using MShop.API.Customer.Extension;
using Polly;

namespace MShop.API.Customer.Configuration
{
    public static class SecurityConfiguration
    {
        public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
        {


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["Authentication:Authority"];
                    options.Audience = configuration["Authentication:Audience"];
                    options.RequireHttpsMetadata = bool.Parse(configuration["Authentication:RequireHttpsMetadata"]);

                    options.MetadataAddress =
                        $"{configuration["Authentication:Authority"]}/.well-known/openid-configuration";

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["Authentication:Authority"],

                        ValidateAudience = true,
                        ValidAudience = configuration["Authentication:Audience"],

                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ClockSkew = TimeSpan.FromSeconds(30),
                        NameClaimType = "preferred_username"
                    };
                });

            
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("CustomerRed", policy => 
                    policy.RequireAssertion(context => 
                        context.User.HasResourceRole("api-customer", "customer:read")));

                opt.AddPolicy("CustomerUpdate", policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasResourceRole("api-customer", "customer:update")));

                opt.AddPolicy("AddressRead", policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasResourceRole("api-customer", "customer:address:read")));

                opt.AddPolicy("AddressCreate", policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasResourceRole("api-customer", "customer:address:create")));

                opt.AddPolicy("AddressDelete", policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasResourceRole("api-customer", "customer:address:delete")));

                /*  
                options.AddPolicy("CatalogAdmin", policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasResourceRole("api-catalogo", "catalog:read") &&
                        context.User.HasResourceRole("api-catalogo", "catalog:write")));
                */

            });

            return services;
        }

    }
}
