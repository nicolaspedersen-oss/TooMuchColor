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
        // This triggers EVEN if you don’t know how enemy dies
        if (arena != null)
        {
            arena.EnemyDied(this);
        }
    }
}