using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MShop.Core.Message;
using MShop.Domain.Entities;
using MShop.Infra.Keycloak.Config;
using MShop.Infra.Keycloak.DTOs;
using MShop.Infra.Keycloak.Interfaces;
using MShop.Infra.Keycloak.Services;
using MShop.IntegrationTest.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.IntegrationTest.Application.Keycloak
{
    public class KeycloakServiceTestFixture : IntegrationBaseFixture
    {
        protected IIdentityProviderService _IdentityProviderService;
        public KeycloakServiceTestFixture() : base()
        {
            _IdentityProviderService = _serviceProvider.GetRequiredService<IIdentityProviderService>();            
        }

        /*public Customer RequestCommandValid()
        {
            var customer = _customerFaker.Generate();
            customer.SetPassword("123456#User");
            return customer;    
        }*/

        public RequestUsers RequestCommandvalid()
        {
            var customer = _customerFaker.Generate();
            return new RequestUsers(customer.Name, customer.Email, customer.Phone, "123456#User");
        }
    }
}
