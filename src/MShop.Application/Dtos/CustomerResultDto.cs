using MShop.Core.DomainObject;

namespace MShop.Application.Dtos
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
        public Guid Id { get; set; }
    }

    public class ListAddressResultDto : IModelOutPut
    {
        public List<AddressResultDto> Addresses { get; set; } = new();
    }
}