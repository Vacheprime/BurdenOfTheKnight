using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void MainMenu()
    {
        SceneManager.LoadScene(PreviousScene.previousSceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
