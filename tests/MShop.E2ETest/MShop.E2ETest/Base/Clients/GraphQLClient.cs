using MShop.E2ETest.Common;
using MShop.Infra.Keycloak.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MShop.E2ETest.Base.Clients
{
    public class GraphQLClient
    {
        protected HttpClient _graphQLCLient;
        protected KeycloakSettings _settingsKeycloak;
        private const string _adminUser = "dev";
        private const string _adminPassword = "123456";
        private const string _client_id = "web-app";

        public GraphQLClient(HttpClient grahQLClient, KeycloakSettings keycloakSettings)
        {
            _graphQLCLient = grahQLClient;
            _settingsKeycloak = keycloakSettings;
            AddAuthorizationHeader();
        }

        private void AddAuthorizationHeader()
        {
            var accessToken = GetAccessTokenAsync(_adminUser, _adminPassword).GetAwaiter().GetResult();
            _graphQLCLient.DefaultRequestHeaders
                .Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer", accessToken);
        }
        public async Task<string> GetAccessTokenAsync(string userName, string password)
        {
            using var client = new HttpClient();

            var url = $"{_settingsKeycloak.AuthServerUrl}/realms/{_settingsKeycloak.Realm}/protocol/openid-connect/token";

            var form = new Dictionary<string, string>
            {
                { "username", userName },
                { "password", password },
                { "grant_type", "password" },
                { "client_id", _client_id },
                { "scope", "openid" }
            };

            using var content = new FormUrlEncodedContent(form);
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            //var json = await response.Content.ReadAsStringAsync();

            var json = await response.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<TokenResponse>(json);
            return token!.access_token;
        }


        public async Task<T> SendQuery<T>(string route ,string query)
        {
            var queryGraphQL = new
            {
                query
            };

            var content = new StringContent(
                    JsonSerializer.Serialize(queryGraphQL), 
                    Encoding.UTF8, "application/json");

            var response = await _graphQLCLient.PostAsync(route, content);

            response.EnsureSuccessStatusCode();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseContent, options);

        }
    }
}
