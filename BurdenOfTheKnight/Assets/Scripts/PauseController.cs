using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject settingsPanel;
    public GameObject hudPanel;
    public static bool isPaused = false;

    private CursorLockMode previousLockMode = CursorLockMode.None;
    private bool previousVisibility = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            Pause();
        } else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
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

        // Set the cursor states
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OpenSettings()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void Resume()
    {
        // Resume the game
        pausePanel.SetActive(false);
        hudPanel.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;

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
}
