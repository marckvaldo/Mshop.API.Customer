using Microsoft.EntityFrameworkCore.Storage;
using Mshop.Core.Data;
using Mshop.Core.DomainObject;
using Mshop.Core.Message.DomainEvent;
using Mshop.Infra.Data.Context;
using MShop.Domain;

namespace Mshop.Infra.Data.UnitOfWork
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