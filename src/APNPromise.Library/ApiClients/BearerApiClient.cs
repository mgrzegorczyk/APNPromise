using APNPromise.Library.ApiClients.Abstractions;
using APNPromise.Library.Models;
using APNPromise.Library.Models.Results;
using Microsoft.Extensions.Logging;

namespace APNPromise.Library.ApiClients
{
    public class BearerApiClient : BaseBearerApiClient
    {
        public BearerApiClient(HttpClient httpClient, string bearerToken, ILogger<BaseBearerApiClient> logger) 
            : base(httpClient, bearerToken, logger)
        {
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await SendRequestAsync<IEnumerable<Book>>("/api/books");
        }

        public async Task<PaginatedResult<Order>> GetOrdersAsync(int pageNumber, int pageSize)
        {
            var url = $"/api/orders?pageNumber={pageNumber}&pageSize={pageSize}";
            return await SendRequestAsync<PaginatedResult<Order>>(url);
        }

        public async Task<HttpResponseMessage> AddBookAsync(Book book)
        {
            return await PostRequestAsync("/api/books", book);
        }
    }
}