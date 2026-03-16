using System.Collections;
using UnityEngine;

public class RotateObstacle : MonoBehaviour
{
    [SerializeField] private Vector3 rotationSpeed = new Vector3(0, 100, 0);
    [SerializeField] private float stopDuration = 5f;

    private Vector3 defaultRotationSpeed;
    private Coroutine stopRoutine;

    private void Awake()
    {
        defaultRotationSpeed = rotationSpeed;
    }

    private void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }

    // Hook this into StopTraps.onStoped
    public void StopTrap()
    {
        if (stopRoutine != null)
            StopCoroutine(stopRoutine);

        stopRoutine = StartCoroutine(StopForSeconds(stopDuration));
    }

    private IEnumerator StopForSeconds(float seconds)
    {
        rotationSpeed = Vector3.zero;
        yield return new WaitForSeconds(seconds);
        rotationSpeed = defaultRotationSpeed;
        stopRoutine = null;
    }
}