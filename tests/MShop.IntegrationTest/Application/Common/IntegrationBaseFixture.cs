using Bogus.DataSets;
using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MShop.Application;
using MShop.Core.Test.Domain.Entity.Address;
using MShop.Core.Test.Domain.Entity.Customer;
using MShop.Domain.Entities;
using MShop.Infra.Data;
using MShop.Infra.Data.Context;
using MShop.Infra.Keycloak;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain = MShop.Domain.ValueObjects;

namespace MShop.IntegrationTest.Application.Common
{
    public class IntegrationBaseFixture 
    {
        protected IConfiguration _configuration;
        protected IServiceProvider _serviceProvider;
        protected CustomerFaker _customerFaker;
        protected AddressFaker _addressFaker;

        public IntegrationBaseFixture()
        {
            _serviceProvider = BuilderProvider();
            _customerFaker = new CustomerFaker();
            _addressFaker = new AddressFaker();

        }

        protected IServiceProvider BuilderProvider()
        {
            var service = new ServiceCollection();

            var inMemorySettings = new Dictionary<string, string>()
            {

                {"ConnectionStrings:RepositoryMysql", "Server=localhost;Port=3308;Database=mshopCustomer;User id=mshop;Password=mshop;Convert Zero Datetime=True"},
                {"ConfigCrypto:SecretKey", "marckvaldowallas"},
                {"ConfigCrypto:SecretIV", "marckvaldowallas"},
                {"Keycloak:AuthServerUrl", "http://keycloak-dev:8080/auth"},
                {"Keycloak:Realm", "mshop"},
                {"Keycloak:ClientId", "mshop-customer"},
                {"Keycloak:ClientSecret", "mshop-customer"},

            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            service.AddDataBaseAndRepository(configuration)
                .AddKeycloakServices(configuration)
                .AddHandlers();

            return service.BuildServiceProvider();
        }

        protected async Task AddMigration(MshopDbContext dbContext)
        {
            if (dbContext != null)
                await dbContext.Database.MigrateAsync();
        }

        protected async Task DeleteDataBase(MshopDbContext dbContext)
        {
            if (dbContext != null)
                await dbContext.Database.EnsureDeletedAsync();
        }

        protected (IServiceScope scope, T ServiceInstance) CreateScopedService<T>() where T : class
        {
            var scope = _serviceProvider.CreateScope();
            var serviceInstance = scope.ServiceProvider.GetRequiredService<T>();
            return (scope, serviceInstance);
        }

        public Customer GetCustomerFaker(string name = "", string email = "", string phone = "")
        {
            var customer = _customerFaker.Generate();

            customer.UpdateCustomer(
                name == "" ? customer.Name : name,
                email == "" ? customer.Email : email,
                phone == "" ? customer.Phone : phone
                );

            return customer;
        }

        public List<Customer> ListCustomerFaker(string name = "", string email = "", string phone = "")
        {
            var customers = _customerFaker.Generate(10);
            var customer = GetCustomerFaker(name, email, phone);
            customers.Add(customer);
            return customers;
        }

        public Domain.ValueObjects.Address GetAddAddressFaker(
            Customer customer,
            string street = "", 
            string number = "", 
            string complement = "", 
            string district = "", 
            string city = "", 
            string state = "", 
            string postalCode = "", 
            string country = "")
        {
            var address = _addressFaker.Generate();
            address.UpdateAddress(
                street == "" ? address.Street : street,
                number == "" ? address.Number : number,
                complement == "" ? address.Complement : complement,
                district == "" ? address.District : district,
                city == "" ? address.City : city,
                state == "" ? address.State : state,
                postalCode == "" ? address.PostalCode : postalCode,
                country == "" ? address.Country : country);

            address.AddCustomer(customer);

            return address;
        }

        public List<Domain.ValueObjects.Address> ListAddressesFaker(Customer customer, int quantity = 10,
            string street = "",
            string number = "",
            string complement = "",
            string district = "",
            string city = "",
            string state = "",
            string postalCode = "",
            string country = "")
        {
            var addresses = _addressFaker.Generate(quantity);
            var address = GetAddAddressFaker(customer,street,number,complement,district,city,state,postalCode,country);
            addresses.Add(address);
            return addresses;
        }
    }
}
