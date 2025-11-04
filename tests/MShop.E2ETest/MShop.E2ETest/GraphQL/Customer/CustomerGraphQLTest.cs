using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MShop.Application.Dtos;
using MShop.Application.Queries;
using MShop.Core.Test.Domain.Entity.Customer;
using MShop.Domain.Entities;
using MShop.E2ETest.Base;
using MShop.E2ETest.GraphQL.Common;
using MShop.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.E2ETest.GraphQL.Customer
{
    [Collection("API Customer GraphQL")]
    [CollectionDefinition("API Customer GraphQL", DisableParallelization = true)]
    public class CustomerGraphQLTest : CustomerGraphQLTestFixture
    {
        //protected IMediator _mediator;
        protected MshopDbContext _context;
        public CustomerGraphQLTest() : base(TypeProject.GraphQL)
        {
            //_mediator = _serviceProvider.GetRequiredService<IMediator>();
            _context = _serviceProvider.GetRequiredService<MshopDbContext>();

            DeleteDataBase(_context).Wait();
            AddMigration(_context).Wait();

        }

        [Fact(DisplayName = nameof(GetCustomerByIdShouldReturnTrue))]
        [Trait("EndToEnd/API", "Customer - Endpoints")]
        public async Task GetCustomerByIdShouldReturnTrue()
        {
            var customers = CustomerFaker();
            await CreateCustmerDataBase(customers);
            var customer = customers.FirstOrDefault();

            string query = $@"
                        {{
                            customerById(id:""{customer.Id}"")
                            {{
                                name,
                                email,
                                phone,
                                address {{
                                    state,
                                    street,
                                    postalCode,
                                    city,
                                    complement,
                                    country,
                                    district,
                                    number
                                }}
                            }}
                        }}";

            var result = await _apiClientGraphQL.SendQuery<ResponseGraphQL<CustomerByIdResponse>>(Configuration.URI_GRAPHQL,query);

            var data = result.Data.CustomerById;

            Assert.NotNull(result.Data);
            Assert.Equal(data.Email, customer.Email);
            Assert.Equal(data.Phone, customer.Phone);
            Assert.Equal(data.Name, customer.Name);

            Assert.Equal(data.Address.State, customer.Address?.State);
            Assert.Equal(data.Address.Street, customer.Address?.Street);
            Assert.Equal(data.Address.Number, customer.Address?.Number);
            Assert.Equal(data.Address.PostalCode, customer.Address?.PostalCode);
            Assert.Equal(data.Address.City, customer.Address?.City);
            Assert.Equal(data.Address.Complement, customer.Address?.Complement);
            Assert.Equal(data.Address.Country, customer.Address?.Country);
            Assert.Equal(data.Address.District, customer.Address?.District);
        }


        [Fact(DisplayName = nameof(GetCustomerByEmailShouldReturnTrue))]
        [Trait("EndToEnd/API", "Customer - Endpoints")]
        public async Task GetCustomerByEmailShouldReturnTrue()
        {
            var customers = CustomerFaker();
            await CreateCustmerDataBase(customers);
            var customer = customers.FirstOrDefault();

            string query = $@"
                        {{
                            customerByEmail(email:""{customer.Email}"")
                            {{
                                name,
                                email,
                                phone,
                                address {{
                                    state,
                                    street,
                                    postalCode,
                                    city,
                                    complement,
                                    country,
                                    district,
                                    number
                                }}
                            }}
                        }}";

            var result = await _apiClientGraphQL.SendQuery<ResponseGraphQL<CustomerByEmailResponse>>(Configuration.URI_GRAPHQL, query);

            var data = result.Data.CustomerByEmail;

            Assert.NotNull(result.Data);
            Assert.Equal(data.Email, customer.Email);
            Assert.Equal(data.Phone, customer.Phone);
            Assert.Equal(data.Name, customer.Name);

            Assert.Equal(data.Address.State, customer.Address?.State);
            Assert.Equal(data.Address.Street, customer.Address?.Street);
            Assert.Equal(data.Address.Number, customer.Address?.Number);
            Assert.Equal(data.Address.PostalCode, customer.Address?.PostalCode);
            Assert.Equal(data.Address.City, customer.Address?.City);
            Assert.Equal(data.Address.Complement, customer.Address?.Complement);
            Assert.Equal(data.Address.Country, customer.Address?.Country);
            Assert.Equal(data.Address.District, customer.Address?.District);
        }


        [Fact(DisplayName = nameof(GetCustomerByNameShouldReturnTrue))]
        [Trait("EndToEnd/API", "Customer - Endpoints")]
        public async Task GetCustomerByNameShouldReturnTrue()
        {
            var customers = CustomerFaker();
            await CreateCustmerDataBase(customers);
            var customer = customers.FirstOrDefault();

            string query = $@"
                        {{
                            customerByName(name:""{customer.Name}"")
                            {{
                                name,
                                email,
                                phone,
                                address {{
                                    state,
                                    street,
                                    postalCode,
                                    city,
                                    complement,
                                    country,
                                    district,
                                    number
                                }}
                            }}
                        }}";

            var result = await _apiClientGraphQL.SendQuery<ResponseGraphQL<CustomerByNameResponse>>(Configuration.URI_GRAPHQL, query);

            var data = result.Data.CustomerByName;

            Assert.NotNull(result.Data);
            Assert.Equal(data.Email, customer.Email);
            Assert.Equal(data.Phone, customer.Phone);
            Assert.Equal(data.Name, customer.Name);

            Assert.Equal(data.Address.State, customer.Address?.State);
            Assert.Equal(data.Address.Street, customer.Address?.Street);
            Assert.Equal(data.Address.Number, customer.Address?.Number);
            Assert.Equal(data.Address.PostalCode, customer.Address?.PostalCode);
            Assert.Equal(data.Address.City, customer.Address?.City);
            Assert.Equal(data.Address.Complement, customer.Address?.Complement);
            Assert.Equal(data.Address.Country, customer.Address?.Country);
            Assert.Equal(data.Address.District, customer.Address?.District);
        }
    }
}
