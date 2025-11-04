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

namespace MShop.E2ETest.GraphQL.Address
{
    [Collection("API Customer GraphQL")]
    [CollectionDefinition("API Customer GraphQL", DisableParallelization = true)]
    public class AddressGraphQLTest : AddressGraphQLTestFixture
    {
        //protected IMediator _mediator;
        protected MshopDbContext _context;
        public AddressGraphQLTest() : base(TypeProject.GraphQL)
        {
            //_mediator = _serviceProvider.GetRequiredService<IMediator>();
            _context = _serviceProvider.GetRequiredService<MshopDbContext>();

            DeleteDataBase(_context).Wait();
            AddMigration(_context).Wait();

        }

        [Fact(DisplayName = nameof(GetAddressByIdShouldReturnTrue))]
        [Trait("EndToEnd/API", "Customer - Endpoints")]
        public async Task GetAddressByIdShouldReturnTrue()
        {
            var customers = CustomerFaker();
            await CreateCustmerDataBase(customers);
            var customer = customers.FirstOrDefault();

            string query = $@"
                        {{
                            addressById(id:""{customer.Address.Id}"")
                            {{
                                state,
                                street,
                                postalCode,
                                city,
                                complement,
                                country,
                                district,
                                number
                            }}
                        }}";

            var result = await _apiClientGraphQL.SendQuery<ResponseGraphQL<AddressByIdResponse>>(Configuration.URI_GRAPHQL,query);

            var data = result.Data.addressById;

            Assert.NotNull(result.Data);
            Assert.Equal(data.State, customer.Address?.State);
            Assert.Equal(data.Street, customer.Address?.Street);
            Assert.Equal(data.Number, customer.Address?.Number);
            Assert.Equal(data.PostalCode, customer.Address?.PostalCode);
            Assert.Equal(data.City, customer.Address?.City);
            Assert.Equal(data.Complement, customer.Address?.Complement);
            Assert.Equal(data.Country, customer.Address?.Country);
            Assert.Equal(data.District, customer.Address?.District);
        }

    }
}
