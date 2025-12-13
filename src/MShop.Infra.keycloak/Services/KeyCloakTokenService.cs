using Microsoft.Extensions.Caching.Distributed;
using MShop.Infra.Keycloak.CircuitBreaker;
using MShop.Infra.Keycloak.Config;
using MShop.Infra.Keycloak.Interfaces;
using RedLockNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MShop.Infra.Keycloak.Services
{
    public class KeyCloakTokenService : IIdentityTokenProviderService
    {
        private readonly KeycloakSettings _settings;
        private readonly IDistributedCache _cache;
        private readonly IDistributedLockFactory _distributedLock;
        private readonly HttpClient _httpClient;
        

        private const string TOKEN_CACHE_KEY = "KC_TOKEN";
        private const string TOKEN_LOCK_KEY = "KC_TOKEN_LOCK";

        public KeyCloakTokenService(
            IDistributedCache cache,
            IDistributedLockFactory distributedLock,
            KeycloakSettings settings,
            IHttpClientFactory factory)
        {
            _settings = settings;
            _cache = cache;
            _distributedLock = distributedLock;

            // Cliente nomeado
            _httpClient = factory.CreateClient("keycloak-token-client");
        }

        public async Task<string> GetTokenAsync(CancellationToken cancellationToken)
        {
            var cachedToken = await GetTokenRedis(cancellationToken);
            if (!string.IsNullOrEmpty(cachedToken))
                return cachedToken;

            using (var redLock = await _distributedLock.CreateLockAsync(
               TOKEN_LOCK_KEY,
               expiryTime: TimeSpan.FromSeconds(10),
               waitTime: TimeSpan.FromSeconds(5),
               retryTime: TimeSpan.FromMilliseconds(200)))
            {

                if (!redLock.IsAcquired)
                {
                    return await _cache.GetStringAsync(TOKEN_CACHE_KEY, cancellationToken);
                }

                cachedToken = await GetTokenRedis(cancellationToken);
                if (!string.IsNullOrEmpty(cachedToken))
                    return cachedToken;

                var newToken = await RequestNewTokenAsync(cancellationToken);
                if (string.IsNullOrEmpty(newToken))
                    return null;

                await SetTokenRedis(newToken, cancellationToken);

                return newToken;
            }

        }
        private async Task<string?> RequestNewTokenAsync(CancellationToken cancellationToken)
        {

            var form = new Dictionary<string, string>
            {
                { "client_id", _settings.ClientId },
                { "client_secret", _settings.ClientSecret },
                { "grant_type", "client_credentials" }
            };

            var tokenUrl = $"/realms/{_settings.Realm}/protocol/openid-connect/token";

            var response = await _httpClient.PostAsync(tokenUrl, new FormUrlEncodedContent(form), cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new Exception(erro);
            }

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("access_token", out var token))
            {
                return null;
            }

            return token.GetString();

        }

        private async Task<string?> GetTokenRedis(CancellationToken cancellationToken)
        {
            return await _cache.GetStringAsync(TOKEN_CACHE_KEY, cancellationToken);
        }

        private async Task SetTokenRedis(string token, CancellationToken cancellationToken)
        {
            await _cache.SetStringAsync(
                TOKEN_CACHE_KEY,
                token,
                   new DistributedCacheEntryOptions
                   {
                       AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(4)
                   },
                cancellationToken
            );
        }
    }
}
