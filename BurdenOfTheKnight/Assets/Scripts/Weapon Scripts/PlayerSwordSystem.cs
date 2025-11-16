using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSwordSystem : MonoBehaviour
{
    public GameObject player;
    public GameObject playerCamera;
    private bool isInCombat = false;
    private GameObject currentTarget = null;


    private Vector2 prevMousePosition;

    public float MouseRequiredDistance = 200;
    public float MouseRequiredVelocity = 10;
    public float MiddleZoneDimension = 100;

    public void Update()
    {
        // Enter combat mode 
        if (Input.GetMouseButtonDown(0) && !isInCombat)
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

        // Reset values
        currentTarget = null;
        prevMousePosition = new Vector2(Screen.width / 2, Screen.height / 2);
    }

    private void manageCombat()
    {
        // Swap targets if requested
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Transform target = GetNextTarget();
            if (target != null)
            {
                CameraManager cameraManager = playerCamera.GetComponent<CameraManager>();
                cameraManager.SetMode(CameraMode.LockOn, target);
            }
        }

        // Get the mouse X and Y position
        Vector2 mousePosition = Input.mousePosition;

        if (CheckMouseMovement(mousePosition))
        {
            Debug.Log("SHOULD ATTACK");
            // Execute attack logic
        }

        prevMousePosition = mousePosition;
    }

    private bool CheckMouseMovement(Vector2 mousePosition)
    {
        
        // Check if first frame of combat mode
        if (prevMousePosition.x == 0 && prevMousePosition.y == 0)
        {
            return false;
        }

        // Calulate mouse velocity and distance
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

        Vector2 delta = mousePosition - prevMousePosition;

        float mouseTravelDistance = delta.magnitude;

        // Normalize by screen size to remove diagonal/resolution bias
        float normDx = delta.x / Screen.width;
        float normDy = delta.y / Screen.height;

        float normalizedDistance = Mathf.Sqrt(normDx * normDx + normDy * normDy);

        float mouseVelocity = normalizedDistance / Time.deltaTime;

        if (prevMousePosition != mousePosition)
        {
            Debug.Log($"Prev: {prevMousePosition}; Curr: {mousePosition}");
        }
        
        
        // Check if mouse is outside reset box
        if (prevMousePosition.x > screenCenter.x - MiddleZoneDimension &&
            prevMousePosition.x < screenCenter.x + MiddleZoneDimension)
        {
            return false;
        }

        if (prevMousePosition.y > screenCenter.y - MiddleZoneDimension &&
            prevMousePosition.y < screenCenter.y + MiddleZoneDimension)
        {
            return false;
        }

        if (mouseVelocity < MouseRequiredVelocity || mouseTravelDistance < MouseRequiredDistance)
        {
            return false;
        }
        return true;
    }

    private List<GameObject> GetTargetsInfront(float maxDistance, string tag, float maxAngle = -1)
    {
        List<GameObject> result = new List<GameObject>();

        // Overlap sphere to find candidates
        Collider[] hits = Physics.OverlapSphere(Camera.main.transform.position, maxDistance);
        Transform cam = transform;
        Vector3 camPos = cam.position;
        Vector3 camForward = cam.forward;

        foreach (var hit in hits)
        {
            GameObject obj = hit.gameObject;

            // Filter by tag
            if (!obj.CompareTag(tag))
                continue;

            if (maxAngle != -1)
            {
                // Check if it's in front of camera (cone check)
                Vector3 toObj = (obj.transform.position - camPos).normalized;
                float angle = Vector3.Angle(camForward, toObj);

                if (angle > maxAngle)
                    continue;
            }
            result.Add(obj);
        }

        // Sort by screen distance to center
        result.Sort((a, b) =>
        {
            // Sort by distance to camera
            float distanceA = Vector3.Distance(camPos, a.transform.position);
            float distanceB = Vector3.Distance(camPos, b.transform.position);
            // Get the closest target between the two
            GameObject closestTarget = null;
            float closestDistance = 0;

            if (distanceA < distanceB)
            {
                closestTarget = a;
                closestDistance = distanceA;
            } else if (distanceB < distanceA)
            {
                closestTarget = b;
                closestDistance = distanceB;
            }
            
            if (closestTarget != null)
            {
                if (closestDistance < 3)
                {
                    return closestTarget == a ? -1 : 1;
                }
            }

            Vector3 aScreen = Camera.main.WorldToScreenPoint(a.transform.position);
            Vector3 bScreen = Camera.main.WorldToScreenPoint(b.transform.position);

            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

            float aDist = Vector2.Distance(aScreen, screenCenter);
            float bDist = Vector2.Distance(bScreen, screenCenter);

            return aDist.CompareTo(bDist);  // closest to center = first
        });

        return result;
    }

    private Transform GetNextTarget()
    {
        GameObject nextTarget = null;

        if (!isInCombat)
        {
            // Get all targets
            List<GameObject> targets = GetTargetsInfront(10, "Target", 45);

            if (targets.Count != 0)
            {
                nextTarget = targets[0];
            }
        }
        else
        {
            // Get all targets (no angle restriction)
            List<GameObject> targets = GetTargetsInfront(10, "Target");

            // If only one target, then its already selected
            if (targets.Count == 1) {
                return null;
            }

            // If more than one, filter by which isn't already selected.
            nextTarget = targets.Where(target => target != currentTarget).FirstOrDefault();
        }

        if (nextTarget == null)
        {
            return null;
        }

        // Set current target
        currentTarget = nextTarget;

        // Return nearest to center
        return nextTarget.transform;
    }
}
