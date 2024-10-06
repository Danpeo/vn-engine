using Vn.Utils;

namespace Vn.Constants;

public static class Pos
{
    public static float PanelY() => Display.GetHeight() - MathEx.ValueFromPercent(Display.GetHeight(), 22.5f);
}