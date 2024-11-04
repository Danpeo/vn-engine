using System.Numerics;
using Vn.Constants;
using Vn.Story;
using Vn.UI;
using Vn.Utils;
using static Vn.Loclization.Loc;

namespace Vn.TheGame;

public class SaveMenu
{
    private const int SlotWidth = 400;
    private const int SlotHeight = 90;
    private const int SlotsPerPage = 5;
    public GameState CurrentState { get; private set; }
    private readonly Background _background;
    public OutlineStyle OutlineStyle { get; set; } = OutlineStyle.Shadow;
    public readonly Font _font;
    private readonly Button _backButton;

    public SaveMenu(GameState state, Background background, Font font)
    {
        CurrentState = state;
        _background = background;
        _font = font;
        _backButton = new Button(L("Назад"), Scenes.GoBack);
    }

    public void Draw(bool toLoad)
    {
        Scenes.GoBackWithRightMouse();
        _background.Draw();
        var text = toLoad ? L("Загрузить игру") : L("Сохранить игру");
        var textSize = MeasureTextEx(_font, text, _font.BaseSize, 0);
        var textPos = Text.CenterPosition(textSize, -90, -90);

        Text.Draw(OutlineStyle, _font, text, textPos, Color.White, Color.Black, 2, new Vector2(6, 6));
        for (int i = 0; i < SlotsPerPage; i++)
        {
            int slotX = 100;
            int slotY = 110 + i * (SlotHeight + 10);

            DrawSaveSlot(i, slotX, slotY, toLoad);
        }

        var backBtnSize = MeasureTextEx(_backButton.Font, L(_backButton.Title), _backButton.Font.BaseSize, 0);
        _backButton.Draw(new Vector2(textPos.Y, SlotHeight * (SlotsPerPage + 2)), backBtnSize);
    }

    public void DrawSaveSlot(int slotIndex, int x, int y, bool toLoad)
    {
        var slotRectange = new Rectangle(x, y, SlotWidth, SlotHeight);
        DrawRectangleRounded(slotRectange, 0.1f, 10, Color.LightGray);
        string[] saveFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), $"Save_{slotIndex}_*.json");
        var saveNotEmpty = saveFiles.Length > 0;
        if (saveNotEmpty)
        {
            DrawTextEx(Fonts.Accent(), $"{L("Слот")} {slotIndex + 1}",
                new Vector2(x + 10, y + 10),
                Fonts.Accent().BaseSize, 2,
                Color.Black);

            DrawTextEx(Fonts.Main(), $"{L("Время сохранения")}: {File.GetCreationTime(saveFiles[0])}",
                new Vector2(x + 10, y + 40), Fonts.Main().BaseSize, 2, Color.DarkBlue);
        }
        else
        {
            DrawTextEx(Fonts.Main(), $"{L("Слот")} {slotIndex + 1} - {L("Пусто")}",
                new Vector2(x + 10, y + 10), Fonts.Main().BaseSize, 2, Color.DarkBlue);
        }

        if (CheckCollisionPointRec(GetMousePosition(), slotRectange))
        {
            DrawRectangleLinesEx(slotRectange, 2.5f, Color.SkyBlue);
            if (IsMouseButtonPressed(MouseButton.Left))
            {
                if (toLoad && saveNotEmpty)
                {
                    var state = Saves.LoadGame(slotIndex);
                    GS.Set(state);
                    if (state.CurrentDialogue != null) Dialogues.SetCurrent(state.CurrentDialogue);
                    Sprites.ToDraw = state.SpritesOnScene;
                    if (state.CurrentBackground != null) Bg.SetCurrent(state.CurrentBackground);
                    Commands.ExecutedCount = state.LastCommandIndex;
                    Scenes.Set(Scene.Game);
                }
                else
                {
                    SaveGame(slotIndex);
                }
            }
        }
    }

    public void SaveGame(int slotIndex)
    {
        var state = new GameState
        {
            SaveCell = slotIndex,
            SaveTime = DateTime.Now,
            CurrentBackground = Bg.CurrentBackground,
            CurrentDialogue = Dialogues.CurDialogue,
            LastCommandIndex = Commands.ExecutedCount,
            SpritesOnScene = Sprites.ToDraw
        };
        Saves.SaveGame(state);
    }
}