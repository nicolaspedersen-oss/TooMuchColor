using UnityEngine;

public class ArenaEnemy : MonoBehaviour
{
    public ArenaManager arenaManager; // Assign in inspector

    // Call this when the enemy dies
    public void Die()
    {
        if (arenaManager != null)
        {
            arenaManager.EnemyDied(this);
        }

        Destroy(gameObject);
    }
}