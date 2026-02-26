using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 2f;

    private Transform player;
    private bool isChasing = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange && distance > attackRange)
        {
            isChasing = true;
        }
        else if (distance <= attackRange)
        {
            isChasing = false;
            AttackPlayer();
        }
        else
        {
            isChasing = false;
        }

        if (isChasing)
        {
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Rotate to face player
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }

    private void AttackPlayer()
    {
        Debug.Log("Enemy attacks!");
    }
}
