using Newtonsoft.Json;
using Vn.Story;

namespace Vn.Utils;

public static class Saves
{
    private const string SaveFile = "save.json";
    
    public static void SaveGame(GameState state)
    {
        string jsonString = JsonConvert.SerializeObject(state);
        File.WriteAllText(SaveFile, jsonString);
    }

    public static GameState LoadGame()
    {
        if (!File.Exists(SaveFile))
        {
            return new GameState { CurrentDialogueIndex = 0 }; 
        }

        string jsonString = File.ReadAllText(SaveFile);
        return JsonConvert.DeserializeObject<GameState>(jsonString)!;
    }
}