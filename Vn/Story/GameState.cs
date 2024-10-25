namespace Vn.Story;

public class GameState
{
    public int SaveCell { get; set; }
    public string FileName { get; set; } = string.Empty;
    public DateTime SaveTime { get; set; }
    
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