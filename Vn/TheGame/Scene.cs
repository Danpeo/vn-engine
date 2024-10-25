namespace Vn.TheGame;

public enum Scene
{
    MainMenu,
    LoadMenu,
    Game
}

public static class Scenes
{
    public static Scene Current { get; private set; } = Scene.MainMenu;
    public static Scene Prev { get; private set; } = Scene.MainMenu;

    public static void Set(Scene layer)
    {
        if (Current == layer) return;

        Prev = Current;
        Current = layer;
    }

    public static void GoBack() => Set(Prev);
}