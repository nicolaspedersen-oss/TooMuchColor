using System.Collections;
using UnityEngine;

public class RaisePlatform : MonoBehaviour
{
    [SerializeField] private Transform platform;      // the moving platform
    [SerializeField] private float raiseHeight = 4f;  // how far up it moves
    [SerializeField] private float raiseTime = 1.2f;  // seconds it takes to move
    [SerializeField] private AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    Vector3 startPos;
    bool moved;

    private void Awake()
    {
        if (!platform) platform = transform;
        startPos = platform.position;
    }

    public void Raise()
    {
        if (moved) return;
        moved = true;
        StopAllCoroutines();
        StartCoroutine(RaiseRoutine());
    }

    IEnumerator RaiseRoutine()
    {
        Vector3 endPos = startPos + Vector3.up * raiseHeight;

        float t = 0f;
        while (t < raiseTime)
        {
            t += Time.deltaTime;
            float a = Mathf.Clamp01(t / raiseTime);
            float eased = curve.Evaluate(a);
            platform.position = Vector3.Lerp(startPos, endPos, eased);
            yield return null;
        }

        platform.position = endPos;
    }
}