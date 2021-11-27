using Microsoft.Extensions.Hosting;
using PixiEditor.PixiBot.Bot.Services;

namespace PixiEditor.PixiBot.CLI;

internal class PixiBotHostingService : IHostedService
{
    private readonly Bot.PixiBot _bot;
    private readonly MessageHandlingService _handlingService;

    public PixiBotHostingService(Bot.PixiBot bot, MessageHandlingService handlingService)
    {
        _bot = bot;
        _handlingService = handlingService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _bot.StartAsync();
        await _handlingService.InitializeAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken) => await _bot.StopAsync();
}
