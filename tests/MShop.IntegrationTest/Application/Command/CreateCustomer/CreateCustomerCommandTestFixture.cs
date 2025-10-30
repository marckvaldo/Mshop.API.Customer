using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MShop.Application.Commands;
using Message = MShop.Core.Message;
using MShop.Infra.Data.Interface;
using MShop.IntegrationTest.Application.Common;
using DTOs = MShop.Application.Dtos;
using MShop.Core.Data;
using Microsoft.EntityFrameworkCore;
using MShop.Infra.Data.Context;

namespace MShop.IntegrationTest.Application.Command.CreateCustomer
{
    public class CreateCustomerCommandTestFixture : IntegrationBaseFixture
    {
        protected IMediator _mediator;
        protected ICustomerRepository _customerRepository;
        protected Message.INotification _notification;
        protected IUnitOfWork _unitOfWork;
        protected MshopDbContext _dbContext;
        public CreateCustomerCommandTestFixture() : base()
        {
            _mediator = _serviceProvider.GetRequiredService<IMediator>();
            _customerRepository = _serviceProvider.GetRequiredService<ICustomerRepository>();
            _notification = _serviceProvider.GetRequiredService<Message.INotification>();
            _unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork>();
            _dbContext = _serviceProvider.GetRequiredService<MshopDbContext>();
        }

        protected CreateCustomerCommand RequestCommandValid(string email = null, string name = null, string phone = null, bool address = false)
        {
            var customer = _customerFaker.Generate();
            var addressValid = _addressFaker.Generate();

            var customerDto = new DTOs.CreateCustomerDto
            {
                Name = name is null ? customer.Name : name,
                Email = email is null ? customer.Email : email,
                Phone = phone is null ? customer.Phone : phone,
                Password = "senha123"
            };

            if (address)
            {
                customerDto.Address = new DTOs.AddressDto
                {
                    Street = addressValid.Street,
                    Number = addressValid.Number,
                    Complement = addressValid.Complement,
                    District = addressValid.District,
                    City = addressValid.City,
                    State = addressValid.State,
                    PostalCode = addressValid.PostalCode,
                    Country = addressValid.Country
                };
            }

            return new CreateCustomerCommand(customerDto);
        }
    }
}

