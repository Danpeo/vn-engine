using System.Numerics;
using Vn.UI;
using Vn.Utils;
using Textures = Vn.UI.Textures;

namespace Vn.Story;

public class Background : ITexture
{
    public string Path { get; }
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
        Path = texturePath;
        
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
        float targetPosX = CenterPosX();
        float slideShiftSpeed = _currentAnimationSpeed switch
        {
            AnimationSpeed.VerySlow => 1f,
            AnimationSpeed.Slow => 3f,
            AnimationSpeed.Normal => 6f,
            AnimationSpeed.Fast => 10f,
            AnimationSpeed.VeryFast => 15f,
            _ => 5f
        };

        float t = slideShiftSpeed * GetFrameTime();
        _slidePosX = MathEx.Lerp(_slidePosX, targetPosX, t);

        if (_slidePosX.AlmostEqual(targetPosX)) 
        {
            _slidePosX = targetPosX;
            _animationCompleted = true;
        }

        DrawTextureEx(_texture, new Vector2(_slidePosX, CenterPosY()), 0.0f, _scaleX, Color.White);
    }

    private void DrawWithFadeAnimation()
    {
        const float targetAlpha = 1.0f;
        float alphaSpeed = AlphaSpeed() * GetFrameTime();

        if (_alpha < targetAlpha)
        {
            float distanceToTarget = targetAlpha - _alpha;
            if (distanceToTarget.NearlyZero())
            {
                alphaSpeed *= distanceToTarget / 0.1f; 
            }

            _alpha += alphaSpeed;

            if (_alpha > targetAlpha)
            {
                _alpha = targetAlpha;
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
            AnimationSpeed.VerySlow => 0.5f,
            AnimationSpeed.Slow => 1.5f,
            AnimationSpeed.Normal => 2.5f,
            AnimationSpeed.Fast => 3.5f,
            AnimationSpeed.VeryFast => 4f,
            _ => 2.5f
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