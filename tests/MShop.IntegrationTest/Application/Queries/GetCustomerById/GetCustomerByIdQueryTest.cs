using MShop.Application.Queries;
using MShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.IntegrationTest.Application.Queries.GetCustomerById
{
    [Collection("Get Customer by Id Collection")]
    [CollectionDefinition("Get Customer by Id Collection", DisableParallelization = true)]
    public class GetCustomerByIdQueryTest : GetCustomerByIdQueryTestFixture
    {
        public GetCustomerByIdQueryTest() : base()
        {
            DeleteDataBase(_dbContext).Wait();
            AddMigration(_dbContext).Wait();
        }

        [Fact(DisplayName = "Get customer by id shuold return success")]
        [Trait("Integration - Application.Command", "Get customer by email")]
        public async Task GetCustomerByIdShouldReturnSuccess()
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
            var result = await _mediator.Send(RequestValid(customer.Id));

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(customer.Email, result.Data.Email);
            Assert.Equal(customer.Name, result.Data.Name);
            Assert.Equal(customer.Phone, result.Data.Phone);
        }

        [Fact(DisplayName = "Get customer by id should return failure when customer does not exist")]
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
            var result = await _mediator.Send(RequestValid(Guid.NewGuid()));

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
        }     


    }
}
