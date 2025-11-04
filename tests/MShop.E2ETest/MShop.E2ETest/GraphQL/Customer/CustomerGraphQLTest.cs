using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.E2ETest.GraphQL.Customer
{
    [Collection("API Customer GraphQL")]
    [CollectionDefinition("API Customer GraphQL", DisableParallelization = true)]
    public class CustomerGraphQLTest : CustomerGraphQLTestFixture
    {
        public CustomerGraphQLTest() : base()
        {
            
        }

        [Fact(DisplayName = nameof(GetCustomerById))]
        [Trait("EndToEnd/API", "Customer - Endpoints")]
        public void GetCustomerById()
        {

        }
    }
}
