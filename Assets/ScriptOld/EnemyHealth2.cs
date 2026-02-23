using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth2 : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private bool destroyOnDeath = true;

    public UnityEvent onDamaged;
    public UnityEvent onDied;

    public float CurrentHealth { get; private set; }
    public bool IsDead { get; private set; }

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (IsDead) return;
        if (amount <= 0f) return;

        CurrentHealth = Mathf.Min(CurrentHealth - amount, 0f);
        onDamaged.Invoke();

        if (CurrentHealth <= 0f)
            Die();
    }

    public void Heal(float amount)
    {
        if(IsDead) return;
        if(amount <= 0f) return;

        CurrentHealth = Mathf.Min(CurrentHealth + amount, maxHealth);
    }

    private void Die()
    {
        IsDead = true;
        onDied?.Invoke();

        // Example: disable collisions / movement here if needed
        // GetComponent<Collider>()?.enabled = false;
        // GetComponent<EnemyAI>()?.enabled = false;

        if (destroyOnDeath)
            Destroy(gameObject);
    }
}
