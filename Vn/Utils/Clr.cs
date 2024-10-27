namespace Vn.Utils;

public static class Clr
{
    public static bool Eq(this Color color1, Color color2) => color1.R == color2.R && color1.G == color2.G && color1.B == color2.B && color1.A == color2.A;
}