namespace Vn.UI;

public static class UILayers
{
    public static UILayer Current { get; private set; }
    
    public static void Set(UILayer layer)
    {
        if (Current == layer) return;
        Current = layer;
    }
}