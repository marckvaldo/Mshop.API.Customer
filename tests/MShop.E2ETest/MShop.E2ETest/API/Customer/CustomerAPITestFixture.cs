using Bogus.DataSets;
using MShop.Application.Commands;
using MShop.Core.Test.Domain.Entity.Address;
using MShop.Core.Test.Domain.Entity.Customer;
using MShop.E2ETest.Base;
using MShop.E2ETest.Common;
using DTOs = MShop.Application.Dtos;

namespace MShop.E2ETest.API.Customer
{
    public class CustomerAPITestFixture : BaseFixture
    {
        
        public CustomerAPITestFixture() : base() 
        {

        }

        protected CreateCustomerCommand RequestCreateCustomerValid(string email = null, string name = null, string phone = null, bool address = false)
        {
            var customer = _customerFaker.Generate();
            var addressValid = _addressFaker.Generate();

            var customerDto = new DTOs.CreateCustomerDto
            {
                Name = name is null ? customer.Name : name,
                Email = email is null ? customer.Email : email,
                Phone = phone is null ? customer.Phone : phone,
                Password = "senha123"
            };

            if (address)
            {
                customerDto.Address = new DTOs.AddressDto
                {
                    Street = addressValid.Street,
                    Number = addressValid.Number,
                    Complement = addressValid.Complement,
                    District = addressValid.District,
                    City = addressValid.City,
                    State = addressValid.State,
                    PostalCode = addressValid.PostalCode,
                    Country = addressValid.Country
                };
            }

            return new CreateCustomerCommand(customerDto);
        }

        protected UpdateCustomerCommand RequestUpdateCustomerValid(Guid id)
        {
            var customer = _customerFaker.Generate();

            var customerDto = new DTOs.UpdateCustomerDto
            {
                Id = id,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone,                
            };

            return new UpdateCustomerCommand(customerDto);
            
        }
    }
}
