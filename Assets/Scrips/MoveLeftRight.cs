using UnityEngine;

public class MoveLeftRight : MonoBehaviour
{
    public float speed = 2f;
    public float distance = 1f;

    public bool startOpposite = false; // Tick this for the object that should start going left first

    private Vector3 startPosition;
    private float offset;

    void Start()
    {
        startPosition = transform.position;

        if (startOpposite)
        {
            offset = Mathf.PI; // shifts the sine wave so it starts opposite
        }
    }

    void Update()
    {
        float newX = startPosition.x + Mathf.Sin(Time.time * speed + offset) * distance;
        transform.position = new Vector3(newX, startPosition.y, startPosition.z);
    }
}