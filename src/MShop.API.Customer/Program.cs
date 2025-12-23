using Mshop.API.Customer.Middlewares;
using MShop.API.Customer.Configuration;
using MShop.API.Customer.Middlewares.Observability;
using MShop.Application;
using MShop.Infra.Data;
using MShop.Infra.Keycloak;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

/*builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = builder.Environment.IsDevelopment();
    options.ValidateOnBuild = builder.Environment.IsDevelopment();
});*/

builder.AddConfigurationLogs(builder.Configuration);

// Add services to the container.
builder.Services.AddConfigurationController()
    .AddConfigurationSwagger()
    .AddConfigurationModelState()
    .AddDataBaseAndRepository(builder.Configuration)
    .AddKeycloakServices(builder.Configuration)
    .AddCacheAndDistributedLock(builder.Configuration)
    .AddConfigurationHealthChecks()
    .AddHandlers()
    .AddSecurity(builder.Configuration);

var app = builder.Build();

//app.UseMiddleware<RequestContextMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate =
        "HTTP {RequestMethod} {RequestPath} => {StatusCode} em {Elapsed:0.0000} ms";
});

app.AddMigrateDatabase();
app.UseDocumentation();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

namespace MShop.API.Customer
{
    public partial class Program
    {

    }
}
