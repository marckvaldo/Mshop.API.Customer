using Microsoft.AspNetCore.Mvc;
using MShop.Infra.Keycloak.CircuitBreaker;
using MShop.Infra.Keycloak.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Infra.Keycloak.Handlers
{
    public class KeycloakHandlerDelegate : DelegatingHandler
    {
        private readonly IIdentityTokenProviderService _identityProviderService;
        private readonly ICircuitBreaker _circuitBreaker;

        public KeycloakHandlerDelegate(IIdentityTokenProviderService identityProviderService, ICircuitBreaker circuitBreaker)
        {
            _identityProviderService = identityProviderService;
            _circuitBreaker = circuitBreaker;

            _circuitBreaker.Start(
               ex =>
               {
                   return ex is Exception || ex is HttpRequestException;
               },
               1,
               TimeSpan.FromSeconds(30)
           );
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                //var token = await _identityProviderService.GetTokenAsync(cancellationToken);
                var token = await _circuitBreaker.ExecuteActinAsync(async () =>
                    await _identityProviderService.GetTokenAsync(cancellationToken)
                );

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await base.SendAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                //return await base.SendAsync(request, cancellationToken);
                throw new Exception("Erro ao obter token de autenticação.", ex);
            }
        }
    }
}
