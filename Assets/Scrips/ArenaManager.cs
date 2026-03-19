using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    public GameObject door;
    public ArenaEnemy[] enemies;

    private int enemiesLeft;

    void Start()
    {
        enemiesLeft = enemies.Length;

        // Tell each enemy who owns it
        foreach (ArenaEnemy enemy in enemies)
        {
            enemy.SetArena(this);
        }

        Debug.Log("Enemies in arena: " + enemiesLeft);
    }

    public void EnemyDied()
    {
        enemiesLeft--;

        Debug.Log("Enemies left: " + enemiesLeft);

        if (enemiesLeft <= 0)
        {
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        Debug.Log("Door opened!");

        if (door != null)
        {
            Destroy(door);
        }
    }
}