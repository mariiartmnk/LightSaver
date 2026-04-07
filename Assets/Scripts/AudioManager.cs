using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {get; private set;}

    [Header ("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio CLips")]
    public AudioClip backgroundMusic;
    public AudioClip sparksSFX;
    public AudioClip switchSFX;
    public AudioClip mainMenuSFX;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
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
            sfxSource.PlayOneShot(clip, 0.05f);
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
}
