namespace Vn.TheGame;

public enum Scene
{
    MainMenu,
    Game
}

public static class Scenes
{
    public static Scene Current { get; set; } = Scene.MainMenu;
}