using System.Numerics;

namespace Vn.UI;

public class ButtonPanel
{
    private readonly List<Button> _buttons;
    private Vector2 _buttonSize = new(100, 50); 
    private float _spacing = 10f; 

    public ButtonPanel(List<Button> buttons)
    {
        _buttons = buttons;
    }
    
    public void AddButton(Button button)
    {
        _buttons.Add(button);
    }

    public void Draw()
    {
        float totalWidth = (_buttonSize.X + _spacing) * _buttons.Count - _spacing;
        float startX = GetScreenWidth() - totalWidth - 20; 
        float startY = GetScreenHeight() - _buttonSize.Y - 20; 

        for (int i = 0; i < _buttons.Count; i++)
        {
            Vector2 position = new Vector2(startX + i * (_buttonSize.X + _spacing), startY);
            _buttons[i].Draw(position, _buttonSize);
        }
    }
}
