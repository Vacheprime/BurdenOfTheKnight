using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ArrowProjectile : MonoBehaviour
{
    public float speed = 30f;
    public float lifeTime = 5f;
    public float damage = 10f;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    // Call this immediately after Instantiate
    public void Fire(Vector3 dir)
    {
        dir.Normalize();
        transform.forward = dir;      // make Z+ face travel direction
        rb.linearVelocity = dir * speed;    // physics-driven travel
        Invoke(nameof(Die), lifeTime);
    }

    // void OnTriggerEnter(Collider other)
    // {
    //     // Example: apply damage to player
    //     var hp = other.GetComponent<PlayerHealth>();
    //     if (hp) hp.TakeDamage(damage);
    //     Die();
    // }

    void Die() => Destroy(gameObject);
}
