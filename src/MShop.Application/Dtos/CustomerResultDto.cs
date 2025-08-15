using Mshop.Core.DomainObject;

namespace Mshop.Application.Dtos
{
    public class CustomerResultDto : IModelOutPut
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public AddressResultDto Address { get; set; }
    }

    public class AddressResultDto : IModelOutPut
    {
        public string Street { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}