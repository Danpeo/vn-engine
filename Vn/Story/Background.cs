using Vn.Constants;
using Vn.UI;
using Vn.Utils;

namespace Vn.Story;

public class Background : ITexture
{
    public Texture2D Texture { get; set; }
    public BackgroundAnimation Animation { get; set; }
    public AnimationSpeed AnimationSpeed { get; set; }
    private float _alpha;
    private float _slidePosX;
    private float _slidePosY;
    private bool _animationCompleted;
    private bool _reseted;

    public Background(string texturePath, BackgroundAnimation animation = BackgroundAnimation.FadeIn, AnimationSpeed animationSpeed = AnimationSpeed.Normal)
    {
        Animation = animation;
        AnimationSpeed = animationSpeed;
        
        if (Img.IsJpeg(texturePath))
        {
            var jpegNotFoundPath = Paths.Bg("jpeg_not_supported.png");
            Img.GeneratePng($"JPEG files are not supported: \"{texturePath}\"", GetScreenWidth(), GetScreenHeight(),
                jpegNotFoundPath);

            texturePath = jpegNotFoundPath;
        }

        Texture = LoadTexture(texturePath);

        if (Texture.Id == 0)
        {
            var placeholderPath = Paths.Bg("placeholder.png");
            Img.GeneratePng($"File not found: \"{texturePath}\"", GetScreenWidth(), GetScreenHeight(), placeholderPath);
            Texture = LoadTexture(placeholderPath);
        }

        _slidePosX = -Texture.Width;
        _slidePosY = CenterPosY();
        TextureManager.Add(this);
    }

    public void Draw()
    {
        if (_animationCompleted)
        {
            DrawWithNoneAnimation();
            return;
        }
        
        switch (Animation)
        {
            case BackgroundAnimation.None:
                DrawWithNoneAnimation();
                break;
            case BackgroundAnimation.FadeIn:
                DrawWithFadeAnimation();
                break;
            case BackgroundAnimation.SlideIn:
                DrawWithSlideAnimation();
                break;
            default:
                DrawWithNoneAnimation();
                break;
        }
    }

    public void CompleteAnimation()
    {
        _animationCompleted = true;
        _alpha = 1.0f;
        _slidePosX = CenterPosX();
        _reseted = true;
    }

    public void ResetAnimation()
    {
        _animationCompleted = false;
        _alpha = 0.0f;
        _slidePosX = -Texture.Width;
        _reseted = false;
    }
    
    private void DrawWithSlideAnimation()
    {
        var slideShiftSpeed = AnimationSpeed switch
        {
            AnimationSpeed.VerySlow => 15f,
            AnimationSpeed.Slow => 30f,
            AnimationSpeed.Normal => 50f,
            AnimationSpeed.Fast => 65f,
            AnimationSpeed.VeryFast => 80f,
            _ => 50f
        };
        
        if (_slidePosX < CenterPosX())
        {
            _slidePosX += slideShiftSpeed;
            if (_slidePosX >= CenterPosX())
            {
                _slidePosX = CenterPosX();
                _animationCompleted = true;
            }
        }
        
        DrawTexture(Texture, (int)_slidePosX, (int)_slidePosY, Color.White);
    }

    private void DrawWithFadeAnimation()
    {
        var alphaSpeed = AnimationSpeed switch
        {
            AnimationSpeed.VerySlow => 0.01f,
            AnimationSpeed.Slow => 0.02f,
            AnimationSpeed.Normal => 0.03f,
            AnimationSpeed.Fast => 0.04f,
            AnimationSpeed.VeryFast => 0.05f,
            _ => 0.03f
        };
        
        if (_alpha < 1.0f)
        {
            _alpha += alphaSpeed;
            if (_alpha > 1.0f)
            {
                _alpha = 1.0f;
                _animationCompleted = true;
            }
        }

        var (posX, posY) = CenterPosition();

        var fadeColor = new Color(255, 255, 255, (int)(_alpha * 255)); 
        DrawTexture(Texture, posX, posY, fadeColor);
    }

    private void DrawWithNoneAnimation()
    {
        var (posX, posY) = CenterPosition();
        DrawTexture(Texture, posX, posY, Color.White);
    }

    private (int posX, int posY) CenterPosition()
    {
        int posX = (int)CenterPosX();
        int posY = (int)CenterPosY();
        return (posX, posY);
    }

    private float CenterPosX() => (GetScreenWidth() - Texture.Width) / 2;
    private float CenterPosY() => (GetScreenHeight() - Texture.Height) / 2;
    
    public void Unload()
    {
        UnloadTexture(Texture);
    }
}