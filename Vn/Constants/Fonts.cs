using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Vn.Constants;

public static class Fonts
{
    private static int[] GetCodepoints()
    {
        int[] codepoints = new int[512];

        for (int i = 0; i < 95; i++) codepoints[i] = 32 + i; // ASCII characters
        for (int i = 0; i < 255; i++) codepoints[96 + i] = 0x400 + i; // Cyrillic characters

        return codepoints;
    }

    private static readonly int[] Codepoints = GetCodepoints();
    public static readonly Font Main = LoadFontEx(@"Resources\Fonts\FiraSans-Regular.ttf", 20, Codepoints, 512);
    public static readonly Font Accent = LoadFontEx(@"Resources\Fonts\FiraSans-Bold.ttf", 24, Codepoints, 512);

    public static void Unload()
    {
        UnloadFont(Main);
        UnloadFont(Accent);
    }
}