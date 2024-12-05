using EDA.Consumer.Adapters;

namespace EDA.Consumer;

public static class ServiceExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        var hostName = configuration["Messaging:HostName"];
        var exchangeName = configuration["Messaging:ExchangeName"];
        var deadLetterExchangeName = configuration["Messaging:DeadLetterExchangeName"];

        if (hostName is null)
        {
            throw new EventBusConnectionException("", "Host name is null");
        }
        
        services.AddSingleton(new RabbitMQConnection(hostName!, exchangeName!, deadLetterExchangeName!));
        services.Configure<RabbitMqSettings>(configuration.GetSection("Messaging"));

        return services;
    }
}