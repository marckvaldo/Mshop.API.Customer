using MediatR;
using MShop.Application.Dtos;

namespace MShop.Application.Commands
{
    public class AddAddressCommand : IRequest<bool>
    {
        public Guid CustomerId { get; set; }
        public AddressDto Address { get; }

        public AddAddressCommand(AddressDto address, Guid customerId)
        {
            Address = address;
            CustomerId = customerId;
        }
    }
}