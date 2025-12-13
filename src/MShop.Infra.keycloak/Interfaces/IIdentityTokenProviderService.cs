using Microsoft.Extensions.Caching.Distributed;
using MShop.Infra.Keycloak.DTOs;
using MShop.Infra.Keycloak.Services;
using System.Net;
using System.Net.Http;
using System.Runtime;
using System.Text;
using System.Text.Json;

namespace MShop.Infra.Keycloak.Interfaces
{
    public interface IIdentityTokenProviderService
    {
        Task<string> GetTokenAsync(CancellationToken cancellationToken);        
    }
}