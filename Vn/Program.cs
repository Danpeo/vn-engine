using System.Numerics;
using Vn.Audio;
using Vn.Constants;
using Vn.Story;
using Vn.TheGame;
using Vn.UI;
using Vn.Utils;
using static Raylib_cs.MouseButton;
using static Vn.Loclization.Loc;
using Textures = Vn.UI.Textures;

//
InitWindow(GameParams.ScreenWidth, GameParams.ScreenHeight, "Visual Novel");
SetConfigFlags(ConfigFlags.Msaa4xHint);
InitAudioDevice();
SetTargetFPS(60);
Set(Saves.LoadSettings().Locale);
LoadTranslation(Paths.Loc("loc.json"));
UILayers.Set(UILayer.MainMenu);
var dv = new Sprite(Paths.Sprites("dv pioneer normal.png"), ImageAnimation.Slide, AnimationSpeed.VeryFast,
    PositionOption.Center);
var dv2 = new Sprite(Paths.Sprites("dv pioneer laugh.png"), ImageAnimation.Slide, AnimationSpeed.VeryFast,
    PositionOption.Left);
var dv3 = new Sprite(Paths.Sprites("dv pioneer rage.png"), ImageAnimation.Slide, AnimationSpeed.VeryFast,
    PositionOption.Center);

var thomasSprite = new Sprite(Paths.Sprites("thomas.png"));
var walterSprite = new Sprite(Paths.Sprites("walter.png"));

var thomas = new Character(L("Томас"), new Dictionary<string, Sprite>
{
    { "normal", thomasSprite },
})
{
    Color = Color.Violet
};

var naruto = new Character("Нарутыч", [])
{
    Color = Color.Orange
};

var sasuke = new Character("Сасаке", new Dictionary<string, Sprite>
{
    { "dv", dv },
    { "dv2", dv2 },
    { "dv3", dv3 }
});

/*
var currDialogueInex = gs.CurrentDialogueIndex;
*/

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

/*
var currentDialogue = dialogues[currDialogueInex];
*/

const int panelPadding = 50;


var dialoguePanel = new DialoguePanel(
    panelPadding,
    Pos.PanelY(),
    Display.Width() - 2 * panelPadding,
    150,
    0.1f,
    16,
    new Color(40, 40, 40, 255),
    DialoguePanelAnimation.Fade
);

var bg = new Background(Paths.Bg("bg1.png"), ImageAnimation.Slide, AnimationSpeed.Normal);
var bg2 = new Background(Paths.Bg("orig.png"), ImageAnimation.Slide, AnimationSpeed.Normal);


var bgs = new List<Background>
{
    new Background(Paths.Bg("blizzard.png"))
};
/*var currBg = bgs.FirstOrDefault(b => b.Path == gs.CurrentBackgroundPath) ?? bgs.First();
Bg.SetCurrent(currBg);*/
Bg.SetCurrent(bgs.FirstOrDefault(b => b.Path == GS.CurrentState?.CurrentBackgroundPath) ?? bgs.First());

var circle = new PulseCircle(circleX(), circleY());

Sounds.Load(AllSounds.ButtonClick, Paths.Audio("click.wav"));
Musics.Load(Paths.Audio("cave.ogg"));
Musics.Play();
var commands = new List<Command>
{
    new Command.SayAct(new(sasuke, "В бурю глаза Томаса слезились, а лицо жгло от неумолимого ветра."), [
        new Command.Music(Paths.Audio("forest.ogg"))
    ]),
    new Command.Say(new(null, "Но его беспокойство было не о собственном комфорте — он сжимал свою драгоценную лютню," +
                              " как будто это был единственный способ защитить её от стихии.")),
    new Command.SayAct(new(null, "Trye with some more"), [
        new Command.DrawSprite(thomasSprite)
    ]),

    new Command.Act(() => thomasSprite.Move(PositionOption.AwayToLeft)),
    new Command.DrawSprite(walterSprite),
    new Command.Say(new Dialogue(sasuke, "hahaha im fucking sasuke!!!")),
    new Command.Say(new Dialogue(sasuke, "coooool !!!")),
    new Command.Say(new Dialogue(sasuke, "WHY!!!")),
    new Command.Bg(new Background(Paths.Bg("bg3.png"), ImageAnimation.Slide, AnimationSpeed.Normal))
};

Dialogues.SetCurrent(GS.CurrentState?.CurrentDialogue);
/*Dialogues.SetCurrent(gs.CurrentDialogue ?? dialogues[0]);
Sprites.ToDraw = gs.SpritesOnScene;*/

/*
var currentCommandIndex = gs.LastCommandIndex;
var currenetCommand = commands[currentCommandIndex];
*/

var exitModal = new YesNoModal("Выйти из игры?", () =>
{
    if (UILayers.Current == UILayer.YesNoModal)
        Environment.Exit(0);
}, () => UILayers.Set(UILayer.Game));

