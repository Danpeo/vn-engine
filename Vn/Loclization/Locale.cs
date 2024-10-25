namespace Vn.Loclization;

public enum Locale
{
    Ru,
    Eng
}

public static class LocaleEx
{
    public static string Txt(this Locale locale)
    {
        return locale switch
        {
            Locale.Ru => "ru",
            Locale.Eng => "eng",
            _ => "eng"
        };
    }
}

