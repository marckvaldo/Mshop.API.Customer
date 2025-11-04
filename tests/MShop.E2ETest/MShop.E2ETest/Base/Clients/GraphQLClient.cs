using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MShop.E2ETest.Base.Clients
{
    public class GraphQLClient
    {
        protected HttpClient _graphQLCLient;

        public GraphQLClient(HttpClient grahQLClient)
        {
            _graphQLCLient = grahQLClient;
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
