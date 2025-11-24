using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private FirstPersonCamera fpsCameraController;
    private LockOnCameraController lockOnCameraController;

    void Start()
    {
        
        // Get the camera controllers
        fpsCameraController = GetComponent<FirstPersonCamera>();
        lockOnCameraController = GetComponent<LockOnCameraController>();

        // Set default camera mode
        SetMode(CameraMode.FirstPerson);
    }

    public void SetMode(CameraMode mode, Transform target = null)
    {
        // Set the camera modes
        if (mode == CameraMode.FirstPerson) {
            // Enable fps, disable lockon
            fpsCameraController.enabled = true;
            lockOnCameraController.enabled = false;
        } 
        else if (mode == CameraMode.LockOn)
        {
            // Disable fps cam controller
            fpsCameraController.enabled = false;

            // Set the target and enable the lock on camera
            lockOnCameraController.SetTarget(target);
            lockOnCameraController.enabled = true;
        }
    }
}

public enum CameraMode
{
    FirstPerson,
    LockOn
}
