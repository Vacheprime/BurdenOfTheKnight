using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FireballProjectile : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 15f;
    public float lifeTime = 4f;
    public float damage = 15f;
    public float knockbackForce = 15f;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    /// <summary>
    /// Launches the fireball in a given direction
    /// </summary>
    public void Fire(Vector3 dir)
    {
        dir.Normalize();

        transform.rotation = Quaternion.LookRotation(dir);
        rb.linearVelocity = dir * speed; // <-- fixed line

        CancelInvoke();
        Invoke(nameof(Die), lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Deal damage
            PlayerManager.Instance?.TakeDamage(damage);

            // Apply knockback if player has Rigidbody
            Rigidbody rbTarget = other.GetComponent<Rigidbody>();
            if (rbTarget != null)
            {
                Vector3 knockbackDir = (other.transform.position - transform.position).normalized;
                Vector3 knockback = knockbackDir * knockbackForce + Vector3.up * 2f;
                rbTarget.AddForce(knockback, ForceMode.Impulse);
            }

            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
