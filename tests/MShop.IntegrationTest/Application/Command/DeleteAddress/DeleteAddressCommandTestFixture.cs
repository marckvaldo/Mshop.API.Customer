using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MShop.Application.Commands;
using MShop.Core.Data;
using MShop.Core.DomainObject;
using MShop.Domain.Entities;
using MShop.Domain.ValueObjects;
using MShop.Infra.Data.Context;
using MShop.Infra.Data.Interface;
using MShop.IntegrationTest.Application.Common;
using DTOs = MShop.Application.Dtos;
using Message = MShop.Core.Message;

namespace MShop.IntegrationTest.Application.Command.DeleteAddress
{
    public class DeleteAddressCommandTestFixture : IntegrationBaseFixture
    {
        protected IMediator _mediator;
        protected ICustomerRepository _customerRepository;
        protected IAddressRepository _addressRepository;
        protected Message.INotification _notification;
        protected IUnitOfWork _unitOfWork;
        protected MshopDbContext _dbContext;
        public DeleteAddressCommandTestFixture() : base()
        {
            _mediator = _serviceProvider.GetRequiredService<IMediator>();
            _customerRepository = _serviceProvider.GetRequiredService<ICustomerRepository>();
            _notification = _serviceProvider.GetRequiredService<Message.INotification>();
            _unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork>();
            _dbContext = _serviceProvider.GetRequiredService<MshopDbContext>();
            _addressRepository = _serviceProvider.GetRequiredService<IAddressRepository>();
        }

        protected DeleteAddressCommand RequestAddressValid(Guid addreesId)
        {
            return new DeleteAddressCommand(addreesId);
        }

        protected Address AddressValid()
        {
            return new Address(
                "Rua Teste",
                "123",
                "Apto 1",
                "Centro",
                "Cidade",
                "ST",
                "00000-000",
                "Brasil"
            );
        }

        public void DetachedEntity(Entity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Detached;
        }
    }
}
