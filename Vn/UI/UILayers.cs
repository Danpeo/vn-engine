namespace Vn.UI;

public static class UILayers
{
    public static UILayer Current { get; private set; }
    public static UILayer Prev { get; private set; }
    
    public static void Set(UILayer layer)
    {
        if (Current == layer) return;
        Prev = Current;
        Current = layer;
    }

    public static void SetToPrev() => Set(Prev);
}