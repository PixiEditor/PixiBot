namespace PixiEditor.PixiBot.Bot.Helpers.Extensions;

public static class MessageHelpers
{
    public static async Task<IUserMessage> ReplyFileAsync(this IMessage message,
                                                          Stream stream,
                                                          string fileName,
                                                          string text = null,
                                                          MessageComponent components = null) =>
        await message.Channel.SendFileAsync(
            new FileAttachment(stream, fileName),
            text,
            components: components,
            messageReference: new MessageReference(message.Id));
}
