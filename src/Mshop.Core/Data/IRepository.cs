using MShop.Core.DomainObject;
using MShop.Core.Paginated;
using System.Linq.Expressions;

namespace MShop.Core.Data
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        public Task Create(TEntity entity, CancellationToken cancellationToken);
        public Task Update(TEntity entity, CancellationToken cancellationToken);
        public Task DeleteById(TEntity entity, CancellationToken cancellationToken);
        public Task<TEntity?> GetById(Guid Id);
        public Task<List<TEntity>> GetValuesList();
        public Task<List<TEntity>> Filter(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetLastRegister(Expression<Func<TEntity, bool>> predicate);
        Task<int> SaveChanges();
    }
}
