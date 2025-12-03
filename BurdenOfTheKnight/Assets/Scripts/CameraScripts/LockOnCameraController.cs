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
        RotateToTarget();
    }

    private void RotateToTarget()
    {
        // Create a modified target transform to block X and Z rotations
        Vector3 modTargetPos = target.position;
        modTargetPos.y = player.transform.position.y;

        player.transform.LookAt(modTargetPos);
        transform.LookAt(target);
    }

    public void SetTarget(Transform target)
    {
        // Get the actual target if possible
        Transform swordTarget = target.Find("SwordTarget");
        if (swordTarget != null)
        {
            target = swordTarget;
        }

        this.target = target;
    }

    public void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
