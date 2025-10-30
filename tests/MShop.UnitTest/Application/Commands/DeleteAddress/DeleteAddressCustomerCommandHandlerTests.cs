using Moq;
using MShop.Application.Commands;
using MShop.Application.Commands.Handlers;
using MShop.Core.Data;
using MShop.Core.Message;
using MShop.Domain.Entities;
using MShop.Domain.ValueObjects;
using MShop.Infra.Data.Interface;

namespace MShop.UnitTest.Application.Commands.DeleteAddress
{
    public class DeleteAddressCustomerCommandHandlerTests : DeleteAddressCustomerCommandHandlerTestsFixture
    {


        public DeleteAddressCustomerCommandHandlerTests() : base()
        {
            
        }

        private DeleteAddressCustomerCommandHandler CreateHandler(
            Mock<ICustomerRepository> customerRepoMock,
            Mock<IAddressRepository> addressRepoMock,
            Mock<IUnitOfWork> unitOfWorkMock,
            Mock<INotification> notificationMock)
        {
            return new DeleteAddressCustomerCommandHandler(
                customerRepoMock.Object,
                addressRepoMock.Object,
                unitOfWorkMock.Object,
                notificationMock.Object
            );
        }

        private DeleteAddressCommand CreateValidCommand(Guid addressId)
        {
            return new DeleteAddressCommand(addressId);
        }

        [Fact]
        public async Task Handle_ShouldReturnTrue_WhenAddressIsDeletedSuccessfully()
        {
            // Arrange

            var address = GetAddAddressFaker();
            var addressId = address.Id;
            var customer = GetCustomerFaker();
            var customerId = customer.Id;


            _addressRepoMock.Setup(r => r.GetById(addressId)).ReturnsAsync(address);
            //_customerRepoMock.Setup(r => r.Filter(It.IsAny<System.Linq.Expressions.Expression<Func<Customer, bool>>>()))
                //.ReturnsAsync(new List<Customer> { customer });
            _customerRepoMock.Setup(r => r.Update(customer, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _addressRepoMock.Setup(r => r.DeleteById(address, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var handler = CreateHandler(_customerRepoMock, _addressRepoMock, _unitOfWorkMock, _notificationMock); 
            var command = CreateValidCommand(addressId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.False(_notificationMock.HasErrors());
            //_customerRepoMock.Verify(r => r.Update(customer, It.IsAny<CancellationToken>()), Times.Once);
            _addressRepoMock.Verify(r => r.DeleteById(address, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnTrue_WhenAddressIsDeletedAndNoCustomerFound()
        {
            // Arrange
            var address = GetAddAddressFaker();
            var addressId = address.Id;

            _addressRepoMock.Setup(r => r.GetById(addressId)).ReturnsAsync(address);
            _customerRepoMock.Setup(r => r.Filter(It.IsAny<System.Linq.Expressions.Expression<Func<Customer, bool>>>()))
                .ReturnsAsync(new List<Customer>());
            _addressRepoMock.Setup(r => r.DeleteById(address, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var handler = CreateHandler(_customerRepoMock, _addressRepoMock, _unitOfWorkMock, _notificationMock);
            var command = CreateValidCommand(addressId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.False(_notificationMock.HasErrors());
            _customerRepoMock.Verify(r => r.Update(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Never);
            _addressRepoMock.Verify(r => r.DeleteById(address, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenAddressNotFound()
        {
            // Arrange
            var addressId = Guid.NewGuid();

            _addressRepoMock.Setup(r => r.GetById(addressId)).ReturnsAsync((Address)null);

            var handler = CreateHandler(_customerRepoMock, _addressRepoMock, _unitOfWorkMock, _notificationMock);
            var command = CreateValidCommand(addressId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            Assert.True(_notificationMock.HasErrors());
            _addressRepoMock.Verify(r => r.DeleteById(It.IsAny<Address>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryThrows()
        {
            // Arrange
            var addressId = Guid.NewGuid();

            _addressRepoMock.Setup(r => r.GetById(addressId)).ThrowsAsync(new Exception("DB error"));

            var handler = CreateHandler(_customerRepoMock, _addressRepoMock, _unitOfWorkMock, _notificationMock);
            var command = CreateValidCommand(addressId);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}