using System.Text.Json;
using EDA.Producer.Core;
using Microsoft.EntityFrameworkCore;

namespace EDA.Producer.Adapters;

public class PostgresOrders(OrdersDbContext context) : IOrders
{
    public async Task<Order> New(string customerId)
    {
        var transaction = await context.Database.BeginTransactionAsync();
        
        var order = new Order()
        {
            CustomerId = customerId,
            OrderId = customerId == "error" ? $"6{Guid.NewGuid().ToString()}" : Guid.NewGuid().ToString()
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

    public async Task<Order> Complete(string orderId)
    {
        var order = await context.Orders.FirstOrDefaultAsync(order => order.OrderId == orderId);

        if (order == null)
        {
            return null;
        }
        
        await context.Outbox.AddAsync(new OutboxItem()
        {
            EventTime = DateTime.UtcNow,
            Processed = false,
            EventData = JsonSerializer.Serialize(new OrderCompletedEvent() { OrderId = order.OrderId }),
            EventType = nameof(OrderCompletedEvent),
        });

        await context.SaveChangesAsync();

        return order;
    }
}