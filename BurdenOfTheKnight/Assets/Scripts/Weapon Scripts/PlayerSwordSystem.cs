using Unity.VisualScripting;
using UnityEngine;

public class PlayerSwordSystem : MonoBehaviour
{
    public GameObject player;
    public GameObject playerCamera;
    public GameObject testTarget;
    private bool isInCombat = false;

    public void Update()
    {
        // Enter combat mode 
        if (Input.GetMouseButtonDown(0))
        {
            enterCombatMode();
        }

        // Exit combat mode
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            exitCombatMode();
        }

        // Execute combat logic if in combat
        if (isInCombat)
        {
            manageCombat();
        }
    }

    private void enterCombatMode()
    {
        // Get target
        Transform target = GetNextTarget();
        if (target == null) {
            return; // No target, 
        }

        // Start combat
        isInCombat = true;

        // Set camera mode to lock on
        CameraManager camManager = playerCamera.GetComponent<CameraManager>();
        camManager.SetMode(CameraMode.LockOn, target);
    }

    private void exitCombatMode()
    {
        // End combat
        isInCombat = false;

        // Set camera mode to First Person
        CameraManager camManager = playerCamera.GetComponent<CameraManager>();
        camManager.SetMode(CameraMode.FirstPerson);

    }

    private void manageCombat()
    {
        // Get cursor 
        return;
    }

    private Transform? GetNextTarget()
    {
        return testTarget.transform;
    }
}
