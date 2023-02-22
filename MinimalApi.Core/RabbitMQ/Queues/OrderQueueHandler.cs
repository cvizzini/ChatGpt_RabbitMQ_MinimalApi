using Microsoft.Extensions.Logging;
using MinimalApi.Core.Model;
using MinimalApi.Core.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MinimalApi.Core.RabbitMQ.Queues;

public class OrderQueueHandler : IQueueHandler
{
    private readonly RabbitMqSettings _rabbitSettings;
    private readonly IModel _channel;
    private readonly ILogger<OrderQueueHandler> _logger;

    public OrderQueueHandler(RabbitMqSettings rabbitSettings, IModel channel, ILogger<OrderQueueHandler> logger)
    {
        _rabbitSettings = rabbitSettings;
        _channel = channel;
        _logger = logger;
    }

    public string ApplicableQueue => RabbitMqQueues.OrderQueue;

    public async Task Consume()
    {
        await Task.Yield();
        _channel.ExchangeDeclare(exchange: _rabbitSettings.ExchangeName,
            type: _rabbitSettings.ExchangeType);

        _channel.QueueDeclare(ApplicableQueue, durable: true, autoDelete: false, exclusive: false);

        _channel.QueueBind(queue: ApplicableQueue, exchange: _rabbitSettings.ExchangeName, routingKey: "storeorder");

        var consumerAsync = new AsyncEventingBasicConsumer(_channel);

        consumerAsync.Received += Async_Consumer_Received;

        _channel.BasicConsume(queue: ApplicableQueue, consumer: consumerAsync);
    }

    private async Task Async_Consumer_Received(object sender, BasicDeliverEventArgs @event)
    {
        await Task.Yield();
        var message = Encoding.UTF8.GetString(@event.Body.ToArray());
        var product = JsonSerializer.Deserialize<ProductDto>(message);
        _channel.BasicAck(@event.DeliveryTag, false);
        _logger.LogInformation($"Order message received: {product}");
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _logger.LogInformation("Order Queue Disposed");
    }
}