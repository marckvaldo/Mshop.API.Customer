using MShop.API.Customer.Configuration;
using MShop.Application;
using MShop.Infra.Data;
using MShop.Infra.Keycloak;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = builder.Environment.IsDevelopment();
    options.ValidateOnBuild = builder.Environment.IsDevelopment();
});

// Add services to the container.
builder.Services.AddConfigurationController()
    .AddConfigurationSwagger()
    .AddConfigurationModelState()
    .AddDataBaseAndRepository(builder.Configuration)
    //.AddCacheAndDistributedLock(builder.Configuration)
    .AddKeycloakServices(builder.Configuration)
    .AddCacheAndDistributedLock(builder.Configuration)
    .AddConfigurationHealthChecks()
    .AddHandlers();

var app = builder.Build();

app.AddMigrateDatabase();
app.UseDocumentation();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

namespace MShop.API.Customer
{
    public partial class Program
    {

    }
}
