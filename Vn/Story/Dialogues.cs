namespace Vn.Story;

public static class Dialogues
{
    public static List<Dialogue> AllDialogues { get; private set; } = new();
    public static Dialogue? CurDialogue { get; private set; }

    public static void Add(Dialogue dialogue)
    {
        AllDialogues.Add(dialogue);
        CurDialogue = dialogue;
    }
}