using Bogus;
using DomainEntity = MShop.Domain.ValueObjects;

namespace MShop.Core.Test.Domain.Entity.Address
{
    public class AddressFaker : Faker<DomainEntity.Address>
    {
        public AddressFaker()
        {
            CustomInstantiator(f =>
                new DomainEntity.Address(
                    f.Address.StreetName(),
                    f.Random.Int(1, 9999).ToString(),
                    f.Address.SecondaryAddress(),
                    f.Address.City(),
                    f.Address.City(),
                    f.Address.StateAbbr(),
                    f.Address.ZipCode(),
                    f.Address.Country()
                )
            );
        }
    }
}