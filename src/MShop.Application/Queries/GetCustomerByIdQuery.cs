using MediatR;
using Mshop.Application.Dtos;
using Mshop.Core.DomainObject;
using Mshop.Core.Message;

namespace Mshop.Application.Queries
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