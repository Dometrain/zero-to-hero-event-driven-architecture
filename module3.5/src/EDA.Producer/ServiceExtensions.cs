using EDA.Producer.Adapters;
using EDA.Producer.Core;
using Microsoft.EntityFrameworkCore;

namespace EDA.Producer;

public static class ServiceExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrdersDbContext>(opt =>
            opt.UseNpgsql(
                configuration.GetConnectionString("OrdersContext"),
                o => o
                    .SetPostgresVersion(17, 0)));

        services.AddScoped<IOrders, PostgresOrders>();

        return services;
    }
    
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        var hostName = configuration["Messaging:HostName"];
        var exchangeName = configuration["Messaging:ExchangeName"];

        if (hostName is null || exchangeName is null)
        {
            throw new EventBusConnectionException("", "Host name is null");
        }
        
        services.AddSingleton(new RabbitMQConnection(hostName!, exchangeName!));
        services.Configure<RabbitMqSettings>(configuration.GetSection("Messaging"));
        services.AddSingleton<IEventPublisher, RabbitMQEventPublisher>();

        return services;
    }
}