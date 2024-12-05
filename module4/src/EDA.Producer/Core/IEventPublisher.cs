namespace EDA.Producer.Core;

public interface IEventPublisher
{
    Task Publish(OrderCreatedEvent evt);
    Task Publish(OrderCompletedEvent evt);
}