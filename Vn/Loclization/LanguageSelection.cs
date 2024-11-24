using Vn.Constants;
using Vn.TheGame;
using Vn.UI;
using Vn.Utils;

namespace Vn.Loclization;

public class LanguageSelection
{
    private readonly Button _ruBtn = new("RU", () =>
    {
        Loc.Set(Locale.Ru);
        GameSettings.I.Locale = Locale.Ru;
        Saves.SaveSettings();
    })
    {
        Font =  Fonts.ElMessiriMedium(35)
    };
    private readonly Button _engBtn = new("ENG", () =>
    {
        Loc.Set(Locale.Eng);
        GameSettings.I.Locale = Locale.Eng;
        Saves.SaveSettings();
    })
    {
        Font =  Fonts.ElMessiriMedium(35)
    };

    public void Draw()
    {
        var textSize = MeasureTextEx(Fonts.ElMessiriMedium(), "RU", Fonts.ElMessiriMedium().BaseSize, 0);
        var textPos = Text.CenterPosition(textSize, -90, -90);
        
        _ruBtn.UpdateTitleColor(_ruBtn.HighlightColor, () => Loc.CurrentLocale is Locale.Ru);
        _ruBtn.Draw(textPos, textSize);
        
        var textSize2 = MeasureTextEx(Fonts.ElMessiriMedium(), "ENG", Fonts.ElMessiriMedium().BaseSize, 0);
        var textPos2 = Text.CenterPosition(textSize2, -85, -90);

        _engBtn.UpdateTitleColor(_engBtn.HighlightColor, () => Loc.CurrentLocale is Locale.Eng);
        _engBtn.Draw(textPos2, textSize2);
    }
}