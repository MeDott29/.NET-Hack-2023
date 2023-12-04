using Azure;
using Azure.AI.OpenAI;
using System;
using System.Text.Json;
using System.Threading.Tasks;

public class EventAssistantBot
{
    private OpenAIClient openAIClient;
    private AzureKeyCredential token;
    private Uri fullProxyUrl;

    public EventAssistantBot(string proxyUrl, string key)
    {
        fullProxyUrl = new Uri(proxyUrl + "/v1/api");
        token = new AzureKeyCredential(key);
        openAIClient = new OpenAIClient(fullProxyUrl, token);
    }

    private string GetEventSchedule(string arguments)
    {
        // Simulate fetching event schedule data
        return "{\"schedule\": \"Event Schedule Data\"}";
    }

    private string GetSessionDetails(string arguments)
    {
        // Simulate fetching specific session details
        return "{\"sessionDetails\": \"Session Details Data\"}";
    }

    private string GetSessionSummary(string arguments)
    {
        // Simulate fetching a summary of a specific session
        return "{\"sessionSummary\": \"Session Summary Data\"}";
    }

    private async Task ProcessFunctionCalls(ChatCompletions chatCompletions)
    {
        foreach (var choice in chatCompletions.Value.Choices)
        {
            if (choice.Message.FunctionCall != null)
            {
                string functionName = choice.Message.FunctionCall.Name;
                string arguments = choice.Message.FunctionCall.Arguments;

                string functionResult = functionName switch
                {
                    "GetEventSchedule" => GetEventSchedule(arguments),
                    "GetSessionDetails" => GetSessionDetails(arguments),
                    "GetSessionSummary" => GetSessionSummary(arguments),
                    _ => throw new Exception($"Unknown function: {functionName}")
                };

                var functionResponseMessage = new ChatMessage(
                    ChatRole.Function,
                    JsonSerializer.Serialize(functionResult)
                );

                chatCompletions.Value.Messages.Add(functionResponseMessage);
            }
        }
    }

    private void HandleError(Exception ex)
    {
        // Log the error message
        Console.WriteLine($"Error: {ex.Message}");
    }

    public async Task StartConversation()
    {
        try
        {
            ChatCompletionsOptions completionOptions = new() {
                MaxTokens = 2048,
                Temperature = 0.7f,
                NucleusSamplingFactor = 0.95f,
                DeploymentName = "gpt-35-turbo"
            };

            completionOptions.Messages.Add(new ChatMessage(ChatRole.System, "you are an event assistant bot."));
            completionOptions.Messages.Add(new ChatMessage(ChatRole.User, "hi there"));

            var response = await openAIClient.GetChatCompletionsAsync(completionOptions);
            await ProcessFunctionCalls(response);
        }
        catch (Exception ex)
        {
            HandleError(ex);
        }
    }
}
