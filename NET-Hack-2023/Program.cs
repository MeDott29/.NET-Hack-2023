using Azure;
using Azure.AI.OpenAI;
using static System.Environment;

string endpoint = GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT");
string key = GetEnvironmentVariable("AZURE_OPENAI_KEY");

if (string.IsNullOrEmpty(endpoint) || string.IsNullOrEmpty(key))
{
    Console.WriteLine("Environment variables for Azure OpenAI endpoint or key are not set.");
    return;
}

try
{
    var client = new OpenAIClient(new Uri(endpoint), new AzureKeyCredential(key));
    Uri imageUri = await GetImageUri(client);
    string completion = await GetCompletion(client);
    Console.WriteLine($"Chatbot: {completion}");
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseWebSockets();
app.UseSignalR(routes =>  // Middleware to support SignalR
{
    routes.MapHub<ChatHub>("/chatHub");
});

app.MapGet("/", async () => 
{
    var client = new OpenAIClient(new Uri(endpoint), new AzureKeyCredential(key));
    Uri imageUri = await GetImageUri(client);
    string completion = await GetCompletion(client);
    return Results.Text($"Chatbot: {completion} <br> Image: <img src='{imageUri}' />", "text/html");
});

app.Run();

// Methods for OpenAI
async Task<Uri> GetImageUri(OpenAIClient client)
{
    Response<ImageGenerations> imageGenerations = await client.GetImageGenerationsAsync(
        new ImageGenerationOptions()
        {
            Prompt = "a happy monkey eating a banana, in watercolor",
            Size = ImageSize.Size256x256,
        });

    // Image Generations responses provide URLs you can use to retrieve requested images
    return imageGenerations.Value.Data[0].Url;
}

async Task<string> GetCompletion(OpenAIClient client)
{
    CompletionsOptions completionsOptions = new()
    {
        DeploymentName = "gpt-35-turbo", 
        Prompts = { "When was Microsoft founded?" },
    };

    Response<Completions> completionsResponse = await client.GetCompletionsAsync(completionsOptions);
    return completionsResponse.Value.Choices[0].Text;
}