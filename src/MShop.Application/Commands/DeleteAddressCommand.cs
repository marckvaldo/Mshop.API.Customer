using MediatR;

namespace MShop.Application.Commands
{
    public class DeleteAddressCommand : IRequest<bool>
    {
        public Guid AddressId { get; }

        public DeleteAddressCommand(Guid addresId)
        {
            AddressId = addresId;
        }
    }
}