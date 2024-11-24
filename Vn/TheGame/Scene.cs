using Vn.UI;

namespace Vn.TheGame;

public enum Scene
{
    MainMenu,
    LoadMenu,
    SaveMenu,
    Game,
    History
}

public static class Scenes
{
    public static Scene Current { get; private set; } = Scene.MainMenu;
    public static Scene Prev { get; private set; } = Scene.MainMenu;

    public static void Set(Scene scene)
    {
        if (Current == scene) return;

        Prev = Current;
        Current = scene;
        SetUiLayer();
    }

    public static void GoBack()
    {
        Set(Prev);
        SetUiLayer();
    }

    private static void SetUiLayer()
    {
        switch (Current)
        {
            case Scene.MainMenu:
                UILayers.Set(UILayer.MainMenu);
                break;
            case Scene.LoadMenu:
                UILayers.Set(UILayer.SaveLoad);
                break;
            case Scene.SaveMenu:
                UILayers.Set(UILayer.SaveLoad);
                break;
            case Scene.Game:
                UILayers.Set(UILayer.Game);
                break;
            default:
                UILayers.Set(UILayer.None);
                break;
        }
    }

    public static void GoBackWithRightMouse(Action? onGoBack = null)
    {
        if (IsMouseButtonPressed(MouseButton.Right))
        {
            onGoBack?.Invoke();
            GoBack();
        }
    }
}