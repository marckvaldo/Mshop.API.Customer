using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.E2ETest.GraphQL.Common
{
    public class ResponseGraphQL<T>
    {
        public T Data { get; set; }
    }
    public class CustomerByNameResponse
    {
        public CustomerGraphQL CustomerByName { get; set; }
    }

    public class CustomerByIdResponse
    {
        public CustomerGraphQL CustomerById { get; set; }
    }

    public class CustomerByEmailResponse
    {
        public CustomerGraphQL CustomerByEmail { get; set; }
    }

    public class CustomerGraphQL
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public AddressGraphQL Address { get; set; }
    }

    public class AddressGraphQL
    {
        public string Number { get; set; }
        public string State { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Complement { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
    }
} 
