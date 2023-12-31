using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace DirectShared;

public static class RabbitMqExtensions
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services)
    {
        services.ConfigureOptions<RabbitMqOptionSetup>();
        AddServices(services);
        return services;
    }

    public static IServiceCollection AddRabbitMq(this IServiceCollection services, Action<RabbitMqOptions> option)
    {
        services.Configure(option);
        AddServices(services);
        return services;
    }

    private static void AddServices(this IServiceCollection services)
    {
        // add connection
        services.AddSingleton(sp =>
        {
            var options = sp.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
            var factory = new ConnectionFactory()
            {
                HostName = options.Hostname,
                VirtualHost = "/",
                UserName = options.Username,
                Password = options.Password,
                Port = options.Port,
                DispatchConsumersAsync = true,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(5),
                RequestedHeartbeat = TimeSpan.FromSeconds(10),
                RequestedConnectionTimeout = TimeSpan.FromSeconds(60),
            };
            var connection = factory.CreateConnection();
            return connection;
        });
    }

    public static IServiceCollection AddRabbitMqPublisher<TData>(this IServiceCollection services, string exchangeName, string routeKey, string queueName)
        where TData : class
    {
        return services.AddSingleton(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<MessageQueuePublisher<TData>>>();
            var connection = sp.GetRequiredService<IConnection>();

            return new MessageQueuePublisher<TData>(logger, connection, exchangeName, routeKey, queueName);
        });
    }

    public static IServiceCollection AddRabbitMqConsumer<TData>(this IServiceCollection services, string exchangeName, string routeKey, string queueName)
        where TData : class
    {
        return services.AddSingleton(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<MessageQueueConsumer<TData>>>();
            var connection = sp.GetRequiredService<IConnection>();

            return new MessageQueueConsumer<TData>(logger, connection, exchangeName, routeKey, queueName);
        });
    }
}