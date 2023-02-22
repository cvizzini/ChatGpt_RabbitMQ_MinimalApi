using MinimalApi.Core.Settings;
using RabbitMQ.Client;

namespace MinimalApi.Core.RabbitMQ;

public class ModelFactory : IDisposable
{
    private readonly IConnection _connection;
    private readonly RabbitMqSettings _settings;
    public ModelFactory(IConnectionFactory connectionFactory, RabbitMqSettings settings)
    {
        _settings = settings;
        _connection = connectionFactory.CreateConnection();
    }

    public void Dispose()
    {
        _connection.Dispose();
    }

    public IModel CreateChannel()
    {
        var channel = _connection.CreateModel();
        channel.ExchangeDeclare(exchange: _settings.ExchangeName, type: _settings.ExchangeType);
        return channel;
    }

}