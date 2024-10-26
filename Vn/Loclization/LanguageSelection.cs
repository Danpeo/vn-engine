using System.Numerics;
using Vn.Constants;
using Vn.UI;
using Vn.Utils;

namespace Vn.Loclization;

public class LanguageSelection
{
    private readonly Button _ruBtn = new("RU", () => Loc.Set(Locale.Ru));
    private readonly Button _engBtn = new("ENG", () => Loc.Set(Locale.Eng));

    public void Draw()
    {
        var textSize = MeasureTextEx(Fonts.Accent(), "RU", Fonts.Accent().BaseSize, 0);
        var textPos = Text.CenterPosition(textSize, -90, -90);

        _ruBtn.Draw(textPos, textSize);
        
        var textSize2 = MeasureTextEx(Fonts.Accent(), "ENG", Fonts.Accent().BaseSize, 0);
        var textPos2 = Text.CenterPosition(textSize2, -85, -90);

        _engBtn.Draw(textPos2, textSize2);
    }
}