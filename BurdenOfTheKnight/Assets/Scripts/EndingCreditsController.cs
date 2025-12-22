using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class EndingCreditsController : MonoBehaviour
{
    public float scrollSpeed = 40f;
    public RectTransform endingText;
    public NewBehaviourScript darkEffect;
    private bool isDark = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (endingText.anchoredPosition.y < 1339.820f)
        {
            Debug.Log(endingText.anchoredPosition.y);
            endingText.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
        }
        else if (endingText.anchoredPosition.y >= 1339.594f && !isDark)
        {
            isDark = true;
            darkEffect.DarkenScreenEnding();
            Cursor.lockState = CursorLockMode.None;
        }
    }
    
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
