using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PixiEditor.Parser;
using PixiEditor.PixiBot.Bot.Interfaces;

namespace PixiEditor.PixiBot.Bot.Services;

public class AttachmentHandlingService : IMessageHandler
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<PixiBotConfig> _config;
    private readonly DiscordSocketClient _client;
    private readonly ILogger _logger;

    public AttachmentHandlingService(HttpClient client, IOptions<PixiBotConfig> config, DiscordSocketClient discord, ILogger<AttachmentHandlingService> logger)
    {
        _httpClient = client;
        _config = config;
        _client = discord;
        _logger = logger;
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task<bool> HandleAsync(IUserMessage message)
    {
        if (!FileAllowed(message.Channel.Id, ".pixi") || !message.Attachments.TryAllPixiFiles(out var files))
            return false;

        _ = Task.Run(async () => await HandleFilesAsync(message, files));

        return true;
    }

    private async Task HandleFilesAsync(IUserMessage message, IEnumerable<IAttachment> files)
    {
        var removeEmoji = await MarkLoadingEmojiAsync(message);

        try
        {
            var tasks = files.Select(async file => (Document: await _httpClient.GetDocumentAsync(file), File: file));
            var results = await Task.WhenAll(tasks);
            
            foreach (var result in results)
            {
                await HandleFileAsync(message, result.Document, result.File);
            }
        }
        finally
        {
            await removeEmoji();
        }
    }

    private async Task HandleFileAsync(IUserMessage message, Document document, IAttachment file)
    {
        _logger.LogDebug("Received .pixi file by {user} in {channel}, message id: {message}", message.Author.Id,
            message.Channel.Id, message.Id);

        try
        {
            _logger.LogDebug(".pixi from msg id: {message} downloaded", message.Id);
            await HandlePixiFile(document, file, message);
        }
        catch (InvalidFileException e)
        {
            await message.ReplyAsync("Your file appears to be corrupted. :(");
            _logger.LogInformation("Invalid file exception\nMessage: {MessageLink}\nException: {Exception}",
                message.GetJumpUrl(), e);
        }
        catch (Exception e)
        {
            await message.ReplyAsync("There was an error handling your image. :(");
            _logger.LogError(
                "Handling file has thrown a exception\nMessage: {MessageLink}\nException: {Exception}",
                message.GetJumpUrl(), e);
        }
    }

    private async Task HandlePixiFile(Document document, IAttachment original, IUserMessage message)
    {
        await using Stream stream = EnsureSize(document);

        _logger.LogDebug(".pixi from msg id: {message} encoded", message.Id);

        var reply = await message.ReplyFileAsync(stream, Path.ChangeExtension(original.Filename, ".png"));

        _logger.LogDebug("encoded .pixi from msg id: {message} send as msg id: {reply}", message.Id, reply.Id);
    }

    private Stream EnsureSize(Document document)
    {
        using var image = SKImage.FromEncodedData(document.PreviewImage);

        if (image.Width < 400 || image.Height < 400)
        {
            using var bitmap = image.Resize(400);
            return bitmap.Encode(SKEncodedImageFormat.Png, 100).AsStream();
        }

        return new MemoryStream(document.PreviewImage);
    }

    private async Task<Func<Task>> MarkLoadingEmojiAsync(IMessage message)
    {
        var emojiText = _config.Value.LoadingEmoji;

        if (emojiText == null)
        {
            return () => Task.CompletedTask;
        }

        Emote emoji = Emote.Parse(_config.Value.LoadingEmoji);
        await message.AddReactionAsync(emoji);

        return () => RemoveReaction();

        async Task RemoveReaction()
        {
            // Make sure there's no rate limiting triggered
            await Task.Delay(1000);
            await message.RemoveReactionAsync(emoji, _client.CurrentUser);
        }
    }

    private bool FileAllowed(ulong channel, string fileExtension)
    {
        PixiBotConfig config = _config.Value;

        if (config.FileChannels == null)
        {
            return true;
        }

        if (!config.FileChannels.ContainsKey(fileExtension))
        {
            _logger.LogDebug("Ignoring message with '{fileExtension}'", fileExtension);
            return false;
        }

        string[] channels = config.FileChannels[fileExtension];

        return channels.Contains(channel.ToString()) || channels.Contains("*");
    }
}
