using Unity.VisualScripting;
using UnityEngine;

public class MenuAudioManager : MonoBehaviour
{
    public static MenuAudioManager Instance {get; private set;}

    [Header ("Audio Sources")]
    public AudioSource menuSound;
    public AudioSource menuSFX;

    [Header("Audio CLips")]
    public AudioClip menuMusic;
    public AudioClip buttonsSFX;

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

    void Start()
    {
        if(menuMusic != null && menuSound != null)
        {
            menuSound.clip = menuMusic;
            menuSound.loop = true;
            menuSound.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if(clip != null && menuSFX != null)
        {
            menuSFX.PlayOneShot(clip);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if(clip != null && menuSound != null)
        {
            menuSound.clip = clip;
            menuSound.loop = true;
            menuSound.Play();
        }
    }
}
