using UnityEngine;

public class InteractBillboard : MonoBehaviour
{
    Camera cam;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get main camera
        cam = Camera.main;    
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        transform.forward = cam.transform.forward;
    }
}
