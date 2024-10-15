using System.Numerics;

namespace Vn.Utils;

public static class Vec2
{
    public static Vector2 CenterPosition() => new(Display.Width() / 2, Display.Height() / 2);
}