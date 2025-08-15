using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Mshop.Core.Message;
using MShop.Infra.Keycloak.Config;
using MShop.Infra.Keycloak.Interfaces;

namespace MShop.Infra.Keycloak.Services
{
    public class KeycloakUserService : IKeycloakUserService
    {
        private readonly HttpClient _httpClient;
        private readonly KeycloakSettings _settings;
        private INotification _notification;

        public KeycloakUserService(INotification notification, HttpClient httpClient, KeycloakSettings settings)
        {
            _httpClient = httpClient;
            _settings = settings;
            _notification = notification;
        }

        public async Task<bool> CreateUserAsync(
            string name,
            string email,
            string phone,
            string password,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var token = await GetKeycloakTokenAsync(cancellationToken);
                if (string.IsNullOrEmpty(token))
                    return false;

                var userPayload = new
                {
                    username = email,
                    email = email,
                    enabled = true,
                    firstName = name,
                    attributes = new
                    {
                        phone = new[] { phone }
                    },
                    credentials = new[]
                    {
                        new {
                            type = "password",
                            value = password,
                            temporary = false
                        }
                    }
                };

                var json = JsonSerializer.Serialize(userPayload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var url = $"{_settings.AuthServerUrl}/admin/realms/{_settings.Realm}/users";
                var response = await _httpClient.PostAsync(url, content, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync(cancellationToken);
                    //_notification.AddNotifications($"Erro ao cadastrar usuário no Keycloak: {error}");
                    _notification.AddNotifications($"Seu usuario foi criado com sucesso! mais por enquanto não é possível você fazer o login! Aguarde mais alguns instantes.");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                //_notification.AddNotifications($"Erro ao cadastrar usuário no Keycloak: {ex.Message}");
                _notification.AddNotifications($"Seu usuario foi criado com sucesso! mais por enquanto não é possível você fazer o login! Aguarde mais alguns instantes.");
                return false;
            }
        }

        private async Task<string?> GetKeycloakTokenAsync(CancellationToken cancellationToken)
        {
            var tokenUrl = $"{_settings.AuthServerUrl}/realms/{_settings.Realm}/protocol/openid-connect/token";
            var parameters = new Dictionary<string, string>
            {
                { "client_id", _settings.ClientId },
                { "client_secret", _settings.ClientSecret },
                { "grant_type", "client_credentials" }
            };

            var content = new FormUrlEncodedContent(parameters);
            var response = await _httpClient.PostAsync(tokenUrl, content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                _notification.AddNotifications($"Erro ao obter token do Keycloak: {error}");
                return null;
            }

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("access_token", out var tokenElement))
                return tokenElement.GetString();

            _notification.AddNotifications("Token de acesso não encontrado na resposta do Keycloak.");
            return null;
        }
    }
}