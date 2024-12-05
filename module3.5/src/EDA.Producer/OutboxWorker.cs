using System.Text.Json;
using EDA.Producer.Adapters;
using EDA.Producer.Core;

namespace EDA.Producer;

public class OutboxWorker(ILogger<OutboxWorker> logger, IServiceScopeFactory serviceScopeFactory, IEventPublisher eventPublisher) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (IServiceScope scope = serviceScopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
                var outboxItems = db.Outbox.Where(item => !item.Processed);
            
                logger.LogInformation("There are {OutboxCount} items in the outbox", outboxItems.Count());

                foreach (var item in outboxItems)
                {
                    switch (item.EventType)
                    {
                        case nameof(OrderCreatedEvent):
                        {
                            var evt = JsonSerializer.Deserialize<OrderCreatedEvent>(item.EventData);
                            await eventPublisher.Publish(evt);
                            item.Processed = true;
                            break;
                        }
                        default:
                        {
                            logger.LogInformation("Unknown event type '{EventType}'", item.EventType);
                            break;
                        }
                    }
                    
                    db.Outbox.Update(item);
                }

                await db.SaveChangesAsync(stoppingToken);
            }
            
            await Task.Delay(5000, stoppingToken);
        }
    }
}