﻿namespace MinimalApi.Core.Settings;

public class RabbitMqSettings
{
    public string HostName { get; set; }
    public string ExchangeName { get; set; }
    public string ExchangeType { get; set; }
}