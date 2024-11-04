namespace Vn.Constants;

public static class Fonts
{
    private static Font MainFont;
    private static Font AccentFont;
    private static Font ArimoBoldFont;

    private static int[] GetCodepoints()
    {
        int[] codepoints = new int[1024]; 

        // ASCII characters (32-126)
        for (int i = 0; i < 95; i++) codepoints[i] = 32 + i;

        // Cyrillic characters (0x0400 - 0x045F)
        for (int i = 0; i < 255; i++) codepoints[96 + i] = 0x400 + i;

        int startIndex = 96 + 255;

        for (int i = 0; i < 24; i++) codepoints[startIndex++] = 0x0391 + i; // Greek uppercase
        for (int i = 0; i < 24; i++) codepoints[startIndex++] = 0x03B1 + i; // Greek lowercase

        char[] specialChars =
        [
            '!', '"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/',
            ':', ';', '<', '=', '>', '?', '@', '[', '\\', ']', '^', '_', '`',
            '{', '|', '}', '~', '—'
        ];

        foreach (var ch in specialChars)
        {
            codepoints[startIndex++] = ch;
        }

        return codepoints;
    }
    
    private static readonly int[] Codepoints = GetCodepoints();

    public static Font Main(int fontSize = 22)
    {
        if (MainFont.Texture.Id == 0 || MainFont.BaseSize != fontSize)
        {
            MainFont = LoadFontEx(Paths.Fonts("FiraSans-Regular.ttf"), fontSize, Codepoints, 512);
        }

        return MainFont;
    }

    public static Font Accent(int fontSize = 22)
    {
        if (AccentFont.Texture.Id == 0 || AccentFont.BaseSize != fontSize)
        {
            AccentFont = LoadFontEx(Paths.Fonts("FiraSans-Bold.ttf"), fontSize, Codepoints, 512);
        }

        return AccentFont;
    }

    public static Font ArimoBold(int fontSize = 22)
    {
        if (ArimoBoldFont.Texture.Id == 0 || ArimoBoldFont.BaseSize != fontSize)
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