namespace PixiEditor.PixiBot.Bot.Helpers.Extensions;

public static class MessageHelpers
{
    public static async Task<IUserMessage> ReplyFileAsync(this IMessage message,
                                                          Stream stream,
                                                          string fileName,
                                                          string text = null,
                                                          MessageComponent component = null) =>
        await message.Channel.SendFileAsync(
            new FileAttachment(stream, fileName),
            text,
            component: component,
            messageReference: new MessageReference(message.Id));
}
