namespace Vn.Story;

public static class Sprites
{
    public static readonly List<Sprite> ToDraw = [];

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