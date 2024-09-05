using Raylib_cs;

namespace Vn.Story;

public class Character
{
    public string Name { get; set; }
    public Color Color { get; set; } = Color.LightGray;

    public Character(string name)
    {
        Name = name;
    }

    public Character(string name, Color color) : this(name)
    {
        Color = color;
    }
}