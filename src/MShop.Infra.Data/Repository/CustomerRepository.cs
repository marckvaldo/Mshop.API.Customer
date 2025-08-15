using MShop.Domain.Entities;
using Mshop.Infra.Data.Context;
using Mshop.Infra.Data.Interface;

namespace Mshop.Infra.Data.Repository
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(MshopDbContext context) : base(context) { }

        // Métodos específicos para Customer, se necessário
    }
}