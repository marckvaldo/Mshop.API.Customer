using MediatR;
using MShop.Core.Base;
using MShop.Core.Data;
using MShop.Infra.Data.Interface;
using MShop.Infra.Data.Repository;
using MShop.Domain.Entities;
using MShop.Domain.Event;
using MShop.Domain.ValueObjects;
using MShop.Infra.Data.Interface;
using Message = MShop.Core.Message;
using MShop.Infra.Keycloak.Interfaces;
using Polly.CircuitBreaker;

namespace MShop.Application.Commands.Handlers
{
    public class CreateCustomerCommandHandler : BaseCommand, IRequestHandler<CreateCustomerCommand, bool>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAddressRepository _addressRepository;
        //private readonly ICryptoService _cryptoService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityProviderService _identityProviderService;

        public CreateCustomerCommandHandler(
            //ICryptoService cryptoService,
            ICustomerRepository customerRepository,
            IAddressRepository address,
            IUnitOfWork unitOfWork,
            Message.INotification notification,
            IIdentityProviderService identityProviderService
        ) : base(notification)
        {
            //_cryptoService = cryptoService;
            _customerRepository = customerRepository;
            _addressRepository = address;
            _unitOfWork = unitOfWork;
            _identityProviderService = identityProviderService;
        }

        public async Task<bool> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            // Verifica se já existe customer com o mesmo email
            var existing = await _customerRepository.Filter(c => c.Email == request.Customer.Email);
            if (existing.Any())
            {
                Notificar("Usuario já está cadastrado para esse email");
                return false;
            }

            var customer = new Customer(
               request.Customer.Name,
               request.Customer.Email,
               request.Customer.Phone
            );

            if (request.Customer.Address is not null)
            {
                var address = new Address(
                    request.Customer.Address.Street,
                    request.Customer.Address.Number,
                    request.Customer.Address.Complement,
                    request.Customer.Address.District,
                    request.Customer.Address.City,
                    request.Customer.Address.State,
                    request.Customer.Address.PostalCode,
                    request.Customer.Address.Country
                );

                address.AddCustomer(customer);
            }

            if (!customer.IsValid(Notifications) || TheareErrors())
                return false;

            customer.SetPassword("-");

            // Adiciona evento de domínio
            /*customer.RegisterEvent(new CreatedCustomerEvent(
                customer.Name,
                customer.Email,
                customer.Phone,
                customer.Password,
                customer.Id
            ));*/

            try
            {
                var resulUserId = await _identityProviderService.CreateUserAsync(
                                    new Infra.Keycloak.DTOs.RequestUsers(
                                        customer.Name,
                                        customer.Email,
                                        customer.Phone,
                                        request.Customer.Password)
                                    );

                if (resulUserId is not null)
                {
                    customer.SetProviderIdentityId(resulUserId);

                    if (customer.Address is not null)
                        await _addressRepository.Create(customer.Address, cancellationToken);


                    await _customerRepository.Create(customer, cancellationToken);
                    await _unitOfWork.CommitAsync(cancellationToken);

                    await _identityProviderService.SendEmailVerifyAsync(resulUserId, cancellationToken);

                    return true;
                }
            }
            catch (Exception ex)
            {
                Notificar(ex.Message);
            }

            //Notificar("Não foi possivel cadastra o usuario");
            return false;

           
        }
    }
}