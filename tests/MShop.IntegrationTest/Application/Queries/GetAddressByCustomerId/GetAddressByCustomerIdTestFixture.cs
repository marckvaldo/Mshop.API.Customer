using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using MShop.Core.Data;
using MShop.Core.DomainObject;
using MShop.Domain.Entities;
using MShop.Infra.Data.Context;
using MShop.Infra.Data.Interface;
using MShop.IntegrationTest.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Message = MShop.Core.Message;
using QueriesApplication = MShop.Application.Queries;

namespace MShop.IntegrationTest.Application.Queries.GetAddressByCustomerId
{
    public class GetAddressByCustomerIdTestFixture : IntegrationBaseFixture
    {
        protected IMediator _mediator;
        protected ICustomerRepository _customerRepository;
        protected IAddressRepository _addressRepository;
        protected Message.INotification _notification;
        protected IUnitOfWork _unitOfWork;
        protected MshopDbContext _dbContext;
        public GetAddressByCustomerIdTestFixture() : base()
        {
            _mediator = _serviceProvider.GetRequiredService<IMediator>();
            _customerRepository = _serviceProvider.GetRequiredService<ICustomerRepository>();
            _notification = _serviceProvider.GetRequiredService<Message.INotification>();
            _unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork>();
            _dbContext = _serviceProvider.GetRequiredService<MshopDbContext>();
            _addressRepository = _serviceProvider.GetRequiredService<IAddressRepository>();
        }

        public QueriesApplication.GetAddressByCustomerIdQuery RequestValidByCustomerId(Guid CustomerId)
        {
            return new QueriesApplication.GetAddressByCustomerIdQuery(CustomerId);
        }

        public QueriesApplication.GetAddressByIdQuery RequestValidById(Guid CustomerId)
        {
            return new QueriesApplication.GetAddressByIdQuery(CustomerId);
        }

        public void DetachedEntity(Entity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Detached;
        }
    }
}
