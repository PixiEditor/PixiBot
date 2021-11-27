using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PixiEditor.PixiBot.Bot.Interfaces;

namespace PixiEditor.PixiBot.Bot.Services;

public class MessageHandlingService
{
    private readonly IServiceProvider _services;
    private readonly ILogger _logger;

    public MessageHandlingService(IServiceProvider services, ILogger<MessageHandlingService> logger)
    {
        _services = services;
        _logger = logger;
    }

    public void Initialize(DiscordSocketClient client) => client.MessageReceived += Client_MessageReceived;

    public async Task InitializeAsync() =>
        await Task.WhenAll(_services.GetServices<IMessageHandler>().Select(x => x.InitializeAsync()));

    private async Task Client_MessageReceived(SocketMessage arg)
    {
        if (arg is not SocketUserMessage message || message.Source == MessageSource.Bot)
        {
            return;
        }

        _logger.LogDebug("Received message by {user} in {channel}, id: {id}", arg.Author.Id, arg.Channel.Id, arg.Id);

        foreach (IMessageHandler handler in _services.GetServices<IMessageHandler>())
        {
            if (await handler.HandleAsync(message))
            {
                _logger.LogDebug("message {id} handled by {handler}", arg.Id, handler.GetType().Name);
                return;
            }

            _logger.LogDebug("message {id} not handled by {handler}", arg.Id, handler.GetType().Name);
        }
    }
}
