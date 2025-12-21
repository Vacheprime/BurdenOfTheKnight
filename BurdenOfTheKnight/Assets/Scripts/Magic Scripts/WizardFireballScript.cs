using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FireballProjectile : MonoBehaviour
{
    public float speed = 15f;
    public float lifeTime = 4f;
    public float damage = 15f;
    public float knockbackForce = 15f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.isKinematic = false;
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
        if (!other.CompareTag("Player")) return;

        PlayerManager.Instance?.TakeDamage(damage);

        Rigidbody rbTarget = other.GetComponent<Rigidbody>();
        if (rbTarget != null)
        {
            Vector3 knockbackDir = (other.transform.position - transform.position).normalized;
            Vector3 knockback = knockbackDir * knockbackForce + Vector3.up * 2f;
            rbTarget.AddForce(knockback, ForceMode.Impulse);
        }

        Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
