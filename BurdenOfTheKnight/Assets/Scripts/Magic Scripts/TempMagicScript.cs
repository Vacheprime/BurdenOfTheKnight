using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMagicScript : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            if (collision.gameObject.GetComponent<TargetScript>() != null) // only used because this is not a prefab and is a testing object
            {
                TargetScript target = collision.gameObject.GetComponent<TargetScript>();
                target.Damage();
            }
            Destroy(this.gameObject);
        }
    }
}
