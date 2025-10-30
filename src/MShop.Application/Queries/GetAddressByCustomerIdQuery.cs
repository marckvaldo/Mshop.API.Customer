using MediatR;
using MShop.Application.Dtos;
using MShop.Core.DomainObject;
using MShop.Core.Message;
using MShop.Domain.Entities;

namespace MShop.Application.Queries
{
    public class GetAddressByCustomerIdQuery : IRequest<Result<ListAddressResultDto>>
    {
        public Guid CustomerId { get; }

        public GetAddressByCustomerIdQuery(Guid customerId)
        {
            CustomerId = customerId;
        }
    }
}