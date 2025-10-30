using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MShop.Core.Data;
using MShop.Infra.Data.Context;
using MShop.Infra.Data.Helpers;
using MShop.Infra.Data.Interface;
using MShop.Infra.Data.Repository;

namespace MShop.Infra.Data
{
    public static class SeviceRegistrationExtensions
    {
        public static IServiceCollection AddDataBaseAndRepository(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("RepositoryMysql");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Connection string 'DefaultConnection' is not configured.");
            }
            // Register the database context
            services.AddDbContext<MshopDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                b => b.MigrationsAssembly("MShop.Infra.Data")));

            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            services.AddSingleton<ICryptoService>(options => new CryptoService(configuration));

            return services;
        }

        public static WebApplication AddMigrateDatabase(this WebApplication app)
        {
            var environment = app.Environment.EnvironmentName;
            if (environment == "EndToEndTest" || environment == "IntegrationTest")
            {
                // Skip migrations in test environments
                return app;
            }

            // Apply migrations at startup
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MshopDbContext>();
                dbContext.Database.Migrate();
            }
            return app;
        }
    }
}
