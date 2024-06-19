using APNPromise.Library.Models;
using Bogus;

namespace APNPromise.Library.Tests.Generators;

public static class BookGenerator
{
    public static List<Book> GenerateFakeBooks(int count)
    {
        var authorFaker = new Faker<Author>()
            .CustomInstantiator(f => new Author(f.Name.FirstName(), f.Name.LastName()));

        var bookFaker = new Faker<Book>()
            .CustomInstantiator(f => new Book(
                f.IndexFaker,
                f.Lorem.Sentence(3),
                decimal.Parse(f.Commerce.Price(5, 100)),
                f.Random.Number(1, 10),
                f.Random.Number(1, 5),
                authorFaker.Generate(f.Random.Number(1, 3))
            ));

        return bookFaker.Generate(count);
    }
}