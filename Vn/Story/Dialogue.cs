using System.Numerics;
using LanguageExt;
using Raylib_cs;
using Vn.Constants;
using Vn.UI;
using static LanguageExt.Option<Vn.Story.Character>;
using static Raylib_cs.Raylib;

namespace Vn.Story;

public class Dialogue
{
    public string Text { get; private set; }
    public Option<Character> Character { get; private set; }
    public Color TextColor { get; set; }
    private int _charIndex;
    private float _timePassed;
    private readonly float _charDisplaySpeed;
    private readonly float _alphaSpeed;
    private readonly List<float> _alphas;

    public Dialogue(string text, float displaySpeed, float alphaSpeed,
        Color? textColor = null, Option<Character>? character = null)
    {
        Text = text;
        _charDisplaySpeed = displaySpeed;
        _alphaSpeed = alphaSpeed;
        _alphas = new List<float>();
        TextColor = textColor ?? Color.LightGray;
        Character = character ?? None;
    }

    public void Update(float deltaTime)
    {
        _timePassed += deltaTime;

        if (_charIndex < Text.Length && _timePassed >= _charDisplaySpeed)
        {
            _charIndex++;
            _timePassed = 0.0f;
            _alphas.Add(0.0f);
        }

        for (int i = 0; i < _alphas.Count; i++)
        {
            if (_alphas[i] < 1.0f)
            {
                _alphas[i] += _alphaSpeed;
                if (_alphas[i] > 1.0f)
                {
                    _alphas[i] = 1.0f;
                }
            }
        }
    }

    public void Draw(DialoguePanel panel, Font font, float fontSize, float spacing)
    {
        const int padding = 10;
        const int namePadding = 40;
        var pos = new Vector2(panel.X + padding, panel.Y + namePadding);

        Character.IfSome(
            character => DrawTextEx(Fonts.Accent, character.Name, new Vector2(panel.X + padding, panel.Y + padding),
                Fonts.Accent.BaseSize, 2,
                Color.White)
        );


        if (!panel.IsFullyVisible()) return; 

        for (int i = 0; i < _charIndex; i++)
        {
            char currentChar = Text[i];
            string charStr = currentChar.ToString();

            TextColor = TextColor with { A = (byte)(_alphas[i] * 255) };

            DrawTextEx(font, charStr, pos, fontSize, spacing, TextColor);

            pos.X += MeasureTextEx(font, charStr, fontSize, spacing).X;
        }
    }
}