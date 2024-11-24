namespace Vn.Story;

public static class Dialogues
{
    public static List<Dialogue> AllDialogues { get; private set; } = new();
    public static List<Dialogue> History { get; private set; } = new();
    public static Dialogue? CurDialogue { get; private set; }
    public static Action? OnAddToHistory { get; set; }

    public static void SetCurrent(Dialogue? dialogue)
    {
        if (CurDialogue == dialogue) return;
        CurDialogue = dialogue;
    }
    
    public static void AddToHistory(Dialogue? dialogue)
    {
        if (dialogue != null && !History.Contains(dialogue))
        {
            History.Add(dialogue);
            OnAddToHistory?.Invoke();            
        }
    }

    public static void Add(Dialogue dialogue)
    {
        if (!AllDialogues.Contains(dialogue))
            AllDialogues.Add(dialogue);

        if (CurDialogue != dialogue)
            CurDialogue = dialogue;
    }
}