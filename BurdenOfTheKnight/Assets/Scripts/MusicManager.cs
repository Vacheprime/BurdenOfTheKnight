using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public AudioSource musicSource;
    public AudioClip explorationMusic;
    public AudioClip combatMusic;
    public float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);   // remove duplicate safely
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();  // prevents MissingReferenceException
    }

    public void PlayExplorationMusic() => PlayMusic(explorationMusic);
    public void PlayCombatMusic() => PlayMusic(combatMusic);

    private void PlayMusic(AudioClip clip)
    {
        if (musicSource == null || musicSource.clip == clip)
            return;

        StopAllCoroutines();
        StartCoroutine(FadeToClip(clip));
    }

    private System.Collections.IEnumerator FadeToClip(AudioClip clip)
    {
        if (musicSource == null) yield break;

        float startVol = musicSource.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            if (musicSource == null) yield break;
            musicSource.volume = Mathf.Lerp(startVol, 0, t / fadeDuration);
            yield return null;
        }

        musicSource.clip = clip;
        musicSource.Play();

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            if (musicSource == null) yield break;
            musicSource.volume = Mathf.Lerp(0, startVol, t / fadeDuration);
            yield return null;
        }

        musicSource.volume = startVol;
    }
}
