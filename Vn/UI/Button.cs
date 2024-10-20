using System.Numerics;
using Vn.Constants;
using Vn.Utils;

namespace Vn.UI;

public class Button
{
    public ButtonType ButtonType { get; set; } = ButtonType.Plain;
    public OutlineStyle OutlineStyle { get; set; } = OutlineStyle.Shadow;
    public Color TitleColor { get; set; } = Color.White;
    public Color OutlineColor { get; set; } = Color.Black;
    public Color ButtonColor { get; set; } = Color.Gray;
    public Color ButtonHighlightColor { get; set; } = Color.Beige; 
    public Color HighlightColor { get; set; } = Color.Gold;
    public int OutlineThickness { get; set; } = 2;
    public Vector2 ShadowOffset { get; set; } = new(6, 6);
    public Font Font { get; set; } = Fonts.ArimoBold();
    public string Title { get; set; }
    private Action _onClick;

    public Button(string title, Action onClick)
    {
        Title = title;
        _onClick = onClick;
    }

    public void Draw(Vector2 position, Vector2 size)
    {
        Color clr;
        Color buttonClr = ButtonColor;
        Vector2 mousePosition = GetMousePosition();

        if (mousePosition.X >= position.X && mousePosition.X <= position.X + size.X &&
            mousePosition.Y >= position.Y && mousePosition.Y <= position.Y + size.Y)
        {
            clr = HighlightColor;
            buttonClr = ButtonHighlightColor;
            
            if (IsMouseButtonPressed(MouseButton.Left))
            {
                _onClick.Invoke();
            }
        }
        else
        {
            clr = TitleColor;
        }

        switch (ButtonType)
        {
            case ButtonType.Outline:
                DrawRectangleRounded(new Rectangle(position, size with{X = size.X + 10, Y = size.Y + 10}), 5, 16, buttonClr);
                break;
            case ButtonType.Plain:
            default:
                    break;
        }
        
        switch (OutlineStyle)
        {
            case OutlineStyle.None:
                DrawTextEx(Font, Title, position, Font.BaseSize, 0, clr);
                break;
            case OutlineStyle.Solid:
                Text.DrawWithOutline(Font, Title, position, Font.BaseSize, 0, clr, OutlineColor,
                    OutlineThickness);
                break;
            case OutlineStyle.Shadow:
                Text.DrawWithShadow(Font, Title, position, Font.BaseSize, 0, clr, OutlineColor, ShadowOffset);
                break;
            default:
                DrawTextEx(Font, Title, position, Font.BaseSize, 0, clr);
                break;
        }
    }
}