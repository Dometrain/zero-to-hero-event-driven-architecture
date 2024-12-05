using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace EDA.Producer.Adapters;

public class RabbitMQConnection
{
    public RabbitMQConnection(string hostName, string exchangeName)
    {
        var connectionRetry = 10;

        while (connectionRetry > 0)
        {
            try
            {
                Console.WriteLine($"Attempting to connect to {hostName}");
                var factory = new ConnectionFactory()
                {
                    HostName = hostName,
                };

                Connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();

                var channel = Connection.CreateChannelAsync().GetAwaiter().GetResult();
                
                channel.ExchangeDeclareAsync(exchange: exchangeName, ExchangeType.Topic, durable: true).GetAwaiter().GetResult();
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
}