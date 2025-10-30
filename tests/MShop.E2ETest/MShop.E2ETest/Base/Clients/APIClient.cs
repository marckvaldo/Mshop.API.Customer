﻿using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Json;

namespace MShop.E2ETest.Base.Clients
{
    public class APIClient
    {
        private readonly HttpClient _httpClient;

        public APIClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
