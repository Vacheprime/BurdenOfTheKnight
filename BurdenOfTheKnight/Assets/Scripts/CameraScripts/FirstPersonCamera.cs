using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    // Mouse sensitivity
    public float sensX;
    public float sensY;

    public GameObject player;

    private float xRotation;
    private float yRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void LateUpdate()
    {
        // get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate cam and orientation
        transform.parent.rotation = Quaternion.Euler(xRotation, yRotation, 0);
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
