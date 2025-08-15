using MediatR;
using Mshop.Core.Message;
using Mshop.Application.Dtos;
using Mshop.Core.DomainObject;

namespace Mshop.Application.Queries
{
    public class GetCustomerByEmailQuery : IRequest<Result<CustomerResultDto>>
    {
        public string Email { get; }

        public GetCustomerByEmailQuery(string email)
        {
            Email = email;
        }
    }
}