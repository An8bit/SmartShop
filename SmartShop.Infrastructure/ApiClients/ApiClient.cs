using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using SmartShop.Core.Interfaces;

namespace SmartShop.Infrastructure.ApiClients
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory;
        public ApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("ApiClient");
        }
        public Task DeleteAsync(string url)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"API request failed with status {response.StatusCode}: {errorContent}");
            }

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public Task<T> PostAsync<T>(string url, object data)
        {
            throw new NotImplementedException();
        }

        public Task<T> PutAsync<T>(string url, object data)
        {
            throw new NotImplementedException();
        }
    }
}
