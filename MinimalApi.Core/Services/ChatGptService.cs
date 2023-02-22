using Microsoft.Extensions.Logging;
using MinimalApi.Core.Settings;
using OpenAI_API;
using OpenAI_API.Completions;

namespace MinimalApi.Core.Services;

public sealed class ChatGptService
{
    private readonly ChatGptSettings _rabbitSettings;
    private readonly ILogger<ChatGptService> _logger;

    public ChatGptService(ChatGptSettings rabbitSettings, ILogger<ChatGptService> logger)
    {
        _rabbitSettings = rabbitSettings;
        _logger = logger;
    }

    public async Task<string> ExecuteCommand(string prompt)
    {
        var answer = new StringBuilder();
        var openAiApi = new OpenAIAPI(_rabbitSettings.ServiceApiKey);
        var completion = new CompletionRequest
        {
            Prompt = prompt,
            Model = OpenAI_API.Models.Model.DavinciText,
            MaxTokens = 4000
        };
        try
        {
            var result = await openAiApi.Completions.CreateCompletionAsync(completion);
            if (result == null) return answer.ToString();

            foreach (var item in result.Completions)
            {
                answer.AppendLine(item.Text);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return e.Message;
        }

        return answer.ToString();
    }
}