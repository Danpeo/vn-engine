using System.Numerics;
using Vn.UI;
using Vn.Utils;
using Textures = Vn.UI.Textures;

namespace Vn.Story;

public class Background : ITexture
{
    public string Path { get; }
    public Texture2D Texture;
    public readonly ImageAnimation Animation;
    public readonly AnimationSpeed OriginalAnimationSpeed;
    public AnimationSpeed CurrentAnimationSpeed;
    public float Alpha;
    public float SlidePosX;
    public bool AnimationCompleted;
    public float ScaleX;
    public float ScaleY;

    public Background(string texturePath, ImageAnimation animation = ImageAnimation.Fade,
        AnimationSpeed originalAnimationSpeed = AnimationSpeed.Normal)
    {
        Animation = animation;
        OriginalAnimationSpeed = originalAnimationSpeed;
        CurrentAnimationSpeed = originalAnimationSpeed;
      
        Textures.Assign(ref Texture, texturePath);
        Path = texturePath;
        
        SlidePosX = -Texture.Width;
        Textures.Add(this);
    }

    private void UpdateScale() => Textures.UpdateScale(ref Texture, out ScaleX, out ScaleY);

    public void Draw()
    {
        UpdateScale();
        if (AnimationCompleted)
        {
            DrawWithNoneAnimation();
            return;
        }

        switch (Animation)
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
        AnimationCompleted = true;
        Alpha = 1.0f;
        SlidePosX = CenterPosX();
        CurrentAnimationSpeed = OriginalAnimationSpeed;
    }

    public void Reset()
    {
        AnimationCompleted = false;
        Alpha = 0.0f;
        SlidePosX = -Texture.Width;
    }

    private void DrawWithSlideAnimation()
    {
        float targetPosX = CenterPosX();
        float slideShiftSpeed = CurrentAnimationSpeed switch
        {
            AnimationSpeed.VerySlow => 1f,
            AnimationSpeed.Slow => 3f,
            AnimationSpeed.Normal => 6f,
            AnimationSpeed.Fast => 10f,
            AnimationSpeed.VeryFast => 15f,
            _ => 5f
        };

        float t = slideShiftSpeed * GetFrameTime();
        SlidePosX = MathEx.Lerp(SlidePosX, targetPosX, t);

        if (SlidePosX.AlmostEqual(targetPosX)) 
        {
            SlidePosX = targetPosX;
            AnimationCompleted = true;
        }

        DrawTextureEx(Texture, new Vector2(SlidePosX, CenterPosY()), 0.0f, ScaleX, Color.White);
    }

    private void DrawWithFadeAnimation()
    {
        const float targetAlpha = 1.0f;
        float alphaSpeed = AlphaSpeed() * GetFrameTime();

        if (Alpha < targetAlpha)
        {
            float distanceToTarget = targetAlpha - Alpha;
            if (distanceToTarget.NearlyZero())
            {
                alphaSpeed *= distanceToTarget / 0.1f; 
            }

            Alpha += alphaSpeed;

            if (Alpha > targetAlpha)
            {
                Alpha = targetAlpha;
                AnimationCompleted = true;
            }
        }

        var fadeColor = new Color(255, 255, 255, (int)(Alpha * 255));
        var pos = CenterPosition();
        DrawTextureEx(Texture, pos, 0.0f, ScaleX, fadeColor);
    }

    private float AlphaSpeed() =>
        CurrentAnimationSpeed switch
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
        DrawTextureEx(Texture, pos, 0.0f, ScaleX, Color.White);
    }

    private Vector2 CenterPosition()
    {
        float posX = CenterPosX();
        float posY = CenterPosY();
        return new Vector2(posX, posY);
    }

    private float CenterPosX() => (Display.Width() - Texture.Width * ScaleX) / 2;
    private float CenterPosY() => (Display.Height() - Texture.Height * ScaleY) / 2;

    public void ChangeAnimationSpeed(AnimationSpeed newAnimationSpeed)
    {
        CurrentAnimationSpeed = newAnimationSpeed;
    }

    public void Unload()
    {
        UnloadTexture(Texture);
    }
}