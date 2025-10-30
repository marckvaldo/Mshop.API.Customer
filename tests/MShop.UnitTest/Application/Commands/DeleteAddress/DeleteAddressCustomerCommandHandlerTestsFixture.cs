using Moq;
using MShop.Application.Commands;
using MShop.Application.Commands.Handlers;
using MShop.Core.Data;
using MShop.Core.Message;
using MShop.Infra.Data.Interface;
using MShop.UnitTest.Application.Commands.Common;

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