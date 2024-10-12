using Raylib_cs;
using Vn.Constants;
using Vn.Utils;
using static Raylib_cs.Raylib;
using static Vn.UI.DialoguePanelAnimation;

namespace Vn.UI;

public class DialoguePanel
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
    public float Roundness { get; set; }
    public int Segments { get; set; }
    public Color Color { get; set; }
    public DialoguePanelAnimation Animation { get; set; }
    public float Alpha { private set; get; } = 1.0f;

    private float _slideAnimTargetY;
    private bool _isVisible = true;
    private float _animationSpeed = 1000.0f;
    private float _fadeSpeed = 2.0f;

    public DialoguePanel(float x, float y, float width, float height, float roundness, int segments, Color color,
        DialoguePanelAnimation animation = DialoguePanelAnimation.Fade)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Roundness = roundness;
        Segments = segments;
        Color = color;
        Animation = animation;
        _slideAnimTargetY = Pos.PanelY();
    }


    public void ToggleVisibility()
    {
        _isVisible = !_isVisible;

        switch (Animation)
        {
            case Slide:
                _slideAnimTargetY = _isVisible ? Pos.PanelY() : 2000.0f;
                break;
            case DialoguePanelAnimation.Fade:
                Alpha = _isVisible ? 0.0f : 1.0f;
                break;
        }
    }

    public void Update()
    {
        _slideAnimTargetY = _isVisible ? Pos.PanelY() : 2000.0f;

        switch (Animation)
        {
            case Slide:
                if (Y.PracticallyNotEqual(_slideAnimTargetY))
                {
                    Y = _isVisible
                        ? Math.Max(_slideAnimTargetY, Y - _animationSpeed * GetFrameTime())
                        : Math.Min(_slideAnimTargetY, Y + _animationSpeed * GetFrameTime());
                }

                break;

            case DialoguePanelAnimation.Fade:
                if (_isVisible && Alpha < 1.0f)
                {
                    Alpha += _fadeSpeed * GetFrameTime();
                    if (Alpha > 1.0f) Alpha = 1.0f;
                }
                else if (!_isVisible && Alpha > 0.0f)
                {
                    Alpha -= _fadeSpeed * GetFrameTime();
                    if (Alpha < 0.0f) Alpha = 0.0f;
                }
                Y = Display.GetHeight() - MathEx.ValueFromPercent(Display.GetHeight(), 22.5f);

                break;

            case None:
                Alpha = _isVisible ? 1.0f : 0.0f;
                Y = Display.GetHeight() - MathEx.ValueFromPercent(Display.GetHeight(), 22.5f);

                break;
        }
    }

    public void Draw()
    {
        var panel = new Rectangle
        {
            X = X,
            Y = Y,
            Width = Width,
            Height = Height
        };

        Color panelColor = Animation is DialoguePanelAnimation.Fade or None
            ? Color with { A = (byte)(Alpha * 255) }
            : Color;

        DrawRectangleRounded(panel, Roundness, Segments, panelColor);
    }

    public bool IsFullyVisible() =>
        (Animation == Slide && X.AlmostEqual(50)) ||
        Alpha >= 1.0f;
}