using UnityEngine;
using System.Collections.Generic;

public class ArenaManager : MonoBehaviour
{
    public GameObject door;

    private List<ArenaEnemy> enemies = new List<ArenaEnemy>();

    void Start()
    {
        // Find ONLY enemies inside this arena
        ArenaEnemy[] allEnemies = FindObjectsOfType<ArenaEnemy>();

        foreach (ArenaEnemy enemy in allEnemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < 50f)
            {
                enemies.Add(enemy);
                enemy.SetArena(this);
            }
        }

        Debug.Log("Enemies in arena: " + enemies.Count);
    }

    public void EnemyDied(ArenaEnemy enemy)
    {
        enemies.Remove(enemy);

        Debug.Log("Enemies left: " + enemies.Count);

        if (enemies.Count == 0)
        {
            Debug.Log("OPENING DOOR");

            if (door != null)
                Destroy(door);
        }
    }
}