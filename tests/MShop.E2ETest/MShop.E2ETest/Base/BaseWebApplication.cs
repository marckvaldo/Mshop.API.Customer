using Microsoft.Extensions.DependencyInjection;
using MShop.E2ETest.Base.Clients;
using MShop.E2ETest.Base.FactoriesApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.E2ETest.Base
{
    public class BaseWebApplication 
    {
        protected CustomerWebApplicationFactory<MShop.API.Customer.Program> _webAPI;
        protected CustomerWebApplicationFactory<MShop.API.GraphQL.Program> _GraphQL;
        protected IServiceProvider _serviceProvider;
        protected HttpClient _httpClient;
        protected APIClient _apiClient;
        protected GraphQLClient _apiClientGraphQL;

        protected BaseWebApplication(TypeProject typeProject)
        {
            if(typeProject == TypeProject.Http)
                BuildWebApplication();

            if(typeProject == TypeProject.GraphQL)
                BuildWebApplicationGraphQL();

        }

        protected async Task BuildWebApplication()
        {
            _webAPI = new CustomerWebApplicationFactory<MShop.API.Customer.Program>();
            _serviceProvider = _webAPI.Services.GetRequiredService<IServiceProvider>();
            _httpClient = _webAPI.CreateClient();
            _apiClient = new APIClient(_httpClient);

        }

        protected async Task BuildWebApplicationGraphQL()
        {
            _GraphQL = new CustomerWebApplicationFactory<MShop.API.GraphQL.Program>();
            _serviceProvider = _GraphQL.Services.GetRequiredService<IServiceProvider>();
            _httpClient = _GraphQL.CreateClient();
            _apiClientGraphQL = new GraphQLClient(_httpClient);

        }
    }
}
