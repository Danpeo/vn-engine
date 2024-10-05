using System.Numerics;
using Vn.Constants;
using Vn.UI;
using Vn.Utils;

namespace Vn.Story;

public class Background : ITexture
{
    public Texture2D Texture { get; private set; }
    public BackgroundAnimation Animation { get; private set; }
    public AnimationSpeed OriginalAnimationSpeed { get; private set; }
    public AnimationSpeed CurrentAnimationSpeed { get; set; }
    private float _alpha;
    private float _slidePosX;
    private float _slidePosY;
    private bool _animationCompleted;
    private float _scaleX;
    private float _scaleY;
    private bool _scalePrev;

    public Background(string texturePath, BackgroundAnimation animation = BackgroundAnimation.FadeIn,
        AnimationSpeed originalAnimationSpeed = AnimationSpeed.Normal)
    {
        Animation = animation;
        OriginalAnimationSpeed = originalAnimationSpeed;
        CurrentAnimationSpeed = originalAnimationSpeed;

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

    public void UpdateScale()
    {
        var prev = Bg.Prev();
        var texture = _scalePrev && prev != null ? prev.Texture : Texture;
        Console.WriteLine(texture.Id);
        _scaleX = (float)GetScreenWidth() / texture.Width;
        _scaleY = (float)GetScreenHeight() / texture.Height;

        float minScale = Math.Min(_scaleX, _scaleY);

        _scaleX = minScale;
        _scaleY = minScale;
    }

    public void Draw()
    {
        UpdateScale();

        if (_animationCompleted)
        {
            DrawWithNoneAnimation(Texture);
            return;
        }

        switch (Animation)
        {
            case BackgroundAnimation.None:
                DrawWithNoneAnimation(Texture);
                break;
            case BackgroundAnimation.FadeIn:
                DrawWithFadeAnimation();
                break;
            case BackgroundAnimation.SlideIn:
                DrawWithSlideAnimation();
                break;
            case BackgroundAnimation.Transition:
                DrawWithTransition();
                break;
            default:
                DrawWithNoneAnimation(Texture);
                break;
        }
    }

    private void DrawWithTransition()
    {
        var prev = Bg.Prev();

        prev.Match(background =>
            {
                if (!background._animationCompleted)
                {
                    var alphaSpeed = AlphaSpeed();

                    if (background._alpha > 0.0f)
                    {
                        background._alpha -= alphaSpeed;
                        if (background._alpha < 0.0f)
                        {
                            background._alpha = 0.0f;
                            background._animationCompleted = true;
                        }

                        var fadeColor = new Color(255, 255, 255, (int)(background._alpha * 255));
                        var pos = CenterPosition(background.Texture); 
                        DrawTextureEx(background.Texture, pos, 0.0f, _scaleX, fadeColor);
                        _scalePrev = true;
                    }
                }
                else
                {
                    _scalePrev = false;
                    DrawWithFadeAnimation();
                }
            },
            () => DrawWithFadeAnimation());
    }


    public void CompleteAnimation()
    {
        _animationCompleted = true;
        _alpha = 1.0f;
        _slidePosX = CenterPosX();
        CurrentAnimationSpeed = OriginalAnimationSpeed;
    }

    public void Reset()
    {
        _animationCompleted = false;
        _alpha = 0.0f;
        _slidePosX = -Texture.Width;
    }

    private void DrawWithSlideAnimation()
    {
        var slideShiftSpeed = CurrentAnimationSpeed switch
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

    private void DrawWithFadeAnimation(Texture2D? texture = null)
    {
        var t = texture ?? Texture;
        var alphaSpeed = AlphaSpeed();

        if (_alpha < 1.0f)
        {
            _alpha += alphaSpeed;
            if (_alpha > 1.0f)
            {
                _alpha = 1.0f;
                _animationCompleted = true;
            }
        }

        var fadeColor = new Color(255, 255, 255, (int)(_alpha * 255));
        var pos = CenterPosition(t);
        DrawTextureEx(t, pos, 0.0f, _scaleX, fadeColor);
    }

    private float AlphaSpeed() =>
        CurrentAnimationSpeed switch
        {
            AnimationSpeed.VerySlow => 0.01f,
            AnimationSpeed.Slow => 0.02f,
            AnimationSpeed.Normal => 0.03f,
            AnimationSpeed.Fast => 0.04f,
            AnimationSpeed.VeryFast => 0.05f,
            _ => 0.03f
        };

    private void DrawWithNoneAnimation(Texture2D? texture = null)
    {
        var pos = CenterPosition(texture);
        DrawTextureEx(texture ?? Texture, pos, 0.0f, _scaleX, Color.White);
    }

    private Vector2 CenterPosition(Texture2D? texture = null)
    {
        var t = texture ?? Texture;
        float posX = (GetScreenWidth() - t.Width * _scaleX) / 2;
        float posY = (GetScreenHeight() - t.Height * _scaleY) / 2;
        return new Vector2(posX, posY);
    }

    private float CenterPosX() => (GetScreenWidth() - Texture.Width) / 2;
    private float CenterPosY() => (GetScreenHeight() - Texture.Height) / 2;

    public void ChangeAnimationSpeed(AnimationSpeed newAnimationSpeed)
    {
        CurrentAnimationSpeed = newAnimationSpeed;
    }

    public void Unload()
    {
        UnloadTexture(Texture);
    }
}