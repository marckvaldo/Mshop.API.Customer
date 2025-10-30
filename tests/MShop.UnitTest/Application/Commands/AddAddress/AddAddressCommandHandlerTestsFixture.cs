using Moq;
using MShop.Application.Commands;
using MShop.Application.Commands.Handlers;
using MShop.Core.Data;
using MShop.Core.Message;
using MShop.Infra.Data.Interface;
using MShop.Core.Test.Domain.Entity.Address;
using MShop.Domain.Entities;
using MShop.Domain.ValueObjects;
using MShop.Infra.Data.Interface;
using MShop.UnitTest.Application.Commands.Common;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MShop.UnitTest.Application.Commands.AddAddress
{
    public class AddAddressCommandHandlerTestsFixture : CommandsFixture
    {
        protected Mock<ICustomerRepository> _customerRepoMock;
        protected Mock<IAddressRepository> _addressRepoMock;
        protected Mock<IUnitOfWork> _unitOfWorkMock;
        protected INotification _notificationMock;

        public AddAddressCommandHandlerTestsFixture()
        {
            _customerRepoMock = new Mock<ICustomerRepository>();
            _notificationMock = new Notifications();
            _addressRepoMock = new Mock<IAddressRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
        }

        protected AddAddressCommandHandler CreateHandler(
            Mock<ICustomerRepository> customerRepoMock,
            Mock<IAddressRepository> addressRepoMock,
            Mock<IUnitOfWork> unitOfWorkMock,
            INotification notificationMock)
        {
            return new AddAddressCommandHandler(
                customerRepoMock.Object,
                addressRepoMock.Object,
                unitOfWorkMock.Object,
                notificationMock
            );
        }


        protected AddAddressCommand RequestAddressValid(Guid customerId)
        {
            var addressDto = new MShop.Application.Dtos.AddressDto
            {
                Street = "Rua Teste",
                Number = "123",
                Complement = "Apto 1",
                District = "Centro",
                City = "Cidade",
                State = "ST",
                PostalCode = "00000-000",
                Country = "Brasil",
                CustomerId = customerId
            };

            /*var updateDto = new MShop.Application.Dtos.UpdateCustomerAddressDto
            {
                CustomerId = customerId,
                Address = addressDto
            };*/
            return new AddAddressCommand(addressDto);
        }

        protected AddAddressCommand RequestAddressIsNotValid(Guid customerId)
        {
            var addressDto = new MShop.Application.Dtos.AddressDto
            {
                Street = "",
                Number = "123",
                Complement = "Apto 1",
                District = "",
                City = "",
                State = "ST",
                PostalCode = "00000-000",
                Country = "Brasil",
                CustomerId = customerId
            };

            /*var updateDto = new MShop.Application.Dtos.UpdateCustomerAddressDto
            {
                CustomerId = customerId,
                Address = addressDto
            };*/
            return new AddAddressCommand(addressDto);
        }
    }
}