using System.Numerics;
using Vn.Story;
using Vn.UI;
using Vn.Utils;
using static Vn.Loclization.Loc;

namespace Vn.TheGame;

public class SaveMenu
{
    private const int SlotWidth = 400;
    private const int SlotHeight = 100;
    private const int SlotsPerPage = 5;
    private GameState currentState;
    private readonly Background _background;
    public OutlineStyle OutlineStyle { get; set; } = OutlineStyle.Shadow;
    public readonly Font _font;

    public SaveMenu(GameState state, Background background, Font font)
    {
        currentState = state;
        _background = background;
        _font = font;
    }

    public void Draw()
    {
        _background.Draw();
        var textSize = MeasureTextEx(_font, L("Загрузить игру"), _font.BaseSize, 0);
        var textPos = Text.CenterPosition(textSize, -90, -90);

        Text.Draw(OutlineStyle, _font, L("Загрузить игру"), textPos, Color.White, Color.Black, 2, new Vector2(6, 6));
        

        for (int i = 0; i < SlotsPerPage; i++)
        {
            int slotX = 100;
            int slotY = 100 + i * (SlotHeight + 20); 

            DrawSaveSlot(i, slotX, slotY);
        }

        if (CheckCollisionPointRec(GetMousePosition(), new Rectangle(10, 500, 100, 40)))
        {
            DrawRectangle(10, 500, 100, 40, Color.LightGray);
            if (IsMouseButtonPressed(MouseButton.Left))
            {
                Scenes.GoBack();
                // Вернуться к предыдущему меню
                // Обработка выхода
            }
        }

        DrawText("Exit", 20, 510, 20, Color.Black);
    }

    public void DrawSaveSlot(int slotIndex, int x, int y)
    {
        DrawRectangle(x, y, SlotWidth, SlotHeight, Color.LightGray);

        string[] saveFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), $"Save_{slotIndex}_*.json");
        if (saveFiles.Length > 0)
        {
            DrawText($"Slot {slotIndex + 1}: Save exists", x + 10, y + 10, 20, Color.Black);

            DrawText("Last saved: " + File.GetCreationTime(saveFiles[0]), x + 10, y + 40, 20,
                Color.DarkBlue);
        }
        else
        {
            DrawText($"Slot {slotIndex + 1}: Empty", x + 10, y + 10, 20, Color.DarkBlue);
        }

        if (CheckCollisionPointRec(GetMousePosition(), new Rectangle(x, y, SlotWidth, SlotHeight)))
        {
            DrawRectangleLines(x, y, SlotWidth, SlotHeight, Color.Red); // Подсветка слота
            if (IsMouseButtonPressed(MouseButton.Left))
            {
                SaveGame(slotIndex);
            }
        }
    }

    public void SaveGame(int slotIndex)
    {
        currentState.SaveCell = slotIndex;
        Saves.SaveGame(currentState);
    }

    public void LoadGame(int slotIndex)
    {
        currentState = Saves.LoadGame(slotIndex);
    }
}