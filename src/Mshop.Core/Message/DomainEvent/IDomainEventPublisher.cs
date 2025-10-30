using CoreObject = MShop.Core.DomainObject;

namespace MShop.Core.Message.DomainEvent
{
    public interface IDomainEventPublisher
    {
        Task<bool> PublishAsync<TDomainEvent>(TDomainEvent entity, CancellationToken cancellationToken) where TDomainEvent : CoreObject.DomainEvent;

        Task<bool> PublishAsync<TDomainEvent>(IEnumerable<TDomainEvent> entities, CancellationToken cancellationToken) where TDomainEvent : CoreObject.DomainEvent;

    }
}
