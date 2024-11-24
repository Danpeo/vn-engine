using System.Numerics;
using Vn.Constants;
using Vn.Loclization;
using Vn.Story;
using Vn.TheGame;
using Vn.Utils;
using static Vn.Loclization.Loc;

namespace Vn.UI;

public class MainMenu
{
    public OutlineStyle OutlineStyle { get; set; } = OutlineStyle.Shadow;
    public Color TitleColor { get; set; } = Color.White;
    public Color OutlineColor { get; set; } = Color.Black;
    public int OutlineThickness { get; set; } = 2;
    public Vector2 ShadowOffset { get; set; } = new(6, 6);
    private readonly Background _background;
    private readonly string _title;
    public readonly Font _font;
    private readonly Button _startButton;
    private readonly Button _exitButton;
    private readonly Button _loadGameButton;
    private readonly YesNoModal _modal;
    private readonly LanguageSelection _languageSelection = new();

    public MainMenu(Background background, string title, Font font)
    {
        _background = background;
        _title = title;
        _font = font;

        _modal = new YesNoModal("sfd", () => { Environment.Exit(0); }, () => UILayers.Set(UILayer.MainMenu));

        _startButton = new Button("Начать игру", () =>
        {
            if (UILayers.Current == UILayer.MainMenu)
            {
                GS.Set(new GameState());
                Scenes.Set(Scene.Game);
            }
        })
        {
            Font = Fonts.ElMessiriMedium(55),
        };
        _loadGameButton = new Button("Загрузить", () =>
        {
            if (UILayers.Current == UILayer.MainMenu)
                Scenes.Set(Scene.LoadMenu);
        })
        {
            Font = Fonts.ElMessiriMedium(55)
        };
        _exitButton = new Button("Выйти", () => { _modal.Show(); })
        {
            Font = Fonts.ElMessiriMedium(55),
        };
    }

    public void Draw()
    {
        _background.Draw();

        var textSize = MeasureTextEx(_font, L(_title), _font.BaseSize, 0);
        var textPos = Text.CenterPosition(textSize, 0, -70);

        switch (OutlineStyle)
        {
            case OutlineStyle.None:
                DrawTextEx(_font, L(_title), textPos, _font.BaseSize, 0, TitleColor);
                break;
            case OutlineStyle.Solid:
                Text.DrawWithOutline(_font, L(_title), textPos, _font.BaseSize, 0, TitleColor, OutlineColor,
                    OutlineThickness);
                break;
            case OutlineStyle.Shadow:
                Text.DrawWithShadow(_font, L(_title), textPos, _font.BaseSize, 0, TitleColor, OutlineColor, ShadowOffset);
                break;
            default:
                DrawTextEx(_font, L(_title), textPos, _font.BaseSize, 0, TitleColor);
                break;
        }

        var startBtnSize = MeasureTextEx(_startButton.Font, L(_startButton.Title), _startButton.Font.BaseSize, 0);
        var startBtnPos = Text.CenterPosition(startBtnSize, 0, 0);

        _startButton.Draw(startBtnPos, startBtnSize);

        var loadBtnSize = MeasureTextEx(_loadGameButton.Font, L(_loadGameButton.Title), _loadGameButton.Font.BaseSize, 0);
        var loadBtnPos = Text.CenterPosition(loadBtnSize, 0, 15);

        _loadGameButton.Draw(loadBtnPos, loadBtnSize);

        var exitBtnSize = MeasureTextEx(_exitButton.Font, L(_exitButton.Title), _exitButton.Font.BaseSize, 0);
        var exitBtnPos = Text.CenterPosition(exitBtnSize, 0, 30);
        _exitButton.Draw(exitBtnPos, startBtnSize);
        _modal.Draw();

        _languageSelection.Draw();
    }
}