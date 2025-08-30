using MShop.Core.Test.Domain.Entity.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MShop.Domain.Entities;

namespace MShop.UnitTest.Application.Queries.Common
{
    public class QueriesFixture
    {
        protected CustomerFaker _customerFaker;

        public QueriesFixture()
        {
            _customerFaker = new CustomerFaker();
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
    }
}
