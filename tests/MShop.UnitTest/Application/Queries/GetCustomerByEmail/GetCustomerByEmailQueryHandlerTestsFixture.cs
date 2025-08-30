using MediatR;
using Moq;
using Mshop.Application.Commands.Handlers;
using Mshop.Application.Dtos;
using Mshop.Application.Queries;
using Mshop.Application.Queries.Handlers;
using Mshop.Core.Data;
using Mshop.Core.Message;
using Mshop.Infra.Data.Interface;
using Mshop.Infra.Data.Repository;
using MShop.Core.Test.Domain.Entity.Customer;
using MShop.Domain.Entities;
using MShop.UnitTest.Application.Queries.Common;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using CoreMessage = Mshop.Core.Message;

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