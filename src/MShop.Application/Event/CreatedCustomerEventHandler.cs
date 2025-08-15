using Mshop.Core;
using Mshop.Core.Message;
using Mshop.Core.Message.DomainEvent;
using Mshop.Infra.Data.Interface;
using MShop.Domain.Event;
using MShop.Infra.Keycloak.Interfaces;
using System.Threading;

namespace Mshop.Application.Event
{
    public class CreatedCustomerEventHandler : IDomainEventHandler<CreatedCustomerEvent>
    {
        private readonly INotification _notification;
        private readonly IKeycloakUserService _keycloakUserService;
        private readonly ICustomerRepository _customerRepository;

        public CreatedCustomerEventHandler(
            INotification notification, 
            IKeycloakUserService keycloakUserService, 
            ICustomerRepository customerRepository)
        {
            _notification = notification;
            _keycloakUserService = keycloakUserService;
            _customerRepository = customerRepository;
        }

        public async Task<bool> HandlerAsync(CreatedCustomerEvent domainEvent, CancellationToken cancellationToken)
        {
            var result = await _keycloakUserService.CreateUserAsync(
                domainEvent.Name,
                domainEvent.Email,
                domainEvent.Phone,
                domainEvent.Password,
                cancellationToken: default
            );

            if (!result)
                return false;

            var customer = await _customerRepository.GetById(domainEvent.CustomerId);

            if (customer is null)
            {
                _notification.AddNotifications("Customer não encontrado.");
                return false;
            }

            customer.SetCreatedInKeycloakTrue();
            await _customerRepository.Update(customer, cancellationToken);
            return true;
        }
    }
}