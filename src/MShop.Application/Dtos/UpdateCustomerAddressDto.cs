namespace MShop.Application.Dtos
{
    public class UpdateCustomerAddressDto
    {
        public Guid CustomerId { get; set; }
        public AddressDto Address { get; set; }
    }
}