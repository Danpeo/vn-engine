using Vn.Constants;
using Vn.Story;
using Vn.Utils;

namespace Vn.UI;

public static class Textures
{
    private static readonly List<ITexture> AllTextures = [];
    
    public static void Add(ITexture texture) => AllTextures.Add(texture);
    
    public static void UnloadAll()
    {
        foreach (var texture in AllTextures)
        {
            texture.Unload();
        }
        
        AllTextures.Clear();
    }
    
    public static void Assign(ref Texture2D texture, string texturePath)
    {
        if (Img.IsJpeg(texturePath))
        {
            var jpegNotFoundPath = Paths.Bg("jpeg_not_supported.png");
            Img.GeneratePng($"JPEG files are not supported: \"{texturePath}\"", GetScreenWidth(), GetScreenHeight(),
                jpegNotFoundPath);

            texturePath = jpegNotFoundPath;
        }

        texture = LoadTexture(texturePath);

        if (texture.Id == 0)
        {
            var placeholderPath = Paths.Bg("placeholder.png");
            Img.GeneratePng($"File not found: \"{texturePath}\"", GetScreenWidth(), GetScreenHeight(), placeholderPath);
            texture = LoadTexture(placeholderPath);
        }
    }

}