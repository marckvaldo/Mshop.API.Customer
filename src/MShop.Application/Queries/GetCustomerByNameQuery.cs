using MediatR;
using Mshop.Core.Message;
using Mshop.Application.Dtos;
using Mshop.Core.DomainObject;

namespace Mshop.Application.Queries
{
    public class GetCustomerByNameQuery : IRequest<Result<CustomerResultDto>>
    {
        public string Name { get; }

        public GetCustomerByNameQuery(string name)
        {
            Name = name;
        }
    }
}