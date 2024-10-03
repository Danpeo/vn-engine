using Vn.Constants;
using Vn.Utils;

namespace Vn.Story;

public class Background : ITexture
{
    public Texture2D Texture { get; set; }

    public Background(string texturePath)
    {
        if (Img.IsJpeg(texturePath))
        {
            var jpegNotFoundPath = Paths.Bg("jpeg_not_supported.png");
            Img.GeneratePngPlaceholder($"JPEG files are not supported: \"{texturePath}\"", GetScreenWidth(),GetScreenHeight(), 
                jpegNotFoundPath);

            texturePath = jpegNotFoundPath;
        }
        
        Texture = LoadTexture(texturePath);
        
        if (Texture.Id == 0)
        {
            var placeholderPath = Paths.Bg("placeholder.png");
            Img.GeneratePngPlaceholder($"File not found: \"{texturePath}\"", GetScreenWidth(),GetScreenHeight(), placeholderPath);
            Texture = LoadTexture(placeholderPath);
        }
        
        TextureManager.Add(this);
    }

    public void Draw()
    {
        int posX = (GetScreenWidth() - Texture.Width) / 2;
        int posY = (GetScreenHeight() - Texture.Height) / 2;

        DrawTexture(Texture, posX, posY, Color.White);
    }
    
    public void Unload()
    {
        UnloadTexture(Texture);
    }
}