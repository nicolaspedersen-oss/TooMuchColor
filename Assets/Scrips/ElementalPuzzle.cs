using UnityEngine;
using UnityEngine.Events;

public class ElementalPuzzle : MonoBehaviour, IHitReceiver
{
    [Header("Rule")]
    [SerializeField] private ElementType requiredElement = ElementType.Water;

    [Header("Visuals")]
    [SerializeField] private GameObject flameVisual;
    [SerializeField] private GameObject extinguishVfx;

    [Header("Puzzle Event")]
    [SerializeField] private UnityEvent onExtinguished;

    private bool extinguished;

    public void ReceiveHit(AttackHit hit, Vector3 hitPoint, GameObject instigator)
    {
        if (extinguished) return;
        if (hit.element != requiredElement) return;

        extinguished = true;

        if (extinguishVfx) Instantiate(extinguishVfx, hitPoint, Quaternion.identity);
        if (flameVisual) flameVisual.SetActive(false);

        onExtinguished?.Invoke();
    }
}