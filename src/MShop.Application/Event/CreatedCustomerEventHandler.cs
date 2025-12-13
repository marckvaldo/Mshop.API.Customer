using MShop.Core.Message;
using MShop.Core.Message.DomainEvent;
using MShop.Domain.Event;
using MShop.Infra.Data.Interface;
using MShop.Infra.Keycloak.Interfaces;

namespace MShop.Application.Event
{
    public class CreatedCustomerEventHandler : IDomainEventHandler<CreatedCustomerEvent>
    {
        //private readonly INotification _notification;
        private readonly IIdentityProviderService _IdentityProviderService;
        //private readonly ICustomerRepository _customerRepository;

        public CreatedCustomerEventHandler(
            //INotification notification, 
            IIdentityProviderService IdentityProviderService)
            //ICustomerRepository customerRepository)
        {
            //_notification = notification;
            _IdentityProviderService = IdentityProviderService;
            //_customerRepository = customerRepository;
        }

        public async Task<bool> HandlerAsync(CreatedCustomerEvent domainEvent, CancellationToken cancellationToken)
        {
            /*var customer = await _customerRepository.GetById(domainEvent.CustomerId);

            if (customer is null)
            {
                _notification.AddNotifications("Customer não encontrado.");
                return false;
            }

            var result = await _IdentityProviderService.CreateUserAsync(
                new Infra.Keycloak.DTOs.RequestUsers(
                domainEvent.Name,
                domainEvent.Email,
                domainEvent.Phone,
                domainEvent.Password),
                cancellationToken: default
            );



            if (Guid.Empty != result)
            {
                await _IdentityProviderService.SendEmailVerifyAsync(result, cancellationToken);
                customer.SetProviderIdentityId(result);
            }

            await _customerRepository.Update(customer, cancellationToken);*/

            return true;
        }
    }
}