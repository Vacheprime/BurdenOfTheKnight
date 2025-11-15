using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WizardSpellProjectile : MonoBehaviour
{
    public float speed    = 15f;
    public float lifeTime = 4f;
    public float damage   = 15f;

    private Rigidbody rb;

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
        transform.forward = dir;

        rb.linearVelocity = dir * speed;

        CancelInvoke();
        Invoke(nameof(Die), lifeTime);
    }

    //Commented out for testing purposes
    // void OnTriggerEnter(Collider other)
    // {
    //     var hp = other.GetComponent<PlayerHealth>();
    //     if (hp != null)
    //     {
    //         hp.TakeDamage(damage);
    //     }

    //     Die();
    // }

    void Die()
    {
        Destroy(gameObject);
    }
}
