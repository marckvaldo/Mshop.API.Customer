using MShop.Domain.Entities;
using MShop.Infra.Data.Context;
using MShop.Infra.Data.Interface;

namespace MShop.Infra.Data.Repository
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(MshopDbContext context) : base(context) { }

        // M�todos espec�ficos para Customer, se necess�rio
    }
}