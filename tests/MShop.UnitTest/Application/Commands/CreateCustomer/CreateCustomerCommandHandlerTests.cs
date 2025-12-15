using Moq;
using MShop.Domain.Entities;
using MShop.Infra.Keycloak.DTOs;
using System.Linq.Expressions;

namespace MShop.UnitTest.Application.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandlerTests : CreateCustomerCommandHandlerTestsFixture
    {

        public CreateCustomerCommandHandlerTests() :base()
        {
            
        }

        [Fact]
        public async Task Handle_ShouldReturnTrue_WhenCustomerIsCreatedSuccessfully()
        {
            // Arrange
            _customerRepoMock.Setup(r => r.Filter(It.IsAny<System.Linq.Expressions.Expression<Func<Customer, bool>>>()))
                .ReturnsAsync(new List<Customer>());

            //_cryptoServiceMock.Setup(c => c.Encrypt(It.IsAny<string>())).Returns("senha123_encrypted");
            _identityProviderService.Setup(i => i.CreateUserAsync(It.IsAny<RequestUsers>(), CancellationToken.None))
                .ReturnsAsync("provider-identity-id-123");

            _unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var handler = CreateHandler(_cryptoServiceMock, _customerRepoMock, _addressRepoMock, _unitOfWorkMock, _notificationMock, _identityProviderService);
            var command = RequestCommandValid();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _customerRepoMock.Verify(r => r.Create(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenCustomerAlreadyExists()
        {
            // Arrange
           
            var customers = _customerFaker.Generate(10);
            _customerRepoMock.Setup(r => r.Filter(It.IsAny<System.Linq.Expressions.Expression<Func<Customer, bool>>>())).ReturnsAsync(customers);

            var handler = CreateHandler(_cryptoServiceMock, _customerRepoMock, _addressRepoMock, _unitOfWorkMock, _notificationMock, _identityProviderService);
            var command = RequestCommandValid();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            Assert.True(_notificationMock.HasErrors());
            _customerRepoMock.Verify(r => r.Create(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Never);
            _customerRepoMock.Verify(r => r.Filter(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenCustomerValidationFails()
        {
            // Arrange
            _customerRepoMock.Setup(r => r.Filter(It.IsAny<System.Linq.Expressions.Expression<Func<Customer, bool>>>()))
                .ReturnsAsync(new List<Customer>());


            var handler = CreateHandler(_cryptoServiceMock, _customerRepoMock, _addressRepoMock, _unitOfWorkMock, _notificationMock, _identityProviderService);
            var command = RequestCommandIsNotValid();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            Assert.True(_notificationMock.HasErrors());
            _customerRepoMock.Verify(r => r.Create(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Never);
            _customerRepoMock.Verify(r => r.Filter(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryThrows()
        {
            // Arrange

            _customerRepoMock.Setup(r => r.Filter(It.IsAny<System.Linq.Expressions.Expression<Func<Customer, bool>>>()))
                .ThrowsAsync(new Exception("DB error"));

            var handler = CreateHandler(_cryptoServiceMock, _customerRepoMock, _addressRepoMock, _unitOfWorkMock, _notificationMock, _identityProviderService);
            var command = RequestCommandValid();

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}