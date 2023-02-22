using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MinimalApi.Core.Mediator;

public static class MediatorClientFactory
{
    public static IServiceCollection SetUpMediator(this IServiceCollection services, IConfiguration config)
    {
        services.AddMediator();
        return services;
    }
}