namespace Vn.Story;

public static class Sprites
{
    public static List<Sprite> ToDraw { get; set; } = [];

    public static void AddToDraw(Sprite sprite)
    {
        if (ToDraw.Contains(sprite)) return;
        
        ToDraw.Add(sprite);
    }

    public static void RemoveFromDraw(Sprite sprite) => ToDraw.Remove(sprite);

    public static void DrawSprites()
    {
        foreach (Sprite sprite in ToDraw)
        {
            sprite.Draw();
        }
    }
}