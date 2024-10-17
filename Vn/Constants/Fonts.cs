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
    public static Font Main(int fontSize = 22) => LoadFontEx(Paths.Fonts("FiraSans-Regular.ttf"), fontSize, Codepoints, 512);
    public static Font Accent(int fontSize = 22) => LoadFontEx(Paths.Fonts("FiraSans-Bold.ttf"), fontSize, Codepoints, 512);
    public static Font ArimoBold(int fontSize = 22) => LoadFontEx(Paths.Fonts("Arimo-Bold.ttf"), fontSize, Codepoints, 512);

    public static void Unload()
    {
        UnloadFont(Main());
        UnloadFont(Accent());
    }
}
