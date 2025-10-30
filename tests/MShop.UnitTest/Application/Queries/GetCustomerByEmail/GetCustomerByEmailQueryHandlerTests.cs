using Moq;
using MShop.Application.Queries;
using MShop.Domain.Entities;
using MShop.Domain.ValueObjects;
using System.Linq.Expressions;

namespace MShop.UnitTest.Application.Queries.GetCustomerByEmail
{
    public class GetCustomerByEmailQueryHandlerTests : GetCustomerByEmailQueryHandlerTestsFixture
    {

        public GetCustomerByEmailQueryHandlerTests() : base() { }
        

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenCustomerExists()
        {
            // Arrange
            var email = "cliente@teste.com";
            var listCustomer = ListCustomerFaker("",email);
            var customer = listCustomer.Where(x => x.Email == email).First();
            var listAddress = ListAddressFaker(customer);

            _customerRepoMock.Setup(r => r.Filter(It.IsAny<Expression<Func<Customer, bool>>>())).ReturnsAsync(listCustomer.Where(x => x.Email == email).ToList());
            _addressRepositoryMock.Setup(r => r.Filter(It.IsAny<Expression<Func<Address, bool>>>())).ReturnsAsync(listAddress);

            var handler = CreateHandler(_customerRepoMock, _addressRepositoryMock, _notification);
            var query = new GetCustomerByEmailQuery(email);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(email, result.Data.Email);
            Assert.Equal(customer.Name, result.Data.Name);
            Assert.Equal(customer.Phone, result.Data.Phone);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenCustomerDoesNotExist()
        {
            // Arrange
            var email = "naoexiste@teste.com";
            _customerRepoMock.Setup(r => r.Filter(It.IsAny<Expression<Func<Customer, bool>>>())).ReturnsAsync(new List<Customer>());

            var handler = CreateHandler(_customerRepoMock, _addressRepositoryMock, _notification);
            var query = new GetCustomerByEmailQuery(email);

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
            var email = "cliente@teste.com";

            _customerRepoMock.Setup(r => r.Filter(It.IsAny<System.Linq.Expressions.Expression<Func<Customer, bool>>>()))
                .ThrowsAsync(new Exception("DB error"));

            var handler = CreateHandler(_customerRepoMock, _addressRepositoryMock, _notification);
            var query = new GetCustomerByEmailQuery(email);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}