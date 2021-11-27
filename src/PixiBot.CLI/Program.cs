using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PixiEditor.PixiBot.CLI;

HostBuilder builder = new();

builder
    .ConfigureAppConfiguration(config =>
    {
        config.AddJsonFile("appsettings.json", true, true);
        config.AddCommandLine(args);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddOptions();
    })
    .ConfigureLogging((context, logging) =>
    {
        logging.AddConfiguration(context.Configuration.GetSection("Logging"));
        logging.AddConsole();
    })
    .ConfigurePixiBot();

await builder.RunConsoleAsync();