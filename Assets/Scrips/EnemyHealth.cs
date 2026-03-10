using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public void Awake()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(float amount)
    {
        if (amount <= 0f) return;

        currentHealth -= amount;

        if (currentHealth <= 0f)
            Die();
    }
    public void Heal(float amount) 
    {
        if (amount <= 0f) return;
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }
    public void AddMaxHealth(float amount, bool alsoHealByAmount = true)
    {
        if (amount <= 0f) return;

        maxHealth += amount;

        if (alsoHealByAmount)
            currentHealth += amount;

        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }
    private void Die()
    {
        Destroy(gameObject);
    }
}