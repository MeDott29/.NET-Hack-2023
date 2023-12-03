using Azure;
using Azure.AI.OpenAI;
using static System.Environment;

string endpoint = GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT");
string key = GetEnvironmentVariable("AZURE_OPENAI_KEY");

if (string.IsNullOrEmpty(endpoint) || string.IsNullOrEmpty(key))
{
    Console.WriteLine("Environment variables for endpoint or key are not set.");
    return;
}

var client = new OpenAIClient(new Uri(endpoint), new AzureKeyCredential(key));

CompletionsOptions completionsOptions = new()
{
    DeploymentName = "gpt-35-turbo", 
    Prompts = { "When was Microsoft founded?" },
};

Response<Completions> completionsResponse = client.GetCompletions(completionsOptions);
string completion = completionsResponse.Value.Choices[0].Text;
Console.WriteLine($"Chatbot: {completion}");