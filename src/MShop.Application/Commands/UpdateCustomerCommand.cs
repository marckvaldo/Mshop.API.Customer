using MediatR;
using Mshop.Application.Dtos;

namespace Mshop.Application.Commands
{
    public class UpdateCustomerCommand : IRequest<bool>
    {
        public UpdateCustomerDto Customer { get; }

        public UpdateCustomerCommand(UpdateCustomerDto customer)
        {
            Customer = customer;
        }
    }
}