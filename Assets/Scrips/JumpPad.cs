using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float launchForce = 25f;

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();

        if (player != null)
        {
            player.Launch(launchForce);
        }
    }
}