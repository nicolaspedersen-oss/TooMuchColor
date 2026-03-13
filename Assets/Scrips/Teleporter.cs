using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform destination;
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (destination == null) return;

        // Move the player to the destination position
        other.transform.position = destination.position;
        other.transform.rotation = destination.rotation;
    }
}