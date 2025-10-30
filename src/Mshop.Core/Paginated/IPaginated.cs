using MShop.Core.DomainObject;

namespace MShop.Core.Paginated
{
    public interface IPaginated<TEntity> where TEntity : Entity
    {
        Task<PaginatedOutPut<TEntity>> FilterPaginated(PaginatedInPut input);
    }
}
