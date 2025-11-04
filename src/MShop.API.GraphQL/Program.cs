using MShop.API.GraphQL.GraphQL.Customer;
using MShop.Application;
using MShop.Infra.Data;
using MShop.Infra.Keycloak;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();  

builder.Services.AddDataBaseAndRepository(builder.Configuration)
    .AddKeycloakServices(builder.Configuration)
    .AddHandlers()
    .AddGraphQLServer()
    .AddQueryType()
    .AddTypeExtension<CustomerQueries>();


var app = builder.Build();

app.AddMigrateDatabase();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGraphQL();

//app.MapControllers();

app.Run();

namespace MShop.API.GraphQL
{
    public partial class Program
    {

    }
}