using Vn.Story;

namespace Vn.Utils;

public static class TextureManager
{
    private static readonly List<ITexture> Textures = [];
    
    public static void Add(ITexture texture) => Textures.Add(texture);
    
    public static void UnloadAll()
    {
        foreach (var texture in Textures)
        {
            texture.Unload();
        }
        
        Textures.Clear();
    }
}