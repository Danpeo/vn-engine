namespace Vn.Audio;

public static class Musics
{
    private static string? LoadedPath;
    private static Music Current;
    private static Music Next;
    public static float FadeSpeed { get; set; } = 0.02f;
    public static bool IsPlaying { get; private set; }
    private static float Volume = 1.0f;

    public static void Update()
    {
        if (IsPlaying)
        {
            UpdateMusicStream(Current);
        }

        if (Next.Stream.Buffer != IntPtr.Zero && Volume > 0)
        {
            Volume -= FadeSpeed;
            SetMusicVolume(Current, Volume);

            if (Volume <= 0)
            {
                StopMusicStream(Current);
                UnloadMusicStream(Current);

                Current = Next;
                Next = new Music();

                PlayMusicStream(Current);
                Volume = 1.0f;
                SetMusicVolume(Current, Volume);
            }
        }
    }

    public static void Load(string path)
    {
        if (LoadedPath == path) return;
        LoadedPath = path;
        
        var music = LoadMusicStream(path);
        if (Current.Stream.Buffer == 0)
        {
            Current = music;
        }
        else
        {
            Next = music;
        }
    }

    public static void Play()
    {
        if (!IsPlaying)
        {
            PlayMusicStream(Current);
            IsPlaying = true;
        }
    }

    public static void Stop()
    {
        if (IsPlaying)
        {
            StopMusicStream(Current);
            UnloadMusicStream(Current);
            IsPlaying = false;
        }
    }

    public static async Task Switch(string path)
    {
        Load(path);
        await Task.Delay(10);
    }
    
    public static void Unload()
    {
        if (IsPlaying)
        {
            Stop();
        }

        if (Current.Stream.Buffer != 0)
        {
            UnloadMusicStream(Current);
        }

        if (Next.Stream.Buffer != 0)
        {
            UnloadMusicStream(Next);
        }
    }
}