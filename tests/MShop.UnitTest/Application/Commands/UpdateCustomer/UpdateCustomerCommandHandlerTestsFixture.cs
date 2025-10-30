using Moq;
using MShop.Application.Commands;
using MShop.Application.Commands.Handlers;
using MShop.Core.Data;
using MShop.Core.Message;
using MShop.Infra.Data.Interface;
using MShop.UnitTest.Application.Commands.Common;

namespace MShop.UnitTest.Application.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandHandlerTestsFixture : CommandsFixture
    {
        protected Mock<ICustomerRepository> _customerRepoMock;
        protected Mock<IAddressRepository> _addressRepoMock;
        protected Mock<IUnitOfWork> _unitOfWorkMock;
        protected INotification _notificationMock;

        public UpdateCustomerCommandHandlerTestsFixture()
        {
            _customerRepoMock = new Mock<ICustomerRepository>();
            _notificationMock = new Notifications();
            _addressRepoMock = new Mock<IAddressRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
        }

        protected UpdateCustomerCommandHandler CreateHandler(
            Mock<ICustomerRepository> customerRepoMock,
            Mock<IUnitOfWork> unitOfWorkMock,
            INotification notificationMock)
        {
            return new UpdateCustomerCommandHandler(
                customerRepoMock.Object,
                unitOfWorkMock.Object,
                notificationMock
            );
        }


        protected UpdateCustomerCommand RequestCommandValid(Guid customerId)
        {
            var dto = new MShop.Application.Dtos.UpdateCustomerDto
            {
                Id = customerId,
                Name = "Cliente Atualizado",
                Email = "atualizado@teste.com",
                Phone = "11988888888"
            };
            return new UpdateCustomerCommand(dto);
        }

        protected UpdateCustomerCommand RequestCommandIsNotValid()
        {
            var customer = _customerFaker.Generate();
            var customerDto = new MShop.Application.Dtos.UpdateCustomerDto
            {
                Name = "",
                Email = "",
                Phone = customer.Phone,
            };
            return new UpdateCustomerCommand(customerDto);
        }
    }
}