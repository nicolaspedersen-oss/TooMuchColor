using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    [Header("Assign the door for this arena")]
    public GameObject door;

    [Header("Assign enemies manually or leave empty for auto count")]
    public ArenaEnemy[] enemies;

    void Start()
    {
        // Optional: auto-find enemies if array is empty
        if (enemies.Length == 0)
        {
            enemies = FindObjectsOfType<ArenaEnemy>();
        }
    }

    // Call this when an enemy dies
    public void EnemyDied(ArenaEnemy enemy)
    {
        // Remove enemy from the list
        enemies = System.Array.FindAll(enemies, e => e != enemy);

        // Check if all enemies are dead
        if (enemies.Length == 0 && door != null)
        {
            Destroy(door);
        }
    }
}