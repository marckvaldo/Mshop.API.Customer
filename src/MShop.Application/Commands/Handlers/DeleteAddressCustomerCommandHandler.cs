using MediatR;
using MShop.Core.Base;
using Message = MShop.Core.Message;
using MShop.Domain.Entities;
using MShop.Infra.Data.Interface;
using MShop.Core.Data;

namespace MShop.Application.Commands.Handlers
{
    public class DeleteAddressCustomerCommandHandler : BaseCommand, IRequestHandler<DeleteAddressCommand, bool>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAddressCustomerCommandHandler(
            ICustomerRepository customerRepository,
            IAddressRepository addressRepository,
            IUnitOfWork unitOfWork,
            Message.INotification notification
        ) : base(notification)
        {
            _customerRepository = customerRepository;
            _addressRepository = addressRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            var address = await _addressRepository.GetById(request.AddressId);
            if (address == null)
            {
                Notificar("Endereço não encontrado.");
                return false;
            }

            await _addressRepository.DeleteById(address, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return true;
        }
    }
}