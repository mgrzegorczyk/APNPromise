# APNPromise Library

APNPromise is a .NET library designed to interact with a set of RESTful APIs for managing books and orders. The library includes authorization handling via a bearer token and provides methods to interact with the following endpoints:
- `/api/books` (GET)
- `/api/orders` (GET)
- `/api/books` (POST)

## Features

- Fetch a collection of books.
- Handle large datasets with pagination for orders.
- Add new books to the system.
- Authorization via Bearer Token.

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
  - [Initialization](#initialization)
  - [Fetch Books](#fetch-books)
  - [Fetch Orders](#fetch-orders)
  - [Add Book](#add-book)
- [Testing](#testing)

## Installation

To use the APNPromise library in your project, you need to add the library to your .NET project. 

```bash
dotnet add package APNPromise
```

## Usage

### Initialization

First, initialize the `BearerApiClient` with the necessary HTTP client and bearer token.

```csharp
using APNPromise.Lib.ApiClients;
using Microsoft.Extensions.Logging;
using System.Net.Http;

var httpClient = new HttpClient { BaseAddress = new Uri("https://localhost/") };
var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<BearerApiClient>();
var apiClient = new BearerApiClient(httpClient, "your_bearer_token", logger);
```

### Fetch Books

To fetch a list of books, use the `GetBooksAsync` method.

```csharp
var books = await apiClient.GetBooksAsync();
foreach (var book in books)
{
    Console.WriteLine($"Title: {book.Title}, Price: {book.Price}");
}
```

### Fetch Orders

To fetch a list of orders with pagination, use the `GetOrdersAsync` method.

```csharp
int pageNumber = 1;
int pageSize = 10;
var paginatedOrders = await apiClient.GetOrdersAsync(pageNumber, pageSize);

foreach (var order in paginatedOrders.Items)
{
    Console.WriteLine($"Order ID: {order.OrderId}");
}
```

### Add Book

To add a new book, use the `AddBookAsync` method.

```csharp
var newBook = new Book(
    Id: 1,
    Title: "New Book",
    Price: 29.99m,
    Bookstand: 1,
    Shelf: 1,
    Authors: new List<Author> { new Author("John", "Doe") }
);

var response = await apiClient.AddBookAsync(newBook);

if (response.IsSuccessStatusCode)
{
    Console.WriteLine("Book added successfully.");
}
else
{
    Console.WriteLine("Failed to add the book.");
}
```

## Testing

The project includes unit tests using xUnit and MockHttp to test the API client methods. 

### Running Tests

To run the tests, use the following command:

```bash
dotnet test
```
