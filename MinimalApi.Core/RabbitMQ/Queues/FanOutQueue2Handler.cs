using Microsoft.Extensions.Logging;
using MinimalApi.Core.Model;
using MinimalApi.Core.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MinimalApi.Core.RabbitMQ.Queues;

public class FanOutQueue2Handler : IQueueHandler
{
    private readonly RabbitMqSettings _rabbitSettings;
    private readonly IModel _channel;
    private readonly ILogger<FanOutQueue2Handler> _logger;

    public FanOutQueue2Handler(RabbitMqSettings rabbitSettings, IModel channel, ILogger<FanOutQueue2Handler> logger)
    {
        _rabbitSettings = rabbitSettings;
        _channel = channel;
        _logger = logger;
    }

    public string ApplicableQueue => RabbitMqQueues.FanOutQueue2;

    public async Task Consume()
    {
        await Task.Yield();

        //declare the exchange after mentioning name and a few property related to that
        _channel.ExchangeDeclare("FanOut", ExchangeType.Fanout, durable: true, autoDelete: false, null);

        _channel.QueueDeclare(ApplicableQueue, durable: true, autoDelete: false, exclusive: false);
        _channel.QueueBind(queue: ApplicableQueue, exchange: "FanOut", routingKey: "");

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += Consumer_Received;

        _channel.BasicConsume(queue: ApplicableQueue, consumer: consumer);
    }

    private async Task Consumer_Received(object sender, BasicDeliverEventArgs ea)
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var product = JsonSerializer.Deserialize<ProductDto>(message);
        _logger.LogInformation($"Fan Out 2 Message Received: {product}");
        _channel.BasicAck(ea.DeliveryTag, false);
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _logger.LogInformation("Fan Out Queue Disposed");
    }
}