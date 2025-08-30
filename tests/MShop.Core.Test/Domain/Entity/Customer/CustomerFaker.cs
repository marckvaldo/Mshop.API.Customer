using Bogus;
using MShop.Core.Test.Domain.Entity.Address;
using MShop.Domain.Entities;
using DomainEntity = MShop.Domain.Entities;

namespace MShop.Core.Test.Domain.Entity.Customer
{
    public class CustomerFaker : Faker<DomainEntity.Customer>
    {
        public CustomerFaker()
        {
            CustomInstantiator(f =>
                new DomainEntity.Customer(
                    f.Person.FullName,
                    f.Internet.Email(),
                    f.Random.Replace("(##) 9####-####")
                )
            );

            RuleFor(c => c.Address, f => new AddressFaker().Generate());
        }
    }
}