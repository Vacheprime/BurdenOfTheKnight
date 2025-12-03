using UnityEngine;

public class DoorController : MonoBehaviour
{
    public float openAngle = 90f;  
    public float speed = 3f;  
    public KeyCode interactKey = KeyCode.E;

    private bool isOpen = false;
    private float currentAngle = 0f;

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            isOpen = !isOpen;
        }

        float targetAngle = isOpen ? openAngle : 0f;
        currentAngle = Mathf.Lerp(currentAngle, targetAngle, Time.deltaTime * speed);

        transform.localRotation = Quaternion.Euler(0f, currentAngle, 0f);
    }
}
