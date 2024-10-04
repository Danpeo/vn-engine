using Vn.Story;

namespace Vn.UI;

public static class Bg
{
    private static Background? CurrentBackground;
    public static void SetCurrent(Background background)
    {
        background.ResetAnimation();
        CurrentBackground = background;
    }

    public static void DrawCurrent() => CurrentBackground?.Draw();
}