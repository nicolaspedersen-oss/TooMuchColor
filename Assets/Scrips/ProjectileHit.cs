using UnityEngine;

public class ProjectileHit : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5f;

    public AttackHit hit; // set by shooter when spawned

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Damage
        if (collision.collider.TryGetComponent(out EnemyHealth enemy))
            enemy.TakeDamage(hit.damage);

        // Element/status
        if (collision.collider.TryGetComponent(out EnemyStatus status))
            status.ApplyHit(hit);

        Destroy(gameObject);
    }
}