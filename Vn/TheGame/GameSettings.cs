using Vn.Loclization;

namespace Vn.TheGame;

public class GameSettings
{
    public Locale Locale { get; set; } = Locale.Eng;

    private static GameSettings? _instance;

    private GameSettings(){}
    public static GameSettings I => _instance ??= new GameSettings();
}