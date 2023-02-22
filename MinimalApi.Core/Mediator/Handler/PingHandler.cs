using Mediator;

namespace MinimalApi.Core.Mediator.Handler;
public sealed record Ping(string Id) : IRequest<Pong>;

public sealed record Pong(string Id);

public sealed class PingHandler : IRequestHandler<Ping, Pong>
{
    public ValueTask<Pong> Handle(Ping request, CancellationToken cancellationToken)
    {
        return new ValueTask<Pong>(new Pong(request.Id));
    }
}