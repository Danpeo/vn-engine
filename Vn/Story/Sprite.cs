using System.Numerics;
using Vn.UI;
using Vn.Utils;

namespace Vn.Story;

public class Sprite : ITexture
{
    public bool Moved { get; private set; }
    private Texture2D _texture;
    private readonly ImageAnimation _animation;
    private readonly AnimationSpeed _originalAnimationSpeed;
    private AnimationSpeed _currentAnimationSpeed;
    private float _alpha;
    private bool _animationCompleted;
    private float _scaleX;
    private float _scaleY;
    private Vector2 _position;
    private Vector2? _moveDestination;
    private readonly Vector2 _originalPos;
    private readonly Vector2? _customPos;
    private readonly PositionOption _positionOption;

    public Sprite(string texturePath, ImageAnimation animation = ImageAnimation.Fade,
        AnimationSpeed originalAnimationSpeed = AnimationSpeed.Normal,
        PositionOption positionOption = PositionOption.Center, Vector2? customPosition = null)
    {
        _animation = animation;
        _originalAnimationSpeed = originalAnimationSpeed;
        _currentAnimationSpeed = originalAnimationSpeed;
        _positionOption = positionOption;
        Textures.Assign(ref _texture, texturePath);

        Textures.Add(this);

        UpdateScale();
        _customPos = customPosition;
        _position = customPosition ?? GetPosition(_positionOption);
        _originalPos = _position;
    }

    private Vector2 GetPosition(PositionOption positionOption, Vector2? customPosition = null)
    {
        int screenWidth = Display.GetWidth();
        int screenHeight = Display.GetHeight();

        if (customPosition != null)
        {
            return new Vector2(customPosition.Value.X * _scaleX, customPosition.Value.Y * _scaleY);
        }
        
        return positionOption switch
        {
            PositionOption.Center => new Vector2((screenWidth - _texture.Width * _scaleX) / 2f, (screenHeight - _texture.Height * _scaleY) / 2f),
            PositionOption.Left => new Vector2(0f, (screenHeight - _texture.Height * _scaleY) / 2f),
            PositionOption.Right => new Vector2(screenWidth - _texture.Width * _scaleX, (screenHeight - _texture.Height * _scaleY) / 2f),
            PositionOption.Top => new Vector2((screenWidth - _texture.Width * _scaleX) / 2f, 0f),
            PositionOption.Bottom => new Vector2((screenWidth - _texture.Width * _scaleX) / 2f, screenHeight - _texture.Height * _scaleY),
            PositionOption.FarLeft => new Vector2(-(screenWidth - _texture.Width * _scaleX) * 0.15f, (screenHeight - _texture.Height * _scaleY) / 2f),
            PositionOption.FarRight => new Vector2(screenWidth, (screenHeight - _texture.Height * _scaleY) / 2f), 
            _ => new Vector2((screenWidth - _texture.Width * _scaleX) / 2f, (screenHeight - _texture.Height * _scaleY) / 2f),
        };
    }

    public void Draw()
    {
        UpdateScale();
        if (!_moveDestination.HasValue)
        {
            _position = GetPosition(_positionOption, _customPos);
        }

        switch (_animation)
        {
            case ImageAnimation.None:
                DrawWithNoneAnimation();
                break;
            case ImageAnimation.Fade:
            case ImageAnimation.Slide:
                DrawWithSlideAnimation();
                break;
            default:
                DrawWithNoneAnimation();
                break;
        }
    }

    private void DrawWithSlideAnimation()
    {
        if (!_moveDestination.HasValue)
        {
            _position = _position with { X = -_texture.Width * _scaleX };
            _moveDestination = GetPosition(_positionOption, _customPos);
        }

        if (Vector2.Distance(_position, _moveDestination.Value) > 0.1f)
        {
            Move(_positionOption, _customPos);
        }
        else
        {
            _moveDestination = null;
            Moved = true;
        }

        DrawTextureEx(_texture, _position, 0, _scaleX, Color.White);
    }

    public void Move(PositionOption positionOption, Vector2? customPos = null)
    {
        var speed = _currentAnimationSpeed switch
        {
            AnimationSpeed.VerySlow => 3f,
            AnimationSpeed.Slow => 5f,
            AnimationSpeed.Normal => 10f,
            AnimationSpeed.Fast => 15f,
            AnimationSpeed.VeryFast => 20f,
            _ => 5f
        };
        _moveDestination = GetPosition(positionOption, customPos);
        if (_moveDestination.HasValue && Vector2.Distance(_position, _moveDestination.Value) > 0.1f)
        {
            Moved = false;
            float step = speed * GetFrameTime();
            _position = MathEx.Lerp(_position, _moveDestination.Value, step);
            _position.X = MathF.Round(_position.X);
            _position.Y = MathF.Round(_position.Y);
        }
        else
        {
            _moveDestination = null;
            Moved = true;
        }
    }

    private void DrawWithNoneAnimation()
    {
        DrawTextureEx(_texture, _position, 0, _scaleX, Color.White);
    }

    public void Unload()
    {
        UnloadTexture(_texture);
    }

    private void UpdateScale() => Textures.UpdateScale(ref _texture, out _scaleX, out _scaleY);
}