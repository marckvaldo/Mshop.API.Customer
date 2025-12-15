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
            var command = RequestCommandvalid();
            // Act
            var result = await _IdentityProviderService.CreateUserAsync(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
        }

    }
}
