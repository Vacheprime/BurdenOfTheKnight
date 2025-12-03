using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSwordSystem : MonoBehaviour
{
    public GameObject player;
    public GameObject playerCamera;

    public float attackRange = 4f;
    public float attackDamage = 40f;

    private Animator swordAnimator;
    private bool isInCombat = false;
    private GameObject currentTarget = null;

    private CursorPath cursorPath = new CursorPath();

    private double sampleInterval = 0.03f; // 10 ms sample time
    private double nextSampleTime = 0f;
    private int requiredSwipeStrength = 90000;

    public AudioSource swordAudioSource;
    public AudioClip swordSwingClip;


    public void Start()
    {
        swordAnimator = GetComponent<Animator>();
    }

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
        if (target == null)
        {
            return; // No target, 
        }
        // Set current target
        currentTarget = target.GameObject();

        // Start combat
        isInCombat = true;
        MusicManager.Instance.PlayCombatMusic();


        // Set camera mode to lock on
        CameraManager camManager = playerCamera.GetComponent<CameraManager>();
        camManager.SetMode(CameraMode.LockOn, target);
    }

    private void exitCombatMode()
    {
        // End combat
        isInCombat = false;
        MusicManager.Instance.PlayExplorationMusic();


        // Set camera mode to First Person
        CameraManager camManager = playerCamera.GetComponent<CameraManager>();
        camManager.SetMode(CameraMode.FirstPerson);

        // Reset values
        currentTarget = null;
        cursorPath.ClearCursorPoints();
    }

    private void manageCombat()
    {
        // Swap targets if requested
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Transform target = GetNextTarget();
            if (target != null)
            {
                currentTarget = target.GameObject();
                CameraManager cameraManager = playerCamera.GetComponent<CameraManager>();
                cameraManager.SetMode(CameraMode.LockOn, target);
            }
        }

        // Get the mouse X and Y position
        Vector2 mousePosition = Input.mousePosition;

        // Register attack positions

        if (Input.GetMouseButtonDown(0))
        {
            cursorPath.AddCursorCoordinate(mousePosition, Time.unscaledTimeAsDouble);
        }

        if (Input.GetMouseButton(0))
        {
            // Register event only if enough time has passed.
            if (Time.unscaledTimeAsDouble > nextSampleTime)
            {
                // Register the coordinate only if it is different than the last one
                Vector2 lastPosition = cursorPath.GetLastCursorPos();

                // Skip if mouse did not move
                if (lastPosition == mousePosition)
                {
                    return;
                }

                cursorPath.AddCursorCoordinate(mousePosition, Time.timeAsDouble);

                nextSampleTime = Time.unscaledTimeAsDouble + sampleInterval;
            }
        }

        // Execute attack
        if (Input.GetMouseButtonUp(0))
        {
            // Execute attack if all points are filled
            if (!cursorPath.IsFilled())
            {
                // Clear cursor paths
                cursorPath.ClearCursorPoints();
                return;
            }

            // Get attack direction
            string attackDirection = CheckMouseMovement();
            if (attackDirection == null)
            {
                // No attack
                cursorPath.ClearCursorPoints();
                return;
            }

            Attack(attackDirection);
        }
    }

    private void Attack(string attackDirection)
    {
        Dictionary<string, string> directionToAnimation = new Dictionary<string, string>
        {
            { "LEFT", "SlashLeft" },
            { "RIGHT", "SlashRight" },
            { "UP", "SlashUp" },
            { "DOWN", "SlashDown" }
        };

        // Play attack animation based on attack direction
        swordAnimator.SetTrigger(directionToAnimation[attackDirection]);
        swordAudioSource.PlayOneShot(swordSwingClip);

        // Attack the enemy if in range
        if (Vector3.Distance(player.transform.position, currentTarget.transform.position) <= attackRange)
        {
            // Get the damageable interface for target
            IDamageable targetHealth = currentTarget.GetComponent<IDamageable>();
            if (targetHealth != null)
            {
                // Get next target
                Transform target = GetNextTarget();
                // Damage
                bool hasDied = targetHealth.TakeDamage(attackDamage);
                if (hasDied)
                {

                    CameraManager cameraManager = playerCamera.GetComponent<CameraManager>();
                    if (target == null)
                    {
                        cameraManager.SetMode(CameraMode.FirstPerson);
                        currentTarget = null;
                        isInCombat = false;
                        return;
                    }
                    currentTarget = target.GameObject();
                    cameraManager.SetMode(CameraMode.LockOn, target);
                }
            }
            else
            {
                Debug.LogWarning("The gameObject tagged with the enemy tag does not have a health component implementing the IDamageable interface.");
            }
        }
    }

    private string CheckMouseMovement()
    {
        (string direction, float strength) swipeData = cursorPath.GetSwipeData();

        // Attempt to compensate strength
        if (swipeData.direction == "DOWN" || swipeData.direction == "UP")
        {
            swipeData.strength *= 2;
        }

        // Check if attack was done
        return swipeData.strength >= requiredSwipeStrength ? swipeData.direction : null;
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
            }
            else if (distanceB < distanceA)
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
            if (targets.Count == 1)
            {
                return null;
            }

            // If more than one, filter by which isn't already selected.
            nextTarget = targets.Where(target => target != currentTarget).FirstOrDefault();
        }

        if (nextTarget == null)
        {
            return null;
        }

        // Return nearest to center
        return nextTarget.transform;
    }

    public void OnDisable()
    {
        exitCombatMode();
    }
}

