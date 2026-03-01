using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;
    private PlayerRespawn respawn;

    private void Awake()
    {
        currentHealth = maxHealth;
        respawn = GetComponent<PlayerRespawn>(); // Gets the PlayerRespawn component from the player
    }
    public void TakeDamage(float amount)
    {
        if (amount <= 0f) return;   

        currentHealth -= amount;

        if (currentHealth <= 0f)
            Die();
    }
    private void Die()
    {
        respawn.Respawn(); // Calls Respawn in PlayerRespawn

        currentHealth = maxHealth;
    }
}