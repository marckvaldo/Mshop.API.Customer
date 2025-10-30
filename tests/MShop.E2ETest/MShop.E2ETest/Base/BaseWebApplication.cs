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
        protected IServiceProvider _serviceProvider;
        protected HttpClient _httpClient;
        protected APIClient _apiClient;

        protected BaseWebApplication(TypeProject typeProject = TypeProject.Http)
        {
            if(typeProject == TypeProject.Http)
                BuildWebApplication();

        }

        protected async Task BuildWebApplication()
        {
            _webAPI = new CustomerWebApplicationFactory<MShop.API.Customer.Program>();
            _serviceProvider = _webAPI.Services.GetRequiredService<IServiceProvider>();
            _httpClient = _webAPI.CreateClient();
            _apiClient = new APIClient(_httpClient);

        }
    }
}
