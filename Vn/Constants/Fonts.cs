namespace Vn.Constants;

public static class Fonts
{
    private static Font MainFont;
    private static Font AccentFont;
    private static Font ArimoBoldFont;
    
    private static int[] GetCodepoints()
    {
        int[] codepoints = new int[512];

        for (int i = 0; i < 95; i++) codepoints[i] = 32 + i; // ASCII characters
        for (int i = 0; i < 255; i++) codepoints[96 + i] = 0x400 + i; // Cyrillic characters

        return codepoints;
    }

    private static readonly int[] Codepoints = GetCodepoints();
    public static Font Main(int fontSize = 22)
    {
        if (MainFont.Texture.Id == 0) // Проверка, загружен ли шрифт
        {
            MainFont = LoadFontEx(Paths.Fonts("FiraSans-Regular.ttf"), fontSize, Codepoints, 512);
        }
        return MainFont;
    }

    public static Font Accent(int fontSize = 22)
    {
        if (AccentFont.Texture.Id == 0)
        {
            AccentFont = LoadFontEx(Paths.Fonts("FiraSans-Bold.ttf"), fontSize, Codepoints, 512);
        }
        return AccentFont;
    }

    public static Font ArimoBold(int fontSize = 22)
    {
        if (ArimoBoldFont.Texture.Id == 0)
        {
            ArimoBoldFont = LoadFontEx(Paths.Fonts("Arimo-Bold.ttf"), fontSize, Codepoints, 512);
        }
        return ArimoBoldFont;
    }

    public static void Unload()
    {
        if (MainFont.Texture.Id != 0) UnloadFont(MainFont);
        if (AccentFont.Texture.Id != 0) UnloadFont(AccentFont);
        if (ArimoBoldFont.Texture.Id != 0) UnloadFont(ArimoBoldFont);
    }
}
