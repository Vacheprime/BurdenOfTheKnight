using UnityEngine;

public class Interactor : MonoBehaviour
{
    public Camera playerCamera;
    public float interactionRadius = 0.25f;
    public float interactionDistance = 5f;
    private LayerMask interactionMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactionMask = LayerMask.GetMask("Interactable");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit? hit = DetectInteractableObject();
         
            // Ignore if no hit
            if (hit == null)
            {
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
}
