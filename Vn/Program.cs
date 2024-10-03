using Vn.Constants;
using Vn.Story;
using Vn.UI;
using Vn.Utils;
using static Raylib_cs.MouseButton;
using static Raylib_cs.Raylib;

InitWindow(800, 600, "Visual Novel");
InitAudioDevice();     
SetWindowState(ConfigFlags.ResizableWindow);
SetTargetFPS(60);

var naruto = new Character("Нарутыч", new Dictionary<string, string>{ ["Naruto"] = "Naruto" }, Color.Orange);

var sasuke = new Character("Сасаке");

int currDialogueInex = 0;

var dialogues = new List<Dialogue>
{
    new("САСУКЕ!!!САСУКЕ!!!САСУКЕ!!!САСУКЕ!!!САСУКЕ!!!САСУКЕ!!!САСУКЕ!!!САСУКЕ!!!САСУКЕ!!!САСУКЕ!!!САСУКЕ!!!САСУКЕ!!!САСУКЕ!!!САСУКЕ!!!САСУКЕ!!!САСУКЕ!!!",
        character: naruto, audioType: DialogueAudioType.SoundEffect, soundEffect: LoadSound("Resources/Audio/voiceSfx.mp3")),
    new("НАРУТОООО!!!", character: sasuke),
    new("НЕТ САСУКЕЕЕЕЕЕЕЕ!!!", character: naruto),
    new("НААААААААРУТО ооооооооооо!!!", character: sasuke),
    new("ААААААААААААААААГр!!!", character: naruto),
    new("ЭУУУУУУУУУУУУУУУУУ!!!", character: sasuke),
    new("ВААУУ!!!"),
};

var currentDialogue = dialogues[currDialogueInex];

const int panelPadding = 50;

var dialoguePanel = new DialoguePanel(
    panelPadding,
    GetScreenHeight() - 200,
    GetScreenWidth() - 2 * panelPadding,
    150,
    0.15f,
    16,
    Color.DarkGray,
    DialoguePanelAnimation.Slide
);

var bg = new Background(Paths.Bg("bg1.png"));

while (!WindowShouldClose())
{
    float deltaTime = GetFrameTime();

    if (IsMouseButtonPressed(Right))
    {
        dialoguePanel.ToggleVisibility();
    }
        
    if (IsMouseButtonPressed(Left))
    {
        if (currDialogueInex < dialogues.Count - 1 && currentDialogue.IsFinishedDrawing())
            currentDialogue = dialogues[++currDialogueInex];
        else
            currentDialogue.Skip();
    }

    dialoguePanel.Update(deltaTime);
    currentDialogue.Update(deltaTime);

    dialoguePanel.Width = GetScreenWidth() - 2 * panelPadding;

    BeginDrawing();
    ClearBackground(Color.White);

    bg.Draw();
    dialoguePanel.Draw();
    dialogues[currDialogueInex].Draw(dialoguePanel, Fonts.Main, Fonts.Main.BaseSize, 2);

    EndDrawing();
}

Fonts.Unload();
CloseAudioDevice();    
TextureManager.UnloadAll();
CloseWindow();