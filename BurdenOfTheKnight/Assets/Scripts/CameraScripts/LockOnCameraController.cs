using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LockOnCameraController : MonoBehaviour
{
    public GameObject player;
    
    private Transform target;
    private int rotateSpeed = 360; // Degres per second

    // Update is called once per frame
    void LateUpdate()
    {
        RotateCameraToTarget();
    }

    private void RotateCameraToTarget()
    {
        transform.parent.LookAt(target);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
