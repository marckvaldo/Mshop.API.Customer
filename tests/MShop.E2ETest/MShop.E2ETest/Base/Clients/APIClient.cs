using Microsoft.AspNetCore.WebUtilities;
using MShop.E2ETest.Common;
using MShop.Infra.Keycloak.Config;
using System.Text;
using System.Text.Json;

namespace MShop.E2ETest.Base.Clients
{
    public class APIClient
    {
        private readonly HttpClient _httpClient;
        private readonly KeycloakSettings _settingsKeycloak;
        public APIClient(HttpClient httpClient, KeycloakSettings settingsKeycloak)
        {
            _httpClient = httpClient;
            _settingsKeycloak = settingsKeycloak;
            AddAuthorizationHeader();
        }


        private void AddAuthorizationHeader()
        {
            var accessToken = GetAccessTokenAsync(Configuration.USER_CUSTOMER_AUTH, Configuration.PASSWORD_CUSTOMER_AUTH).GetAwaiter().GetResult();
            _httpClient.DefaultRequestHeaders
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
                { "client_id", Configuration.CLIENT_ID_AUTH },
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

        public async Task<(HttpResponseMessage?, TOutPut?)> Post<TOutPut>(string route, object payload) where TOutPut : class
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var response = await _httpClient.PostAsync(
                route,
                new StringContent(
                    JsonSerializer.Serialize(payload, options),
                    Encoding.UTF8,
                    "application/json")
                );

            var outputString = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(outputString)) 
            { 
                return (response, null);
            }

            var outPut = JsonSerializer.Deserialize<TOutPut>(outputString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return (response, outPut);
        }

        public async Task<(HttpResponseMessage?, TOutPut?)> Put<TOutPut>(string route, object payload) where TOutPut : class
        {
            var response = await _httpClient.PutAsync(
                route,
                new StringContent(
                    JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"
                ));

            var outputString = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(outputString))
            {
                return (response, null);
            }

            var outPut = JsonSerializer.Deserialize<TOutPut>(
                outputString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

            return (response, outPut);
        }

        public async Task<(HttpResponseMessage?, TOutPut?)> Delete<TOutPut>(string route) where TOutPut : class
        {
            var response = await _httpClient.DeleteAsync(route);

            var outputString = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(outputString))
            {
                return (response, null);
            }

            var output = JsonSerializer.Deserialize<TOutPut>
            (
                outputString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
            return (response, output);
        }

        public async Task<(HttpResponseMessage?, TOutPut?)> Get<TOutPut>(string route, object? queryStringParameters = null) where TOutPut : class
        {

            var url = PrepareParameteGetRote(route, queryStringParameters);
            var response = await _httpClient.GetAsync(url);
            var outPutString = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(outPutString))
            {
                return (response, null);
            }

            var outPut = JsonSerializer.Deserialize<TOutPut>(
                outPutString,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return (response, outPut);

        }
        
        private string PrepareParameteGetRote(string route, object? queryStringParameters)
        {
            if (queryStringParameters is null)
                return route;

            var parametersJson = JsonSerializer.Serialize(queryStringParameters);
            var parametersDictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(parametersJson);
            return QueryHelpers.AddQueryString(route, parametersDictionary!);

        }

    }
}
