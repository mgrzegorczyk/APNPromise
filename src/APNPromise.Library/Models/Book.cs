namespace APNPromise.Library.Models;

public record Book(
    int Id,
    string Title,
    decimal Price,
    int Bookstand,
    int Shelf,
    List<Author> Authors
);