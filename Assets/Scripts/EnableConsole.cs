using UnityEngine;

// This runs before any Awake or Start in your scene
public static class DevConsoleBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void EnableDevConsole()
    {
        // Allow the console to ever appear
        Debug.developerConsoleEnabled = true;
        // Show it immediately
        Debug.developerConsoleVisible = true;
    }
}