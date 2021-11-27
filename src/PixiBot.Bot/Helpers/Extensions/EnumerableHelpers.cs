namespace PixiEditor.PixiBot.Bot.Helpers.Extensions;

public static class EnumerableHelpers
{
    public static bool TryFirstPixiFile<T>(this IEnumerable<T> attachments, out T attachment) where T : IAttachment =>
        FirstFile(attachments, ".pixi", out attachment);

    public static bool TryFirstImageFile<T>(this IEnumerable<T> attachments, out T attachment) where T : IAttachment =>
        FirstFile(attachments, out attachment, ".png", ".jpeg", ".jpg");

    private static bool FirstFile<T>(IEnumerable<T> attachments, string extension, out T attachment)
        where T : IAttachment
    {
        attachment = attachments.FirstOrDefault(x => Path.GetExtension(x.Filename) == extension);

        return attachment is not null;
    }

    private static bool FirstFile<T>(IEnumerable<T> attachments, out T attachment, params string[] extensions)
        where T : IAttachment
    {
        attachment = attachments.FirstOrDefault(x => extensions.Contains(Path.GetExtension(x.Filename)));

        return attachment is not null;
    }
}
