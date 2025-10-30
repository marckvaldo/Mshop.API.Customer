using MShop.Domain.Entities;
using MShop.Core.Data;

namespace MShop.Infra.Data.Interface
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        // Métodos específicos para Customer, se necessário
    }
}