namespace Vn.Utils;

public static class Display
{
    public static void ToggleFullscreenWindow(int width, int height)
    {
        if (!IsWindowFullscreen())
        {
            int monitor = GetCurrentMonitor();
            SetWindowSize(GetMonitorWidth(monitor), GetMonitorHeight(monitor));
            ToggleFullscreen();
        }
        else
        {
            ToggleFullscreen();
            SetWindowSize(width, height);
        }
    }

    public static int GetWidth()
    {
        if (IsWindowFullscreen())
        {
            int monitor = GetCurrentMonitor();
            return GetMonitorWidth(monitor);
        }

        return GetScreenWidth();
    }
    
    public static int GetHeight()
    {
        if (IsWindowFullscreen())
        {
            int monitor = GetCurrentMonitor();
            return GetMonitorHeight(monitor);
        }

        return GetScreenHeight();
    }
}