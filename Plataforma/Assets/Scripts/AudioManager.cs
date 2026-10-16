using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private const float MusicVolume = 0.55f;
    private const float SfxVolume = 0.9f;

    private static AudioManager instance;

    private readonly Dictionary<string, AudioClip> clipCache = new();
    private AudioSource musicSource;
    private AudioSource sfxSource;

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject audioManagerObject = new GameObject("AudioManager");
                instance = audioManagerObject.AddComponent<AudioManager>();
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource = GetComponent<AudioSource>();
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }

        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = MusicVolume;

        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources.Length < 2)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            sfxSource = audioSources[1];
        }

        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        sfxSource.volume = SfxVolume;

        SceneManager.sceneLoaded += OnSceneLoaded;
        UpdateMusicForCurrentScene();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (instance == this)
        {
            instance = null;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateMusicForCurrentScene();
    }

    public static void PlayMusic(string clipName)
    {
        if (instance == null)
        {
            _ = Instance;
        }

        if (instance == null)
        {
            return;
        }

        instance.PlayMusicClip(clipName);
    }

    public static void PlaySfx(string clipName)
    {
        if (instance == null)
        {
            _ = Instance;
        }

        if (instance == null)
        {
            return;
        }

        instance.PlaySoundEffect(clipName);
    }

    private void UpdateMusicForCurrentScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.Contains("Fase2"))
        {
            PlayMusicClip("Fase2");
            return;
        }

        PlayMusicClip("Fase1");
    }

    private void PlayMusicClip(string clipName)
    {
        AudioClip clip = LoadClip(clipName);
        if (clip == null)
        {
            return;
        }

        if (musicSource.clip == clip && musicSource.isPlaying)
        {
            return;
        }

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    private void PlaySoundEffect(string clipName)
    {
        AudioClip clip = LoadClip(clipName);
        if (clip == null)
        {
            return;
        }

        sfxSource.PlayOneShot(clip);
    }

    private AudioClip LoadClip(string clipName)
    {
        if (string.IsNullOrWhiteSpace(clipName))
        {
            return null;
        }

        if (clipCache.TryGetValue(clipName, out AudioClip cachedClip) && cachedClip != null)
        {
            return cachedClip;
        }

        AudioClip loadedClip = Resources.Load<AudioClip>($"Audio/{clipName}");
        if (loadedClip == null)
        {
            loadedClip = Resources.Load<AudioClip>(clipName);
        }

        if (loadedClip != null)
        {
            clipCache[clipName] = loadedClip;
        }
        else
        {
            Debug.LogWarning($"Audio '{clipName}' nao foi encontrado em Assets/Resources/Audio ou Resources/.", this);
        }

        return loadedClip;
    }
}
