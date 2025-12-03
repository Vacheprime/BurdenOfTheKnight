using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform player;
    public float maxDistance = 10.0f;
    public bool isNear;
    public float openAngle = 90f;  
    public float speed = 3f;  
    public KeyCode interactKey = KeyCode.E;

    private bool isOpen = false;
    private float currentAngle = 0f;

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        isNear = distance <= maxDistance;

        if (Input.GetKeyDown(interactKey) && isNear)
        {
            isOpen = !isOpen;
        }

        float targetAngle = isOpen ? openAngle : 0f;
        currentAngle = Mathf.Lerp(currentAngle, targetAngle, Time.deltaTime * speed);

        transform.localRotation = Quaternion.Euler(0f, currentAngle, 0f);
    }
}
