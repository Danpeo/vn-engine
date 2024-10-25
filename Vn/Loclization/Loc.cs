using Newtonsoft.Json;
using static Vn.Loclization.Locale;

namespace Vn.Loclization;

public static class Loc
{
    private static Dictionary<string, Dictionary<string, string>>? _translations;
    public static Locale CurrentLocale { get; private set; } = Eng;

    public static void LoadTranslation(string filePath)
    {
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            _translations = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);
        }
        else
        {
            _translations = new Dictionary<string, Dictionary<string, string>>
            {
                {
                    $"Файл \"{filePath}\" не найден",
                    new Dictionary<string, string>(new Dictionary<string, string>
                    {
                        { Ru.Txt(), $"Файл \"{filePath}\" не найден" },
                        { Eng.Txt(), $"File \"{filePath}\" not found" }
                    })
                },
            };
        }
    }

    public static void Set(Locale locale)
    {
        if (CurrentLocale == locale) return;
        CurrentLocale = locale;
    }

    public static string L(string key)
    {
        if (_translations != null && _translations.TryGetValue(key, out Dictionary<string, string>? entry) &&
            entry.TryGetValue(CurrentLocale.Txt(), out var translatedText))
        {
            return translatedText;
        }

        return key;
    }
}