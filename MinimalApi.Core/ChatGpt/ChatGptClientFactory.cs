using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinimalApi.Core.Services;
using MinimalApi.Core.Settings;

namespace MinimalApi.Core.ChatGpt;

public static class ChatGptClientFactory
{
    public static IServiceCollection SetupChatGpt(this IServiceCollection services, IConfiguration config)
    {
        var configSection = config.GetSection("ChatGPT");
        var settings = new ChatGptSettings();
        configSection.Bind(settings);
        // add the settings for later use by other classes via injection
        services.AddSingleton(settings);
        services.AddScoped<ChatGptService>();

        return services;
    }
}