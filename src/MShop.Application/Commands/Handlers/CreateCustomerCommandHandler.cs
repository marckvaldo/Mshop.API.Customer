using MediatR;
using Mshop.Core.Base;
using Mshop.Core.Data;
using Mshop.Infra.Data.Interface;
using Mshop.Infra.Data.Repository;
using MShop.Domain.Entities;
using MShop.Domain.Event;
using MShop.Domain.ValueObjects;
using MShop.Infra.Data.Interface;
using Message = Mshop.Core.Message;

namespace Mshop.Application.Commands.Handlers
{
    public class CreateCustomerCommandHandler : BaseCommand, IRequestHandler<CreateCustomerCommand, bool>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly ICryptoService _cryptoService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCustomerCommandHandler(
            ICryptoService cryptoService,
            ICustomerRepository customerRepository,
            IAddressRepository address,
            IUnitOfWork unitOfWork,
            Message.INotification notification
        ) : base(notification)
        {
            _cryptoService = cryptoService;
            _customerRepository = customerRepository;
            _addressRepository = address;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            // Verifica se já existe customer com o mesmo email
            var existing = await _customerRepository.Filter(c => c.Email == request.Customer.Email);
            if (existing.Any())
            {
                Notificar("O customer já está cadastrado.");
                return false;
            }

            /*var address = new Address(
                request.Customer.Address.Street,
                request.Customer.Address.Number,
                request.Customer.Address.Complement,
                request.Customer.Address.District,
                request.Customer.Address.City,
                request.Customer.Address.State,
                request.Customer.Address.PostalCode,
                request.Customer.Address.Country
            );*/

            var customer = new Customer(
                request.Customer.Name,
                request.Customer.Email,
                request.Customer.Phone
            );

            //customer.AddAddress(address);
            customer.SetPassword(request.Customer.Password);
            customer.SetCreatedInKeycloakFalse();

            if (!customer.IsValid(Notifications) || TheareErrors())
                return false;

            customer.SetPassword(_cryptoService.Encrypt(request.Customer.Password));

            // Adiciona evento de domínio
            customer.RegisterEvent(new CreatedCustomerEvent(
                customer.Name,
                customer.Email,
                customer.Phone,
                //customer.Address.Street,
                //customer.Address.Number,
                //customer.Address.Complement,
                //customer.Address.District,
                //customer.Address.City,
                //customer.Address.State,
                //customer.Address.PostalCode,
                //customer.Address.Country,
                customer.Password,
                customer.Id
            ));

            //await _addressRepository.Create(customer.Address,cancellationToken);
            await _customerRepository.Create(customer, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return true;
        }
    }
}