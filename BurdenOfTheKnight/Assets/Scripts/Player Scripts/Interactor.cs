using UnityEngine;

public class Interactor : MonoBehaviour
{
    public Camera playerCamera;
    public float interactionRadius = 0.25f;
    public float interactionDistance = 5f;
    public float surfaceOffset = 0.05f;
    private LayerMask interactionMask;
    public GameObject promptPrefab;

    private GameObject promptInstance;
    IInteractable currentTarget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactionMask = LayerMask.GetMask("Interactable");
        // Instantiate the E prompt
        promptInstance = Instantiate(promptPrefab);
        promptInstance.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Get hit
        RaycastHit? hit = DetectInteractableObject();
        
        // Ignore if no hit
        if (hit == null)
        {
            ClearTarget();
            return;
        }

        // Get the object
        GameObject hitObject = hit?.collider.gameObject;

        // Attempt to get the interactable
        IInteractable interactable = hitObject.GetComponent<IInteractable>();
        if (interactable == null)
        {
            Debug.LogWarning("Game object is tagged as Interactable, but does not implement the IInteractable interface.");
            return;
        }

        // Display interact prompt
        currentTarget = interactable;
        ShowPrompt(hit.Value.point + hit.Value.normal * surfaceOffset);


        if (Input.GetKeyDown(KeyCode.E))
        {
            // Interact by giving a reference to this game object (interactor)
            interactable.Interact(gameObject);
        }
    }

    private RaycastHit? DetectInteractableObject()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.yellow);
        // Return the hit if any
        if (Physics.SphereCast(ray, interactionRadius, out RaycastHit hit, interactionDistance, interactionMask)) {
            Debug.Log("HIT");
            return hit;
        }

        // Return null if none
        return null;
    }

    void ShowPrompt(Vector3 worldPos)
    {
        promptInstance.transform.position = worldPos;
        promptInstance.SetActive(true);
    }

    void ClearTarget()
    {
        currentTarget = null;
        promptInstance.SetActive(false);
    }
}
