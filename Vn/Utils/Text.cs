using System.Numerics;

namespace Vn.Utils;

public static class Text
{
    public static Vector2 CenterPosition(Vector2 textSize, float offsetX = 0, float offsetY = 0)
    {
        var x = Display.Width() - textSize.X;
        var y = Display.Height() - textSize.Y;
        return new Vector2((x + x.ValueFromPercent(offsetX)) / 2, (y + y.ValueFromPercent(offsetY)) / 2);
    }
    
    public static void DrawWithOutline(Font font, string text, Vector2 position, float fontSize, float spacing,
        Color textColor, Color outlineColor, int outlineThickness)
    {
        for (int x = -outlineThickness; x <= outlineThickness; x++)
        {
            for (int y = -outlineThickness; y <= outlineThickness; y++)
            {
                if (x == 0 && y == 0) continue;

                var offsetPosition = new Vector2(position.X + x, position.Y + y);
                DrawTextEx(font, text, offsetPosition, fontSize, spacing, outlineColor);
            }
        }

        DrawTextEx(font, text, position, fontSize, spacing, textColor);
    }

    public static void DrawWithShadow(Font font, string text, Vector2 position, float fontSize, float spacing,
        Color textColor, Color shadowColor, Vector2 shadowOffset)
    {
        var shadowPosition = new Vector2(position.X + shadowOffset.X, position.Y + shadowOffset.Y);
        DrawTextEx(font, text, shadowPosition, fontSize, spacing, shadowColor);
        DrawTextEx(font, text, position, fontSize, spacing, textColor);
    }
}