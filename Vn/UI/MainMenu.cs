using Vn.Constants;
using Vn.Story;
using Vn.Utils;

namespace Vn.UI;

public class MainMenu
{
    private readonly Background _background;
    private readonly string _title;
    
    public MainMenu(Background background, string title)
    {
        _background = background;
        _title = title;
    }

    public void Draw()
    {
        _background.Draw();
        
        DrawTextEx(Fonts.ArimoBold, _title, Text.CenterPosition(ArimoBold.MeasureStringSize(_title)), Fonts.ArimoBold.BaseSize, 0, Color.Red);
    }
}