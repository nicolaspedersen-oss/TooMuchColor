using UnityEngine;

public class FireOnHit : MonoBehaviour, IHitReceiver
{
    [Header("Prefabs")]
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private GameObject prefabAbove;
    [SerializeField] private float heightOffset = 1f;

    private GameObject spawnedFire;
    private bool hasSpawnedAbove = false;   // <-- prevents respawn

    public void ReceiveHit(AttackHit hit, Vector3 point, GameObject source)
    {
        // --- FIRE HIT ---
        if (hit.element == ElementType.Fire)
        {
            // If fire already exists, do nothing
            if (spawnedFire != null)
                return;

            // Spawn fire
            spawnedFire = Instantiate(firePrefab, point, Quaternion.identity);

            // Spawn the above prefab ONLY ONCE
            if (!hasSpawnedAbove && prefabAbove != null)
            {
                Vector3 abovePos = point + Vector3.up * heightOffset;
                Instantiate(prefabAbove, abovePos, Quaternion.identity);
                hasSpawnedAbove = true; // <-- never spawns again
            }

            return;
        }

        // --- WATER HIT ---
        if (hit.element == ElementType.Water)
        {
            // Turn off fire
            if (spawnedFire != null)
            {
                Destroy(spawnedFire);
                spawnedFire = null;
            }

            
        }
    }
}
