using System.Numerics;
using Vn.Constants;
using Vn.Story;
using Vn.TheGame;
using Vn.Utils;

namespace Vn.UI;

public class HistoryPanel
{
    private bool _sceneSetted;
    private int _scrollOffset;
    public int FontSize { get; set; } = 20;

    public HistoryPanel()
    {
        Dialogues.OnAddToHistory += () => _sceneSetted = false;
    }

    private readonly DialoguePanel _dialoguePanel = new(
        x: 25,
        y: 0,
        width: Display.Width() - 25,
        height: Display.Height() - 50,
        roundness: 0.05f,
        segments: 10,
        color: Colors.MainBg,
        animation: DialoguePanelAnimation.Fade,
        () => Display.Height() - MathEx.ValueFromPercent(Display.Height(), 94));
    

    private List<string> WrapText(string text, float maxWidth)
    {
        var result = new List<string>();
        var words = text.Split(' ');

        var currentLine = "";
        foreach (var word in words)
        {
            var testLine = string.IsNullOrEmpty(currentLine) ? word : $"{currentLine} {word}";
            var textWidth = MeasureTextEx(Fonts.Main(FontSize), testLine, Fonts.Main(FontSize).BaseSize, 1).X;

            if (textWidth > maxWidth)
            {
                if (!string.IsNullOrEmpty(currentLine))
                {
                    result.Add(currentLine);
                }
                currentLine = word;
            }
            else
            {
                currentLine = testLine;
            }
        }

        if (!string.IsNullOrEmpty(currentLine))
        {
            result.Add(currentLine);
        }

        return result;
    }

    public void Update()
    {
        Scenes.GoBackWithRightMouse(() => _sceneSetted = false);
        _dialoguePanel.Update();
        _dialoguePanel.Width = Display.Width() - 2 * 25;
        _dialoguePanel.Height = Display.Height() - 2 * 50;

        const int scrollAmount = 10;
        _scrollOffset -= (int)GetMouseWheelMove() * scrollAmount;

        var totalTextHeight = 0;
        var maxWidth = _dialoguePanel.Width - 20;
        foreach (var dialogue in Dialogues.History)
        {
            if (!string.IsNullOrEmpty(dialogue.Character?.Name))
            {
                totalTextHeight += FontSize + 5;
            }

            var wrappedLines = WrapText(dialogue.Text, maxWidth);
            totalTextHeight += wrappedLines.Count * (FontSize + 5);
        }

        var maxScroll = Math.Max(0, totalTextHeight - _dialoguePanel.Height);
        
        if (!_sceneSetted)
        {
            _scrollOffset = (int)maxScroll;
            _dialoguePanel.ToggleVisibility(true);
            _sceneSetted = true;
        }

        _scrollOffset = (int)Math.Clamp(_scrollOffset, 0, maxScroll);
    }

    public void Draw()
    {
        Bg.DrawCurrent();
        _dialoguePanel.Draw();

        if (Dialogues.History.Count > 0)
        {
            var textX = _dialoguePanel.X + 10;
            var textY = _dialoguePanel.Y + 10 - _scrollOffset;
            var maxWidth = _dialoguePanel.Width - 20;

            BeginScissorMode((int)_dialoguePanel.X, (int)_dialoguePanel.Y, (int)_dialoguePanel.Width, (int)_dialoguePanel.Height);

            foreach (var dialogue in Dialogues.History)
            {
                if (!string.IsNullOrEmpty(dialogue.Character?.Name))
                {
                    DrawTextEx(
                        Fonts.Main(FontSize), 
                        dialogue.Character.Name, 
                        new Vector2(textX, textY), 
                        Fonts.Main(FontSize).BaseSize, 
                        1, 
                        dialogue.Character.Color
                    );
                    textY += FontSize + 5; 
                }

                var wrappedLines = WrapText(dialogue.Text, maxWidth);
                foreach (var line in wrappedLines)
                {
                    if (textY + FontSize > _dialoguePanel.Y + _dialoguePanel.Height)
                    {
                        break;
                    }

                    if (textY + FontSize >= _dialoguePanel.Y)
                    {
                        DrawTextEx(
                            Fonts.Main(FontSize), 
                            line, 
                            new Vector2(textX, textY), 
                            Fonts.Main(FontSize).BaseSize, 
                            1, 
                            dialogue.TextColor 
                        );
                    }
                    textY += FontSize + 5; 
                }
            }

            EndScissorMode();
        }
    }
}
