using UnityEngine;

public class FallRespawn : MonoBehaviour
{
    public float killY = -20f;
    private PlayerHealth die;

    private void Awake() => die = GetComponent<PlayerHealth>();

    private void Update()
    {
        if (transform.position.y < killY)
            die.Die();
    }
}