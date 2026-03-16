using System.Collections;
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
    [SerializeField] private UnityEvent onStopped;

    [Header("Re-arm")]
    [SerializeField] private float rearmDelay = 5f; // match your trap stop duration

    private bool stopped;
    private Coroutine rearmRoutine;

    public void ReceiveHit(AttackHit hit, Vector3 hitPoint, GameObject instigator)
    {
        if (stopped) return;
        if (hit.element != requiredElement) return;

        stopped = true;

        if (stopVfx) Instantiate(stopVfx, hitPoint, Quaternion.identity);
        if (stopVisual) stopVisual.SetActive(false);

        onStopped?.Invoke();

        if (rearmRoutine != null) StopCoroutine(rearmRoutine);
        rearmRoutine = StartCoroutine(RearmAfterDelay());
    }

    private IEnumerator RearmAfterDelay()
    {
        yield return new WaitForSeconds(rearmDelay);

        stopped = false;
        if (stopVisual) stopVisual.SetActive(true);

        rearmRoutine = null;
    }
}