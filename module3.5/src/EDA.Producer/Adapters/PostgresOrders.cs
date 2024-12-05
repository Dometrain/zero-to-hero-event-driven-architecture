using System.Text.Json;
using EDA.Producer.Core;

namespace EDA.Producer.Adapters;

public class PostgresOrders(OrdersDbContext context) : IOrders
{
    public async Task<Order> New(string customerId)
    {
        var transaction = await context.Database.BeginTransactionAsync();
        
        var order = new Order()
        {
            CustomerId = customerId,
            OrderId = Guid.NewGuid().ToString()
        };

        await context.Orders.AddAsync(order);
        await context.Outbox.AddAsync(new OutboxItem()
        {
            EventTime = DateTime.UtcNow,
            Processed = false,
            EventData = JsonSerializer.Serialize(new OrderCreatedEvent() { OrderId = order.OrderId }),
            EventType = nameof(OrderCreatedEvent),
        });

        await context.SaveChangesAsync();
        await transaction.CommitAsync();

        return order;
    }
}