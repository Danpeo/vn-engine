using System.Numerics;
using Vn.Constants;
using Vn.Story;
using Vn.Utils;

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
    private readonly Button _startButton = null!;
    private readonly Button _exitButton = null!;
    private readonly YesNoModal _modal;

    public MainMenu(Background background, string title, Font font)
    {
        _background = background;
        _title = title;
        _font = font;

        _modal = new YesNoModal("sfd", () =>
        {
            if (UILayers.Current == UILayer.MainMenu)
                Environment.Exit(0);
        }, () => UILayers.Current = UILayer.MainMenu);

        _startButton = new Button("Start", () =>
        {
            if (UILayers.Current == UILayer.MainMenu)
                Console.WriteLine("START");
        })
        {
            Font = Fonts.ArimoBold(55),
        };
        _exitButton = new Button("Exit", () => { _modal.Show(); })
        {
            Font = Fonts.ArimoBold(55),
        };
    }

    public void Draw()
    {
        _background.Draw();

        var textSize = MeasureTextEx(_font, _title, _font.BaseSize, 0);
        var textPos = Text.CenterPosition(textSize, 0, -70);

        switch (OutlineStyle)
        {
            case OutlineStyle.None:
                DrawTextEx(_font, _title, textPos, _font.BaseSize, 0, TitleColor);
                break;
            case OutlineStyle.Solid:
                Text.DrawWithOutline(_font, _title, textPos, _font.BaseSize, 0, TitleColor, OutlineColor,
                    OutlineThickness);
                break;
            case OutlineStyle.Shadow:
                Text.DrawWithShadow(_font, _title, textPos, _font.BaseSize, 0, TitleColor, OutlineColor, ShadowOffset);
                break;
            default:
                DrawTextEx(_font, _title, textPos, _font.BaseSize, 0, TitleColor);
                break;
        }

        var ts = MeasureTextEx(_startButton.Font, _startButton.Title, _startButton.Font.BaseSize, 0);
        var tp = Text.CenterPosition(ts, 0, 0);

        _startButton.Draw(tp, ts);

        var ts2 = MeasureTextEx(_exitButton.Font, _exitButton.Title, _exitButton.Font.BaseSize, 0);
        var tp2 = Text.CenterPosition(ts2, 0, 15);
        _exitButton.Draw(tp2, ts);
        _modal.Draw();
    }
}