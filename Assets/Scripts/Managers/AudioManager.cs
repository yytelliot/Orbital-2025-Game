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
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        sfxSource.PlayOneShot(clip, volume);
    }

    // Start/switch BGM
    public void PlayMusic(AudioClip clip, float volume = 1f)
    {
        if (musicSource.clip == clip) return;  // already playing
        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.Play();
    }

    // Stop music
    public void StopMusic()
    {
        musicSource.Stop();
    }
}