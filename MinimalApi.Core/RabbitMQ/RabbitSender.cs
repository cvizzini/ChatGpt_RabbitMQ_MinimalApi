﻿using Microsoft.Extensions.Logging;
using MinimalApi.Core.Settings;
using RabbitMQ.Client;

namespace MinimalApi.Core.RabbitMQ;

public class RabbitSender
{
    private readonly IModel _channel;
    private readonly ILogger<RabbitSender> _logger;

    private readonly RabbitMqSettings _rabbitSettings;

    public RabbitSender(RabbitMqSettings rabbitSettings, IModel channel, ILogger<RabbitSender> logger)
    {
        _channel = channel;
        _logger = logger;
        _rabbitSettings = rabbitSettings;
    }

    public void PublishMessage<T>(T entity, string key, string queue) where T : class
    {
        var message = JsonSerializer.Serialize(entity);
        var body = Encoding.UTF8.GetBytes(message);
        //declare the queue after mentioning name and a few property related to that
        _channel.QueueDeclare(queue, durable: true, autoDelete: false, exclusive: false);

        _channel.BasicPublish(exchange: _rabbitSettings.ExchangeName,
            routingKey: key,
            basicProperties: null,
            body: body);

        _logger.LogInformation("Sent '{0}':'{1}'", key, message);
    }



    public void PublishFanOutMessage<T>(T entity, string exchange) where T : class
    {
        var message = JsonSerializer.Serialize(entity);
        var body = Encoding.UTF8.GetBytes(message);
        //declare the exchange after mentioning name and a few property related to that
        _channel.ExchangeDeclare(exchange, ExchangeType.Fanout, durable: true, autoDelete: false, null);

        _channel.BasicPublish(exchange: exchange,
            routingKey: string.Empty,
            basicProperties: null,
            body: body);

        _logger.LogInformation("Sent Fan Out'{0}':'{1}'", exchange, message);
    }
}