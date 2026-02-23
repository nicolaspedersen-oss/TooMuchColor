using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    [SerializeField] private float damage = 25f;
    [SerializeField] private float lifeTime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out EnemyHealth enemy))
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}