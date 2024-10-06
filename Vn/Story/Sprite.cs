using System.Numerics;
using Vn.UI;
using Vn.Utils;

namespace Vn.Story;

public class Sprite : ITexture
{
    private readonly Texture2D _texture;
    private readonly ImageAnimation _animation;
    private readonly AnimationSpeed _originalAnimationSpeed;
    private AnimationSpeed _currentAnimationSpeed;
    private float _alpha;
    private float _slidePosX;
    private bool _animationCompleted;
    private float _scaleX;
    private float _scaleY;
    private Vector2 _position;
    private readonly Vector2 _originalPos;
    private readonly PositionOption _positionOption;

    public Sprite(string texturePath, ImageAnimation animation = ImageAnimation.Fade,
        AnimationSpeed originalAnimationSpeed = AnimationSpeed.Normal, PositionOption positionOption = PositionOption.Center, Vector2? customPosition = null)
    {
        _animation = animation;
        _originalAnimationSpeed = originalAnimationSpeed;
        _currentAnimationSpeed = originalAnimationSpeed;
        _positionOption = positionOption;
        Textures.Assign(ref _texture, texturePath);

        _slidePosX = -_texture.Width;
        Textures.Add(this);

        UpdateScale(); 
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

    private void UpdateScale()
    {
        _scaleX = (float)Display.GetWidth() / _texture.Width;
        _scaleY = (float)Display.GetHeight() / _texture.Height;

        float minScale = Math.Min(_scaleX, _scaleY);

        _scaleX = minScale;
        _scaleY = minScale;
    }

    public void Draw()
    {
        UpdateScale();
        _position = GetPosition(_positionOption, _originalPos); 
        DrawWithNoneAnimation();
    }

    private void DrawWithNoneAnimation()
    {
        DrawTextureEx(_texture, _position, 0, _scaleX, Color.White);
    }

    public void Unload()
    {
        UnloadTexture(_texture);
    }
}
