using UnityEngine;
using UnityEngine.Events;

public class StopTraps : MonoBehaviour, IHitReceiver
{
    [Header("Rule")]
    [SerializeField] private ElementType requiredElement = ElementType.Water;

    [Header("Visuals")]
    [SerializeField] private GameObject stopVisual;
    [SerializeField] private GameObject stopVfx;

    [Header("Puzzle Event")]
    [SerializeField] private UnityEvent onStoped;

    private bool stoped;

    public void ReceiveHit(AttackHit hit, Vector3 hitPoint, GameObject instigator)
    {
        if (stoped) return;
        if (hit.element != requiredElement) return;

        stoped = true;

        if (stopVfx) Instantiate(stopVfx, hitPoint, Quaternion.identity);
        if (stopVisual) stopVisual.SetActive(false);

        onStoped?.Invoke();
    }
}