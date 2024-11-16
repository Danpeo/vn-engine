namespace Vn.Story;

public static class Sprites
{
    private static Sprite? _current;
    public static List<Sprite> ToDraw { get; set; } = [];

    public static Sprite? Current
    {
        get => _current;
        set
        {
            if (_current == value) return;
            Prev = _current;
            _current = value;
        }
    }

    public static Sprite? Prev { get; set; }

    public static void AddToDraw(Sprite sprite)
    {
        if (ToDraw.Contains(sprite)) return;
        Current = sprite;
        ToDraw.Add(sprite);
    }

    public static void PrevMoveAway()
    {
        Prev?.Move(PositionOption.AwayToLeft);
    }

    public static void RemoveFromDraw(Sprite sprite)
    {
        ToDraw.Remove(sprite);
    }

    public static void DrawSprites()
    {
        foreach (Sprite sprite in ToDraw)
        {
            sprite.Draw();
        }
    }
}