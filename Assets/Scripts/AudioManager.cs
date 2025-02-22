using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonBehaviour<AudioManager>
{

    [Header("AudioSourcePrefab")]
    [SerializeField] private AudioSource audioSourcePrefab;

    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] private List<AudioSource> audioSources = new List<AudioSource>();
    //[SerializeField] AudioSource boxCrashingSource;

    [Header("Audio Clip")]
    [SerializeField] AudioClip backgroundMusic;
    [SerializeField] AudioClip boxCrashing;
    [SerializeField] AudioClip characterFalling;
    [SerializeField] AudioClip walkingSound;
    [SerializeField] AudioClip characterRespawnSound;
    [SerializeField] AudioClip CoinCollectSound;
    [SerializeField] AudioClip DropCoinSound;

    // Track when the box crashing sound can be played again
    private float nextBoxCrashingTime = 0f;

    private Dictionary<AudioClip, AudioSource> soundToSourceMap = new Dictionary<AudioClip, AudioSource>();


    private void Start()
    {
        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        AudioSource source = GetOrCreateAudioSource();
        if (source != null)
        {
            source.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Failed to create a new audio source.");
        }
    }

    private AudioSource GetOrCreateAudioSource()
    {
        foreach (var source in audioSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        if (audioSourcePrefab != null)
        {
            AudioSource newSource = Instantiate(audioSourcePrefab, transform);
            audioSources.Add(newSource);
            // Debug2.Log("Instantiated a new AudioSource.");
            return newSource;
        }

        Debug.LogWarning("AudioSource prefab is not assigned.");
        return null;
    }

    public bool IsSoundPlaying(AudioClip clip)
    {
        foreach (var source in audioSources)
        {
            // Check if the source is playing the specific clip
            if (source.isPlaying && source.clip == clip)
            {

                return true; // The clip is currently playing
            }
        }
        return false; // No source is playing the clip
    }

    public void StopBoxCrashingSound()
    {
        foreach (var source in audioSources)
        {
            if (source.clip == boxCrashing && source.isPlaying)
            {
                source.Stop();  // Stop the sound
                source.loop = false;  // Disable looping
            }
        }
    }

    public void PlayBoxCrashingSound()
    {

        AudioSource source = GetOrCreateAudioSource();
        if (source != null && !IsSoundPlaying(boxCrashing))
        {
            source.loop = true; // Set the source to loop
            source.clip = boxCrashing; // Assign the clip to the source
            source.Play(); // Start playing the sound 
                           // Calculate the next time this sound can be played based on the current time and the length of the clip
            nextBoxCrashingTime = Time.time + boxCrashing.length - 1f;
        }
        else
        {
            Debug.LogWarning("Failed to create a new audio source.");
        }

    }

    public void PlayCharacterFallSound()
    {
        PlaySound(characterFalling);
    }

    public void PlayWalkingSound()
    {
        PlaySound(walkingSound);
    }

    public void PlayCharacterRespawnSound()
    {
        PlaySound(characterRespawnSound);
    }

    public void PlayCoinCollectSound()
    {
        PlaySound(CoinCollectSound);
    }

    public void PlayDropCoinSound()
    {
        PlaySound(DropCoinSound);
    }
}