using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MShop.Application.Queries;
using MShop.Core.Data;
using MShop.Infra.Data.Context;
using MShop.Infra.Data.Interface;
using MShop.IntegrationTest.Application.Common;
using Message = MShop.Core.Message;

namespace MShop.IntegrationTest.Application.Queries.GetCustomerByName
{
    public class GetCustomerByNameQueryTestFixture : IntegrationBaseFixture
    {
        protected IMediator _mediator;
        protected ICustomerRepository _customerRepository;
        protected IAddressRepository _addressRepository;
        protected Message.INotification _notification;
        protected IUnitOfWork _unitOfWork;
        protected MshopDbContext _dbContext;
        public GetCustomerByNameQueryTestFixture():base()
        {
            _mediator = _serviceProvider.GetRequiredService<IMediator>();
            _customerRepository = _serviceProvider.GetRequiredService<ICustomerRepository>();
            _notification = _serviceProvider.GetRequiredService<Message.INotification>();
            _unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork>();
            _dbContext = _serviceProvider.GetRequiredService<MshopDbContext>();
            _addressRepository = _serviceProvider.GetRequiredService<IAddressRepository>();
        }

        protected GetCustomerByNameQuery RequestValid(string name)
        {
            return new GetCustomerByNameQuery(name);
        }
    }
}
