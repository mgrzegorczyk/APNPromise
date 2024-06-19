using APNPromise.Library.Models;
using Bogus;

namespace APNPromise.Library.Tests.Generators;

public static class OrderGenerator
{
    public static List<Order> GenerateFakeOrders(int count)
    {
        var orderLineFaker = new Faker<OrderLine>()
            .CustomInstantiator(f => new OrderLine(f.Random.Number(1, 20), f.Random.Number(1, 5)));

        var orderFaker = new Faker<Order>()
            .CustomInstantiator(f => new Order(
                f.Random.Guid().ToString(),
                orderLineFaker.Generate(f.Random.Number(1, 5))
            ));

        return orderFaker.Generate(count);
    }
}