using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PixiEditor.PixiBot.Bot.Services;

namespace PixiEditor.PixiBot.Bot;

public class PixiBot
{
    private readonly PixiBotConfig _config;
    private readonly DiscordSocketClient _discord;

    private ILogger<PixiBot> BotLog { get; set; }

    private ILogger<DiscordSocketClient> ClientLog { get; set; }

    public PixiBot(DiscordSocketClient discord, IOptions<PixiBotConfig> config, ILoggerFactory loggerFactory, MessageHandlingService messageHandler)
    {
        AddLoggerFactory(loggerFactory);

        _discord = discord;
        discord.Log += Discord_Log;

        messageHandler.Initialize(_discord);

        _config = config.Value;
    }

    public async Task StartAsync()
    {
        BotLog.LogInformation("Starting");

        await _discord.LoginAsync(TokenType.Bot, _config.BotToken);
        await _discord.StartAsync();

        BotLog.LogInformation("Started");
    }

    public async Task StopAsync()
    {
        BotLog.LogInformation("Stopping");
        await _discord.StopAsync();
        await _discord.LogoutAsync();
        BotLog.LogInformation("Stopped");
    }

    public void AddLoggerFactory(ILoggerFactory factory)
    {
        BotLog = factory.CreateLogger<PixiBot>();
        ClientLog = factory.CreateLogger<DiscordSocketClient>();
    }

    private Task Discord_Log(LogMessage message)
    {
        ClientLog.Log(message.GetLogLevel(), message.Exception, "{message}", message.Message);

        return Task.CompletedTask;
    }
}
