using Vn.Utils;

namespace Vn.Constants;

public static class Pos
{
    public static float PanelY() => Display.Height() - MathEx.ValueFromPercent(Display.Height(), 22.5f);
}