using MediatR;
using Mshop.Application.Dtos;

namespace Mshop.Application.Commands
{
    public class AddAddressCommand : IRequest<bool>
    {
        public UpdateCustomerAddressDto Address { get; }

        public AddAddressCommand(UpdateCustomerAddressDto address)
        {
            Address = address;
        }
    }
}