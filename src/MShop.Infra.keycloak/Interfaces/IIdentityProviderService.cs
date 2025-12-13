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
    public interface IIdentityProviderService
    {
        Task<string?> CreateUserAsync(RequestUsers request, CancellationToken cancellationToken = default);
        Task<bool> SendEmailVerifyAsync(string userId, CancellationToken cancellationToken);
        Task<List<ResultUser>?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);       
    }
}