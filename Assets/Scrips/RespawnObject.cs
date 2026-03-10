using UnityEngine;
using System.Collections;

public class RespawnObject : MonoBehaviour
{
    public float respawnTime = 5f;

    private Vector3 startPosition;
    private Renderer objRenderer;
    private Collider objCollider;

    void Start()
    {
        startPosition = transform.position;
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

            yield return new WaitForSeconds(respawnTime);

            // Respawn object
            transform.position = startPosition;
            objRenderer.enabled = true;
            objCollider.enabled = true;
        }
    }
}