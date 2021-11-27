using PixiEditor.Parser;
using PixiEditor.Parser.Skia;

namespace PixiEditor.PixiBot.Bot.Helpers.Extensions;

public static class DocumentHelpers
{
    public static SKBitmap ToSKBitmap(this SerializableDocument document)
    {
        SKImageInfo info = new(document.Width, document.Height, SKColorType.RgbaF32, SKAlphaType.Unpremul, SKColorSpace.CreateSrgb());
        using SKSurface surface = SKSurface.Create(info);
        SKCanvas canvas = surface.Canvas;
        using SKPaint paint = new();

        foreach (SerializableLayer layer in document)
        {
            bool visible = document.Layers.GetFinalLayerVisibilty(layer);

            if (!visible)
            {
                continue;
            }

            double opacity = document.Layers.GetFinalLayerOpacity(layer);

            if (opacity == 0)
            {
                continue;
            }

            using SKColorFilter filter = SKColorFilter.CreateBlendMode(SKColors.White.WithAlpha((byte)(opacity * 255)), SKBlendMode.DstIn);
            paint.ColorFilter = filter;

            canvas.DrawImage(layer.ToSKImage(), layer.OffsetX, layer.OffsetY, paint);
        }

        SKBitmap bitmap = new(info);

        surface.ReadPixels(info, bitmap.GetPixels(), info.RowBytes, 0, 0);

        return bitmap;
    }
}
