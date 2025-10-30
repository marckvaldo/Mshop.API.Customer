using Moq;
using MShop.Application.Queries.Handlers;
using MShop.Core.Message;
using MShop.Core.Test.Domain.Entity.Customer;
using MShop.Infra.Data.Interface;
using MShop.UnitTest.Application.Queries.Common;
using CoreMessage = MShop.Core.Message;

namespace MShop.UnitTest.Application.Queries.GetCustomerById
{
    public class GetCustomerByIdQueryHandlerTestsFixture : QueriesFixture
    {
        protected Mock<ICustomerRepository> _customerRepoMock;
        protected Mock<IAddressRepository> _addressRepositoryMock;
        protected CoreMessage.INotification _notification;
        protected CustomerFaker _customerFaker;

        public GetCustomerByIdQueryHandlerTestsFixture()
        {
            _customerRepoMock = new Mock<ICustomerRepository>();
            _addressRepositoryMock = new Mock<IAddressRepository>();
            _notification = new Notifications();
            _customerFaker = new CustomerFaker();
        }


        protected GetCustomerByIdQueryHandler CreateHandler(
           Mock<ICustomerRepository> customerRepoMock,
           Mock<IAddressRepository> addressRepoMock,
           CoreMessage.INotification notificationMock)
        {
            return new GetCustomerByIdQueryHandler(
                addressRepoMock.Object,
                customerRepoMock.Object,
                notificationMock
            );
        }
    }
}