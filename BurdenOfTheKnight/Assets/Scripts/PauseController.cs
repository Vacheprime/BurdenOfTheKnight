using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class PauseController : MonoBehaviour
{
    public GameObject canvas;
    public GameObject pausePanel;
    public GameObject settingsPanel;
    public GameObject controlsCenter;
    public GameObject micCenter;
    public GameObject hudPanel;
    public static bool isPaused = false;
    public Button controlsButton;

    [Header("Pause Music")]
    public AudioSource pauseMusicSource;   // assign in Inspector
    public AudioClip pauseMusicClip;       // assign in Inspector

    private CursorLockMode previousLockMode = CursorLockMode.None;
    private bool previousVisibility = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        // Set the previous lock mode and visibility
        previousLockMode = Cursor.lockState;
        previousVisibility = Cursor.visible;

        // Set the pause panel to active and pause the game.
        pausePanel.SetActive(true);
        hudPanel.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;

        // Play pause music
        if (pauseMusicSource != null && pauseMusicClip != null)
            pauseMusicSource.Play();
        // Set the cursor states
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OpenSettings()
    {
        canvas.SetActive(true);
        pausePanel.SetActive(false);

        EventSystem.current.SetSelectedGameObject(controlsButton.gameObject);
        ControlsCenterButton();
    }

    public void CloseSettings()
    {
        canvas.SetActive(false);

        pausePanel.SetActive(true);
    }

    public void OpenMicSettings()
    {
        controlsCenter.SetActive(false);
        micCenter.SetActive(true);
    }

    public void ControlsCenterButton()
    {
        controlsCenter.SetActive(true);
        micCenter.SetActive(false);
    }

    public void Resume()
    {
        // Resume the game
        pausePanel.SetActive(false);
        hudPanel.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;

        // Pause the pause music
        if (pauseMusicSource != null)
            pauseMusicSource.Pause();
        // Put back the cursor to previous state.
        Cursor.visible = previousVisibility;
        Cursor.lockState = previousLockMode;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        // Set the mouse locks and set visible
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
