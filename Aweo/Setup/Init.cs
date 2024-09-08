namespace Aweo.Setup;

public static class Init
{
    internal static Font MainMont {  get; private set; }
    internal static Font AccentFont { get; private set; }
    
    public static void Fonts(Font mainMont, Font accentFont)
    {
        MainMont = mainMont;
        AccentFont = accentFont;
    }
}