using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject pausePanel;
    public static bool isPaused = false;

    [Header("Pause Music")]
    public AudioSource pauseMusicSource;   // assign in Inspector
    public AudioClip pauseMusicClip;       // assign in Inspector

    void Start()
    {
        pausePanel.SetActive(false);

        if (pauseMusicSource != null)
        {
            pauseMusicSource.clip = pauseMusicClip;
            pauseMusicSource.loop = true;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            Pause();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            Resume();
        }
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // Play pause music
        if (pauseMusicSource != null && pauseMusicClip != null)
            pauseMusicSource.Play();
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        // Pause the pause music
        if (pauseMusicSource != null)
            pauseMusicSource.Pause();
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
