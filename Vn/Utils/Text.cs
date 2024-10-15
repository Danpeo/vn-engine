using System.Numerics;

namespace Vn.Utils;

public class Text
{
    public static Vector2 CenterPosition(Vector2 textSize) =>
        new((GetScreenWidth() - textSize.X) / 2, (GetScreenHeight() - textSize.Y) / 2);
}