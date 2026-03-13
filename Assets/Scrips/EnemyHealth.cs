using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hitSound;

    [SerializeField] private float hitSoundCooldown = 0.2f;
    private float lastHitSoundTime = -999f;


    public void Awake()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(float amount)
    {
        if (amount <= 0f) return;

        currentHealth -= amount;
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