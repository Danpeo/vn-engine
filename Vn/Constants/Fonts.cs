using System.Numerics;
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
        
        /*
        // Hiragana characters
        for (int i = 0; i < 96; i++) codepoints[351 + i] = 0x3040 + i; // 0x3040 to 0x309F

        // Katakana characters
        for (int i = 0; i < 96; i++) codepoints[447 + i] = 0x30A0 + i; // 0x30A0 to 0x30FF
        */
        
        return codepoints;
    }

    private static readonly int[] Codepoints = GetCodepoints();
    public static readonly Font Main = LoadFontEx(@"Resources\Fonts\FiraSans-Regular.ttf", 20, Codepoints, 512);
    public static readonly Font Accent = LoadFontEx(@"Resources\Fonts\FiraSans-Bold.ttf", 24, Codepoints, 512);
    public static readonly Font ArimoBold = LoadFontEx(Paths.Fonts("Arimo-Bold.ttf"), 55, Codepoints, 512);

    public static void Unload()
    {
        UnloadFont(Main);
        UnloadFont(Accent);
    }
}

public static class AccentFont
{
    public static int Size => Fonts.Accent.BaseSize;

    public static Vector2 MeasureStringSize(string text, int spacing = 0) =>
        MeasureTextEx(Fonts.Accent, text, Fonts.Accent.BaseSize, spacing);
}

public static class ArimoBold
{
    public static int Size => Fonts.Accent.BaseSize;

    public static Vector2 MeasureStringSize(string text, int spacing = 0) =>
        MeasureTextEx(Fonts.ArimoBold, text, Fonts.ArimoBold.BaseSize, spacing);
}