using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public float maxHealth = 100;
    private float currentHealth;

    public void Start()
    {
        currentHealth = maxHealth;
    }

    public bool TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("DAMAGED");
        if (currentHealth <= 0) {
            Die();
            return true;
        }
        return false;
    }

    public void Die()
    {
        // Deactivate
        gameObject.SetActive(false);
        // Destroy the enemy
        Destroy(gameObject);
    }
}
