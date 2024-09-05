using Raylib_cs;
using Vn.Constants;
using Vn.Story;
using Vn.UI;
using static Raylib_cs.Raylib;

InitWindow(800, 600, "Visual Novel");
SetWindowState(ConfigFlags.ResizableWindow);
SetTargetFPS(60);

var dialogue2 = new Dialogue("Это2 пример диалога в визуальной новелле.", 0.02f, 0.03f,
    character: new Character("Нарутыч"), textColor: Color.Pink);

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

while (!WindowShouldClose())
{
    float deltaTime = GetFrameTime();

    if (IsMouseButtonPressed(MouseButton.Right))
    {
        dialoguePanel.ToggleVisibility();
    }

    dialoguePanel.Update(deltaTime);
    dialogue2.Update(deltaTime);
    
    //dialoguePanel.X = panelPadding;
    /*
    dialoguePanel.Y = GetScreenHeight() - 200;*/
    dialoguePanel.Width = GetScreenWidth() - 2 * panelPadding;

    BeginDrawing();
    ClearBackground(Color.White);

    dialoguePanel.Draw();
    dialogue2.Draw(dialoguePanel, Fonts.Main, Fonts.Main.BaseSize, 2);

    EndDrawing();
}

Fonts.Unload();

CloseWindow();