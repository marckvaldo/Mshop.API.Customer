namespace MShop.IntegrationTest.Application.Command.CreateCustomer
{
    [Collection("Create Customer Collection")]
    [CollectionDefinition("Create Customer Collection", DisableParallelization = true)]
    public class CreateCustomerCommandTest : CreateCustomerCommandTestFixture
    {
        public CreateCustomerCommandTest():base()
        {
            DeleteDataBase(_dbContext).Wait();
            AddMigration(_dbContext).Wait();
        }

        [Fact(DisplayName = "Register customer without address should result success")]
        [Trait("Integration - Application.Command", "Create Customer")]
        public async Task RegisterCustomerWithoutAddressShouldReturnSuccess()
        {
            // Arrange
            var command = RequestCommandValid();
            // Act
            var result = await _mediator.Send(command);
            var customer = _customerRepository.Filter(c => c.Email == command.Customer.Email).Result.FirstOrDefault();

            // Assert
            Assert.True(result);
            Assert.False(_notification.HasErrors());
            Assert.NotNull(customer);
            Assert.Equal(command.Customer.Name, customer.Name);
            Assert.Equal(command.Customer.Email, customer.Email);
            Assert.Equal(command.Customer.Phone, customer.Phone);
        }

        [Fact(DisplayName = "Register customer with an address valid should result success")]
        [Trait("Integration - Application.Command", "Create Customer")]
        public async Task RegisterCustomerWithAnAddressShouldReturnSuccess()
        {
            // Arrange
            var command = RequestCommandValid(null,null,null,true);
            // Act
            var result = await _mediator.Send(command);
            var customer = _customerRepository.Filter(c => c.Email == command.Customer.Email).Result.FirstOrDefault();

            // Assert
            Assert.True(result);
            Assert.False(_notification.HasErrors());
            Assert.NotNull(customer);
            Assert.Equal(command.Customer.Name, customer.Name);
            Assert.Equal(command.Customer.Email, customer.Email);
            Assert.Equal(command.Customer.Phone, customer.Phone);
        }

        [Theory(DisplayName = "Register Customer with invalid information")]
        [Trait("Integration - Application.Command", "Create Customer")]
        [InlineData("marckvaldo", "", "879801875", false)]
        [InlineData("marckvaldo@hotmail", "marckvaldo", "879801875", false)]
        [InlineData("marckvaldo@hotmail.com", "marckvaldo", "8798018750", true)]
        [InlineData("marckvaldo@marckvaldo.com.br", "marckvaldo wallas", "8798018750", true)]
        [InlineData("marckvaldo@marckvaldo.com.br", "marckvaldo wallas", "98018750", false)]
        public async Task RegisterCustomerWithInvalidInformationShoudReturnNotCreateCustomer(string email, string nome, string phone, bool expectedResult)
        {
            // Arrange
            var command = RequestCommandValid(email,nome,phone);
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
            var customerFaker = _customerFaker.Generate();
            customerFaker.SetPassword("senha123");
            var command = RequestCommandValid(customerFaker.Email);
            await _customerRepository.Create(customerFaker, CancellationToken.None);
            await _unitOfWork.CommitAsync(CancellationToken.None);
            // Act
            var result = await _mediator.Send(command);

            // Assert
            Assert.False(result);
            Assert.True(_notification.HasErrors());
        }
    }
}
