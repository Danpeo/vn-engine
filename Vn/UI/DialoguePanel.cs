using Raylib_cs;
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

    private float _slideAnimTargetY;
    private float _alpha = 1.0f;
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
        _slideAnimTargetY = Y;
    }


    public void ToggleVisibility()
    {
        _isVisible = !_isVisible;

        switch (Animation)
        {
            case Slide:
                _slideAnimTargetY = _isVisible ? GetScreenHeight() - 200 : 2000.0f;
                break;
            case DialoguePanelAnimation.Fade:
                _alpha = _isVisible ? 0.0f : 1.0f;
                break;
        }
    }

    public void Update(float deltaTime)
    {
        switch (Animation)
        {
            case Slide:
                if (Y.IsBarelyEqual(_slideAnimTargetY))
                {
                    Y = _isVisible
                        ? Math.Max(_slideAnimTargetY, Y - _animationSpeed * deltaTime)
                        : Math.Min(_slideAnimTargetY, Y + _animationSpeed * deltaTime);
                }

                break;

            case DialoguePanelAnimation.Fade:
                if (_isVisible && _alpha < 1.0f)
                {
                    _alpha += _fadeSpeed * deltaTime;
                    if (_alpha > 1.0f) _alpha = 1.0f;
                }
                else if (!_isVisible && _alpha > 0.0f)
                {
                    _alpha -= _fadeSpeed * deltaTime;
                    if (_alpha < 0.0f) _alpha = 0.0f;
                }

                break;

            case None:
                _alpha = _isVisible ? 1.0f : 0.0f;
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
            ? Color with { A = (byte)(_alpha * 255) }
            : Color;

        DrawRectangleRounded(panel, Roundness, Segments, panelColor);
    }

    public bool IsFullyVisible() =>
        (Animation == Slide && X.IsAlmostEqual(50)) ||
        _alpha >= 1.0f;
}