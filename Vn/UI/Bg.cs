using Vn.Story;

namespace Vn.UI;

public static class Bg
{
    private static Background? PreviousBackground = null;
    private static Background? CurrentBackground;
    public static void SetCurrent(Background background)
    {
        PreviousBackground = CurrentBackground;
        background.Reset();
        CurrentBackground = background;
    }

    public static void DrawCurrent() => CurrentBackground?.Draw();
    public static void DrawPrev() => PreviousBackground?.Draw();

    public static Background? Prev() => PreviousBackground;
}