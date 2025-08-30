using Microsoft.AspNetCore.SignalR.Protocol;
using Moq;
using Mshop.Application.Dtos;
using Mshop.Application.Queries;
using Mshop.Application.Queries.Handlers;
using Mshop.Core.Message;
using Mshop.Infra.Data.Interface;
using MShop.Core.Test.Domain.Entity.Customer;
using MShop.Domain.Entities;
using MShop.UnitTest.Application.Queries.Common;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using CoreMessage = Mshop.Core.Message;

namespace MShop.UnitTest.Application.Queries.GetCustomerByName
{
    public class GetCustomerByNameQueryHandlerTestsFixture : QueriesFixture
    {
        protected Mock<ICustomerRepository> _customerRepoMock;
        protected Mock<IAddressRepository> _addressRepositoryMock;
        protected CoreMessage.INotification _notification;
        protected CustomerFaker _customerFaker;

        public GetCustomerByNameQueryHandlerTestsFixture()
        {
            _customerRepoMock = new Mock<ICustomerRepository>();
            _addressRepositoryMock = new Mock<IAddressRepository>();
            _notification = new Notifications();
            _customerFaker = new CustomerFaker();
        }


        protected GetCustomerByNameQueryHandler CreateHandler(
           Mock<ICustomerRepository> customerRepoMock,
           Mock<IAddressRepository> addressRepoMock,
           CoreMessage.INotification notificationMock)
        {
            return new GetCustomerByNameQueryHandler(
                addressRepoMock.Object,
                customerRepoMock.Object,
                notificationMock
            );
        }

    }
}