using SkiaSharp;

namespace Vn.Utils;

public static class Img
{
    public static void GeneratePng(string text, int width, int height, string fileName)
    {
        using var bitmap = new SKBitmap(width, height);
        using (var canvas = new SKCanvas(bitmap))
        {
            canvas.Clear(SKColors.LightCoral);

            using (var paint = new SKPaint())
            {
                paint.Color = SKColors.LightPink;
                paint.Style = SKPaintStyle.Fill;
                canvas.DrawRect(10, 10, width - 20, height - 20, paint);
            }

            using (var paint = new SKPaint())
            {
                paint.Color = SKColors.Black;
                paint.TextSize = 24;
                paint.IsAntialias = true;
                paint.TextAlign = SKTextAlign.Center;

                float textX = width / 2;
                float textY = height / 2 + paint.TextSize / 2;
                canvas.DrawText(text, textX, textY, paint);
            }
        }

        using (var image = SKImage.FromBitmap(bitmap))
        using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
        using (var stream = File.OpenWrite(fileName))
        {
            data.SaveTo(stream);
        }
    }

    public static bool IsJpeg(string path)
    {
        if (!File.Exists(path)) return false;

        try
        {
            using (var stream = File.OpenRead(path))
            {
                using (var codec = SKCodec.Create(stream))
                {
                    if (codec == null) return false;

                    return codec.EncodedFormat == SKEncodedImageFormat.Jpeg;
                }
            }
        }
        catch
        {
            return false;
        }
    }

}