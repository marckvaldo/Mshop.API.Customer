using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MShop.Application.Commands;
using MShop.Application.Queries;
using MShop.Core.DomainObject;
using MShop.Domain.Entities;
using MShop.E2ETest.Common;
using MShop.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.E2ETest.API.Address
{
    public class AddressAPITestFixture : BaseFixture
    {
        protected MshopDbContext _dbContext;
        public AddressAPITestFixture() : base()
        {
            _dbContext = _serviceProvider.GetRequiredService<MshopDbContext>();
        }

        public AddAddressCommand RequestAddressValidCommand(Guid customerId)
        {
            var addressDto = new MShop.Application.Dtos.AddressDto
            {
                Street = "Rua Teste",
                Number = "123",
                Complement = "Apto 1",
                District = "Centro",
                City = "Cidade",
                State = "ST",
                PostalCode = "00000-000",
                Country = "Brasil",
                CustomerId = customerId
            };

            /*var updateDto = new MShop.Application.Dtos.UpdateCustomerAddressDto
            {
                CustomerId = customerId,
                Address = addressDto
            };*/
            return new AddAddressCommand(addressDto);
        }

        public void DetachedEntity(Entity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Detached;
        }
    }
}