var menuBg = new Background(Paths.Bg("tavern.png"));
var mainMenu = new MainMenu(menuBg, "Таверна", Fonts.ElMessiriMedium(120))
{
    TitleColor = new Color(251, 241, 199, 255),
    OutlineStyle = OutlineStyle.Solid
};
var saveMenu = new SaveMenu(new(), menuBg, Fonts.ArimoBold(50));
var panel = new ButtonPanel([
    new("Сохранить", () => Scenes.Set(Scene.SaveMenu))
    {
        Font = Fonts.Main()
    },
    new("Загрузить", () => Scenes.Set(Scene.LoadMenu))
    {
        Font = Fonts.Main()
    },
    new("Настройки", () => Console.WriteLine("Config clicked"))
    {
        Font = Fonts.Main()
    },
    new("Выйти", () => exitModal.Show())
    {
        Font = Fonts.Main(),
    }
]);
while (!WindowShouldClose())
{
    Musics.Update();
    if (IsKeyPressed(KeyboardKey.F))
    {
        Display.ToggleFullscreenWindow(GameParams.ScreenWidth, GameParams.ScreenHeight);
    }

    /*if (IsKeyPressed(KeyboardKey.R))
    {
        gs.Reset();
        Saves.SaveGame(gs);
        currDialogueInex = 0;
        currentDialogue = dialogues[currDialogueInex];
        currBg = bgs.First();
        Bg.SetCurrent(currBg);
    }*/

    if (IsMouseButtonPressed(Right) && UILayers.Current == UILayer.Game)
    {
        dialoguePanel.ToggleVisibility();
    }

    /*if (IsMouseButtonPressed(Left))
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
    }*/


    dialoguePanel.Update();
    /*
    currentDialogue.Update();
    */


    dialoguePanel.Width = Display.Width() - 2 * panelPadding;
    dialoguePanel.Height = Display.Height() / 5;

    BeginDrawing();
    ClearBackground(Color.Black);

    switch (Scenes.Current)
    {
        case Scene.MainMenu:
            mainMenu.Draw();
            break;
        case Scene.Game:
            Bg.DrawPrev();
            Bg.DrawCurrent();

            Sprites.DrawSprites();
            Commands.Execute(commands[Commands.ExecutedCount]);

            if (Dialogues.CurDialogue != null)
            {
                dialoguePanel.Draw();
            }


            Dialogues.CurDialogue?.Update();
            Dialogues.CurDialogue?.Draw(dialoguePanel, Fonts.Main(), Fonts.Main().BaseSize, 2);
            //dialogues[currDialogueInex].Draw(dialoguePanel, Fonts.Main, Fonts.Main.BaseSize, 2);

            if (IsMouseButtonPressed(Left) && UILayers.Current == UILayer.Game && !panel.IsAnyHovering())
            {
                if (Dialogues.CurDialogue != null)
                {
                    if (Dialogues.CurDialogue.IsFinishedDrawing())
                    {
                        Commands.ExecutedCount++;
                    }
                    else
                    {
                        Dialogues.CurDialogue.Skip();
                    }
                }
                else
                {
                    Commands.ExecutedCount++;
                }
            }

            if (GetMouseWheelMoveV().Y > 0)
            {
            }

            panel.Draw();
            circle.Update(circleX(), circleY(), circleAlpha());
            circle.Draw();
            exitModal.Draw();

            break;
        case Scene.LoadMenu:
            saveMenu.Draw(toLoad: true);
            break;
        case Scene.SaveMenu:
            saveMenu.Draw(toLoad: false);
            break;
        default:
            throw new ArgumentOutOfRangeException();
    }

    /*
    Bg.DrawPrev();
    Bg.DrawCurrent();

    if (IsKeyDown(KeyboardKey.D))
    {
        dv.Move(PositionOption.Center);
    }

    dv.Draw();

    Sprites.DrawSprites();
    Commands.Execute(currenetCommand);

    dialoguePanel.Draw();
    circle.Update(circleX(), circleY(), circleAlpha());
    circle.Draw();
    Dialogues.CurDialogue?.Draw(dialoguePanel, Fonts.Main, Fonts.Main.BaseSize, 2);
    //dialogues[currDialogueInex].Draw(dialoguePanel, Fonts.Main, Fonts.Main.BaseSize, 2);

    if (IsKeyPressed(KeyboardKey.Space))
    {
        currenetCommand = commands[++currentCommandIndex];
    }
    */

    EndDrawing();
}

Sounds.Unload();
Musics.Unload();
Fonts.Unload();
CloseAudioDevice();
Textures.UnloadAll();
CloseWindow();
return;

int circleY() => (int)(dialoguePanel.Y + dialoguePanel.Height - dialoguePanel.Height.ValueFromPercent(80));

int circleX() => (int)(dialoguePanel.X + dialoguePanel.Width - dialoguePanel.Width.ValueFromPercent(3));

float circleAlpha() =>
    dialoguePanel.Alpha < 1.0f ? 0.0f :
    Dialogues.CurDialogue != null && Dialogues.CurDialogue.IsFinishedDrawing() ? 1.0f : 0.0f;