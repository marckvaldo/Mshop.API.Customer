using MShop.Domain.ValueObjects;
using Mshop.Infra.Data.Context;
using Mshop.Infra.Data.Interface;

namespace Mshop.Infra.Data.Repository
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(MshopDbContext context) : base(context) { }
        // M�todos espec�ficos para Address, se necess�rio
    }
}