using MediatR;
using MShop.Application.Dtos;

namespace MShop.Application.Commands
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