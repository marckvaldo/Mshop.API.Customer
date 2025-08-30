using Moq;
using Mshop.Application.Commands;
using Mshop.Application.Commands.Handlers;
using Mshop.Core.Data;
using Mshop.Core.Message;
using Mshop.Infra.Data.Interface;
using MShop.Domain.Entities;
using MShop.Domain.ValueObjects;
using MShop.Infra.Data.Interface;
using MShop.UnitTest.Application.Commands.Common;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MShop.UnitTest.Application.Commands.DeleteAddress
{
    public class DeleteAddressCustomerCommandHandlerTestsFixture : CommandsFixture
    {
        protected Mock<ICustomerRepository> _customerRepoMock;
        protected Mock<IAddressRepository> _addressRepoMock;
        protected Mock<IUnitOfWork> _unitOfWorkMock;
        protected INotification _notificationMock;

        public DeleteAddressCustomerCommandHandlerTestsFixture()
        {
            _customerRepoMock = new Mock<ICustomerRepository>();
            _notificationMock = new Notifications();
            _addressRepoMock = new Mock<IAddressRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
        }

        protected DeleteAddressCustomerCommandHandler CreateHandler(
            Mock<ICustomerRepository> customerRepoMock,
            Mock<IAddressRepository> addressRepoMock,
            Mock<IUnitOfWork> unitOfWorkMock,
            INotification notificationMock)
        {
            return new DeleteAddressCustomerCommandHandler(
                customerRepoMock.Object,
                addressRepoMock.Object,
                unitOfWorkMock.Object,
                notificationMock
            );
        }

        protected DeleteAddressCommand CreateValidCommand(Guid addressId)
        {
            return new DeleteAddressCommand(addressId);
        }
    }
}