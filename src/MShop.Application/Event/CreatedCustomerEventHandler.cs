using MShop.Core;
using MShop.Core.Message;
using MShop.Core.Message.DomainEvent;
using MShop.Infra.Data.Interface;
using MShop.Domain.Event;
using MShop.Infra.Keycloak.Interfaces;
using System.Threading;

namespace MShop.Application.Event
{
    public class CreatedCustomerEventHandler : IDomainEventHandler<CreatedCustomerEvent>
    {
        private readonly INotification _notification;
        private readonly IKeycloakService _keycloakService;
        private readonly ICustomerRepository _customerRepository;

        public CreatedCustomerEventHandler(
            INotification notification, 
            IKeycloakService keycloakUserService, 
            ICustomerRepository customerRepository)
        {
            _notification = notification;
            _keycloakService = keycloakUserService;
            _customerRepository = customerRepository;
        }

        public async Task<bool> HandlerAsync(CreatedCustomerEvent domainEvent, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetById(domainEvent.CustomerId);

            if (customer is null)
            {
                _notification.AddNotifications("Customer não encontrado.");
                return false;
            }

            var result = await _keycloakService.CreateUserAsync(
                domainEvent.Name,
                domainEvent.Email,
                domainEvent.Phone,
                domainEvent.Password,
                cancellationToken: default
            );

            if (!result)
                return false;


            customer.SetCreatedInKeycloakTrue();
            await _customerRepository.Update(customer, cancellationToken);
            return true;
        }
    }
}