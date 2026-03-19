using UnityEngine;

public class ArenaEnemy : MonoBehaviour
{
    private ArenaManager arena;

    public void SetArena(ArenaManager manager)
    {
        arena = manager;
    }

    void OnDestroy()
    {
        if (arena != null)
        {
            arena.EnemyDied();
        }
    }
}