using System.Numerics;
using Vn.UI;
using Vn.Utils;

namespace Vn.Story;

public class Sprite : ITexture
{
    public bool Moved { get; private set; }
    public Texture2D Texture;
    public readonly ImageAnimation Animation;
    public readonly AnimationSpeed OriginalAnimationSpeed;
    public AnimationSpeed CurrentAnimationSpeed;
    public float Alpha;
    public bool AnimationCompleted;
    public float ScaleX;
    public float ScaleY;
    public Vector2 Position;
    public Vector2? MoveDestination;
    public readonly Vector2 OriginalPos;
    public readonly Vector2? CustomPos;
    public PositionOption PositionOption { get; set; }

    public Sprite(string texturePath, ImageAnimation animation = ImageAnimation.Fade,
        AnimationSpeed originalAnimationSpeed = AnimationSpeed.Normal,
        PositionOption positionOption = PositionOption.Center, Vector2? customPosition = null)
    {
        Animation = animation;
        OriginalAnimationSpeed = originalAnimationSpeed;
        CurrentAnimationSpeed = originalAnimationSpeed;
        PositionOption = positionOption;
        Textures.Assign(ref Texture, texturePath);

        Textures.Add(this);

        UpdateScale();
        CustomPos = customPosition;
        Position = customPosition ?? GetPosition(PositionOption);
        OriginalPos = Position;
    }

    private Vector2 GetPosition(PositionOption positionOption, Vector2? customPosition = null)
    {
        int screenWidth = Display.Width();
        int screenHeight = Display.Height();

        if (customPosition != null)
        {
            return new Vector2(customPosition.Value.X * ScaleX, customPosition.Value.Y * ScaleY);
        }

        return positionOption switch
        {
            PositionOption.Center => new Vector2((screenWidth - Texture.Width * ScaleX) / 2f,
                (screenHeight - Texture.Height * ScaleY) / 2f),
            PositionOption.Left => new Vector2(0f, (screenHeight - Texture.Height * ScaleY) / 2f),
            PositionOption.Right => new Vector2(screenWidth - Texture.Width * ScaleX,
                (screenHeight - Texture.Height * ScaleY) / 2f),
            PositionOption.Top => new Vector2((screenWidth - Texture.Width * ScaleX) / 2f, 0f),
            PositionOption.Bottom => new Vector2((screenWidth - Texture.Width * ScaleX) / 2f,
                screenHeight - Texture.Height * ScaleY),
            PositionOption.FarLeft => new Vector2(-(screenWidth - Texture.Width * ScaleX) * 0.15f,
                (screenHeight - Texture.Height * ScaleY) / 2f),
            PositionOption.FarRight => new Vector2(screenWidth, (screenHeight - Texture.Height * ScaleY) / 2f),
            PositionOption.AwayToLeft => new Vector2(-(screenWidth + Texture.Width * ScaleX + 500),
                (screenHeight - Texture.Height * ScaleY) / 2f),
            _ => new Vector2((screenWidth - Texture.Width * ScaleX) / 2f,
                (screenHeight - Texture.Height * ScaleY) / 2f),
        };
    }

    public void Draw()
    {
        Console.WriteLine(MoveDestination.HasValue ? MoveDestination.Value : -1);
        UpdateScale();
        if (!MoveDestination.HasValue)
        {
            Position = GetPosition(PositionOption, CustomPos);
            Console.WriteLine(Position);
        }

        switch (Animation)
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
        if (!MoveDestination.HasValue)
        {
            Position = Position with { X = -Texture.Width * ScaleX };
            MoveDestination = GetPosition(PositionOption, CustomPos);
        }

        Move(PositionOption, CustomPos);

        DrawTextureEx(Texture, Position, 0, ScaleX, Color.White);
    }

    public void Move(PositionOption positionOption, Vector2? customPos = null)
    {
        PositionOption = positionOption;
        var speed = CurrentAnimationSpeed switch
        {
            AnimationSpeed.VerySlow => 1f,
            AnimationSpeed.Slow => 3f,
            AnimationSpeed.Normal => 6f,
            AnimationSpeed.Fast => 10f,
            AnimationSpeed.VeryFast => 15f,
            _ => 5f
        };

        MoveDestination = GetPosition(positionOption, customPos);
        float step = speed * GetFrameTime();

        Position = MathEx.Lerp(Position, MoveDestination.Value, step);
        Position.X = MathF.Round(Position.X);
        Position.Y = MathF.Round(Position.Y);
    }

    
    private void DrawWithNoneAnimation()
    {
        DrawTextureEx(Texture, Position, 0, ScaleX, Color.White);
    }

    public void Unload()
    {
        UnloadTexture(Texture);
    }

    private void UpdateScale() => Textures.UpdateScale(ref Texture, out ScaleX, out ScaleY);
}