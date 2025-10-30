using Moq;
using MShop.Application.Queries;
using MShop.Domain.Entities;
using MShop.Domain.ValueObjects;
using System.Linq.Expressions;

namespace MShop.UnitTest.Application.Queries.GetCustomerById
{
    public class GetCustomerByIdQueryHandlerTests : GetCustomerByIdQueryHandlerTestsFixture
    {
        public GetCustomerByIdQueryHandlerTests() :base() { }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenCustomerExists()
        {
            // Arrange
            var customer = GetCustomerFaker();
            var id = customer.Id;   
            var listAddress = ListAddressFaker(customer);

            _customerRepoMock.Setup(r => r.GetById(id)).ReturnsAsync(customer);
            _addressRepositoryMock.Setup(r => r.Filter(It.IsAny<Expression<Func<Address, bool>>>())).ReturnsAsync(listAddress);

            var handler = CreateHandler(_customerRepoMock, _addressRepositoryMock, _notification);
            var query = new GetCustomerByIdQuery(id);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(customer.Id, result.Data.Id);
            Assert.Equal(customer.Name, result.Data.Name);
            Assert.Equal(customer.Email, result.Data.Email);
            Assert.Equal(customer.Phone, result.Data.Phone);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenCustomerDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            //var customerRepoMock = new Mock<ICustomerRepository>();
            _customerRepoMock.Setup(r => r.GetById(id)).ReturnsAsync((Customer)null);

            var handler = CreateHandler(_customerRepoMock, _addressRepositoryMock, _notification);
            var query = new GetCustomerByIdQuery(id);

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
            var id = Guid.NewGuid();
            //var customerRepoMock = new Mock<ICustomerRepository>();
            _customerRepoMock.Setup(r => r.GetById(id)).ThrowsAsync(new Exception("DB error"));

            var handler = CreateHandler(_customerRepoMock, _addressRepositoryMock, _notification);
            var query = new GetCustomerByIdQuery(id);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}