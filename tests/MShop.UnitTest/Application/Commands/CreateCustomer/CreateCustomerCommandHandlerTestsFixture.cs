using Moq;
using MShop.Application.Commands;
using MShop.Application.Commands.Handlers;
using MShop.Core.Data;
using MShop.Core.Message;
using MShop.Infra.Data.Interface;
using MShop.UnitTest.Application.Commands.Common;

namespace MShop.UnitTest.Application.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandlerTestsFixture : CommandsFixture
    {
        protected Mock<ICustomerRepository> _customerRepoMock;
        protected Mock<IAddressRepository> _addressRepoMock;
        protected Mock<IUnitOfWork> _unitOfWorkMock;
        protected INotification _notificationMock;
        protected Mock<ICryptoService> _cryptoServiceMock;

        public CreateCustomerCommandHandlerTestsFixture()
        {
            _customerRepoMock = new Mock<ICustomerRepository>();
            _notificationMock = new Notifications();
            _addressRepoMock = new Mock<IAddressRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cryptoServiceMock = new Mock<ICryptoService>();
        }

        protected CreateCustomerCommandHandler CreateHandler(
            Mock<ICryptoService> cryptoServiceMock,
            Mock<ICustomerRepository> customerRepoMock,
            Mock<IAddressRepository> addressRepoMock,
            Mock<IUnitOfWork> unitOfWorkMock,
            INotification notificationMock)
        {
            return new CreateCustomerCommandHandler(
                cryptoServiceMock.Object,
                customerRepoMock.Object,
                addressRepoMock.Object,
                unitOfWorkMock.Object,
                notificationMock
            );
        }


        protected CreateCustomerCommand RequestCommandValid()
        {
            var customer = _customerFaker.Generate();
            var customerDto = new MShop.Application.Dtos.CreateCustomerDto
            {
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone,
                Password = "senha123"
            };
            return new CreateCustomerCommand(customerDto);
        }

        protected CreateCustomerCommand RequestCommandIsNotValid()
        {
            var customer = _customerFaker.Generate();
            var customerDto = new MShop.Application.Dtos.CreateCustomerDto
            {
                Name = "",
                Email = "",
                Phone = customer.Phone,
                Password = "senha123"
            };
            return new CreateCustomerCommand(customerDto);
        }

    }
}