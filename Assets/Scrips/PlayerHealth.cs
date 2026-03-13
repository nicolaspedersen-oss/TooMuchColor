using UnityEngine;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private float hitSoundCooldown = 0.2f;


    private float lastHitSoundTime = -999f;
    private float currentHealth;
    private PlayerRespawn respawn;

    [SerializeField] private DamageFlash damageFlash;

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

        if (damageFlash != null)
            damageFlash.TriggerFlash();

        if (audioSource != null && hitSound != null)
        {
            if (Time.time >= lastHitSoundTime + hitSoundCooldown)
            {
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.PlayOneShot(hitSound);
                audioSource.pitch = 1f;

                lastHitSoundTime = Time.time;
            }
        }

        if (currentHealth <= 0f)
            Die();
    }

    public void Die()
    {
        respawn.Respawn();
        respawn.RespawnPlatforms();

        currentHealth = maxHealth;

        if (healthSlider != null)
            healthSlider.value = currentHealth;
    }
}
