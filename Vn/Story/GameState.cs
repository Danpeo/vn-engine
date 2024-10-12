namespace Vn.Story;

public class GameState
{
    public int CurrentDialogueIndex { get; set; }
    public string? CurrentBackgroundPath { get; set; }
    
    public void Reset()
    {
        CurrentDialogueIndex = 0;
        CurrentBackgroundPath = null;
    }
}