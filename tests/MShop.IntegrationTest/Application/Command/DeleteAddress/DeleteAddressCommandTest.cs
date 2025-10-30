using MShop.Core.Test.Domain.Entity.Address;
using MShop.Domain.Entities;
using MShop.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.IntegrationTest.Application.Command.DeleteAddress
{
    [Collection("Delete customer collection")]
    [CollectionDefinition("Delete customer collection", DisableParallelization = true)]
    public class DeleteAddressCommandTest : DeleteAddressCommandTestFixture
    {
        public DeleteAddressCommandTest() : base()
        {
            DeleteDataBase(_dbContext).Wait();
            AddMigration(_dbContext).Wait();
        }

        [Fact(DisplayName = "Delete an address with success")]
        [Trait("Integration - Application.Command", "Delete Address")]
        public async Task DeleteAddressShouldReturnSuccess()
        {
            // Arrange
            var customer = await CreateCustomer();
            var address = await CreateAddress(customer);
            
            var command = RequestAddressValid(address.Id);
            // Act
            var result = await _mediator.Send(command);
            var filter = await _addressRepository.Filter(a=>a.CustomerId == customer.Id);
            var addressDb = filter.FirstOrDefault();
            

            // Assert
            Assert.True(result);
            Assert.False(_notification.HasErrors());
            Assert.Null(addressDb);

        }


        [Fact(DisplayName = "Delete an address that does not exist in the database")]
        [Trait("Integration - Application.Command", "Delete Address")]
        public async Task DeleteAnAddressThatDontExistInDataBaseShouldReturnFalso()
        {
            // Arrange
            var customer = await CreateCustomer();

            var command = RequestAddressValid(Guid.NewGuid());
            // Act
            var result = await _mediator.Send(command);

            // Assert
            Assert.False(result);
            Assert.True(_notification.HasErrors());
        }


        private async Task<Customer> CreateCustomer(Address address = null)
        {
            var customer = GetCustomerFaker();
            customer.SetPassword("Senha@123");
            if (address != null)
                customer.AddAddress(address);

            await _customerRepository.Create(customer, CancellationToken.None);
            await _unitOfWork.CommitAsync(CancellationToken.None);
            DetachedEntity(customer);
            return customer;
        }

        private async Task<Address> CreateAddress(Customer customer)
        {
            var address = GetAddAddressFaker(customer);
            await _addressRepository.Create(address, CancellationToken.None);
            await _unitOfWork.CommitAsync(CancellationToken.None);
            DetachedEntity(address);
            return address;
        }
    }
}
