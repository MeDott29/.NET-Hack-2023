using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using Azure.AI.OpenAI;
using Azure;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NET_Hack_2023.Hubs
{
    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<string, List<ChatMessage>> _conversations = new ConcurrentDictionary<string, List<ChatMessage>>();
        private readonly OpenAIClient _client;

        public ChatHub(OpenAIClient client)
        {
            _client = client;
        }

        public override async Task OnConnectedAsync()
        {
            _conversations.TryAdd(Context.ConnectionId, new List<ChatMessage>());
            await base.OnConnectedAsync();
        }

        public async Task SendMessage(string message)
        {
            var conversation = _conversations[Context.ConnectionId];
            conversation.Add(new ChatMessage(ChatRole.User, message));

            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                DeploymentName = "gpt-35-turbo-0613",
                Messages = conversation
            };

            Response<ChatCompletions> completionsResponse = await _client.GetChatCompletionsAsync(chatCompletionsOptions);
            string response = completionsResponse.Value.Choices[0].Message.Content;

            conversation.Add(new ChatMessage(ChatRole.System, response));
            await Clients.Caller.SendAsync("ReceiveMessage", response);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _conversations.TryRemove(Context.ConnectionId, out _);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
