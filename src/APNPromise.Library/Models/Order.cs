namespace APNPromise.Library.Models;

public record Order(
    string OrderId,
    List<OrderLine> OrderLines
);