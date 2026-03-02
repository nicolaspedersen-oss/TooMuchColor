using Unity.VisualScripting;
using UnityEngine;

public class EnemyGhostScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float damage = 10f;

    private Transform player;
    private bool isChasing;

    private void Start()
    {
        TryFindPlayer();
    }

    private void Update()
    {
        if (player == null)
        {
            isChasing = false;

            return;
        }

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
            ChasePlayer();
    }
    private void TryFindPlayer()
    {
        var go = GameObject.FindGameObjectWithTag("Player");
        player = go != null ? go.transform : null;
    }
    private void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }
    private void AttackPlayer()
    {
        player.GetComponent<PlayerHealth>().TakeDamage(damage);
        Debug.Log("Enemy attacks!");
    }
}