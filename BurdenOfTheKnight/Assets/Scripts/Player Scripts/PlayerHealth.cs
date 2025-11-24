using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public float maxHealth = 100f;
    float currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public bool TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log($"Player took {amount} dmg | HP = {currentHealth}");

        if (currentHealth <= 0f)
        {
            Die();
            return true;
        }
        return false;
    }

    void Die()
    {
        Debug.Log("Player died!");
    }
}
