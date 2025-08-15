using MediatR;
using Mshop.Core.Base;
using Message = Mshop.Core.Message;
using MShop.Domain.Entities;
using MShop.Domain.ValueObjects;
using Mshop.Infra.Data.Interface;
using Mshop.Core.Data;

namespace Mshop.Application.Commands.Handlers
{
    public class AddAddressCommandHandler : BaseCommand, IRequestHandler<AddAddressCommand, bool>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddAddressCommandHandler(
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

        public async Task<bool> Handle(AddAddressCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetById(request.Address.CustomerId);
            if (customer == null)
            {
                Notificar("Customer não encontrado.");
                return false;
            }

            var hasAddress =  await _addressRepository.GetById(customer.AddressId);
            if (hasAddress is not null)
            {
                Notificar("Cliente já tem Endereço!");
                return false;
            }

            var address = new Address(
                request.Address.Address.Street,
                request.Address.Address.Number,
                request.Address.Address.Complement,
                request.Address.Address.District,
                request.Address.Address.City,
                request.Address.Address.State,
                request.Address.Address.PostalCode,
                request.Address.Address.Country
            );
           
            customer.AddAddress(address);

            if (!customer.IsValid(Notifications) || TheareErrors())
                return false;

            await _addressRepository.Create(address, cancellationToken);
            await _customerRepository.Update(customer, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return true;
        }
    }
}