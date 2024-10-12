using Vn.Constants;
using Vn.Story;
using Vn.UI;
using Vn.Utils;
using static Raylib_cs.MouseButton;
using Textures = Vn.UI.Textures;

InitWindow(GameParams.ScreenWidth, GameParams.ScreenHeight, "Visual Novel");
SetConfigFlags(ConfigFlags.Msaa4xHint);
InitAudioDevice();
SetTargetFPS(60);

var naruto = new Character("Нарутыч", new Dictionary<string, string> { ["Naruto"] = "Naruto" }, Color.Orange);

var sasuke = new Character("Сасаке");

var gs = Saves.LoadGame();
var currDialogueInex = gs.CurrentDialogueIndex;

var dialogues = new List<Dialogue>
{
    new(naruto, "fasdfs"),
    new(naruto, "sdaf sdfav"),
    new(naruto, "vgh"),
    new(naruto, "sdffffff"),
    new(naruto, "cvbcvbcvb"),
    new(naruto, "zxczxc"),
    new(naruto, "345"),
    new(naruto, "nruto"),
    new(naruto, "fgddg"),
};

var currentDialogue = dialogues[currDialogueInex];

const int panelPadding = 50;


var dialoguePanel = new DialoguePanel(
    panelPadding,
    Pos.PanelY(),
    Display.GetWidth() - 2 * panelPadding,
    150,
    0.15f,
    16,
    Color.DarkGray,
    DialoguePanelAnimation.Fade
);

var bg = new Background(Paths.Bg("bg1.png"), ImageAnimation.Slide, AnimationSpeed.Normal);
var bg2 = new Background(Paths.Bg("orig.png"), ImageAnimation.Slide, AnimationSpeed.Normal);
var dv = new Sprite(Paths.Sprites("dv pioneer normal.png"), ImageAnimation.Slide, AnimationSpeed.VeryFast,
    PositionOption.Center);

var bgs = new List<Background>
{
    new(Paths.Bg("bg1.png"), ImageAnimation.Slide, AnimationSpeed.Normal),
    new(Paths.Bg("orig.png"), ImageAnimation.Slide, AnimationSpeed.Normal),
};
var currBg = bgs.FirstOrDefault(b => b.Path == gs.CurrentBackgroundPath) ?? bgs.First();
Bg.SetCurrent(currBg);

var circle = new PulseCircle(circleX(), circleY());

while (!WindowShouldClose())
{
    if (IsKeyPressed(KeyboardKey.F))
    {
        Display.ToggleFullscreenWindow(GameParams.ScreenWidth, GameParams.ScreenHeight);
    }

    if (IsKeyPressed(KeyboardKey.R))
    {
        gs.Reset();
        Saves.SaveGame(gs);
        currDialogueInex = 0;
        currentDialogue = dialogues[currDialogueInex];
        currBg = bgs.First();
        Bg.SetCurrent(currBg);
    }

    if (IsMouseButtonPressed(Right))
    {
        dialoguePanel.ToggleVisibility();
    }

    if (IsMouseButtonPressed(Left))
    {
        if (currDialogueInex < dialogues.Count - 1 && currentDialogue.IsFinishedDrawing())
        {
            currentDialogue = dialogues[++currDialogueInex];
            if (currDialogueInex % 2 == 0)
            {
                Bg.SetCurrent(bg);
            }
            else
            {
                Bg.SetCurrent(bg2);
            }

            Saves.SaveGame(new GameState
                { CurrentDialogueIndex = currDialogueInex, CurrentBackgroundPath = Bg.CurrentBackground?.Path });
        }
        else
        {
            currentDialogue.Skip();
            bg.CompleteAnimation();
        }
    }

    dialoguePanel.Update();
    currentDialogue.Update();

    dialoguePanel.Width = Display.GetWidth() - 2 * panelPadding;
    dialoguePanel.Height = Display.GetHeight() / 5;

    BeginDrawing();
    ClearBackground(Color.White);

    Bg.DrawPrev();
    Bg.DrawCurrent();

    if (IsKeyDown(KeyboardKey.D))
    {
        dv.Move(PositionOption.Center);
    }

    dv.Draw();

    dialoguePanel.Draw();
    circle.Update(circleX(), circleY(), circleAlpha());
    circle.Draw();
    dialogues[currDialogueInex].Draw(dialoguePanel, Fonts.Main, Fonts.Main.BaseSize, 2);

    EndDrawing();
}

Fonts.Unload();
CloseAudioDevice();
Textures.UnloadAll();
CloseWindow();
return;

int circleY() => (int)(dialoguePanel.Y + dialoguePanel.Height - dialoguePanel.Height.ValueFromPercent(20));

int circleX() => (int)(dialoguePanel.X + dialoguePanel.Width - dialoguePanel.Width.ValueFromPercent(3));

float circleAlpha() => dialoguePanel.Alpha < 1.0f ? 0.0f : dialogues[currDialogueInex].IsFinishedDrawing() ? 1.0f : 0.0f;
