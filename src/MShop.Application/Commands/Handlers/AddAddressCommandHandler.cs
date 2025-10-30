using MediatR;
using MShop.Core.Base;
using Message = MShop.Core.Message;
using MShop.Domain.Entities;
using MShop.Domain.ValueObjects;
using MShop.Infra.Data.Interface;
using MShop.Core.Data;

namespace MShop.Application.Commands.Handlers
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

            var address = new Address(
                request.Address.Street,
                request.Address.Number,
                request.Address.Complement,
                request.Address.District,
                request.Address.City,
                request.Address.State,
                request.Address.PostalCode,
                request.Address.Country
            );
           address.AddCustomer(customer);

            if (!address.IsValid(Notifications) || TheareErrors())
                return false;

            await _addressRepository.Create(address, cancellationToken);
            //await _customerRepository.Update(customer, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return true;
        }
    }
}