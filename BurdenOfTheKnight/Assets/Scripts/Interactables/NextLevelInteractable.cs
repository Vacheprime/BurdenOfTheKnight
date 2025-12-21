using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelInteractable : MonoBehaviour, IInteractable
{
    public string nextSceneName;

    public void Interact(GameObject interactor)
    {
        if (GetEnemyCount() != 0)
        {
            return;
        }

        SceneManager.LoadScene(nextSceneName);
    }

    private int GetEnemyCount()
    {
        return GameObject.FindGameObjectsWithTag("Target").Length;
    }
}
