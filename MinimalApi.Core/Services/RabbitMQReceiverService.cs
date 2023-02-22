using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MinimalApi.Core.RabbitMQ.Queues;

namespace MinimalApi.Core.Services;

public class RabbitMqReceiverService : IHostedService, IDisposable
{
    private int _executionCount;
    private readonly ILogger<RabbitMqReceiverService> _logger;
    private readonly IEnumerable<IQueueHandler> _queueHandlers;


    public RabbitMqReceiverService(ILogger<RabbitMqReceiverService> logger,  IEnumerable<IQueueHandler> queueHandlers)
    {
        _logger = logger;
        _queueHandlers = queueHandlers;
    }

    public async Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("RabbitMq Receiver Hosted Service running.");
        await DoWork(stoppingToken);
    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var count = Interlocked.Increment(ref _executionCount);
            // await DoMediator(stoppingToken, count);
            await DoRabbit(stoppingToken);
            _logger.LogInformation(
                "RabbitMq Receiver Service is working. Count: {Count}", count);

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }

    private async Task DoRabbit(CancellationToken stoppingToken)
    {
        var tasks = _queueHandlers.Select(async queueHandler =>
        {
            await queueHandler.Consume();
        });
        await Task.WhenAll(tasks);
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping.");

        return Task.CompletedTask;
    }

    public void Dispose()
    {

    }
}