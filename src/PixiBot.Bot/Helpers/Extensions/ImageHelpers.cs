namespace PixiEditor.PixiBot.Bot.Helpers.Extensions;

public static class ImageHelpers
{
    public static SKSizeI GetRatioSize(this SKImage image, int newSize)
    {
        if (image.Width < image.Height)
        {
            return image.GetRatioSizeByWidth(newSize);
        }
        else
        {
            return image.GetRatioSizeByHeight(newSize);
        }
    }

    public static SKSizeI GetRatioSizeByWidth(this SKImage image, int newWidth)
    {
        float ratio = 1f * newWidth / image.Width;
        int height = (int)(image.Height * ratio);

        return new(newWidth, height);
    }

    public static SKSizeI GetRatioSizeByHeight(this SKImage image, int newHeight)
    {
        float ratio = 1f * newHeight / image.Height;
        int width = (int)(image.Width * ratio);

        return new(width, newHeight);
    }

    public static SKBitmap Resize(this SKImage image, int newSize, bool disposeOld = false)
    {
        SKSizeI size = image.GetRatioSize(newSize);
        var bitmap = SKBitmap.FromImage(image);

        SKBitmap newBitmap = bitmap.Resize(size, SKFilterQuality.None);

        if (disposeOld)
        {
            image.Dispose();
        }

        return newBitmap;
    }
}
