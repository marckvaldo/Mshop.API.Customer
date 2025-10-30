using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MShop.Application.Commands;
using MShop.Core.Data;
using MShop.Infra.Data.Context;
using MShop.Infra.Data.Interface;
using MShop.IntegrationTest.Application.Common;
using Message = MShop.Core.Message;
using DTOs = MShop.Application.Dtos;

namespace MShop.IntegrationTest.Application.Command.AddAddress
{
    public class AddAddresCommandTestFixture : IntegrationBaseFixture
    {
        protected IMediator _mediator;
        protected ICustomerRepository _customerRepository;
        protected IAddressRepository _addressRepository;
        protected Message.INotification _notification;
        protected IUnitOfWork _unitOfWork;
        protected MshopDbContext _dbContext;
        public AddAddresCommandTestFixture() : base()
        {
            _mediator = _serviceProvider.GetRequiredService<IMediator>();
            _customerRepository = _serviceProvider.GetRequiredService<ICustomerRepository>();
            _notification = _serviceProvider.GetRequiredService<Message.INotification>();
            _unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork>();
            _dbContext = _serviceProvider.GetRequiredService<MshopDbContext>();
            _addressRepository = _serviceProvider.GetRequiredService<IAddressRepository>();
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
                Country = "Brasil"
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
