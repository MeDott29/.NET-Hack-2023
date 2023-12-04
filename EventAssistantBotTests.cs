using NUnit.Framework;
using Moq;
using Azure.AI.OpenAI;
using System;

public class EventAssistantBotTests
{
    private EventAssistantBot bot;
    private Mock<OpenAIClient> mockClient;

    [SetUp]
    public void Setup()
    {
        mockClient = new Mock<OpenAIClient>();
        bot = new EventAssistantBot(mockClient.Object);
    }

    [Test]
    public void Constructor_InitializesCorrectly()
    {
        Assert.IsNotNull(bot);
    }

    [Test]
    public void StartConversation_CallsCorrectMethods()
    {
        bot.StartConversation();

        mockClient.Verify(m => m.GetChatCompletionsAsync(It.IsAny<ChatCompletionsOptions>()), Times.Once);
    }

    [Test]
    public void GetEventSchedule_ReturnsCorrectResult()
    {
        string result = bot.GetEventSchedule("{}");

        Assert.AreEqual("{\"schedule\": \"Event Schedule Data\"}", result);
    }

    [Test]
    public void GetSessionDetails_ReturnsCorrectResult()
    {
        string result = bot.GetSessionDetails("{}");

        Assert.AreEqual("{\"sessionDetails\": \"Session Details Data\"}", result);
    }

    [Test]
    public void GetSessionSummary_ReturnsCorrectResult()
    {
        string result = bot.GetSessionSummary("{}");

        Assert.AreEqual("{\"sessionSummary\": \"Session Summary Data\"}", result);
    }

    [Test]
    public void HandleError_LogsError()
    {
        bot.HandleError(new Exception("Test error"));

        // Check that the error was logged
    }
}
