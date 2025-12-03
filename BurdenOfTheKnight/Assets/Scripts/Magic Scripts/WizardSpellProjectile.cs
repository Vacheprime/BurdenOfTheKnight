using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WizardSpellProjectile : MonoBehaviour
{
    public float speed = 15f;
    public float lifeTime = 4f;
    public float damage = 15f;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    public void Fire(Vector3 dir)
    {
        dir.Normalize();

        transform.rotation = Quaternion.LookRotation(dir);
        rb.linearVelocity = dir * speed;

        CancelInvoke();
        Invoke(nameof(Die), lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerManager.Instance != null)
                PlayerManager.Instance.TakeDamage(damage);
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
