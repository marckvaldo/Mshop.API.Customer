using MShop.Domain.Entities;
using Mshop.Core.Data;

namespace Mshop.Infra.Data.Interface
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        // Métodos específicos para Customer, se necessário
    }
}