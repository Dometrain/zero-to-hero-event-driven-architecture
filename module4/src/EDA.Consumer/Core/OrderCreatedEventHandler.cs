using EDA.Consumer.Core.ExternalEvents;

namespace EDA.Consumer.Core;

public class OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger)
{
    public async Task Handle(OrderCreatedEvent orderCreatedEvent)
    {
        logger.LogInformation($"Order {orderCreatedEvent.OrderId} has been created");

        if (orderCreatedEvent.OrderId.StartsWith("6"))
        {
            throw new Exception("Invalid event");
        }
    }    
}