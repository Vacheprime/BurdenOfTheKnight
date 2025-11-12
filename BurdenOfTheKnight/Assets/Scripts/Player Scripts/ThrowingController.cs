using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingController : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;

    [Header("Settings")]
    public int totalThrows;
    public float throwCooldown;

    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.Mouse0;
    public float throwForce;
    public float throwUpwardForce;

    bool readyToThrow;
    bool enoughMagicPoints;

    private void Start()
    {
        readyToThrow = true;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(throwKey) && readyToThrow && totalThrows > 0)
        {
            enoughMagicPoints = PlayerManager.Instance.CastTestMagic();
            if (enoughMagicPoints == true)
            {
                Throw();
            }
        }
    }

    private void Throw()
    {
        readyToThrow = false;

        // instantiate object to throw
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

        // get rigidbody component
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // calculate direction
        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;

        if(Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        // add force
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce; 
        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows--;
        Destroy(projectile, 3f); // Delay of 3 seconds

        // Invoke throw cooldown
        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }

}
