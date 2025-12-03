using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMagicScript : MonoBehaviour
{
    public float attackDamage = 30;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

            if (damageable == null) 
            {
                Debug.LogWarning("The gameObject tagged with the enemy tag does not have a health component implementing the IDamageable interface.");
            }
            damageable.TakeDamage(attackDamage);

            Destroy(this.gameObject);
        }
    }
}
