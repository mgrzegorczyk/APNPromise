using APNPromise.Library.ApiClients.Abstractions;
using Microsoft.Extensions.Logging;

namespace APNPromise.Library.ApiClients.Extensions;

public static class HttpResponseMessageExtensions
{
    public static async Task HandleErrorResponse(this HttpResponseMessage response, ILogger<BaseBearerApiClient> logger)
    {
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Unexpected status code: {response.StatusCode}, Content: {errorContent}");
        }
    }
}