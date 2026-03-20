using Unity.Hierarchy;
using UnityEditor;
using UnityEngine;

public class EnemyHitReceiver : MonoBehaviour, IHitReceiver
{
    [SerializeField] private EnemyHealth health;
    [SerializeField] private EnemyStatus status;

    [Header("Resistance Modifier")]
    [SerializeField] private float fireMultiplier = 1.0f;
    [SerializeField] private float waterMultiplier = 1.0f;
    [SerializeField] private float lightningMultiplier = 1.0f;
    [SerializeField] private float grassMultiplier = 1.0f;

    [Header("Element Affinity")]
    [SerializeField] private ElementType affinity = ElementType.Fire;
    [SerializeField] private bool sameElementNegatesDamage = true;

    [Header("Power up")]
    [SerializeField] private float sameElementHealFactor = 0.5f;
    [SerializeField] private float maxHealthGainFactor = 0.25f;
    [SerializeField] private float maxHealthGainFlat = 0f;
    [SerializeField] private float maxMaxHealthMultiplier = 2.0f;

    [Header("Growth")]
    [SerializeField] private float scalePerSameHit = 0.05f;
    [SerializeField] private float maxScaleMultiplier = 2.0f;

    [Header("Element Hit VFX")]
    [SerializeField] private GameObject fireHitVfx;
    [SerializeField] private GameObject waterHitVfx;
    [SerializeField] private GameObject lightningHitVfx;
    [SerializeField] private GameObject grassHitVfx;

    [Header("Element VFX Lifetime")]
    [SerializeField] private float fireVfxLifetime = 2f;
    [SerializeField] private float waterVfxLifetime = 2f;
    [SerializeField] private float lightningVfxLifetime = 2f;
    [SerializeField] private float grassVfxLifetime = 2f;

    [SerializeField] private bool orientToHitNormal = true;

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
        // --- Spawn Element-Specific VFX ---
        var vfxInfo = GetElementVFX(hit.element);

        Vector3 normal = (hitPoint - transform.position).normalized;
        SpawnHitVFX(vfxInfo, hitPoint, normal);
        // ----------------------------------

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

    // ---------------- VFX SYSTEM ----------------

    private struct VfxInfo
    {
        public GameObject prefab;
        public float lifetime;
    }

    private VfxInfo GetElementVFX(ElementType element)
    {
        return element switch
        {
            ElementType.Fire => new VfxInfo { prefab = fireHitVfx, lifetime = fireVfxLifetime },
            ElementType.Water => new VfxInfo { prefab = waterHitVfx, lifetime = waterVfxLifetime },
            ElementType.Lightning => new VfxInfo { prefab = lightningHitVfx, lifetime = lightningVfxLifetime },
            ElementType.Grass => new VfxInfo { prefab = grassHitVfx, lifetime = grassVfxLifetime },
            _ => new VfxInfo { prefab = null, lifetime = 0f }
        };
    }

    private void SpawnHitVFX(VfxInfo info, Vector3 position, Vector3 normal)
    {
        if (!info.prefab) return;

        Quaternion rotation = orientToHitNormal
            ? Quaternion.LookRotation(normal)
            : Quaternion.identity;

        GameObject vfx = Instantiate(info.prefab, position, rotation);

        // Make VFX follow the enemy
        vfx.transform.SetParent(transform, worldPositionStays: true);

        if (info.lifetime > 0f)
            Destroy(vfx, info.lifetime);
    }

    // --------------------------------------------

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
