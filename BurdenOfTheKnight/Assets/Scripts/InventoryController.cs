using UnityEngine;
using UnityEngine.UI;
public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel;
    private bool isPaused = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && isPaused == false)
        {
            isPaused = true;
            Time.timeScale = 0f;
            inventoryPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
        } else if (Input.GetKeyDown(KeyCode.Tab) && isPaused == true)
        {
            isPaused = false;
            Time.timeScale = 1f;
            inventoryPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
        }
    }
}
