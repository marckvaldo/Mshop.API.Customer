using Microsoft.EntityFrameworkCore;
using MShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.IntegrationTest.Application.Queries.GetAddressByCustomerId
{
    [Collection("Get Address by customer id Collection")]
    [CollectionDefinition("Get Address by customer id Collection", DisableParallelization = true)]
    public class GetAddressByCustomerIdTest : GetAddressByCustomerIdTestFixture
    {

        public GetAddressByCustomerIdTest()
        {
            DeleteDataBase(_dbContext).Wait();
            AddMigration(_dbContext).Wait();
        }

        [Fact(DisplayName = "Get address by customer id shuold return success")]
        [Trait("Integration - Application.Command", "Get address by customer id")]
        public async Task GetAddressByCustomerIdShouldReturnSuccess()
        {
            // Arrange
            var customer = await CreaterCustomer();
            var listAddress = ListAddressesFaker(customer,2);
            //var address = _addressFaker
            foreach (var item in listAddress)
            {
                await _addressRepository.Create(item, CancellationToken.None);
            }
            await _unitOfWork.CommitAsync();


            // Act
            var result = await _mediator.Send(RequestValidByCustomerId(customer.Id));

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);

            foreach (var item in result.Data.Addresses)
            {
                var addrress = listAddress.Where(x => x.Id == item.Id).First();
                Assert.NotNull(addrress);
                Assert.Equal(addrress.State, item.State);
                Assert.Equal(addrress.Street, item.Street);
                Assert.Equal(addrress.District, item.District);
                Assert.Equal(addrress.Country, item.Country);
                Assert.Equal(addrress.PostalCode, item.PostalCode);
                Assert.Equal(addrress.City, item.City);
                Assert.Equal(addrress.Complement, item.Complement);
                Assert.Equal(addrress.Number, item.Number);
            }
        }



        [Fact(DisplayName = "Get address by id shuold return success")]
        [Trait("Integration - Application.Command", "Get address by id")]
        public async Task GetAddressByIdShouldReturnSuccess()
        {
            // Arrange
            var customer = await CreaterCustomer();
            var Address = await CreateAddress(customer);

            // Act
            var result = await _mediator.Send(RequestValidById(Address.Id));

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(Address.State, result.Data.State);
            Assert.Equal(Address.Street, result.Data.Street);
            Assert.Equal(Address.District, result.Data.District);
            Assert.Equal(Address.Country, result.Data.Country);
            Assert.Equal(Address.PostalCode, result.Data.PostalCode);
            Assert.Equal(Address.City, result.Data.City);
            Assert.Equal(Address.Complement, result.Data.Complement);
            Assert.Equal(Address.Number, result.Data.Number);
        }



        private async Task<Customer> CreaterCustomer()
        {
            var customer = _customerFaker.Generate();
            customer.SetPassword("password");
            await _customerRepository.Create(customer, CancellationToken.None);
            await _unitOfWork.CommitAsync();
            DetachedEntity(customer);
            return customer;
        }

        private async Task<Domain.ValueObjects.Address> CreateAddress(Domain.Entities.Customer customer)
        {
            var address = _addressFaker.Generate();
            address.AddCustomer(customer);
            await _addressRepository.Create(address, CancellationToken.None);
            await _unitOfWork.CommitAsync(CancellationToken.None);
            DetachedEntity(address);
            return address;

        }

        private async Task<Domain.ValueObjects.Address> CreateAddresses(Domain.Entities.Customer customer)
        {
            var address = _addressFaker.Generate();
            address.AddCustomer(customer);
            await _addressRepository.Create(address, CancellationToken.None);
            await _unitOfWork.CommitAsync(CancellationToken.None);
            DetachedEntity(address);
            return address;

        }
    }

}
