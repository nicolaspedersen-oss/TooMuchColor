using UnityEngine;

public class EnemyHitReceiver : MonoBehaviour, IHitReceiver
{
    [SerializeField] private EnemyHealth health;
    [SerializeField] private EnemyStatus status;

    private void Awake()
    {
        if (!health) health = GetComponent<EnemyHealth>();
        if (!status) status = GetComponent<EnemyStatus>();
    }

    public void ReceiveHit(AttackHit hit, Vector3 hitPoint, GameObject instigator)
    {
        if (health && hit.damage > 0f)
        {
            health.TakeDamage(hit.damage);
        }

        if (status != null)
        {
            status.ApplyHit(hit);
        }
    }
}
