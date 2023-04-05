using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinimalApi.Core.CapQueue.Handler;
using Savorboard.CAP.InMemoryMessageQueue;

namespace MinimalApi.Core.CapQueue;

public static class CapQueueFactory
{
    public static IServiceCollection SetUpCapQueue(this IServiceCollection services, IConfiguration config)
    {
        services.AddCap(x =>
            {
                x.UseInMemoryStorage();
                x.UseInMemoryMessageQueue();
                x.UseDashboard(opt => { opt.PathMatch = "/mycap"; });
            });

        services.AddSingleton<CapProductHandler>();
        services.AddSingleton<CapDefaultHandler>();
        return services;
    }
}