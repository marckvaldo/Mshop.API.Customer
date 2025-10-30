using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MShop.E2ETest.Base.FactoriesApplication
{
    public class CustomerWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

            var environment = "E2ETests";
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment); //setando globalmente
            builder.UseEnvironment(environment);// no host da application

            base.ConfigureWebHost(builder);
        }
    }
}
