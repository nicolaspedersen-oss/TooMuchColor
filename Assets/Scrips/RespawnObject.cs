using UnityEngine;
using System.Collections;

public class RespawnObject : MonoBehaviour
{
    public float respawnTime = 5f;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private Rigidbody rb;
    private Renderer objRenderer;
    private Collider objCollider;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        rb = GetComponent<Rigidbody>();
        objRenderer = GetComponent<Renderer>();
        objCollider = GetComponent<Collider>();

        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnTime);

            // Hide object
            objRenderer.enabled = false;
            objCollider.enabled = false;

            yield return new WaitForSeconds(0.1f);

            // Reset position and rotation
            transform.position = startPosition;
            transform.rotation = startRotation;

            // Reset physics
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            // Show object again
            objRenderer.enabled = true;
            objCollider.enabled = true;
        }
    }
}