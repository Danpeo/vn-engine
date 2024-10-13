using Vn.UI;

namespace Vn.Story;

public abstract record Command
{
    private Command()
    {
    }

    public record Say(Dialogue Dialogue) : Command;

    public record Bg(Background Background) : Command;

    public record DrawSprite(Sprite Sprite) : Command;
    
    public record DontDrawSprite(Sprite Sprite) : Command;

    public record Act(Action Action) : Command;
}

public static class Commands
{ 
    public static void Execute(Command command)
    {
        switch (command)
        {
            case Command.Say(var dialogue):
                Dialogues.Add(dialogue);
                break;
            case Command.Bg(var background):
                Bg.SetCurrent(background);
                break;
            case Command.DrawSprite(var sprite):
                Sprites.AddToDraw(sprite);
                break;
            case Command.DontDrawSprite(var sprite):
                Sprites.RemoveFromDraw(sprite);
                break;
            case Command.Act(var action):
                action();
                break;
        }
    }
}