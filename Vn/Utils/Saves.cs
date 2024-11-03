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
        var saveFiles = SaveFiles(state.SaveCell);

        foreach (var file in saveFiles)
        {
            File.Delete(file);
        }

        string newFileName = $"Save_{state.SaveCell}_{state.SaveTime:yyyy-MM-dd_HH-mm-ss}.json";
        string jsonString = JsonConvert.SerializeObject(state, Formatting.Indented);
        File.WriteAllText(newFileName, jsonString);
    }

    public static GameState LoadGame(int saveCell)
    {
        var saveFiles = SaveFiles(saveCell);

        if (saveFiles.Length == 0)
        {
            return new GameState { CurrentDialogueIndex = 0 };
        }

        string saveFile = saveFiles.OrderByDescending(f => f).First();
        string jsonString = File.ReadAllText(saveFile);
        return JsonConvert.DeserializeObject<GameState>(jsonString)!;
    }

    private static string[] SaveFiles(int saveCell) =>
        Directory.GetFiles(Directory.GetCurrentDirectory(), $"Save_{saveCell}_*.json");
}