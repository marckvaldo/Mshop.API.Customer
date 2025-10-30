using MediatR;
using MShop.Application.Dtos;

namespace MShop.Application.Commands
{
    public class AddAddressCommand : IRequest<bool>
    {
        public AddressDto Address { get; }

        public AddAddressCommand(AddressDto address)
        {
            Address = address;
        }
    }
}