using MShop.Core.DomainObject;
using MShop.Core.Message;
using MShop.Domain.Entities;
using MShop.Domain.Validation;

namespace MShop.Domain.ValueObjects
{
    public class Address : Entity
    {
        public string Street { get; private set; }
        public string Number { get; private set; }
        public string Complement { get; private set; }
        public string District { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string PostalCode { get; private set; }
        public string Country { get; private set; }
        public Guid CustomerId { get; private set; }

        public Customer Customer { get; private set; }

        public Address(
            string street,
            string number,
            string complement,
            string district,
            string city,
            string state,
            string postalCode,
            string country)
        {
            Street = street;
            Number = number;
            Complement = complement;
            District = district;
            City = city;
            State = state;
            PostalCode = postalCode;
            Country = country;
        }

        public void UpdateAddress(string street,
            string number,
            string complement,
            string district,
            string city,
            string state,
            string postalCode,
            string country)
        {
            Street = street;
            Number = number;
            Complement = complement;
            District = district;
            City = city;
            State = state;
            PostalCode = postalCode;
            Country = country;
        }

        public bool IsValid(INotification notification)
        {
            var validator = new AddressValidation();
            var result = validator.Validate(this);

            foreach (var error in result.Errors)
            {
                notification.AddNotifications(error.ErrorMessage);
            }

            return result.IsValid;
        }

        public bool AddCustomer(Customer customer)
        {
            Customer = customer;
            CustomerId = customer.Id;
            return true;
        }
    }
}