using Castle.Core.Resource;
using MShop.Domain.Entities;

namespace MShop.IntegrationTest.Application.Command.UpdateCustomer
{
    [Collection("Update Customer Collection")]
    [CollectionDefinition("Update Customer Collection", DisableParallelization = true)]
    public class UpdateCustomerCommandTest : UpdateCustomerCommandTestFixture
    {
        public UpdateCustomerCommandTest():base()
        {
            DeleteDataBase(_dbContext).Wait();
            AddMigration(_dbContext).Wait();
        }

        [Fact(DisplayName = "Upadte customer should result success")]
        [Trait("Integration - Application.Command", "Create Customer")]
        public async Task UpdateCustomerShouldReturnSuccess()
        {
            // Arrange
            var customer = await CriarCustomer();
            var command = RequestCommandValid(customer.Id, customer.Email);
            // Act

            var result = await _mediator.Send(command);
            var customerDb = _customerRepository.Filter(c => c.Email == command.Customer.Email).Result.FirstOrDefault();

            // Assert
            Assert.True(result);
            Assert.False(_notification.HasErrors());
            Assert.NotNull(customer);
            Assert.Equal(command.Customer.Name, customerDb.Name);
            Assert.Equal(command.Customer.Email, customerDb.Email);
            Assert.Equal(command.Customer.Phone, customerDb.Phone);
        }


        [Theory(DisplayName = "Update Customer with invalid information")]
        [Trait("Integration - Application.Command", "Create Customer")]
        [InlineData("marckvaldo@marckvaldo.com.br", "", "79801875", false)]
        [InlineData("marckvaldo@marckvaldo.com.br", "marckvaldo", "", false)]
        [InlineData("marckvaldo@marckvaldo.com.br", "marckvaldo", "(87)9801-8750", true)]
        [InlineData("marckvaldo@marckvaldo.com.br", "marckvaldo wallas", "8798018750", true)]
        [InlineData("marckvaldo@marckvaldo.com.br", "marckvaldo wallas", "98018750", false)]
        public async Task UpdateCustomerWithInvalidInformationShoudReturnNotCreateCustomer(string email, string name, string phone, bool expectedResult)
        {
            // Arrange
            var customer = await CriarCustomer(email,name,phone);
            var customerDb = await GetCustomerByEmail(email);
            var command = RequestCommandValid(customerDb.Id, email, name, phone);

            // Act
            var result = await _mediator.Send(command);

            // Assert
            if (!expectedResult)
            {
                Assert.False(result);
                Assert.True(_notification.HasErrors());
            }
            else
            {
                Assert.True(result);
                Assert.False(_notification.HasErrors());
            }

        }

        [Fact(DisplayName = "Register Customer when there is another cutomer with the same email")]
        [Trait("Integration - Application.Command", "Create Customer")]
        public async Task RegisterCustomerWhenThereISCustomerWithTheSameEmailShouldNotCreatedCustomer()
        {
            // Arrange
            var customer = await CriarCustomer();
            var command = RequestCommandValid(customer.Email);

            // Act
            var result = await _mediator.Send(command);

            // Assert
            Assert.False(result);
            Assert.True(_notification.HasErrors());
        }


        private async Task<Customer> CriarCustomer()
        {
            var customer = _customerFaker.Generate();
            customer.SetPassword("password");
            await _customerRepository.Create(customer, CancellationToken.None);
            await _unitOfWork.CommitAsync();
            DetachedEntity(customer);
            return customer;
        }

        private async Task<Customer> CriarCustomer(string email, string name, string phone)
        {
            var customer = new Customer(name, email, phone);
            customer.SetPassword("password");
            await _customerRepository.Create(customer, CancellationToken.None);
            await _unitOfWork.CommitAsync();
            DetachedEntity(customer);
            return customer;
        }

        private async Task<Customer> GetCustomerByEmail(string email)
        {
            var customerFilter = await _customerRepository.Filter(c => c.Email == email);
            var customerDb = customerFilter.FirstOrDefault();
            DetachedEntity(customerDb);
            return customerDb;
        }
    }
}
