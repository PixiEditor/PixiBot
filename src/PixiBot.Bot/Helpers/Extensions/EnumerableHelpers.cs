namespace PixiEditor.PixiBot.Bot.Helpers.Extensions;

public static class EnumerableHelpers
{
    public static bool TryAllPixiFiles<T>(this IEnumerable<T> attachments, out IEnumerable<T> attachment) where T : IAttachment =>
        AllFiles(attachments, ".pixi", out attachment);

    public static bool TryAllImageFiles<T>(this IEnumerable<T> attachments, out IEnumerable<T> attachment) where T : IAttachment =>
        AllFiles(attachments, out attachment, ".png", ".jpeg", ".jpg");

    private static bool AllFiles<T>(IEnumerable<T> attachments, string extension, out IEnumerable<T> attachment)
        where T : IAttachment
    {
        attachment = attachments.Where(x => Path.GetExtension(x.Filename) == extension);

        return attachment.Any();
    }

    private static bool AllFiles<T>(IEnumerable<T> attachments, out IEnumerable<T> attachment, params string[] extensions)
        where T : IAttachment
    {
        attachment = attachments.Where(x => extensions.Contains(Path.GetExtension(x.Filename)));

        return attachment.Any();
    }
}
