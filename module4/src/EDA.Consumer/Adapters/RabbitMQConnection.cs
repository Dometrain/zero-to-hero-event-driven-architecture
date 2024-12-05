using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace EDA.Consumer.Adapters;

public record ChannelConfig(string ExchangeName, string QueueName, string RoutingKey, string DeadLetterExchangeName, string DeadLetterQueueName,  AsyncEventHandler<BasicDeliverEventArgs> EventHandler);

public record ConfiguredChannel(IChannel Channel, AsyncEventingBasicConsumer Consumer);

public class RabbitMQConnection
{
    public RabbitMQConnection(string hostName, string exchangeName, string deadLetterExchangeName)
    {
        var connectionRetry = 10;

        while (connectionRetry > 0)
        {
            try
            {
                Console.WriteLine($"Attempting to connect to {hostName}");
                var factory = new ConnectionFactory()
                {
                    HostName = hostName
                };

                Connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();

                var channel = Connection.CreateChannelAsync().GetAwaiter().GetResult();
                
                channel.ExchangeDeclareAsync(exchange: exchangeName, ExchangeType.Topic, durable: true).GetAwaiter().GetResult();
                channel.ExchangeDeclareAsync(exchange: deadLetterExchangeName, ExchangeType.Direct, durable: true).GetAwaiter().GetResult();
                break;
            }
            catch (BrokerUnreachableException e)
            {
                Console.WriteLine(e.Message);
                
                connectionRetry--;
                
                Task.Delay(TimeSpan.FromSeconds(2)).GetAwaiter().GetResult();
            }   
        }

        if (Connection is null)
        {
            throw new EventBusConnectionException(hostName, "Unable to connect to RabbitMQ");
        }
    }
    public IConnection Connection { get; init; }

    public async Task<ConfiguredChannel> SetupConsumerFor(ChannelConfig config, CancellationToken stoppingToken)
    {
        var channel = await Connection.CreateChannelAsync(cancellationToken: stoppingToken);
        
        // This is an internal queue to the consumer service
        await channel.QueueDeclareAsync(config.DeadLetterQueueName,
            exclusive: false,
            durable: true,
            autoDelete: false,
            cancellationToken: stoppingToken);
        
        await channel.QueueBindAsync(queue: config.DeadLetterQueueName, exchange: config.DeadLetterExchangeName, routingKey: config.RoutingKey, cancellationToken: stoppingToken);
        
        // This is an internal queue to the consumer service
        await channel.QueueDeclareAsync(config.QueueName,
            exclusive: false,
            durable: true,
            autoDelete: false,
            arguments: new Dictionary<string, object>()
            {
                {"x-dead-letter-exchange", config.DeadLetterExchangeName},
                {"x-dead-letter-routing-key", config.RoutingKey},
                {"x-queue-type", "quorum"}
            }!,
            cancellationToken: stoppingToken);
        
        await channel.QueueBindAsync(queue: config.QueueName, exchange: config.ExchangeName, routingKey: config.RoutingKey, cancellationToken: stoppingToken);
        
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += config.EventHandler;

        return new ConfiguredChannel(channel, consumer);
    }
}