using Microsoft.EntityFrameworkCore;
using Mshop.Core.Data;
using Mshop.Core.Paginated;
using System.Linq.Expressions;

namespace Mshop.Infra.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Mshop.Core.DomainObject.Entity
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual async Task Create(TEntity entity, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public virtual async Task Update(TEntity entity, CancellationToken cancellationToken)
        {
            _dbSet.Update(entity);
            await Task.CompletedTask;
        }

        public virtual async Task DeleteById(TEntity entity, CancellationToken cancellationToken)
        {
            _dbSet.Remove(entity);
            await Task.CompletedTask;
        }

        public virtual async Task<TEntity?> GetById(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<List<TEntity>> GetValuesList()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<List<TEntity>> Filter(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<TEntity> GetLastRegister(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).OrderByDescending(e => e.Id).FirstOrDefaultAsync();
        }

        public virtual async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}