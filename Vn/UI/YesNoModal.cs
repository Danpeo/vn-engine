using System.Numerics;
using Vn.Utils;

namespace Vn.UI;

public class YesNoModal
{
    public bool IsVisible { get; set; }
    public string YesText { get; set; } = "Да";
    public string NoText { get; set; } = "Нет";
    public Color FontColor { get; set; } = Color.RayWhite;
    public Color BackgroundColor { get; set; } = Color.DarkGray;
    private readonly string _message = string.Empty;
    private Button _yesButton;
    private Button _noButton;
    private readonly Action _onYes;
    private readonly Action? _onNo;

    public YesNoModal(string message, Action onYes, Action? onNo = null)
    {
        _message = message;
        _onYes = onYes;
        _onNo = onNo;

        _yesButton = new Button(YesText, () =>
        {
            if (UILayers.Current == UILayer.YesNoModal)
                _onYes.Invoke();
        });

        _noButton = new Button(NoText, () =>
        {
            if (UILayers.Current == UILayer.YesNoModal)
            {
                _onNo?.Invoke();
                IsVisible = false;
            }
        });
    }

    public void Show()
    {
        IsVisible = true;
        UILayers.Current = UILayer.YesNoModal;
    }

    public void ToggleVisibility() => IsVisible = !IsVisible;

    public void Draw()
    {
        if (!IsVisible) return;

        var size = new Vector2(Display.Width() / 3, Display.Height() / 3);
        var pos = new Vector2(Display.Width() / 2 - size.X / 2, Display.Height() / 2 - size.Y / 2);

        DrawRectangleRounded(new Rectangle(pos, size), 0.05f, 16, BackgroundColor);

        DrawText(_message, (int)pos.X + 20, (int)pos.Y + 20, 20, FontColor);

        Vector2 yesButtonPosition = new Vector2(pos.X + 50, pos.Y + 100);
        Vector2 noButtonPosition = new Vector2(pos.X + 250, pos.Y + 100);
        Vector2 buttonSize = new Vector2(100, 40);

        // Рисуем кнопки
        _yesButton.Draw(yesButtonPosition, buttonSize);
        _noButton.Draw(noButtonPosition, buttonSize);

        // Обрабатываем события кнопок
        _yesButton.Draw(yesButtonPosition, buttonSize);
        _noButton.Draw(noButtonPosition, buttonSize);
    }
}