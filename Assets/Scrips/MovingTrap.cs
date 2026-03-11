using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovingTrap : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed;
    [SerializeField] private List<Transform> patrolPoints = new List<Transform>();

    private int currentIndex = 0;
    private int direction = 1;
    private NavMeshAgent enemyAgent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Patrol();
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
    }

    private void MoveTowards()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }

    private void Patrol()
    {
        // move to closest patrolPoint from the list.
        target = patrolPoints[currentIndex];
        MoveTowards();

        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            if (currentIndex == 0)
            {
                direction = 1;
            }
            else if (currentIndex == patrolPoints.Count - 1)
            {
                direction = -1;
            }

            currentIndex += direction;
        }
    }
}
