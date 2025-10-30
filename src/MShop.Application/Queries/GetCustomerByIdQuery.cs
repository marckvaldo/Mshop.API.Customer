using MediatR;
using MShop.Application.Dtos;
using MShop.Core.DomainObject;
using MShop.Core.Message;

namespace MShop.Application.Queries
{
    public class GetCustomerByIdQuery : IRequest<Result<CustomerResultDto>>
    {
        public Guid Id { get; }

        public GetCustomerByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}