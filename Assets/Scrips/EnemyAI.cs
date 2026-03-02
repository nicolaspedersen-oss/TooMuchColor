using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController))] // Attaches CharacterController component
[RequireComponent(typeof(NavMeshAgent))] // Attaches NavMeshAgent component
public class EnemyScript : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float detectionRange = 30f;

    [Header("Combat")]
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float damage = 25f;

    [Header("Gravity")]
    [SerializeField] private float gravity = -50f;

    private Transform player;
    private bool isChasing;

    private CharacterController controller;
    private NavMeshAgent agent;

    private float verticalVelocity;
    private float nextAttackTime;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();

        // Dissable agent movement
        agent.updatePosition = false;
        agent.updateRotation = false;

        agent.speed = moveSpeed;
    }

    private void Start()
    {
        TryFindPlayer();
    }

    private void Update()
    {
        ApplyGravity();

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

            if (Time.time >= nextAttackTime)
            {
                AttackPlayer();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
        else
        {
            isChasing = false;
        }

        if (isChasing)
            ChasePlayerWithPathing();

        agent.nextPosition = transform.position;
    }

    private void TryFindPlayer()
    {
        var go = GameObject.FindGameObjectWithTag("Player");
        player = go != null ? go.transform : null;
    }

    private void ChasePlayerWithPathing()
    {
        agent.SetDestination(player.position);

        Vector3 target = agent.steeringTarget;

        Vector3 toTarget = target - transform.position;
        toTarget.y = 0f;

        if (toTarget.sqrMagnitude < 0.001f)
        {
            return;
        }

        Vector3 dir = toTarget.normalized;

        Vector3 move = dir * moveSpeed;
        move.y = verticalVelocity;
        controller.Move(move * Time.deltaTime);

        transform.rotation = Quaternion.LookRotation(dir);
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;
        controller.Move(new Vector3(0f, verticalVelocity, 0f) * Time.deltaTime);
    }

    private void AttackPlayer()
    {
        var health = player.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }

        Debug.Log("Enemy attacks!");
    }
}