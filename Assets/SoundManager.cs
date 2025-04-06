using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("Audio Clips")]
    [SerializeField] AudioClip backgroundMusic;
    [SerializeField] AudioClip _introMusic;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayMusic(backgroundMusic);
    }

    private void PlayIntroMusic()
    {
        if (_introMusic != null)
        {
            musicSource.clip = _introMusic;
            musicSource.loop = false;
            musicSource.Play();
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void PlayLoopingSFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.clip = clip;
        sfxSource.loop = true;
        sfxSource.Play();
    }

    public void StopLoopingSFX()
    {
        sfxSource.Stop();
        sfxSource.loop = false;
    }

    public void StopAllSounds()
    {
        musicSource.Stop();
        sfxSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
