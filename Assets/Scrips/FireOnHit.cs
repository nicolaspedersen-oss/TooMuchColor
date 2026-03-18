using UnityEngine;
using UnityEngine.Events;

public class FireOnHit : MonoBehaviour, IHitReceiver
{
    [Header("Rule")]
    [SerializeField] private ElementType requiredElement = ElementType.Fire;

    [Header("Prefabs")]
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private GameObject prefabAbove;
    [SerializeField] private float heightOffset = 1f;

    [Header("Event")]
    public UnityEvent onInteract;

    private GameObject spawnedFire;
    private bool hasSpawnedAbove = false;

    public void ReceiveHit(AttackHit hit, Vector3 point, GameObject source)
    {
        if (hit.element == requiredElement)
        {
            if (spawnedFire != null)
                return;

            // Spawn fire
            spawnedFire = Instantiate(firePrefab, point, Quaternion.identity);
            // Invoke Event
            onInteract?.Invoke();

            // Spawn the above prefab ONLY ONCE
            if (!hasSpawnedAbove && prefabAbove != null)
            {
                Vector3 abovePos = point + Vector3.up * heightOffset;
                Instantiate(prefabAbove, abovePos, Quaternion.identity);
                hasSpawnedAbove = true; // <-- never spawns again
            }

            return;
        }

        if (hit.element == ElementType.Water)
        {
            if (spawnedFire != null)
            {
                Destroy(spawnedFire);
                spawnedFire = null;
            }
        }
    }
}
