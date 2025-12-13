using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.IntegrationTest.Application.Keycloak
{
    public class KeycloakServiceTest : KeycloakServiceTestFixture
    {
        public KeycloakServiceTest() : base()
        {
            
        }

        [Fact(DisplayName = "Register User in Identity Provider Should Return success")]
        [Trait("Integration - Application.Command", "Create User")]
        public async Task RegisterUserInIdentiryProviderShouldReturnSuccess()
        {
            // Arrange
            var command = RequestCommandValid();
            // Act
            var result = await _IdentityProviderService.CreateUserAsync(command.Name, command.Email, command.Phone, command.Password, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.False(_notification.HasErrors());
            Assert.NotNull(customer);
            Assert.Equal(command.Customer.Name, customer.Name);
            Assert.Equal(command.Customer.Email, customer.Email);
            Assert.Equal(command.Customer.Phone, customer.Phone);
        }

    }
}
