using FluentValidation;
using Mshop.Core.DomainObject;
using Mshop.Core.Message;
using MShop.Domain.Validation;
using MShop.Domain.ValueObjects;
using System.Net;

namespace MShop.Domain.Entities
{
    public class Customer : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string? Password { get; private set; }
        public bool CreatedInKeycloak { get; private set; }
        public Address? Address { get; private set; }
        public Guid AddressId { get; private set; }

        public Customer(string name, string email, string phone)
        {
            Name = name;
            Email = email;
            Phone = phone;
        }

        public bool AddAddress(Address address)
        {
            Address = address;
            AddressId = address.Id;
            return true;
        }

        public bool RemoveAddress()
        {
            Address = null;
            AddressId = Guid.Empty;
            return true;
        }

        public void UpdateCustomer(string name, string email, string phone)
        {
            Name = name;
            Email = email;
            Phone = phone;
        }

        public override bool IsValid(INotification notification)
        {
            var validator = new CustomerValidation(false);
            var result = validator.Validate(this);

            foreach (var error in result.Errors)
            {
                notification.AddNotifications(error.ErrorMessage);
            }

            if (Address is not null)
                Address.IsValid(notification);

            return !notification.HasErrors();
        }

        public bool IsValidPassword(INotification notification)
        {
            var validator = new CustomerValidation();
            var result = validator.Validate(this);

            foreach (var error in result.Errors)
            {
                notification.AddNotifications(error.ErrorMessage);
            }

            if (Address is not null)
                Address.IsValid(notification);

            return !notification.HasErrors();
        }

        public void SetPassword(string password)
        {
            Password = password;
        }

        public void SetCreatedInKeycloakFalse()
        {
            CreatedInKeycloak = false;
        }

        public void SetCreatedInKeycloakTrue()
        {
            CreatedInKeycloak = true;
            Password = null;
        }

    }
}