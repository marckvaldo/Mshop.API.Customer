using MediatR;

namespace Mshop.Application.Commands
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