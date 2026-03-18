using UnityEngine;

public class OrbitAroundPlayer : MonoBehaviour
{
    public Transform player;
    public float distance = 2f;
    public float speed = 50f;

    public float startAngle; // Set different values for each gem

    private float angle;

    void Start()
    {
        angle = startAngle;
    }

    void Update()
    {
        if (player == null) return;

        angle += speed * Time.deltaTime;

        float rad = angle * Mathf.Deg2Rad;

        float x = Mathf.Cos(rad) * distance;
        float z = Mathf.Sin(rad) * distance;

        transform.position = player.position + new Vector3(x, 0, z);
    }
}