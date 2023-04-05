using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinimalApi.Core.RabbitMQ.Queues;
using MinimalApi.Core.Settings;
using RabbitMQ.Client;

namespace MinimalApi.Core.RabbitMQ;

public static class RabbitMqClientFactory
{
    public static IServiceCollection SetUpRabbitMq(this IServiceCollection services, IConfiguration config)
    {
        var configSection = config.GetSection("RabbitMQSettings");
        var settings = new RabbitMqSettings();
        configSection.Bind(settings);
        // add the settings for later use by other classes via injection
        services.AddSingleton(settings);
        // As the connection factory is disposable, need to ensure container disposes of it when finished
        services.AddSingleton<IConnectionFactory>(sp => new ConnectionFactory
        {
            HostName = settings.HostName,
            ClientProvidedName = "MinimalAPI_Net",
            DispatchConsumersAsync = true
        });

        services.AddSingleton<ModelFactory>();
        services.AddSingleton(sp => sp.GetRequiredService<ModelFactory>().CreateChannel());
        services.AddSingleton<IQueueHandler, ProductQueueHandler>();
        services.AddSingleton<IQueueHandler, OrderQueueHandler>();
        services.AddSingleton<IQueueHandler, FanOutQueue1Handler>();
        services.AddSingleton<IQueueHandler, FanOutQueue2Handler>();
        services.AddSingleton<IQueueHandler, FanOutQueue3Handler>();
        services.AddSingleton<RabbitSender>();
        return services;
    }
}