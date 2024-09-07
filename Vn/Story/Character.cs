using Raylib_cs;

namespace Vn.Story;

public class Character
{
    public string Name { get; set; }
    public string CurrentDisplayNameKey { get; set; } = "Main";
    public Color Color { get; set; }
    public Dictionary<string, string> AltNames { get; set; }
    
    public Character(string name, Dictionary<string, string>? altNames = null, Color? color = null)
    {
        Name = name;
        Color = color ?? Color.LightGray;
        AltNames = new Dictionary<string, string>
        {
            ["Main"] = name
        };
        
        var names = altNames ?? [];
        foreach (var entry in names)
        {
            AltNames[entry.Key] = entry.Value;
        }
    }
    
    public string CurrentDisplayName() => AltNames[CurrentDisplayNameKey];
}