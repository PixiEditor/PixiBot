using PixiEditor.Parser;

namespace PixiEditor.PixiBot.Bot.Helpers.Extensions;

public static class HttpClientHelpers
{
    public static async Task<Document> GetDocumentAsync(this HttpClient client, string url)
    {
        using HttpResponseMessage message = await client.GetAsync(url);
        using Stream stream = await message.Content.ReadAsStreamAsync();

        return PixiParser.Deserialize(stream);
    }

    public static async Task<Document> GetDocumentAsync(this HttpClient client, IAttachment attachment) =>
        await client.GetDocumentAsync(attachment.Url);
}
