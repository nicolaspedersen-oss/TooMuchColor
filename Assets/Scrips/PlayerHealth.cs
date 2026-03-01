using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Slider healthSlider;   // UI reference

    private float currentHealth;
    private PlayerRespawn respawn;

    private void Awake()
    {
        currentHealth = maxHealth;
        respawn = GetComponent<PlayerRespawn>();

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(float amount)
    {
        if (amount <= 0f) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthSlider != null)
            healthSlider.value = currentHealth;

        if (currentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        respawn.Respawn();
        currentHealth = maxHealth;

        if (healthSlider != null)
            healthSlider.value = currentHealth;
    }
}
