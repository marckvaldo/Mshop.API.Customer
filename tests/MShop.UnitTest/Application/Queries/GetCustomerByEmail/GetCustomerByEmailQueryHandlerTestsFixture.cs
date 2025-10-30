using Moq;
using MShop.Application.Queries.Handlers;
using MShop.Core.Message;
using MShop.Infra.Data.Interface;
using MShop.UnitTest.Application.Queries.Common;
using CoreMessage = MShop.Core.Message;

namespace MShop.UnitTest.Application.Queries.GetCustomerByEmail
{
    public class GetCustomerByEmailQueryHandlerTestsFixture : QueriesFixture
    {
        protected Mock<ICustomerRepository> _customerRepoMock;
        protected Mock<IAddressRepository> _addressRepositoryMock;
        protected CoreMessage.INotification _notification;

        public GetCustomerByEmailQueryHandlerTestsFixture() : base()
        {
            _customerRepoMock = new Mock<ICustomerRepository>();
            _addressRepositoryMock = new Mock<IAddressRepository>();
            _notification = new Notifications();
        }

        protected GetCustomerByEmailQueryHandler CreateHandler(
            Mock<ICustomerRepository> customerRepoMock,
            Mock<IAddressRepository> addressRepoMock,
            CoreMessage.INotification notificationMock)
        {
            return new GetCustomerByEmailQueryHandler(
                customerRepoMock.Object,
                addressRepoMock.Object,
                notificationMock
            );
        }

    }
}