using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {get; private set;}

    [Header ("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource voiceAudioSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip sparksSFX;
    public AudioClip switchSFX;
    public AudioClip mainMenuSFX;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if(backgroundMusic != null && musicSource != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip, bool loop = false)
    {
        if(clip != null && sfxSource != null)
        {
            if(loop)
            {
                sfxSource.clip = clip;
                sfxSource.loop = true;
                sfxSource.volume = 0.5f;
                sfxSource.Play();
            }
            sfxSource.PlayOneShot(clip, 1.0f);
        }
    }

    public void PlayButtonSFX(AudioClip clip)
    {
        if(clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if(clip != null && musicSource != null)
        {
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void SetSFXLoop(bool shouldLoop)
    {
        if (sfxSource != null)
        {
            sfxSource.loop = shouldLoop;
        }
    }

    public void StopSFX()
    {
        if(sfxSource != null)
        {
            sfxSource.Stop();
            sfxSource.loop = false;
        }
    }

    public void PlayVoice(AudioClip clip, float basePitch)
    {
        if (clip != null && voiceAudioSource != null)
        {
            voiceAudioSource.pitch = basePitch + Random.Range(-0.1f, 0.1f); 
            voiceAudioSource.PlayOneShot(clip);
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null) musicSource.volume = Mathf.Clamp01(volume);
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null) sfxSource.volume = Mathf.Clamp01(volume);
    }
}
