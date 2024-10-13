using Raylib_cs;

namespace Vn.Story;

public class Character
{
    public string Name { get; set; }
    public string CurrentDisplayNameKey { get; set; } = "Main";
    public Color Color { get; set; } = Color.LightGray;
    public Dictionary<string, string> AltNames { get; set; }
    public Dictionary<string, Sprite> Sprites { get; set; }
    
    public Character(string name, Dictionary<string, Sprite> sprites, Dictionary<string, string>? altNames = null)
    {
        Name = name;
        AltNames = new Dictionary<string, string>
        {
            ["Main"] = name
        };

        Sprites = sprites;
        
        var names = altNames ?? [];
        foreach (var entry in names)
        {
            AltNames[entry.Key] = entry.Value;
        }
    }
    
    public string CurrentDisplayName() => AltNames[CurrentDisplayNameKey];
}