using MShop.Infra.Keycloak.Config;
using MShop.Infra.Keycloak.DTOs;
using MShop.Infra.Keycloak.Interfaces;
using System.Net;
using System.Text;
using System.Text.Json;

namespace MShop.Infra.Keycloak.Services
{
    public class KeycloakService : IIdentityProviderService
    {
        private readonly HttpClient _httpClient;
        private readonly KeycloakSettings _settings;

        public KeycloakService(
            HttpClient httpClient,            
            KeycloakSettings settings)
        {
            _httpClient = httpClient;
            _settings = settings;;
        }

        public async Task<string?> CreateUserAsync(RequestUsers request, CancellationToken cancellationToken)
        {
            var url = $"/admin/realms/{_settings.Realm}/users";

            var userPayload = new
            {
                username = request.email,
                email = request.email,
                enabled = true,
                firstName = request.name,
                emailVerified = true,
                attributes = new
                {
                    phone = new[] { request.phone }
                },
                credentials = new[]
                {
                    new {
                        type = "password",
                        value = request.password,
                        temporary = false
                    }
                }
            };

            var json = JsonSerializer.Serialize(userPayload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    throw new Exception("Usuario já cadastrado com esse e-email");
                }

                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new Exception(error);
            }

            var userId = string.Empty;
            if (response.StatusCode == HttpStatusCode.Created)
            {
                if (response.Headers.TryGetValues("Location", out var values))
                {
                    var location = values.FirstOrDefault();
                    if (!string.IsNullOrEmpty(location))
                    {
                        userId = location.Substring(location.LastIndexOf('/') + 1);
                    }
                }
            }

            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }
            
            await AddGroupUserAsync(userId, cancellationToken);

            return userId;
        }
        public async Task<bool> SendEmailVerifyAsync(string userId, CancellationToken cancellationToken)
        {
           
            var sendEmailUrl = $"{_settings.AuthServerUrl}/admin/realms/{_settings.Realm}/users/{userId}/execute-actions-email";
            var payload = new[] { "VERIFY_EMAIL" };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(sendEmailUrl, content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new Exception(error);
            }

            return true;            
        }
        private async Task<bool> AddGroupUserAsync(string userId, CancellationToken cancellationToken)
        {
            var groupId = await GetGroupNameAsync(_settings.GroupName, cancellationToken);
            if (groupId == null)
                throw new Exception($"Não foi possivel localizar um grupo com o nome {_settings.GroupName}");

            var addGrouplUrl = $"{_settings.AuthServerUrl}/admin/realms/{_settings.Realm}/users/{userId}/groups/{groupId}";
                
            var content = new StringContent("", Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(addGrouplUrl, content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new Exception(error);
            }

            return true;
        }
        private async Task<string?> GetGroupNameAsync(string groupName, CancellationToken cancellationToken)
        {
            var addGrouplUrl = $"{_settings.AuthServerUrl}/admin/realms/{_settings.Realm}/groups?search={groupName}";
            var response = await _httpClient.GetAsync(addGrouplUrl, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new Exception(error);
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var json = await response.Content.ReadAsStringAsync();
            var groups = JsonSerializer.Deserialize<List<KeycloakGroup>>(json, options);
            var group = groups?.FirstOrDefault(g => g.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase));

            if (group is null)
                return null;
           
            return group.Id;

        } 
        public async Task<List<ResultUser>?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var addGrouplUrl = $"{_settings.AuthServerUrl}/admin/realms/{_settings.Realm}/users?search={email}";
            var response = await _httpClient.GetAsync(addGrouplUrl, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new Exception(error);
            }

            var json = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<ResultUser>>(json);

            return users;
        }
        public async Task<bool> DeleteUserAsync(string userId, CancellationToken cancellationToken)
        {
            var deleteUserUrl = $"{_settings.AuthServerUrl}/admin/realms/{_settings.Realm}/users/{userId}";
            var response = await _httpClient.DeleteAsync(deleteUserUrl, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new Exception(error);
            }
            return true;
        }


    }

    public class KeycloakGroup
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}