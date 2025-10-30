using MShop.Core.Data;
using MShop.Core.DomainObject;
using MShop.Core.Message.DomainEvent;
using MShop.Infra.Data.Context;

namespace MShop.Infra.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MshopDbContext _context;
        private readonly IDomainEventPublisher _publisher;

        public UnitOfWork(MshopDbContext context, IDomainEventPublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            var aggregateRoot = _context.ChangeTracker
                 .Entries<Entity>()
                 .Where(x => x.Entity.Events.Any())
                 .Select(x => x.Entity)
                 .ToList();

            var events = aggregateRoot
                .SelectMany(x => x.Events)
                .ToList();

            //end to get events from domain

            await _context.SaveChangesAsync(cancellationToken);

            foreach (var @event in events)
                await _publisher.PublishAsync((dynamic)@event, cancellationToken);

        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}