using UnityEngine;
using UnityEngine.Events;

public class DestroyObstacle : MonoBehaviour, IHitReceiver
{
    [Header("Puzzle Rule")]
    [SerializeField] private ElementType requiredElement;
    [SerializeField] private int hitsToDestroy = 1;

    [Header("Feedback")]
    [SerializeField] private GameObject correctVfx;
    [SerializeField] private GameObject wrongVfx;

    [Header("Events")]
    [SerializeField] private UnityEvent onDestroyed;

    private int remainingHits;

    private void Awake()
    {
        remainingHits = hitsToDestroy;
    }

    public void ReceiveHit(AttackHit hit, Vector3 hitPoint, GameObject instigator)
    {
        // Wrong element
        if (hit.element != requiredElement)
        {
            SpawnVfx(wrongVfx, hitPoint);
            return;
        }

        // Correct element
        SpawnVfx(correctVfx, transform.position);

        remainingHits--;

        if (remainingHits <= 0)
        {
            onDestroyed?.Invoke();
            Destroy(gameObject);
        }
    }

    private void SpawnVfx(GameObject vfxPrefab, Vector3 position)
    {
        if (vfxPrefab == null)
            return;

        // Spawn at parent object, not hit point
        Instantiate(vfxPrefab, position, Quaternion.identity, transform);
    }
}
