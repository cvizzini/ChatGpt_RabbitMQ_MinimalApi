using Mediator;
using Microsoft.Extensions.Logging;
using MinimalApi.Core.Model;

namespace MinimalApi.Core.Mediator.Handler;

public sealed class OrderHandler : IRequestHandler<OrderDto, bool>
{
    private readonly ILogger<OrderHandler> _logger;

    public OrderHandler(ILogger<OrderHandler> logger)
    {
        _logger = logger;
    }

    public ValueTask<bool> Handle(OrderDto request, CancellationToken cancellationToken)
    {
        _logger.LogInformation( $"Order received: {request}");
        return new ValueTask<bool>(true);
    }
}


