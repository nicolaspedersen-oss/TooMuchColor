using UnityEngine;

public class MoveUpDown : MonoBehaviour
{
    public float speed = 2f;
    public float height = 1f;

    public bool startOpposite = false; // Tick this for the object that should go down first

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
        float newY = startPosition.y + Mathf.Sin(Time.time * speed + offset) * height;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
