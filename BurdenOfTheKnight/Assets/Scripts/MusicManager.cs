using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    public AudioSource musicSource;
    public AudioClip explorationMusic;
    public AudioClip combatMusic;
    public float fadeDuration = 1f;

    private AudioClip currentClip;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;

        DontDestroyOnLoad(gameObject); // keep music across scenes
    }

    private void Start()
    {
        PlayMusic(explorationMusic);
    }

    public void PlayExplorationMusic()
    {
        PlayMusic(explorationMusic);
    }

    public void PlayCombatMusic()
    {
        PlayMusic(combatMusic);
    }

    private void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return;

        currentClip = clip;
        StopAllCoroutines();
        StartCoroutine(FadeToClip(clip));
    }

    private System.Collections.IEnumerator FadeToClip(AudioClip clip)
    {
        float startVolume = musicSource.volume;

        // Fade out
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        musicSource.clip = clip;
        musicSource.Play();

        // Fade in
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
            yield return null;
        }

        musicSource.volume = startVolume;
    }
}
