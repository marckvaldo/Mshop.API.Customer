using MShop.Core.DomainObject;

namespace MShop.Domain.Event
{
    public class CreatedCustomerEvent : DomainEvent
    {
        public string Name { get; }
        public string Email { get; }
        public string Phone { get; }
        //public string Street { get; }
        //public string Number { get; }
        //public string Complement { get; }
        //public string District { get; }
        //public string City { get; }
        //public string State { get; }
        //public string PostalCode { get; }
        //public string Country { get; }
        public string Password { get; }
        public Guid CustomerId { get; }

        public CreatedCustomerEvent(
            string name, string email, string phone,
            /*string street, string number, string complement, string district,
            string city, string state, string postalCode, string country*/ string password, Guid customerId)
        {
            Name = name;
            Email = email;
            Phone = phone;
            //Street = street;
            //Number = number;
            //Complement = complement;
            //District = district;
            //City = city;
            //State = state;
            //PostalCode = postalCode;
            //Country = country;
            Password = password;
            CustomerId = customerId;
        }
    }
}