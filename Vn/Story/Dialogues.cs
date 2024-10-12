namespace Vn.Story;

public static class Dialogues
{
    public static List<Dialogue> AllDialogues = new();
    public static Dialogue? CurDialogue;
    
    public static void Add(Dialogue dialogue)
    {
        AllDialogues.Add(dialogue);
        CurDialogue = dialogue;
    }
}