using System.Diagnostics;
using System.Text;
using System.Text.Json;
using EDA.Producer.Core;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace EDA.Producer.Adapters;

public class RabbitMQEventPublisher : IEventPublisher
{
    private readonly RabbitMqSettings _rabbitMqSettings;
    private readonly IConnection _connection;
    private readonly ILogger<RabbitMQEventPublisher> _logger;
    
    public RabbitMQEventPublisher(IOptions<RabbitMqSettings> settings, ILogger<RabbitMQEventPublisher> logger, RabbitMQConnection connection)
    {
        _logger = logger;
        _connection = connection.Connection;
        _rabbitMqSettings = settings.Value;
    }
    
    public async Task Publish(OrderCreatedEvent evt)
    {
        var eventName = "order-created";
        var channel = await _connection.CreateChannelAsync();

        var body = JsonSerializer.SerializeToUtf8Bytes(evt);
        
        this._logger.LogInformation($"Publishing '{eventName}' to '{_rabbitMqSettings.ExchangeName}'");
        
        //put the data on to the product queue
        await channel.BasicPublishAsync(exchange: _rabbitMqSettings.ExchangeName, routingKey: eventName, body: body);
    }
}