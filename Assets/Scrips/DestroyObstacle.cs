using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DestroyObstacle : MonoBehaviour, IHitReceiver
{
    [Header("Puzzle Rule")]
    [SerializeField] private ElementType requiredElement;
    [SerializeField] private int hitsToDestroy = 1;
    //[SerializeField] private bool consumeHitOnlyIfCorrect = true;

    [Header("Feedback")]
    [SerializeField] private GameObject correctVfx;
    [SerializeField] private GameObject wrongVfx;

    [Header("Events")]
    [SerializeField] private UnityEvent onDestroyed;

    private int remaning;

    private void Awake()
    {
        remaning = hitsToDestroy;
    }

    public void ReceiveHit(AttackHit hit, Vector3 hitpoint, GameObject instigator)
    {
        if (hit.element != requiredElement)
        {
            if (wrongVfx)
            {
                Instantiate(wrongVfx, hitpoint, Quaternion.identity);
            }
            return;
        }

        if (correctVfx)
        {
            Instantiate(correctVfx, hitpoint, Quaternion.identity);
        }

        remaning -= 1;

        if (remaning  <= 0)
        {
            onDestroyed?.Invoke();
            Destroy(gameObject);
        }
    }
}