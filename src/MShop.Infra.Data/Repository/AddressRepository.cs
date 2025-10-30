using MShop.Domain.ValueObjects;
using MShop.Infra.Data.Context;
using MShop.Infra.Data.Interface;

namespace MShop.Infra.Data.Repository
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(MshopDbContext context) : base(context) { }
        // Métodos específicos para Address, se necessário
    }
}