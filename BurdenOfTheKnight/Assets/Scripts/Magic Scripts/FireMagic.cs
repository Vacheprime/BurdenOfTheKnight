using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FireMagicScript : MonoBehaviour
{
    [Header("Stats")]
    public float attackDamage = 20f;

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

    private void OnTriggerEnter(Collider other)
    {
        GameObject otherObject = other.gameObject;
        if (otherObject.CompareTag("Target"))
        {
            IDamageable damageable = otherObject.GetComponent<IDamageable>();

            if (damageable != null)
            {
                // Deal damage
                damageable.TakeDamage(attackDamage);
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
