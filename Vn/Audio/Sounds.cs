namespace Vn.Audio;

public static class Sounds
{
    private static readonly Dictionary<AllSounds, Sound> LoadedSounds = new();
    public static float Volume { get; set; } = 1.0f;

    public static void Load(AllSounds key, string path)
    {
        if (!LoadedSounds.ContainsKey(key))
        {
            var sound = LoadSound(path);
            SetSoundVolume(sound, Volume);
            LoadedSounds[key] = sound;
        }
    }

    public static void Play(AllSounds key)
    {
        if (LoadedSounds.TryGetValue(key, out Sound sound))
        {
            PlaySound(sound);
        }
    }
    
    public static void Stop(AllSounds key)
    {
        if (LoadedSounds.TryGetValue(key, out Sound sound))
        {
            StopSound(sound);
        }
    }
    
    public static void Unload()
    {
        foreach (var sound in LoadedSounds.Values)
        {
            UnloadSound(sound);
        }
        LoadedSounds.Clear();
    }
}