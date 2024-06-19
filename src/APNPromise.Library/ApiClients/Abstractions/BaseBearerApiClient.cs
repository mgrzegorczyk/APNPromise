using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using APNPromise.Library.ApiClients.Extensions;
using Microsoft.Extensions.Logging;

namespace APNPromise.Library.ApiClients.Abstractions
{
    public abstract class BaseBearerApiClient
    {
        protected readonly HttpClient HttpClient;
        private readonly ILogger<BaseBearerApiClient> _logger;

        protected BaseBearerApiClient(HttpClient httpClient, string bearerToken, ILogger<BaseBearerApiClient> logger)
        {
            HttpClient = httpClient;
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", bearerToken);
            _logger = logger;
        }

        protected async Task<T> SendRequestAsync<T>(string url)
        {
            try
            {
                var response = await HttpClient.GetAsync(url);
                await response.HandleErrorResponse(_logger);

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(content);
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is NotSupportedException || ex is JsonException)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        protected async Task<HttpResponseMessage> PostRequestAsync<T>(string url, T data)
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync(url, data);
                await response.HandleErrorResponse(_logger);
                return response;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"Request error: {ex.Message}");
                throw;
            }
        }
    }
}