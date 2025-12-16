using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FireMagicScript : MonoBehaviour
{
    [Header("Stats")]
    public float attackDamage = 20f;
    public float knockbackForce = 10f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clip;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
            {
                // Deal damage
                damageable.TakeDamage(attackDamage);

                // Apply knockback if Rigidbody exists
                Rigidbody rbTarget = collision.gameObject.GetComponent<Rigidbody>();
                if (rbTarget != null)
                {
                    Vector3 knockbackDir = (collision.transform.position - transform.position).normalized;
                    Vector3 knockback = knockbackDir * knockbackForce + Vector3.up * 2f;
                    rbTarget.AddForce(knockback, ForceMode.Impulse);
                }
            }
            else
            {
                Debug.LogWarning("The gameObject tagged with 'Target' does not have a component implementing IDamageable.");
            }

            // Play audio
            if (audioSource != null && clip != null)
                audioSource.PlayOneShot(clip);

            // Destroy fireball
            Destroy(gameObject);
        }
    }
}
