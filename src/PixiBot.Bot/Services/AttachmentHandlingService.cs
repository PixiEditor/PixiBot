﻿using Discord.WebSocket;
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
        if (FileAllowed(message.Channel.Id, ".pixi") && message.Attachments.TryFirstPixiFile(out IAttachment file))
        {
            _logger.LogDebug("Received .pixi file by {user} in {channel}, message id: {message}", message.Author.Id, message.Channel.Id, message.Id);

            Func<Task> removeEmoji = await MarkLoadingEmojiAsync(message);

            try
            {
                Document document = await _httpClient.GetDocumentAsync(file);
                _logger.LogDebug(".pixi from msg id: {message} downloaded", message.Id);
                await HandlePixiFile(document, file, message);
            }
            catch (InvalidFileException e)
            {
                await message.ReplyAsync("Your file appears to be corrupted. :(");
                _logger.LogInformation("Invalid file exception\nMessage: {MessageLink}\nException: {Exception}", message.GetJumpUrl(), e);
            }
            catch (Exception e)
            {
                await message.ReplyAsync("There was an error handling your image. :(");
                _logger.LogError("Handling file has thrown a exception\nMessage: {MessageLink}\nException: {Exception}", message.GetJumpUrl(), e);
                return false;
            }
            finally
            {
                await removeEmoji();
            }


            return true;
        }

        return false;
    }

    private async Task HandlePixiFile(Document document, IAttachment original, IUserMessage message)
    {
        using SKBitmap bitmap = SKImage.FromEncodedData(document.PreviewImage).Resize(400, true);
        await using Stream stream = bitmap.Encode(SKEncodedImageFormat.Png, 100).AsStream();

        _logger.LogDebug(".pixi from msg id: {message} encoded", message.Id);

        IUserMessage reply = await message.ReplyFileAsync(stream, Path.ChangeExtension(original.Filename, ".png"));

        _logger.LogDebug("encoded .pixi from msg id: {message} send as msg id: {reply}", message.Id, reply.Id);
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
