namespace Vn.Story;

public class GameState
{
    public int SaveCell { get; set; }
    public string FileName { get; set; } = string.Empty;
    public DateTime SaveTime { get; set; }
    public Dialogue? CurrentDialogue { get; set; }
    public Background? CurrentBackground { get; set; }
    public List<Sprite> SpritesOnScene { get; set; } = [];
    public int LastCommandIndex { get; set; }
    public int CurrentDialogueIndex { get; set; }
    public string? CurrentBackgroundPath { get; set; }
    
    public void Reset()
    {
        FileName = string.Empty;  
        CurrentDialogueIndex = 0;
        CurrentBackgroundPath = null;
        SaveTime = DateTime.Now;
    }
}

public static class GS
{
    public static GameState? CurrentState { get; private set; }
    
    public static void Set(GameState state)
    {
        if (CurrentState == state) return;
        CurrentState = state;
    }
}