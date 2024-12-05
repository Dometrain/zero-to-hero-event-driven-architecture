using EDA.Producer.Core;

namespace EDA.Producer.Adapters;

public class PostgresOrders(OrdersDbContext context) : IOrders
{
    public async Task<Order> New(string customerId)
    {
        var order = new Order()
        {
            CustomerId = customerId,
            OrderId = Guid.NewGuid().ToString()
        };

        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        return order;
    }
}