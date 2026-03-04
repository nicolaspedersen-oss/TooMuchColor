using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveDistance = 3f;
    public float speed = 2f;

    private Vector3 startPos;
    private Vector3 lastPos;

    public float CurrentXVelocity { get; private set; }

    void Start()
    {
        startPos = transform.position;
        lastPos = startPos;
    }

    void Update()
    {
        transform.position = startPos +
            new Vector3(Mathf.PingPong(Time.time * speed, moveDistance), 0, 0);

        // Calculate X velocity
        CurrentXVelocity = (transform.position.x - lastPos.x) / Time.deltaTime;
        lastPos = transform.position;
    }
}