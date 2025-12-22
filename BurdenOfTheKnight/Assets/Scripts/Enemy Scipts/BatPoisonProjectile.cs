using UnityEngine;

public class BatProjectile : MonoBehaviour
{
    public float damage = 10f;

    void OnTriggerEnter(Collider other)
    {
        HandleHit(other.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        HandleHit(collision.collider.gameObject);
    }

    void HandleHit(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerManager.Instance != null)
            {
                PlayerManager.Instance.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
        else if (!other.CompareTag("Target"))
        {
            Destroy(gameObject);
        }
    }
}
