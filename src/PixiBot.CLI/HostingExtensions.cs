using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PixiEditor.PixiBot.Bot;
using PixiEditor.PixiBot.Bot.Interfaces;
using PixiEditor.PixiBot.Bot.Services;

namespace PixiEditor.PixiBot.CLI;

public static class HostingExtensions
{
    public static IHostBuilder ConfigurePixiBot(this IHostBuilder builder) => builder.ConfigureServices(ConfigureServices);

    private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        services
               .Configure<PixiBotConfig>(context.Configuration.GetSection("Bot"))
               .Configure<DiscordSocketConfig>(context.Configuration.GetSection("Discord"))
               .AddSingleton(x => new DiscordSocketClient(x.GetRequiredService<IOptions<DiscordSocketConfig>>().Value))
               .AddSingleton<HttpClient>()
               .AddSingleton<IMessageHandler, AttachmentHandlingService>()
               .AddSingleton<MessageHandlingService>()
               .AddSingleton<Bot.PixiBot>()
               .AddHostedService<PixiBotHostingService>();
    }
}
