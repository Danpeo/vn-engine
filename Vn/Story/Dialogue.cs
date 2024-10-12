using System.Numerics;
using Vn.Constants;
using Vn.UI;
using Vn.Utils;
using static Raylib_cs.Raylib;

namespace Vn.Story;

public class Dialogue
{
    public string Text { get; private set; }
    public Character? Character { get; private set; }
    public Color TextColor { get; set; }
    private int _charIndex;
    private float _timePassed;
    private readonly float _charDisplaySpeed;
    private readonly float _alphaSpeed;
    private readonly List<float> _alphas;

    private DialogueAudioType _audioType;
    private Sound? _voiceLine;
    private Sound? _soundEffect;
    
    public Dialogue(Character? character, string text, float displaySpeed = 0.015f, float alphaSpeed = 0.04f,
        Color? textColor = null, DialogueAudioType audioType = DialogueAudioType.None, Sound? voiceLine = null, Sound? soundEffect = null)
    {
        Text = text;
        _charDisplaySpeed = displaySpeed;
        _alphaSpeed = alphaSpeed;
        _alphas = new List<float>();
        _audioType = audioType;
        _voiceLine = voiceLine;
        _soundEffect = soundEffect;
        TextColor = textColor ?? Color.LightGray;
        Character = character ?? null;
        Dialogues.Add(this);
    }

    public void Update()
    {
        _timePassed += GetFrameTime();
        
        if (_charIndex < Text.Length && _timePassed >= _charDisplaySpeed)
        {
            _charIndex++;
            _timePassed = 0.0f;
            _alphas.Add(0.0f);
            
            PlayCharacterAudio();
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

    private void PlayCharacterAudio()
    {
        if (_audioType is DialogueAudioType.SoundEffect)
        {
            _soundEffect.IfSome(PlaySound);
        }
    }

    private void AdjustCharSpeedForSoundEffect()
    {
        if (_audioType is DialogueAudioType.SoundEffect && _soundEffect.HasValue)
        {
            // TODO
        }
    }

    public void Draw(DialoguePanel panel, Font font, float fontSize, float spacing)
    {
        const int padding = 10;
        const int namePadding = 40;
        var pos = new Vector2(panel.X + padding, panel.Y + namePadding);

        if (!panel.IsFullyVisible()) return;
        
        Character.IfSome(
            character => DrawTextEx(Fonts.Accent, character.CurrentDisplayName(), new Vector2(panel.X + padding, panel.Y + padding),
                Fonts.Accent.BaseSize, 2,
                character.Color)
        );
        
        float maxWidth = panel.Width - 2 * padding; 
        float currentLineWidth = 0;
        
        for (int i = 0; i < _charIndex; i++)
        {
            char currentChar = Text[i];
            string charStr = currentChar.ToString();

            Vector2 charSize = MeasureTextEx(font, charStr, fontSize, spacing);
            
            // If the line length is greater than the max line length, start a new line
            if (currentLineWidth + charSize.X > maxWidth)
            {
                // Start new line
                pos.X = panel.X + padding;  
                
                // Move Y down to start a new line
                pos.Y += charSize.Y;        
                currentLineWidth = 0;       
            }
            
            currentLineWidth += charSize.X;
            
            TextColor = TextColor with { A = (byte)(_alphas[i] * 255) };

            DrawTextEx(font, charStr, pos, fontSize, spacing, TextColor);

            pos.X += charSize.X;
        }
    }
    
    public bool IsFinishedDrawing() => _charIndex >= Text.Length && _alphas.All(a => a.AlmostEqual(1.0f));

    public void Skip()
    {
        _charIndex = Text.Length;

        for (int i = 0; i < Text.Length; i++)
        {
            if (i >= _alphas.Count)
            {
                _alphas.Add(1.0f);
            }
            else
            {
                _alphas[i] = 1.0f;
            }
        }
    }
}