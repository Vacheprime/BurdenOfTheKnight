using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    // Mouse sensitivity
    public float sensX;
    public float sensY;

    public Transform playerTransform;

    private float xRotation;
    private float yRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        // Read mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * sensX * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensY * Time.deltaTime;

        // Calculate rotation
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        yRotation += mouseX;

        // Apply rotation to the camera
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, transform.localEulerAngles.z);

        // Calculate player rotation
        Vector3 forward = transform.forward;
        forward.y = 0f;

        if (forward.sqrMagnitude > 0.0001f)
        {
            Quaternion lookYaw = Quaternion.LookRotation(forward);
            // apply only Y rotation to player
            playerTransform.rotation = Quaternion.Euler(0f, lookYaw.eulerAngles.y, 0f);
        }
    }

    public void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // Set the xRotation and yRotation to the current camera transform
        Vector3 cameraAngles = transform.eulerAngles;

        // Convert angles properly
        xRotation = cameraAngles.x;
        if (xRotation > 180)
        {
            xRotation -= 360;
        }
        yRotation = cameraAngles.y;
    }
}
