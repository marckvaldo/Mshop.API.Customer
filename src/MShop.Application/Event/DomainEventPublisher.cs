using Microsoft.Extensions.DependencyInjection;
using Mshop.Core.DomainObject;
using Mshop.Core.Message.DomainEvent;
using Message = Mshop.Core.Message;

namespace Mshop.Application.Event
{
    public class DomainEventPublisher : IDomainEventPublisher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Message.INotification _notification;

        public DomainEventPublisher(IServiceProvider serviceProvider, Message.INotification notification)
        {
            _serviceProvider = serviceProvider;
            _notification = notification;
        }

        public async Task<bool> PublishAsync<TDomainEvent>(TDomainEvent @event, CancellationToken cancellationToken) where TDomainEvent : DomainEvent
        {
            var handlerType = _serviceProvider.GetServices<IDomainEventHandler<TDomainEvent>>();

            if (handlerType is null || !handlerType.Any())
            {
                _notification.AddNotifications($"Handler not found in {@event}");
                return false;
            }

            var result = true;
            foreach (var handler in handlerType)
            {

                if (!await handler.HandlerAsync(@event, cancellationToken))
                {
                    Console.WriteLine($"Error in handler {handler.GetType().Name} for event {@event}");
                    result = false;
                }
            }

            return result;
        }

        public async Task<bool> PublishAsync<TDomainEvent>(IEnumerable<TDomainEvent> events, CancellationToken cancellationToken) where TDomainEvent : DomainEvent
        {
            foreach (var @event in events)
            {
                await PublishAsync(@event, cancellationToken);
            }
            return true;
        }
    }
}