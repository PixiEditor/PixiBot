namespace PixiEditor.PixiBot.Bot.Helpers.Extensions;

public static class ImageHelpers
{
    public static SKSizeI GetRatioSize(this SKBitmap image, int newSize)
    {
        if (image.Width > image.Height)
        {
            return image.GetRatioSizeByWidth(newSize);
        }
        else
        {
            return image.GetRatioSizeByHeight(newSize);
        }
    }

    public static SKSizeI GetRatioSizeByWidth(this SKBitmap image, int newSize)
    {
        float ratio = 1f * newSize / image.Width;
        int height = (int)(image.Height * ratio);

        return new(newSize, height);
    }

    public static SKSizeI GetRatioSizeByHeight(this SKBitmap image, int newSize)
    {
        float ratio = 1f * newSize / image.Height;
        int width = (int)(image.Width * ratio);

        return new(width, newSize);
    }

    public static SKBitmap Resize(this SKBitmap bitmap, int newSize, bool disposeOld = false)
    {
        SKSizeI size = bitmap.GetRatioSize(newSize);

        SKBitmap newBitmap = bitmap.Resize(size, SKFilterQuality.None);

        if (disposeOld)
        {
            bitmap.Dispose();
        }

        return newBitmap;
    }
}
