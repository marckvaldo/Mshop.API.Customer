using MShop.Core.Test.Domain.Entity.Address;
using MShop.Core.Test.Domain.Entity.Customer;
using MShop.Domain.Entities;
using MShop.Domain.ValueObjects;

namespace MShop.UnitTest.Application.Queries.Common
{
    public class QueriesFixture
    {
        protected CustomerFaker _customerFaker;
        protected AddressFaker _addressFaker;

        public QueriesFixture()
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

        public Address GetAddressFaker(Customer customer)
        {
            var address = _addressFaker.Generate();
            address.AddCustomer(customer);
            return address;
        }

        public List<Address> ListAddressFaker(Customer customer)
        {
            var addresss = _addressFaker.Generate(10);
            var address = GetAddressFaker(customer);
            address.AddCustomer(customer);
            return addresss;

        }
    }
}
