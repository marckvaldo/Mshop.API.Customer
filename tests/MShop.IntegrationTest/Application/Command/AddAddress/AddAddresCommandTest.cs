using MShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.IntegrationTest.Application.Command.AddAddress
{
    [Collection("add address collection")]
    [CollectionDefinition("add address collection", DisableParallelization = true)]
    public class AddAddresCommandTest : AddAddresCommandTestFixture
    {
        public AddAddresCommandTest() : base()
        {
            DeleteDataBase(_dbContext).Wait();
            AddMigration(_dbContext).Wait();
        }

        [Fact(DisplayName = "Register Address With Success")]
        [Trait("Integration - Application.Command", "Create Address")]
        public async Task RegisterAddressShouldReturnSuccess()
        {
            // Arrange
            var customer = await CreateCustomer();
            var command = RequestAddressValid(customer.Id);
            // Act
            var result = await _mediator.Send(command);
            var filter = await _addressRepository.Filter(a=>a.CustomerId == customer.Id);
            var addressDb = filter.FirstOrDefault();

            // Assert
            Assert.True(result);
            Assert.False(_notification.HasErrors());
            Assert.NotNull(addressDb);
            Assert.Equal(command.Address.Number, addressDb.Number);
            Assert.Equal(command.Address.Street, addressDb.Street);
            Assert.Equal(command.Address.City, addressDb.City);
            Assert.Equal(command.Address.State, addressDb.State);
            Assert.Equal(command.Address.Country, addressDb.Country);
            Assert.Equal(command.Address.PostalCode, addressDb.PostalCode);
            Assert.Equal(command.Address.District, addressDb.District);
            Assert.Equal(command.Address.Complement, addressDb.Complement);

        }

        [Fact(DisplayName = "Register address without customer")]
        [Trait("Integration - Application.Command", "Create Address")]
        public async Task RegisterAddressWithoutCustomerShouldReturnNotCreated()
        {
            // Arrange
            var idCustomer = Guid.NewGuid();
            var command = RequestAddressValid(idCustomer);
            // Act
            var result = await _mediator.Send(command);
            var customerDb = await _customerRepository.GetById(idCustomer);

            // Assert
            Assert.False(result);
            Assert.True(_notification.HasErrors());
            Assert.Null(customerDb);

        }


        [Fact(DisplayName = "Register invalid address")]
        [Trait("Integration - Application.Command", "Create Address")]
        public async Task RegisterInvalidAddressShouldReturnNotCreated()
        {
            // Arrange
            var idCustomer = Guid.NewGuid();
            var command = RequestAddressIsNotValid(idCustomer);
            // Act
            var result = await _mediator.Send(command);
            var customerDb = await _customerRepository.GetById(idCustomer);

            // Assert
            Assert.False(result);
            Assert.True(_notification.HasErrors());
            Assert.Null(customerDb);

        }

        private async Task<Customer> CreateCustomer()
        {
            var customer = GetCustomerFaker();
            customer.SetPassword("Senha@123");
            await _customerRepository.Create(customer, CancellationToken.None);
            await _unitOfWork.CommitAsync(CancellationToken.None);
            return customer;
        }
    }
}
