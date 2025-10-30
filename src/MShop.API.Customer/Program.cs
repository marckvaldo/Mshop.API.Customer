using MShop.API.Customer.Configuration;
using MShop.Application;
using MShop.Infra.Data;
using MShop.Infra.Keycloak;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();

builder.Services.AddConfigurationController()
    .AddConfigurationSwagger()
    .AddConfigurationModelState()
    .AddDataBaseAndRepository(builder.Configuration)
    .AddKeycloakServices(builder.Configuration)
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
