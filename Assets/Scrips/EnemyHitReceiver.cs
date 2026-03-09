using Unity.Hierarchy;
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

    private void Awake()
    {
        if (!health) health = GetComponent<EnemyHealth>();
        if (!status) status = GetComponent<EnemyStatus>();
    }

    public void ReceiveHit(AttackHit hit, Vector3 hitPoint, GameObject instigator)
    {
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
