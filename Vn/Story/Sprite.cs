using System.Numerics;
using Vn.UI;

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
    private PositionOption _positionOption;


    public Sprite(string texturePath, ImageAnimation animation = ImageAnimation.Fade,
        AnimationSpeed originalAnimationSpeed = AnimationSpeed.Normal, PositionOption positionOption = PositionOption.Center, Vector2? customPosition = null)
    {
        _animation = animation;
        _originalAnimationSpeed = originalAnimationSpeed;
        _currentAnimationSpeed = originalAnimationSpeed;
        _position = customPosition ?? GetPosition(positionOption, _texture);
        Textures.Assign(ref _texture, texturePath);
        
        _slidePosX = -_texture.Width;
        Textures.Add(this);
    }

    private static Vector2 GetPosition(PositionOption positionOption, Texture2D texture)
    {
        int screenWidth = GetScreenWidth();
        int screenHeight = GetScreenHeight();

        return positionOption switch
        {
            PositionOption.Center => new Vector2((screenWidth - texture.Width) / 2f, (screenHeight - texture.Height) / 2f),
            PositionOption.Left => new Vector2(0f, (screenHeight - texture.Height) / 2f),
            PositionOption.Right => new Vector2(screenWidth - texture.Width, (screenHeight - texture.Height) / 2f),
            PositionOption.Top => new Vector2((screenWidth - texture.Width) / 2f, 0f),
            PositionOption.Bottom => new Vector2((screenWidth - texture.Width) / 2f, screenHeight - texture.Height),
            _ => throw new ArgumentOutOfRangeException(nameof(positionOption), positionOption, null)
        };
    }


    public void Draw()
    {
        _position = GetPosition(_positionOption, _texture);
        DrawWithNoneAnimation();
    }
    
    private void DrawWithNoneAnimation()
    {
        DrawTextureEx(_texture, _position, 0.0f, 1.0f, Color.White);
    }
    
    public void Unload()
    {
        UnloadTexture(_texture);
    }
}