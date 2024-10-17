using System.Numerics;
using Vn.Constants;
using Vn.Utils;

namespace Vn.UI;

public class Button
{
    public OutlineStyle OutlineStyle { get; set; } = OutlineStyle.Shadow;
    public Color TitleColor { get; set; } = Color.White;
    public Color OutlineColor { get; set; } = Color.Black;
    public int OutlineThickness { get; set; } = 2;
    public Vector2 ShadowOffset { get; set; } = new(6, 6);
    public Font Font { get; set; } = Fonts.ArimoBold();
    public string Text { get; set; }
    private Action _onClick;

    public Button(string text, Action onClick)
    {
        Text = text;
        _onClick = onClick;
    }

    public void Draw(Vector2 position)
    {
        switch (OutlineStyle)
        {
            case OutlineStyle.None:
                DrawTextEx(Font, Text, position, Font.BaseSize, 0, TitleColor);
                break;
            case OutlineStyle.Solid:
                Utils.Text.DrawWithOutline(Font, Text, position, Font.BaseSize, 0, TitleColor, OutlineColor,
                    OutlineThickness);
                break;
            case OutlineStyle.Shadow:
                Utils.Text.DrawWithShadow(Font, Text, position, Font.BaseSize, 0, TitleColor, OutlineColor, ShadowOffset);
                break;
            default:
                DrawTextEx(Font, Text, position, Font.BaseSize, 0, TitleColor);
                break;
        }
    }
}