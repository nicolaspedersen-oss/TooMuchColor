using Unity.Hierarchy;
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class EnemyHitReceiver : MonoBehaviour, IHitReceiver
{
    [SerializeField] private EnemyHealth health;
    [SerializeField] private EnemyStatus status;

    [Header("Ressistance Modifier")] // 1 = Normal : 0.5 = 50%
    [SerializeField] private float fireMultiplier = 1.0f;
    [SerializeField] private float waterMultiplier = 1.0f;
    [SerializeField] private float lightningMultiplier = 1.0f;
    [SerializeField] private float grassMultiplier = 1.0f;

    [Header("Element Affintity")] 
    [SerializeField] private ElementType affinity = ElementType.Fire;
    [SerializeField] private bool sameElementNegatesDamage = true;

    [Header("Power up")] // enemy gains max health if hit with its element affinity
    [SerializeField] private float sameElementHealFactor = 0.5f;
    [SerializeField] private float maxHealthGainFactor = 0.25f;
    [SerializeField] private float maxHealthGainFlat = 0f;
    [SerializeField] private float maxMaxHealthMultiplier = 2.0f;

    [Header("Growth")] // enemy grows if hit with its element affinity
    [SerializeField] private float scalePerSameHit = 0.05f;
    [SerializeField] private float maxScaleMultiplier = 2.0f;

    private Vector3 baseScale;
    private float currentScaleMultiplier = 1f;
    private float baseMaxHealth;

    private void Awake()
    {
        if (!health) health = GetComponent<EnemyHealth>();
        if (!status) status = GetComponent<EnemyStatus>();

        baseScale = transform.localScale;
        baseMaxHealth = health != null ? health.maxHealth : 0f;
    }

    public void ReceiveHit(AttackHit hit, Vector3 hitPoint, GameObject instigator)
    {
        bool sameElement = hit.element == affinity;

        if (sameElement)
        {
            if (health && hit.damage > 0f)
            {
                float cap = baseMaxHealth * maxMaxHealthMultiplier;
                float gain = (hit.damage * maxHealthGainFactor) + maxHealthGainFlat;

                float allowed = Mathf.Max(0f, cap - health.maxHealth);
                gain = Mathf.Min(gain, allowed);

                if (gain > 0f)
                {
                    health.AddMaxHealth(gain, alsoHealByAmount: true);
                }

                float healAmount = hit.damage * sameElementHealFactor;
                health.Heal(healAmount);
            }

            Grow();
            if (sameElementNegatesDamage)
                return;
        }

        float multiplier = GetMultiplier(hit.element);

        if (health && hit.damage > 0f)
        {
            health.TakeDamage(hit.damage * multiplier);
        }

        if (status != null)
        {
            AttackHit modified = hit;

            modified.dotDps *= multiplier;
            modified.slowPercent *= multiplier;
            modified.rootDuration *= multiplier;

            status.ApplyHit(modified);
        }
    }

    void Grow()
    {
        currentScaleMultiplier = Mathf.Min(currentScaleMultiplier + scalePerSameHit, maxScaleMultiplier);
        transform.localScale = baseScale * currentScaleMultiplier;
    }

    float GetMultiplier(ElementType element)
    {
        return element switch
        {
            ElementType.Fire => fireMultiplier,
            ElementType.Water => waterMultiplier,
            ElementType.Lightning => lightningMultiplier,
            ElementType.Grass => grassMultiplier,
            _ => 1f
        };
    }
}
