using MediatR;
using MShop.Core.Message;
using MShop.Application.Dtos;
using MShop.Core.DomainObject;

namespace MShop.Application.Queries
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