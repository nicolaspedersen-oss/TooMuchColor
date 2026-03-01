using UnityEngine;

public class FallRespawn : MonoBehaviour
{
    public float killY = -20f;
    private PlayerRespawn respawn;

    private void Awake() => respawn = GetComponent<PlayerRespawn>();

    private void Update()
    {
        if (transform.position.y < killY)
            respawn.Respawn();
    }
}