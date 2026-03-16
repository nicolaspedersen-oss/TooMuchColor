using UnityEngine;

public class FireOnHit : MonoBehaviour, IHitReceiver
{
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private GameObject PickUp;
    [SerializeField] private float heightOffset = 1;

    private bool hasSpawnedFire = false;

    public void ReceiveHit(AttackHit hit, Vector3 point, GameObject source)
    {
        if (hasSpawnedFire) return;
        // Spawns only when hit by fire
        if (hit.element == ElementType.Fire)
        {
            Instantiate(firePrefab, point, Quaternion.identity);
            Vector3 abovePos = point + Vector3.up * heightOffset;
            Instantiate(PickUp, abovePos, Quaternion.identity);
            hasSpawnedFire = true;
                
        }

        
    }
}
