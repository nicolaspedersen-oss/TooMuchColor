using UnityEngine;

public class DestroyObstacle : MonoBehaviour
{
    [SerializeField] private float damage = 25f;
    [SerializeField] private float lifeTime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Red"))
        {
            TryGetComponent(out EnemyHealth obstacle);

            obstacle.TakeDamage(damage);
        }

            Destroy(gameObject);
    }
}