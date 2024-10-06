using System.Numerics;
using Vn.UI;
using Vn.Utils;
using Textures = Vn.UI.Textures;

namespace Vn.Story;

public class Background : ITexture
{
    private Texture2D _texture;
    private readonly ImageAnimation _animation;
    private readonly AnimationSpeed _originalAnimationSpeed;
    private AnimationSpeed _currentAnimationSpeed;
    private float _alpha;
    private float _slidePosX;
    private bool _animationCompleted;
    private float _scaleX;
    private float _scaleY;

    public Background(string texturePath, ImageAnimation animation = ImageAnimation.Fade,
        AnimationSpeed originalAnimationSpeed = AnimationSpeed.Normal)
    {
        _animation = animation;
        _originalAnimationSpeed = originalAnimationSpeed;
        _currentAnimationSpeed = originalAnimationSpeed;
      
        Textures.Assign(ref _texture, texturePath);
        
        _slidePosX = -_texture.Width;
        Textures.Add(this);
    }

    private void UpdateScale() => Textures.UpdateScale(ref _texture, out _scaleX, out _scaleY);

    public void Draw()
    {
        UpdateScale();
        if (_animationCompleted)
        {
            DrawWithNoneAnimation();
            return;
        }

        switch (_animation)
        {
            case ImageAnimation.None:
                DrawWithNoneAnimation();
                break;
            case ImageAnimation.Fade:
                DrawWithFadeAnimation();
                break;
            case ImageAnimation.Slide:
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
        _currentAnimationSpeed = _originalAnimationSpeed;
    }

    public void Reset()
    {
        _animationCompleted = false;
        _alpha = 0.0f;
        _slidePosX = -_texture.Width;
    }

    private void DrawWithSlideAnimation()
    {
        var slideShiftSpeed = _currentAnimationSpeed switch
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

        DrawTextureEx(_texture, new Vector2(_slidePosX, CenterPosY()), 0.0f, _scaleX, Color.White);
    }

    private void DrawWithFadeAnimation()
    {
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
        var pos = CenterPosition();
        DrawTextureEx(_texture, pos, 0.0f, _scaleX, fadeColor);
    }

    private float AlphaSpeed() =>
        _currentAnimationSpeed switch
        {
            AnimationSpeed.VerySlow => 0.01f,
            AnimationSpeed.Slow => 0.02f,
            AnimationSpeed.Normal => 0.03f,
            AnimationSpeed.Fast => 0.04f,
            AnimationSpeed.VeryFast => 0.05f,
            _ => 0.03f
        };

    private void DrawWithNoneAnimation()
    {
        var pos = CenterPosition();
        DrawTextureEx(_texture, pos, 0.0f, _scaleX, Color.White);
    }

    private Vector2 CenterPosition()
    {
        float posX = CenterPosX();
        float posY = CenterPosY();
        return new Vector2(posX, posY);
    }

    private float CenterPosX() => (Display.GetWidth() - _texture.Width * _scaleX) / 2;
    private float CenterPosY() => (Display.GetHeight() - _texture.Height * _scaleY) / 2;

    public void ChangeAnimationSpeed(AnimationSpeed newAnimationSpeed)
    {
        _currentAnimationSpeed = newAnimationSpeed;
    }

    public void Unload()
    {
        UnloadTexture(_texture);
    }
}