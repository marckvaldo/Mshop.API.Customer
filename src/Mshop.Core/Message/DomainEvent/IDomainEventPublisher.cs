using CoreObject = Mshop.Core.DomainObject;

namespace Mshop.Core.Message.DomainEvent
{
    public interface IDomainEventPublisher
    {
        Task<bool> PublishAsync<TDomainEvent>(TDomainEvent entity, CancellationToken cancellationToken) where TDomainEvent : CoreObject.DomainEvent;

        Task<bool> PublishAsync<TDomainEvent>(IEnumerable<TDomainEvent> entities, CancellationToken cancellationToken) where TDomainEvent : CoreObject.DomainEvent;

    }
}
