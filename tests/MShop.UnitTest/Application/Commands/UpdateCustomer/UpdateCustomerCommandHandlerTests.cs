using Moq;
using MShop.Domain.Entities;

namespace MShop.UnitTest.Application.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandHandlerTests : UpdateCustomerCommandHandlerTestsFixture
    {

        public UpdateCustomerCommandHandlerTests() : base()
        {
            
        }



        [Fact]
        public async Task Handle_ShouldReturnTrue_WhenCustomerIsUpdatedSuccessfully()
        {
            // Arrange
            var customer = GetCustomerFaker();
            var customerId = customer.Id;


            _customerRepoMock.Setup(r => r.GetById(customerId)).ReturnsAsync(customer);
            _unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var handler = CreateHandler(_customerRepoMock, _unitOfWorkMock, _notificationMock);
            var command = RequestCommandValid(customerId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.False(_notificationMock.HasErrors());
            _customerRepoMock.Verify(r => r.Update(customer, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenCustomerNotFound()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            _customerRepoMock.Setup(r => r.GetById(customerId)).ReturnsAsync((Customer)null);

            var handler = CreateHandler(_customerRepoMock, _unitOfWorkMock, _notificationMock);
            var command = RequestCommandValid(customerId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            Assert.True(_notificationMock.HasErrors());
            _customerRepoMock.Verify(r => r.Update(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenCustomerValidationFails()
        {
            // Arrange
            var customer = GetCustomerFaker();
            var customerId = customer.Id;

            _customerRepoMock.Setup(r => r.GetById(customerId)).ReturnsAsync(customer);

            var handler = CreateHandler(_customerRepoMock, _unitOfWorkMock, _notificationMock);
            var command = RequestCommandIsNotValid();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            Assert.True(_notificationMock.HasErrors());
            _customerRepoMock.Verify(r => r.Update(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryThrows()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            _customerRepoMock.Setup(r => r.GetById(customerId)).ThrowsAsync(new Exception("DB error"));

            var handler = CreateHandler(_customerRepoMock, _unitOfWorkMock, _notificationMock);
            var command = RequestCommandValid(customerId);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}