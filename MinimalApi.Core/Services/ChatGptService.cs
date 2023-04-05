using Microsoft.Extensions.Logging;
using MinimalApi.Core.Model;
using MinimalApi.Core.Settings;
using OpenAI_API;
using OpenAI_API.Completions;
using OpenAI_API.Images;

namespace MinimalApi.Core.Services;

public sealed class ChatGptService
{
    private readonly ChatGptSettings _openAiSettings;
    private readonly ILogger<ChatGptService> _logger;

    public ChatGptService(ChatGptSettings openAiSettings, ILogger<ChatGptService> logger)
    {
        _openAiSettings = openAiSettings;
        _logger = logger;
    }

    public async Task<string> ExecuteCommand(string prompt)
    {
        var answer = new StringBuilder();
        var openAiApi = new OpenAIAPI(_openAiSettings.ServiceApiKey);
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

    public async Task<ImageResult> ExecuteDallECommand(DallEInput input)
    {
        try
        {
            var openAiApi = new OpenAIAPI(_openAiSettings.ServiceApiKey);
            // for example
            var result = await openAiApi.ImageGenerations.CreateImageAsync(new ImageGenerationRequest(input.Prompt, input.N, ImageSize._256));
            return result;

        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
        return null;
    }
}