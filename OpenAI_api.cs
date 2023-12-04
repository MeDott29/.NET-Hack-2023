
using Azure;
using Azure.AI.OpenAI;

string proxyUrl = "https://aoai.hacktogether.net/"; // replace with your proxy URL
string key = "d26adf46-a5b4-4c32-b623-5775821e247f"; // replace with your key

// the full url is appended by /v1/api
Uri fullProxyUrl = new Uri(proxyUrl + "/v1/api");

// the full key is appended by "/YOUR-GITHUB-ALIAS"
AzureKeyCredential token = new AzureKeyCredential(key + "/MeDott29");

// instantiate the client with the "full" values for the url and key/token
OpenAIClient openAIClient = new OpenAIClient(fullProxyUrl, token);

// Define FunctionDefinitions
FunctionDefinition getCurrentTaxRules = new FunctionDefinition("getCurrentTaxRules", new List<FunctionParameter>());
FunctionDefinition calculateTaxSavings = new FunctionDefinition("calculateTaxSavings", new List<FunctionParameter> { new FunctionParameter("income", "float"), new FunctionParameter("deductions", "float") });
FunctionDefinition offerPersonalizedAdvice = new FunctionDefinition("offerPersonalizedAdvice", new List<FunctionParameter> { new FunctionParameter("income", "float"), new FunctionParameter("deductions", "float"), new FunctionParameter("expenses", "float") });

ChatCompletionsOptions completionOptions = new() {
    MaxTokens=2048,
    Temperature=0.7f,
    NucleusSamplingFactor= 0.95f,
    DeploymentName = "gpt-35-turbo",
    FunctionDefinitions = new List<FunctionDefinition> { getCurrentTaxRules, calculateTaxSavings, offerPersonalizedAdvice }
};

completionOptions.Messages.Add(new ChatMessage(ChatRole.System, "you are a helpful tax accountant and want to lower everybody's taxes."));
completionOptions.Messages.Add(new ChatMessage(ChatRole.User, "hi there"));

var response = await openAIClient.GetChatCompletionsAsync(completionOptions);
foreach (var choice in response.Value.Choices)
{
    Console.WriteLine($"AI >>> {choice.Message.Content}");
}
// Handle FunctionCalls in the chat response
foreach (var choice in response.Value.Choices)
{
    if (choice.Message.Role == ChatRole.Function)
    {
        FunctionCall functionCall = choice.Message.FunctionCall;
        switch (functionCall.FunctionName)
        {
            case "getCurrentTaxRules":
                // Call internal logic or external API to get current tax rules
                break;
            case "calculateTaxSavings":
                // Call internal logic or external API to calculate tax savings
                break;
            case "offerPersonalizedAdvice":
                // Call internal logic or external API to offer personalized tax advice
                break;
        }
    }
}

// Serialize the function call result into a new ChatMessage
ChatMessage functionResultMessage = new ChatMessage(ChatRole.Function, JsonConvert.SerializeObject(functionCallResult));
completionOptions.Messages.Add(functionResultMessage);
