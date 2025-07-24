using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioSource sfxSource;
    private AudioSource musicSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // SFX source
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;

            // Music source
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.playOnAwake = false;
            musicSource.loop = true;        // loop music by default
            musicSource.volume = 0.5f;      // default BGM volume
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Play a one-shot SFX
    public static void PlaySound(AudioClip clip, float volume = 1, float pitchMin = 1f, float pitchMax = 1f)
    {
        // Debug.Log($"Playing sound: {clip} at volume {volume}");

        float pitch = Random.Range(pitchMin, pitchMax);
        Instance.sfxSource.pitch = pitch;
        Instance.sfxSource.PlayOneShot(clip, volume);
        Instance.sfxSource.pitch = 1f;
    }

     public static void PlaySoundVariation(AudioClip clip, float volume = 1f)
    {

        int[] pentatonicSemitones = new[] { 0, 2, 4, 7, 9 };
        int semitone = pentatonicSemitones[Random.Range(0, pentatonicSemitones.Length)];

        float pitch = Mathf.Pow(1.059463f, semitone); // Semitone-based pitch

        Instance.sfxSource.pitch = pitch;
        Instance.sfxSource.clip = clip;
        Instance.sfxSource.volume = volume;

        // Debug.Log($"Playing {clip} at pitch ({pitch}) with semitone offset: {semitone}");
        Instance.sfxSource.Play();
    }


    // Start/switch BGM
    public static void PlayMusic(AudioClip clip, float volume = 1f)
    {
        if (Instance.musicSource.clip == clip) return;  // already playing
        Instance.musicSource.clip = clip;
        Instance.musicSource.volume = volume;
        Instance.musicSource.Play();
    }

    // Stop music
    public void StopMusic()
    {
        musicSource.Stop();
    }
}