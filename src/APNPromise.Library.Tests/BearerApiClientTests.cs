using System.Net;
using System.Text.Json;
using APNPromise.Library.ApiClients;
using APNPromise.Library.ApiClients.Abstractions;
using APNPromise.Library.Models;
using APNPromise.Library.Models.Results;
using APNPromise.Library.Tests.Extensions;
using APNPromise.Library.Tests.Generators;
using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;

namespace APNPromise.Library.Tests;

public class BearerApiClientTests
{
    private readonly BearerApiClient _apiClient;
    private readonly MockHttpMessageHandler _mockHttp;
    private const string DummyBearer = "dummy_token";
    private const string BaseAddress = "https://localhost/";

    public BearerApiClientTests()
    {
        _mockHttp = new MockHttpMessageHandler();

        var httpClient = _mockHttp.ToHttpClient();
        httpClient.BaseAddress = new Uri(BaseAddress);

        var logger = Mock.Of<ILogger<BaseBearerApiClient>>();
        _apiClient = new BearerApiClient(httpClient, DummyBearer, logger);
    }

    [Fact]
    public async Task GetBooksAsync_ReturnsBooks()
    {
        // Arrange
        var expectedBooks = BookGenerator.GenerateFakeBooks(20);

        _mockHttp.When("/api/books")
            .Respond(request =>
            {
                request.AssertAuthorizationHeader(DummyBearer);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(expectedBooks))
                };
            });

        // Act
        var books = await _apiClient.GetBooksAsync();

        // Assert
        Assert.NotNull(books);
        Assert.Equal(20, books.Count());
    }

    [Fact]
    public async Task GetOrdersAsync_ReturnsPaginatedOrders()
    {
        // Arrange
        var expectedOrders = new PaginatedResult<Order>
        {
            Items = OrderGenerator.GenerateFakeOrders(80),
            TotalCount = 80,
            PageSize = 10,
            CurrentPage = 1
        };

        _mockHttp.When("/api/orders?pageNumber=1&pageSize=10")
            .Respond(request =>
            {
                request.AssertAuthorizationHeader(DummyBearer);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(expectedOrders))
                };
            });

        // Act
        var paginatedOrders = await _apiClient.GetOrdersAsync(1, 10);

        // Assert
        Assert.NotNull(paginatedOrders);
        Assert.Equal(80, paginatedOrders.Items.Count());
        Assert.Equal(10, paginatedOrders.PageSize);
        Assert.Equal(1, paginatedOrders.CurrentPage);
    }

    [Fact]
    public async Task AddBookAsync_ReturnsSuccess()
    {
        // Arrange
        var book = new Book(1,
            "Test Book",
            19.99m,
            1,
            1,
            Authors: new List<Author>
            {
                new Author("John", "Doe")
            });

        _mockHttp.When(HttpMethod.Post, "/api/books")
            .Respond(request =>
            {
                request.AssertAuthorizationHeader(DummyBearer);
                return new HttpResponseMessage(HttpStatusCode.Created);
            });

        // Act
        var response = await _apiClient.AddBookAsync(book);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task AddBookAsync_ThrowsHttpRequestException_WhenResponseStatusIsNotSuccess()
    {
        // Arrange
        var book = new Book(1,
            "Test Book",
            19.99m,
            1,
            1,
            Authors: new List<Author>
            {
                new Author("John", "Doe")
            });

        _mockHttp.When(HttpMethod.Post, "/api/books")
            .Respond(request =>
            {
                request.AssertAuthorizationHeader(DummyBearer);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            });

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(async () => await _apiClient.AddBookAsync(book));
    }

    [Fact]
    public async Task GetOrdersAsync_ReturnsEmptyList_WhenNoOrdersAvailable()
    {
        // Arrange
        var expectedOrders = new PaginatedResult<Order>
        {
            Items = new List<Order>(),
            TotalCount = 0,
            PageSize = 10,
            CurrentPage = 1
        };

        _mockHttp.When("/api/orders?pageNumber=1&pageSize=10")
            .Respond(request =>
            {
                request.AssertAuthorizationHeader(DummyBearer);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(expectedOrders))
                };
            });

        // Act
        var paginatedOrders = await _apiClient.GetOrdersAsync(1, 10);

        // Assert
        Assert.NotNull(paginatedOrders);
        Assert.Empty(paginatedOrders.Items);
        Assert.Equal(10, paginatedOrders.PageSize);
        Assert.Equal(1, paginatedOrders.CurrentPage);
        Assert.Equal(0, paginatedOrders.TotalCount);
    }
}