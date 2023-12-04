
using System;
using System.Threading.Tasks;

string proxyUrl = "https://aoai.hacktogether.net/"; // replace with your proxy URL
string key = "d26adf46-a5b4-4c32-b623-5775821e247f"; // replace with your key

// Instantiate the bot with the proxy URL and key
EventAssistantBot bot = new EventAssistantBot(proxyUrl, key);

try
{
    // Start the conversation with the bot
    await bot.StartConversation();
}
catch (Exception ex)
{
    // Log the error message
    Console.WriteLine($"Error: {ex.Message}");
}
