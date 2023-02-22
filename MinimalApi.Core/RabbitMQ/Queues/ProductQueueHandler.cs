using Microsoft.Extensions.Logging;
using MinimalApi.Core.Model;
using MinimalApi.Core.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MinimalApi.Core.RabbitMQ.Queues;

public class ProductQueueHandler : IQueueHandler
{
    private readonly RabbitMqSettings _rabbitSettings;
    private readonly IModel _channel;
    private readonly ILogger<ProductQueueHandler> _logger;

    public ProductQueueHandler(RabbitMqSettings rabbitSettings, IModel channel, ILogger<ProductQueueHandler> logger)
    {
        _rabbitSettings = rabbitSettings;
        _channel = channel;
        _logger = logger;
    }

    public string ApplicableQueue => RabbitMqQueues.ProductQueue;

    public async Task Consume()
    {
        await Task.Yield();
        _channel.ExchangeDeclare(exchange: _rabbitSettings.ExchangeName,
            type: _rabbitSettings.ExchangeType);

        _channel.QueueDeclare(ApplicableQueue, durable: true, autoDelete: false, exclusive: false);

        _channel.QueueBind(queue: ApplicableQueue, exchange: _rabbitSettings.ExchangeName, routingKey: "storeproduct");

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += Consumer_Received;

        _channel.BasicConsume(queue: ApplicableQueue, consumer: consumer);
    }

    private async Task Consumer_Received(object sender, BasicDeliverEventArgs ea)
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var product = JsonSerializer.Deserialize<ProductDto>(message);
        _logger.LogInformation($"Product message received: {product}");
        _channel.BasicAck(ea.DeliveryTag, false);
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _logger.LogInformation("Product Queue Disposed");
    }
}