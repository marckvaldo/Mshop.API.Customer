using MediatR;
using MShop.Core.Message;
using MShop.Application.Dtos;
using MShop.Core.DomainObject;

namespace MShop.Application.Queries
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