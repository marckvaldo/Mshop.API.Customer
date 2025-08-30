using MShop.Core.Test.Domain.Entity.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MShop.Domain.Entities;
using MShop.Domain.ValueObjects;
using MShop.Core.Test.Domain.Entity.Address;

namespace MShop.UnitTest.Application.Commands.Common
{
    public class CommandsFixture
    {
        protected CustomerFaker _customerFaker;
        protected AddressFaker _addressFaker;

        public CommandsFixture()
        {
            _customerFaker = new CustomerFaker();
            _addressFaker = new AddressFaker();
        }
        public Customer GetCustomerFaker(string name = "", string email = "", string phone = "")
        {
            var customer = _customerFaker.Generate();

            customer.UpdateCustomer(
                name == "" ? customer.Name : name,
                email == "" ? customer.Email : email,
                phone == "" ? customer.Phone : phone
                );

            return customer;
        }

        public List<Customer> ListCustomerFaker(string name = "", string email = "", string phone = "")
        {
            var customers = _customerFaker.Generate(10);
            var customer = GetCustomerFaker(name, email, phone);
            customers.Add(customer);
            return customers;
        }

        public Address GetAddAddressFaker()
        {
            var address = _addressFaker.Generate();
            return address;
        }
    }
}
