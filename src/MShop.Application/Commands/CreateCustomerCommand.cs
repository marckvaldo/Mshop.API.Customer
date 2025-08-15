using MediatR;
using Mshop.Application.Dtos;

namespace Mshop.Application.Commands
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