using MediatR;
using MShop.Application.Dtos;

namespace MShop.Application.Commands
{
    public class CreateCustomerCommand : IRequest<bool>
    {
        public CreateCustomerDto Customer { get; }

        public CreateCustomerCommand(CreateCustomerDto customer)
        {
            Customer = customer;
        }
    }
}