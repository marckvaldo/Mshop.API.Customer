using CoreObject = MShop.Core.DomainObject;

namespace MShop.Core.Message.DomainEvent
{
    public interface IDomainEventHandler<TDomainEvent> where TDomainEvent : CoreObject.DomainEvent
    {
        Task<bool> HandlerAsync(TDomainEvent domainEvent, CancellationToken cancellationToken);
    }
}
