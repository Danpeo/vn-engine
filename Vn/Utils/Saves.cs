using Newtonsoft.Json;
using Vn.Story;
using Vn.TheGame;

namespace Vn.Utils;

public static class Saves
{
    public static void SaveSettings()
    {
        string jsonString = JsonConvert.SerializeObject(GameSettings.I);
        File.WriteAllText("Settings.json", jsonString);
    }

    public static GameSettings LoadSettings()
    {
        if (!File.Exists("Settings.json"))
        {
            return GameSettings.I;
        }

        string jsonString = File.ReadAllText("Settings.json");
        return JsonConvert.DeserializeObject<GameSettings>(jsonString)!;
    }

    public static void SaveGame(GameState state)
    {
        string jsonString = JsonConvert.SerializeObject(state);
        File.WriteAllText($"Save_{state.SaveCell}_{state.SaveTime:yyyy-MM-dd_HH-mm-ss}.json", jsonString);
    }

    public static GameState LoadGame(int saveCell)
    {
        var saveFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), $"Save_{saveCell}_*.json");

        if (saveFiles.Length == 0)
        {
            return new GameState { CurrentDialogueIndex = 0 };
        }

        string saveFile = saveFiles.OrderByDescending(f => f).First();
        string jsonString = File.ReadAllText(saveFile);
        return JsonConvert.DeserializeObject<GameState>(jsonString)!;
    }
}