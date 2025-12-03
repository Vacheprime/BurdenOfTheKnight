using UnityEngine;

public class AudioManagerObject : MonoBehaviour
{
    private static GameObject instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    void Awake()
    {
        if (instance != null && instance != gameObject)
        {
            Destroy(gameObject);
            return;
        }

        instance = gameObject;
        DontDestroyOnLoad(gameObject);
    }
}
