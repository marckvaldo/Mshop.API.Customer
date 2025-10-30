using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MShop.Application.Commands;
using MShop.Core.Data;
using MShop.Core.DomainObject;
using MShop.Domain.Entities;
using MShop.Infra.Data.Context;
using MShop.Infra.Data.Interface;
using MShop.IntegrationTest.Application.Common;
using DTOs = MShop.Application.Dtos;
using Message = MShop.Core.Message;

namespace MShop.IntegrationTest.Application.Command.UpdateCustomer
{
    public class UpdateCustomerCommandTestFixture : IntegrationBaseFixture
    {
        protected IMediator _mediator;
        protected ICustomerRepository _customerRepository;
        protected Message.INotification _notification;
        protected IUnitOfWork _unitOfWork;
        protected MshopDbContext _dbContext;
        public UpdateCustomerCommandTestFixture() : base()
        {
            _mediator = _serviceProvider.GetRequiredService<IMediator>();
            _customerRepository = _serviceProvider.GetRequiredService<ICustomerRepository>();
            _notification = _serviceProvider.GetRequiredService<Message.INotification>();
            _unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork>();
            _dbContext = _serviceProvider.GetRequiredService<MshopDbContext>();
        }

        protected UpdateCustomerCommand RequestCommandValid(Guid id, string email = null, string name = null, string phone = null)
        {
            var customer = _customerFaker.Generate();
            var addressValid = _addressFaker.Generate();

            var customerDto = new DTOs.UpdateCustomerDto
            {
                Name = name is null ? customer.Name : name,
                Email = email is null ? customer.Email : email,
                Phone = phone is null ? customer.Phone : phone,
                Id = id,
            };

            return new UpdateCustomerCommand(customerDto);
        }

        protected UpdateCustomerCommand RequestCommandValid(string email = null, string name = null, string phone = null)
        {
            var customer = _customerFaker.Generate();
            var addressValid = _addressFaker.Generate();

            var customerDto = new DTOs.UpdateCustomerDto
            {
                Name = name is null ? customer.Name : name,
                Email = email is null ? customer.Email : email,
                Phone = phone is null ? customer.Phone : phone,
            };

            return new UpdateCustomerCommand(customerDto);
        }

        public void DetachedEntity(Entity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Detached;
        }
    }
}

