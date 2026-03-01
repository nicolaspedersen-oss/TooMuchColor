using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform respawnPoint;

    private void Awake()
    {
        if (respawnPoint == null) respawnPoint = transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("Checkpoint Colected!");

        CheckpointManager.Instance.SetCheckpoint(respawnPoint);
    }
}
