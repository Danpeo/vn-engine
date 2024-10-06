﻿using System.Numerics;
using Vn.Constants;
using Vn.Story;
using Vn.UI;
using Vn.Utils;
using static Raylib_cs.MouseButton;
using static Raylib_cs.Raylib;
using Textures = Vn.UI.Textures;

InitWindow(GameParams.ScreenWidth, GameParams.ScreenHeight, "Visual Novel");
InitAudioDevice();
SetTargetFPS(60);

var naruto = new Character("Нарутыч", new Dictionary<string, string> { ["Naruto"] = "Naruto" }, Color.Orange);

var sasuke = new Character("Сасаке");

int currDialogueInex = 0;

var dialogues = new List<Dialogue>
{
    new(
        "САСУКЕ!!!САСУКЕ!!!САСУКЕ!!!САСУКЕ!!!САСУКЕ!!!САСУКЕ!!!САСУКЕ!!!САСУКЕ!!!САСУКЕ!!!САСУКЕ!!!САСУКЕ!!!САСУКЕ!!!САСУКЕ!!!САСУКЕ!!!САСУКЕ!!!САСУКЕ!!!",
        character: naruto, audioType: DialogueAudioType.SoundEffect,
        soundEffect: LoadSound("Resources/Audio/voiceSfx.mp3")),
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
    Display.GetHeight() - 200,
    Display.GetWidth() - 2 * panelPadding,
    150,
    0.15f,
    16,
    Color.DarkGray,
    DialoguePanelAnimation.Slide
);

var bg = new Background(Paths.Bg("bg1.png"), ImageAnimation.None, AnimationSpeed.Normal);
var bg2 = new Background(Paths.Bg("orig.png"), ImageAnimation.Fade, AnimationSpeed.Normal);
var dv = new Sprite(Paths.Sprites("dv pioneer normal.png"), ImageAnimation.None, AnimationSpeed.VeryFast,
    PositionOption.FarRight);

Bg.SetCurrent(bg);

while (!WindowShouldClose())
{
    float deltaTime = GetFrameTime();

    if (IsKeyPressed(KeyboardKey.F))
    {
        Display.ToggleFullscreenWindow(GameParams.ScreenWidth, GameParams.ScreenHeight);
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
        }
        else
        {
            currentDialogue.Skip();
            bg.CompleteAnimation();
        }
    }

    dialoguePanel.Update(deltaTime);
    currentDialogue.Update(deltaTime);

    dialoguePanel.Width = GetScreenWidth() - 2 * panelPadding;

    BeginDrawing();
    ClearBackground(Color.White);

    Bg.DrawPrev();
    Bg.DrawCurrent();

    dv.Draw();

    dialoguePanel.Draw();
    dialogues[currDialogueInex].Draw(dialoguePanel, Fonts.Main, Fonts.Main.BaseSize, 2);

    EndDrawing();
}

Fonts.Unload();
CloseAudioDevice();
Textures.UnloadAll();
CloseWindow();
return;
