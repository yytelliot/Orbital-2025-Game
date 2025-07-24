using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Audio Library")]
public class AudioLibrary : ScriptableObject
{
    [System.Serializable]
    public struct Entry
    {
        public string key;
        public AudioClip clip;
    }

    public Entry[] entries;

    // The global lookup table
    private static Dictionary<string, AudioClip> _lookup;

    // Call this once—e.g. from an initializer in your first scene
    public void Init()
    {
        if (_lookup != null) return;
        _lookup = new Dictionary<string, AudioClip>(entries.Length);
        foreach (var e in entries)
        {
            if (!_lookup.ContainsKey(e.key))
                _lookup.Add(e.key, e.clip);
            else
                Debug.LogWarning($"Duplicate sound key: {e.key}");
        }
    }

    // Public accessor
    public static AudioClip GetClip(string key)
    {
        if (_lookup != null && _lookup.TryGetValue(key, out var clip))
            return clip;
        Debug.LogError($"SoundLibrary: No clip found for key '{key}'");
        return null;
    }
}