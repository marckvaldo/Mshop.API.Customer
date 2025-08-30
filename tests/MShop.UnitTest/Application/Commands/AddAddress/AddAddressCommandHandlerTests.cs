using Xunit;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Mshop.Application.Commands.Handlers;
using MShop.Domain.Entities;
using MShop.Domain.ValueObjects;
using Mshop.Infra.Data.Interface;
using Mshop.Core.Message;
using MShop.Core.Test.Domain.Entity.Address;
using Mshop.Core.Data;
using Mshop.Application.Commands;

namespace MShop.UnitTest.Application.Commands.AddAddress
{
    public class AddAddressCommandHandlerTests : AddAddressCommandHandlerTestsFixture
    {
        public AddAddressCommandHandlerTests() : base()
        {
            
        }


        [Fact]
        public async Task Handle_ShouldReturnTrue_WhenAddressIsAddedSuccessfully()
        {
            // Arrange
            var customer = GetCustomerFaker();
            var customerId = customer.Id;

            _customerRepoMock.Setup(r => r.GetById(customerId)).ReturnsAsync(customer);
            _addressRepoMock.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync((Address)null);
            _unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var handler = CreateHandler(_customerRepoMock, _addressRepoMock, _unitOfWorkMock, _notificationMock);
            var command = RequestAddressValid(customerId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.False(_notificationMock.HasErrors());
            _addressRepoMock.Verify(r => r.Create(It.IsAny<Address>(), It.IsAny<CancellationToken>()), Times.Once);
            _customerRepoMock.Verify(r => r.Update(customer, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenCustomerNotFound()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            _customerRepoMock = new Mock<ICustomerRepository>();
            _addressRepoMock = new Mock<IAddressRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _customerRepoMock.Setup(r => r.GetById(customerId)).ReturnsAsync((Customer)null);

            var handler = CreateHandler(_customerRepoMock, _addressRepoMock, _unitOfWorkMock, _notificationMock);
            var command = RequestAddressValid(customerId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            Assert.True(_notificationMock.HasErrors());
            _addressRepoMock.Verify(r => r.GetById(It.IsAny<Guid>()), Times.Never);
            _customerRepoMock.Verify(c => c.GetById(It.IsAny<Guid>()), Times.Once);
            _customerRepoMock.Verify(c => c.Update(It.IsAny<Customer>(), CancellationToken.None), Times.Never);
            _addressRepoMock.Verify(r => r.Create(It.IsAny<Address>(), CancellationToken.None), Times.Never);

        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenCustomerAlreadyHasAddress()
        {
            // Arrange
           
            var customer = GetCustomerFaker();
            var customerId = customer.Id;
            var addressOld = new AddressFaker().Generate();
            customer.AddAddress(addressOld);

            var address = new AddressFaker().Generate();

            _customerRepoMock.Setup(r => r.GetById(customerId)).ReturnsAsync(customer);
            _addressRepoMock.Setup(r => r.GetById(customer.AddressId)).ReturnsAsync(address);

            var handler = CreateHandler(_customerRepoMock, _addressRepoMock, _unitOfWorkMock, _notificationMock);
            var command = RequestAddressValid(customerId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            Assert.True(_notificationMock.HasErrors());
            _addressRepoMock.Verify(r => r.GetById(It.IsAny<Guid>()), Times.Once);
            _customerRepoMock.Verify(c => c.GetById(It.IsAny<Guid>()), Times.Once);
            _customerRepoMock.Verify(c => c.Update(It.IsAny<Customer>(), CancellationToken.None), Times.Never);
            _addressRepoMock.Verify(r => r.Create(It.IsAny<Address>(), CancellationToken.None), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenAddressValidationFails()
        {
            // Arrange
            var customer = GetCustomerFaker();
            var customerId = Guid.NewGuid();

            _customerRepoMock.Setup(r => r.GetById(customerId)).ReturnsAsync(customer);
            _addressRepoMock.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync((Address)null);

            var handler = CreateHandler(_customerRepoMock, _addressRepoMock, _unitOfWorkMock, _notificationMock);
            var command = RequestAddressIsNotValid(customerId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            Assert.True(_notificationMock.HasErrors());
            _addressRepoMock.Verify(r => r.GetById(It.IsAny<Guid>()), Times.Once);
            _customerRepoMock.Verify(c => c.GetById(It.IsAny<Guid>()), Times.Once);
            _customerRepoMock.Verify(c => c.Update(It.IsAny<Customer>(), CancellationToken.None), Times.Never);
            _addressRepoMock.Verify(r => r.Create(It.IsAny<Address>(), CancellationToken.None), Times.Never);

        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenExceptionIsThrown()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            _customerRepoMock.Setup(r => r.GetById(customerId)).ThrowsAsync(new Exception("DB error"));

            var handler = CreateHandler(_customerRepoMock, _addressRepoMock, _unitOfWorkMock, _notificationMock);
            var command = RequestAddressValid(customerId); ;

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}