using MShop.Application.Queries;
using MShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.IntegrationTest.Application.Queries.GetCustomerByEmail
{
    [Collection("Get Customer by Email Collection")]
    [CollectionDefinition("Get Customer by Email Collection", DisableParallelization = true)]
    public class GetCustomerByEmailQueryTest : GetCustomerByEmailQueryTestFixture
    {
        public GetCustomerByEmailQueryTest() : base()
        {
            DeleteDataBase(_dbContext).Wait();
            AddMigration(_dbContext).Wait();
        }

        [Fact(DisplayName = "Get customer by email shuold return success")]
        [Trait("Integration - Application.Command", "Get customer by email")]
        public async Task GetCustomerByEmailShouldReturnSuccess()
        {
            // Arrange
            var listCustomer = ListCustomerFaker();
            var customer = listCustomer.First();
            foreach (var item in listCustomer)
            {
                item.SetPassword("password");
                await _customerRepository.Create(item, CancellationToken.None);
            }
            await _unitOfWork.CommitAsync();


            // Act
            var result = await _mediator.Send(RequestValid(customer.Email));

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(customer.Email, result.Data.Email);
            Assert.Equal(customer.Name, result.Data.Name);
            Assert.Equal(customer.Phone, result.Data.Phone);
        }

        [Fact(DisplayName = "Get customer by email should return failure when customer does not exist")]
        [Trait("Integration - Application.Command", "Get customer by email")]
        public async Task GetCustomerByEmailSouldReturnFailureWhenCustomerDoesNotExist()
        {
            // Arrange
            var listCustomer = ListCustomerFaker();
            var customer = listCustomer.First();
            foreach (var item in listCustomer)
            {
                item.SetPassword("password");
                await _customerRepository.Create(item, CancellationToken.None);
            }
            await _unitOfWork.CommitAsync();


            // Act
            var result = await _mediator.Send(RequestValid("clienteteste@cliente.com.br"));

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
        }     


    }
}
