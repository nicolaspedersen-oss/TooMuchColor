using System.Collections;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    [SerializeField] private EnemyHealth health;
    [SerializeField] private EnemyScript ai;

    Coroutine dotRoutine;
    Coroutine slowRoutine;
    Coroutine rootRoutine;

    private void Awake()
    {
        if (health == null) health = GetComponent<EnemyHealth>();
        if (ai == null) ai = GetComponent<EnemyScript>();
    }

    private void Reset()
    {
        health = GetComponent<EnemyHealth>();
        ai = GetComponent<EnemyScript>();
    }

    public void ApplyHit(AttackHit hit)
    {
        Debug.Log($"ApplyHit: {hit.element} root={hit.rootDuration} ai={(ai != null)}", this);

        switch (hit.element)
        {
            case ElementType.Fire:
                if (hit.dotDps > 0 && hit.dotDuration > 0)
                {
                    if (dotRoutine != null) StopCoroutine(dotRoutine);
                    dotRoutine = StartCoroutine(DoDot(hit.dotDps, hit.dotDuration));
                }
                break;

            case ElementType.Water:
                if (hit.slowPercent > 0 && hit.slowDuration > 0)
                {
                    if (slowRoutine != null) StopCoroutine(slowRoutine);
                    slowRoutine = StartCoroutine(DoSlow(hit.slowPercent, hit.slowDuration));
                }
                break;
            case ElementType.Grass:
                if (hit.rootDuration > 0 && ai != null)
                {
                    if (rootRoutine != null) StopCoroutine(rootRoutine);
                    rootRoutine = StartCoroutine(DoRoot(hit.rootDuration));
                }
                break;

                // Lightning here
        }
    }

    IEnumerator DoDot(float dps, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            health.TakeDamage(dps * Time.deltaTime);
            t += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator DoSlow(float slowPercent, float duration)
    {
        float original = ai.SpeedMultiplier;
        ai.SpeedMultiplier = Mathf.Min(ai.SpeedMultiplier, 1f - Mathf.Clamp01(slowPercent));  
        yield return new WaitForSeconds(duration);
        ai.SpeedMultiplier = original;
    }

    IEnumerator DoRoot(float duration)
    {
        Debug.Log($"ROOT START {duration}s", this);
        ai.IsRooted = true;

        yield return new WaitForSeconds(duration);
        
        ai.IsRooted = false;
        Debug.Log("ROOT END", this);
    }
}