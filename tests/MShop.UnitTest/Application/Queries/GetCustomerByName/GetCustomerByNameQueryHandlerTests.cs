using Xunit;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Mshop.Application.Queries;
using MShop.Domain.Entities;
using Mshop.Infra.Data.Interface;
using Mshop.Core.Message;
using Mshop.Application.Dtos;
using System.Linq.Expressions;
using Mshop.Application.Queries.Handlers;

namespace MShop.UnitTest.Application.Queries.GetCustomerByName
{
    public class GetCustomerByNameQueryHandlerTests : GetCustomerByNameQueryHandlerTestsFixture
    {

        public GetCustomerByNameQueryHandlerTests() : base()
        {
            
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenCustomerExists()
        {
            // Arrange
            var name = "Cliente Teste";
            var listCustomer = ListCustomerFaker(name);
            var customer = listCustomer.Where(x=>x.Name == name).First();

            //_customerRepoMock = new Mock<ICustomerRepository>();
            _customerRepoMock.Setup(r => r.Filter(It.IsAny<Expression<Func<Customer, bool>>>())).ReturnsAsync(listCustomer.Where(x => x.Name == name).ToList());

            var handler = CreateHandler(_customerRepoMock, _addressRepositoryMock, _notification);
            var query = new GetCustomerByNameQuery(name);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(customer.Name, result.Data.Name);
            Assert.Equal(customer.Phone, result.Data.Phone);
            Assert.Equal(customer.Email, result.Data.Email);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenCustomerDoesNotExist()
        {
            // Arrange
            var name = "Inexistente";
            _customerRepoMock.Setup(r => r.Filter(It.IsAny<Expression<Func<Customer, bool>>>())).ReturnsAsync(new List<Customer>());

            var handler = CreateHandler(_customerRepoMock, _addressRepositoryMock, _notification);
            var query = new GetCustomerByNameQuery(name);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryThrows()
        {
            // Arrange
            var name = "Cliente Teste";
            //var customerRepoMock = new Mock<ICustomerRepository>();
            _customerRepoMock.Setup(r => r.Filter(It.IsAny<Expression<Func<Customer, bool>>>()))
                .ThrowsAsync(new Exception("DB error"));

            var handler = CreateHandler(_customerRepoMock, _addressRepositoryMock, _notification);
            var query = new GetCustomerByNameQuery(name);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}